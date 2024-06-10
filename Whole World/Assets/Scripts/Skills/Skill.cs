using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;
using UnityEngine.Events;

public class Skill : MonoBehaviour {
	public enum Type {
		WOOD_WORKING = 1,
		STONE_PROCESSING = 2,
		MINING = 3,
        HERBALISM = 4,
        FARMING = 5,

        CRAFTING_TOOLS = 11,
		CRAFTING_SWORD = 12,
		CRAFTING_AXE = 13,
		CRAFTING_BOW = 14,
        CRAFTING_POTION = 15,

		FIGHTING_SWORD = 21,
        FIGHTING_AXE = 22,
		FIGHTING_BOW = 23
	}
	public enum Applymode {
		ON_LEVELUP,
		ON_ACTION
	}

	public string skillName = "unnamed skill";
	public Skill.Type skillType;
	//[HideInInspector]
	public int level;
	public int levelBonus;
	//[HideInInspector]
	public int exp;
	//[HideInInspector]
	public int maxEXP;

	public int intValue;
	public float floatValue;

	private bool unlocked = false;

    public delegate void SkillUnlockedEvent(Skill skill);
    public event SkillUnlockedEvent onSkillUnlocked;
    public delegate void SkilllevelupEvent(Skill skill);
    public event SkilllevelupEvent onSkillLevelup;

	public void changeLevel(int val) {
		level += val;
		exp = 0;
		maxEXP = getMaxEXP();
	}
	public void changeLevelBonus(int lvl) {
		levelBonus += lvl;
    }

    public void changeEXP(int val) {
        if (!unlocked) {
            unlock();
        }

		exp += val;

		if (exp > maxEXP) {
			int rest = exp - maxEXP;
			level++;
			exp = 0;
			maxEXP = getMaxEXP();

			changeEXP(rest);
		}
	}

	private int getMaxEXP() {
		return level * level * 100;
	}

	public void unlock() {
		level = 1;
		maxEXP = getMaxEXP();

		unlocked = true;

        onSkillUnlocked(this);
    }

    public bool isUnlocked() {
        return unlocked;
    }

	public void apply() {
		
	}
}
