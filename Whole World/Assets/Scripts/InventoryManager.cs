using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour {
    public enum InventoryLayout {
        GRID,
        ROW
    }

    public InventoryLayout layout;
    public int initialSlotCount = 9;
    public bool alwaysVisible = false;
    public Object slotPrefab;
    public InventorySlot[] slots;

    private Image backdrop;
    private bool isOpen = false;

    // grid UI
    private float slotWidth = 48;
    private float margin = 16;

    void Start() {
        addSlots(initialSlotCount);
        initSlots();
        positionSlots();

        backdrop = this.gameObject.GetComponent<Image>();

        setVisible(false);
    }

    void Update() {

        // TEST: toggle inventory
        if (Input.GetKeyDown(KeyCode.Return))
            setVisible(!isOpen);
    }

    public void setVisible(bool val) {
        isOpen = val;

        if (alwaysVisible) {
            isOpen = true;
        }

        backdrop.enabled = isOpen;

        foreach (InventorySlot s in slots)
            s.gameObject.SetActive(isOpen);
    }

    public bool isVisible() {
        if (alwaysVisible)
            return true;
        else
            return isOpen;
    }

    public bool addItem(ItemUnit obj) {
        int emptySlotIndex = -1; // first empty slot, used later when item not found in slots
        int foundSlotIndex = -1; // item already exists in this slot

        for (int i = 0; i < slots.Length; i++) {
            if (slots[i].items.Count == 0) {
                if (emptySlotIndex == -1)
                    emptySlotIndex = i;
            } else {
                if (slots[i].items[0].unitName == obj.unitName) {
                    foundSlotIndex = i;
                    slots[i].addItem(obj);

                    return true;
                }
            }
        }

        if (emptySlotIndex != -1) {
            slots[emptySlotIndex].addItem(obj);

            return true;
        }

        return false;
    }

    public void removeItem(ItemUnit item) {
        // done in InventorySlot
    }

    public List<ItemUnit> getItemsAll() {
        List<ItemUnit> retList = new List<ItemUnit>();

        for (int i = 0; i < slots.Length; i++) {
            for (int c = 0; c < slots[i].items.Count; c++) {
                retList.Add(slots[i].items[c]);
            }
        }

        return retList;
    }

    public void addSlots(int count) {
        for (int i = 0; i < count; i++) {
            GameObject slot = (GameObject)Object.Instantiate(slotPrefab);
            slot.transform.SetParent(this.transform, false);
        }
    }

    private void initSlots() {
        slots = this.gameObject.GetComponentsInChildren<InventorySlot>();

        foreach (InventorySlot slot in slots) {
            slot.transform.SetParent(this.transform, false);
        }
    }

    private void positionSlots() {
        RectTransform rect = this.gameObject.GetComponent<RectTransform>();
        float rows = layout == InventoryLayout.GRID ? Mathf.Floor(Mathf.Sqrt(slots.Length)) : 1;
        float cols = layout == InventoryLayout.GRID ? Mathf.Ceil(Mathf.Sqrt(slots.Length)) : slots.Length;

        rect.sizeDelta = new Vector2(cols * slotWidth + (cols + 1) * margin, rows * slotWidth + (rows + 1) * margin);

        for (int i = 0; i < slots.Length; i++) {
            GameObject slot = slots[i].gameObject;
            float step = slotWidth + margin;
            
            slot.transform.localPosition = new Vector3(-margin, margin, 0) + new Vector3((Mathf.Floor(i % cols) * step + slotWidth), rect.sizeDelta.y - (Mathf.Floor(i / cols) * step + slotWidth), 0);
            slot.transform.localRotation = Quaternion.identity;
        }
    }
}
