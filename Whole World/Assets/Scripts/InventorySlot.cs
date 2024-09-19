using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler {
    public Image imageSlot;
    public Image imageBackdrop;
    public Image imageHighlight;
    public List<ItemUnit> items = new List<ItemUnit>();

    private Text textCount;
    private CustomMouseManager mouseManager;

    void Awake() {
        imageSlot.sprite = imageBackdrop.sprite;
        textCount = this.gameObject.GetComponentInChildren<Text>();
        imageHighlight.gameObject.SetActive(false);

        mouseManager = WorldConstants.Instance.getMouseManager();
    }

    void Update() {

    }

    public void highlightSlot(bool isHighlighted) {
        imageHighlight.gameObject.SetActive(isHighlighted);
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

    public bool canComsumeItem(ItemUnit item) {
        if (item.itemType == ItemUnit.ItemType.AXE ||
            item.itemType == ItemUnit.ItemType.PICKAXE ||
            item.itemType == ItemUnit.ItemType.WEAPON_MELEE) {

            return false;
        } else if (item.itemType == ItemUnit.ItemType.FOOD ||
            item.itemType == ItemUnit.ItemType.POTION ||
            item.itemType == ItemUnit.ItemType.HERB) {

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
        int pointerId = CustomMouseManager.GetValidPointerId(eventData);

        // arrange items in slot via mouse
        if (pointerId == -1) {
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
        if (pointerId == -2) {
            InventorySlot clickedSlot = clickedGO.GetComponentInChildren<InventorySlot>();

            if (clickedSlot != null) {
                
                // check if parent has inventory manager (own only!)
                InventoryManager invManager = WorldConstants.Instance.getPlayerInventory();
                // check if it's the one the player has
                if (invManager != null) {
                    if (invManager == clickedSlot.gameObject.GetComponentInParent<InventoryManager>()) {
                        if (clickedSlot.items.Count > 0) {  // only when there is an item in this slot
                            bool doRemove = canComsumeItem(clickedSlot.items[0]);

                            if (doRemove) { // consume it!
                                mouseManager.playerUnit.consumeItem(clickedSlot.items[0]);
                                ItemUnit rmvItem = removeItem();
                                if (rmvItem != null) {
                                    Object.DestroyImmediate(rmvItem.gameObject);
                                }
                            } else {        // equip it!

                                // unequip item of same type, if equipped
                                ItemUnit _itemToUnequip = mouseManager.playerUnit.getItemAtBodypart(clickedSlot.items[0].bodyPart);
                                if (_itemToUnequip != null) {
                                    mouseManager.playerUnit.unequipItem(_itemToUnequip);

                                    // update highlight visuals at Inventory>Slot
                                    InventoryManager inventoryManager = this.GetComponentInParent<InventoryManager>();
                                    if (inventoryManager != null) {
                                        int slotIndex = inventoryManager.getSlotIndex(_itemToUnequip);
                                        inventoryManager.setHighlightSlot(slotIndex, false);
                                    }
                                }

                                // equip clicked item, if it is different from equipped item before
                                if (_itemToUnequip == null || _itemToUnequip.unitName != clickedSlot.items[0].unitName) {
                                    mouseManager.playerUnit.equipItem(clickedSlot.items[0]);
                                    highlightSlot(true);
                                }

                                ItemUnit[] aaa = mouseManager.playerUnit.equipment;
                                int bbb = 0;
                            }
                        }
                    }
                }
            }
        }
    }
}
