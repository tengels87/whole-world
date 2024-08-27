using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using RPGCharacterAnims;
using RPGCharacterAnims.Lookups;
using RPGCharacterAnims.Actions;

public class CustomMouseManager : MonoBehaviour {
    public Camera mainCamera;
    public HighlightObject highlighter;
    public LayerMask terrainLayer;
    public LayerMask structureLayer;
    public LayerMask itemLayer;
    public LayerMask plantLayer;
    public LayerMask npcLayer;
    public LayerMask selectableLayer;
    public PlayerUnit playerUnit;
    public List<ItemUnit> appendedItems = new List<ItemUnit>();

    public Image hpImage;
    public Text hpText;

    public Tooltip cursorTooltip;
    public Tooltip worldTooltip;

    private RPGCharacterController characterController;

    private Vector3 moveTarget;
    public ItemUnit collectTarget = null;
    public Unit attackTarget = null;

    public delegate void ItemCollectedEvent(ItemUnit item);
    public event ItemCollectedEvent onItemCollected;

    void Awake() {
        characterController = playerUnit.GetComponentInChildren<RPGCharacterController>();
        if (characterController == null) {
        }
    }

    void Start() {

    }

    void Update() {

        // update position on mouse UI
        cursorTooltip.setPosition2D(Input.mousePosition);
        worldTooltip.setPosition2D(Input.mousePosition);

        // raycast Units
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (!EventSystem.current.IsPointerOverGameObject()) { // not touching 2D UI

            // drop item if appended to mouse
            if (Input.GetMouseButtonDown(1)) {
                if (appendedItems.Count > 0) {
                    if (Physics.Raycast(ray, out hit, 1000, terrainLayer)) {
                        ItemUnit lastItem = removeLastItem();
                        lastItem.drop(hit.point);
                    }
                }
            }

            // move on terrain, collect, attack
            if (Input.GetMouseButtonDown(0)) {
                LayerMask lmask = LayerMask.GetMask(new string[3] { "Walkable", "collectable", "attackable" });
                if (Physics.Raycast(ray, out hit, float.PositiveInfinity, lmask)) {
                    if (((1 << hit.collider.gameObject.layer) & terrainLayer) != 0) {
                        //UnityEngine.AI.NavMeshPath path = new UnityEngine.AI.NavMeshPath();
                        //if (UnityEngine.AI.NavMesh.CalculatePath(playerUnit.transform.position, h.point, UnityEngine.AI.NavMesh.AllAreas, path)) {
                        //    Vector3[] nodes = path.corners;
                        collectTarget = null;
                        attackTarget = null;

                        moveTarget = playerUnit.setMoveTarget(hit.point);
                        characterController.StartAction(HandlerTypes.Navigation, moveTarget);
                        //}
                    } else if (((1 << hit.collider.gameObject.layer) & itemLayer) != 0) {
                        if (hit.collider.isTrigger) {
                            ItemUnit hitUnit = (ItemUnit)hit.collider.gameObject.transform.GetComponentInParent<Unit>();
                            if (hitUnit != null) {
                                attackTarget = null;
                                collectTarget = hitUnit;
                                moveTarget = playerUnit.setMoveTarget(hit.point - (hit.point - playerUnit.gameObject.transform.position).normalized);
                                characterController.StartAction(HandlerTypes.Navigation, moveTarget);
                            }
                        }
                    } else if (((1 << hit.collider.gameObject.layer) & plantLayer) != 0) {
                        if (hit.collider.isTrigger) {
                            Unit hitUnit = hit.collider.gameObject.transform.GetComponent<Unit>();
                            if (hitUnit != null) {
                                collectTarget = null;
                                attackTarget = hitUnit;
                                moveTarget = playerUnit.setMoveTarget(hit.point - (hit.point - playerUnit.gameObject.transform.position).normalized);
                                characterController.StartAction(HandlerTypes.Navigation, moveTarget);
                            }
                        }
                    }
                }
            }

            // point on units and highlight them
            if (Physics.Raycast(ray, out hit, float.PositiveInfinity, selectableLayer)) {
                if (hit.collider.isTrigger) {
                    Unit hitUnit = hit.transform.GetComponentInParent<Unit>();
                    if (hitUnit != null) {
                        highlighter.setTarget(hit.collider.gameObject);
                        hitUnit.highlight();
                    }
                }
            } else {
                highlighter.setTarget(null);
            }
        }

        // pickup item
        if (collectTarget != null) {
            if (Vector3.Distance(playerUnit.mainGO.transform.position, moveTarget) < 1.0f) {
                RPGCharacterController charRPG = playerUnit.GetComponentInChildren<RPGCharacterController>();
                if (charRPG != null) {
                    if (charRPG.HandlerExists(HandlerTypes.Attack)) {
                        if (charRPG.CanStartAction(HandlerTypes.Attack)) {
                            ItemUnit item = collectTarget;
                            collectTarget = null;
                            bool success = WorldConstants.Instance.getPlayerInventory().addItem(item);
                            if (success) {
                                charRPG.StartAction(HandlerTypes.Attack, new AttackContext("Attack", Side.Left));

                                if (onItemCollected != null)
                                    onItemCollected(item);
                            }
                        }
                    }
                }
            }
        }
    }

    void LateUpdate() {

        // attack plant, NPC
        if (attackTarget != null) {

            // show unit stats
            hpImage.transform.parent.gameObject.SetActive(true);
            hpImage.rectTransform.sizeDelta = new Vector2(128 * (attackTarget.statsController.getHPpercentage()), 32);
            hpText.text = attackTarget.unitName;

            if (Vector3.Distance(playerUnit.gameObject.transform.position, moveTarget) < 2.0f) {
                moveTarget = playerUnit.gameObject.transform.position;
                characterController.StartAction(HandlerTypes.Navigation, moveTarget);

                RPGCharacterController charRPG = playerUnit.GetComponentInChildren<RPGCharacterController>();
                if (charRPG != null) {
                    if (charRPG.canAction) {
                        Unit unit = attackTarget;
                        bool doAttack = playerUnit.Attack(unit);
                        if (doAttack) {
                            //charRPG.Attack(1);
                        }

                        // continue attacking until mouse button is released
                        if (!Input.GetMouseButton(0) || unit.unitState == Unit.UnitState.DEAD) {
                            attackTarget = null;
                        }
                    }
                }
            }
        } else {

            // hide unit stats
            //hpImage.transform.parent.gameObject.SetActive(false);
        }
    }

    public void setItems(ItemUnit[] items) {
        appendedItems.Clear();

        if (items != null) {
            appendedItems.AddRange(new List<ItemUnit>(items));

            cursorTooltip.setIcon(items[0].icon);
            cursorTooltip.setText("" + appendedItems.Count);
            cursorTooltip.show();
        } else {
            cursorTooltip.setIcon(null);
            cursorTooltip.setText("");
            cursorTooltip.hide();
        }
    }

    private ItemUnit removeLastItem() {
        if (appendedItems.Count > 0) {
            ItemUnit lastItem = appendedItems[appendedItems.Count - 1];
            appendedItems.Remove(lastItem);

            cursorTooltip.setText("" + appendedItems.Count);

            if (appendedItems.Count == 0) {
                cursorTooltip.setIcon(null);
                cursorTooltip.setText("");
                cursorTooltip.hide();
            }

            return lastItem;
        }

        return null;
    }
}
