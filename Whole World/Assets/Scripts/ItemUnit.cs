using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemUnit : Unit {
    public enum ItemState {
        IN_WORLD,
        IN_INVENTORY,
        EQUIPPED,
    }

    public enum ItemType {
        AXE,
        PICKAXE,

        WEAPON_MELEE,
        WEAPON_RANGED,
        SHIELD,

        ARMOR_HEAD,
        ARMOR_BODY,
        ARMOR_LEGS,

        WOOD,
        STONE,
        ORE,
        GEM,

        FOOD,
        POTION,
        HERB
    }

    public Sprite icon;
    public string description;
    public ItemUnit.ItemState itemState;
    public ItemUnit.ItemType itemType;
    public NpcUnit.Bodyparts bodyPart = NpcUnit.Bodyparts.ARM_RIGHT;

    public int actionValue = 0;

    public override void Start() {
        base.Start();

        string buffText = "";

        Buff[] buffList = this.gameObject.GetComponentsInChildren<Buff>();
        foreach (Buff b in buffList) {
            buffText = buffText + b.getBuffText() + "\n";
        }

        description = "<color=green>" + this.unitName + "</color>" + "\n" + buffText;
    }

    public override void Update() {
        base.Update();
    }

    public override void resetComponentPositions(Vector3 newPos) {
        base.resetComponentPositions(newPos);

        Rigidbody[] rbs = this.gameObject.GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody b in rbs) {
            b.isKinematic = (itemState == ItemState.IN_INVENTORY);
            b.useGravity = !b.isKinematic;
        }
    }

    public override void clicked(CustomMouseManager mouseManager) {

        // TEST: add item to inventory
        if (itemState == ItemUnit.ItemState.IN_WORLD) {

        }

        // TEST: grab item with mouse from inventory
        if (itemState == ItemUnit.ItemState.IN_INVENTORY) {

        }
    }

    public virtual int getActionValue() {
        return actionValue;
    }

    public void drop(Vector3 pos) {
        itemState = ItemUnit.ItemState.IN_WORLD;
        transform.SetParent(null);
        this.resetComponentPositions(pos + Vector3.up * 5);
        setVisible(true);
    }

    private Buff[] getBuffs() {
        return this.gameObject.GetComponents<Buff>();
    }

    private void applyBuffs() {

        // get stats controller and apply buffs
        Stats stats = this.gameObject.GetComponent<Stats>();

    }
}
