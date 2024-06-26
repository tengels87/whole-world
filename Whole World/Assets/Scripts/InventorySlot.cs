﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler {
    public Image imageSlot;
    public Image imageBackdrop;
    public List<ItemUnit> items = new List<ItemUnit>();

    private Text textCount;
    private CustomMouseManager mouseManager;

    void Awake() {
        imageSlot.sprite = imageBackdrop.sprite;
        textCount = this.gameObject.GetComponentInChildren<Text>();

        mouseManager = WorldConstants.Instance.getMouseManager();
    }

    void Update() {

    }

    public void addItem(ItemUnit obj) {
        obj.itemState = ItemUnit.ItemState.IN_INVENTORY;
        obj.setVisible(false);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.identity;
        obj.resetComponentPositions(Vector3.zero);
        obj.transform.SetParent(this.transform);

        if (items.Count == 0) {
            imageSlot.sprite = obj.icon;
        }

        items.Add(obj);
        textCount.text = "" + items.Count;
    }

    public void addItems(List<ItemUnit> objList) {
        foreach (ItemUnit it in objList) {
            addItem(it);
        }
    }

    public ItemUnit removeItem() { // return null if there is nothing to remove
        if (items.Count > 0) {
            ItemUnit toBeRemoved = items[items.Count - 1];

            items.Remove(toBeRemoved); // remove last in list

            if (items.Count > 0) {
                textCount.text = "" + items.Count;
            } else {
                textCount.text = "";
                imageSlot.sprite = imageBackdrop.sprite;
            }

            return toBeRemoved;
        }

        return null;
    }

    public ItemUnit[] removeItems() { // return null if there is nothing to remove
        if (items.Count > 0) {
            ItemUnit[] toBeRemovedArray = items.ToArray();

            items.Clear();

            imageSlot.sprite = imageBackdrop.sprite;
            textCount.text = "" + items.Count;

            return toBeRemovedArray;
        }

        return null;
    }

    public bool useItem(ItemUnit item, NpcUnit npc) {
        if (item.itemType == ItemUnit.ItemType.AXE ||
            item.itemType == ItemUnit.ItemType.PICKAXE ||
            item.itemType == ItemUnit.ItemType.WEAPON_MELEE) {

            // unequip old
            bool doUnequip = npc.unequipItem(item);

            // equip new
            if (doUnequip) {
                npc.equipItem(item);
            }

            return false;
        } else if (item.itemType == ItemUnit.ItemType.FOOD ||
            item.itemType == ItemUnit.ItemType.POTION ||
            item.itemType == ItemUnit.ItemType.HERB) {
            npc.consumeItem(item);

            return true;
        }

        return false;
    }

    public void dropItem(ItemUnit obj, Vector3 pos) {
        obj.drop(pos);
    }

    public void OnPointerEnter(PointerEventData eventData) {
        GameObject clickedGO = eventData.pointerEnter;
        
        // arrange items in slot via mouse
        InventorySlot clickedSlot = clickedGO.GetComponentInParent<InventorySlot>();

        if (clickedSlot != null) {
            if (clickedSlot.items.Count > 0) {
                mouseManager.worldTooltip.setIcon(clickedSlot.items[0].icon);
                mouseManager.worldTooltip.setText(clickedSlot.items[0].description);
                mouseManager.worldTooltip.show();
            }
        }
    }

    //Detect when Cursor leaves the GameObject
    public void OnPointerExit(PointerEventData eventData) {
        mouseManager.worldTooltip.hide();
    }

    public void OnPointerClick(PointerEventData eventData) {
        GameObject clickedGO = eventData.pointerPress;

        // arrange items in slot via mouse
        if (eventData.pointerId == -1) {
            InventorySlot clickedSlot = clickedGO.GetComponentInParent<InventorySlot>();

            if (clickedSlot != null) {

                // get inventory manager (as parent of InventorxSlot)
                // This way we can pick and arrange Items in every slot we can find
                // (own inventory, corpse, treasure, ...)
                InventoryManager invManager = this.gameObject.GetComponentInParent<InventoryManager>();
                if (invManager != null) {

                    ItemUnit[] tmpSlotItems = null;

                    // add last item held by mouse / replace by one in slot
                    if (mouseManager.appendedItems.Count > 0) {
                        if (clickedSlot.items.Count > 0) {

                            if (clickedSlot.items[0].unitName.Equals(mouseManager.appendedItems[0].unitName)) {
                                // append to already existing (similar one)
                                clickedSlot.addItems(mouseManager.appendedItems);
                                mouseManager.setItems(null);
                            } else {
                                // swap items on mouse and in slot
                                tmpSlotItems = clickedSlot.removeItems();
                                clickedSlot.addItems(mouseManager.appendedItems);
                                mouseManager.setItems(tmpSlotItems);
                            }
                        } else {
                            clickedSlot.addItems(mouseManager.appendedItems);
                            mouseManager.setItems(null);
                        }
                    } else {
                        tmpSlotItems = clickedSlot.removeItems();
                        mouseManager.setItems(tmpSlotItems);
                    }
                }
            }
        }

        // consume / equip item in slot (RMB)
        if (eventData.pointerId == -2) {
            InventorySlot clickedSlot = clickedGO.GetComponentInChildren<InventorySlot>();

            if (clickedSlot != null) {
                
                // check if parent has inventory manager (own only!)
                InventoryManager invManager = WorldConstants.Instance.getPlayerInventory();
                // check if it's the one the player has
                if (invManager != null) {
                    if (invManager == clickedSlot.gameObject.GetComponentInParent<InventoryManager>()) {
                        bool doRemove = useItem(clickedSlot.items[0], mouseManager.playerUnit);

                        if (doRemove) {
                            ItemUnit rmvItem = removeItem();
                            if (rmvItem != null) {
                                Object.DestroyImmediate(rmvItem.gameObject);
                            }
                        }
                    }
                }
            }
        }
    }
}
