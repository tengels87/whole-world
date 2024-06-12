using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnit : NpcUnit {
    private SkillManager skillManager;

    public override void Start() {
        base.Start();
    }

    public override void Update() {
        base.Update();
    }

    public override void init() {
        base.init();

        skillManager = GameObject.Find(WorldConstants.objName_skillmanager).GetComponent<SkillManager>();
    }

    public override bool Attack(Unit target) {
        bool success = base.Attack(target);

        if (success) {
            if (skillManager != null) {

                // weapon skills
                ItemUnit mainWeapon = equipment[(int)Bodyparts.ARM_RIGHT];
                if (mainWeapon != null) {
                    if (mainWeapon.itemType == ItemUnit.ItemType.WEAPON_MELEE)
                        skillManager.getSkill(Skill.Type.FIGHTING_SWORD).changeEXP(1);
                }

                // harvesting, mining
                if (target.unitType == Unit.UnitType.TREE) {

                    if (target.unitState == Unit.UnitState.DEAD) {
                        skillManager.getSkill(Skill.Type.WOOD_WORKING).changeEXP(1);
                    }
                }

                if (target.unitType == Unit.UnitType.STONE_HEAP) {

                    if (target.unitState == Unit.UnitState.DEAD) {
                        skillManager.getSkill(Skill.Type.STONE_PROCESSING).changeEXP(1);
                    }
                }
            }
        }

        return success;
    }
}
