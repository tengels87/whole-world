using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatType {
	HP = 0,
	maxHP = 1,
	MP = 2,
	maxMP = 3,
	stamina = 4,
	maxStamina = 5,
	hunger = 6,
	maxHunger = 7,
	EXP = 8,
	maxEXP = 9,
	level = 10,

	attack = 20,
	defense = 21,
	blockChance = 22, // [0..1]
	criticalChance = 23 // [0..1]
}

[System.Serializable]
public class Stats {
	public int[] values = new int[24];

	public void init() {
		set(StatType.level, 1);

		set(StatType.HP, get(StatType.maxHP));
		set(StatType.MP, get(StatType.maxMP));
		set(StatType.maxEXP, getMaxEXP());
		set(StatType.EXP, get(StatType.maxEXP));
		set(StatType.hunger, get(StatType.maxHunger));
		set(StatType.stamina, get(StatType.maxStamina));
	}

	public void set(StatType statType, int val) {
		values[(int)statType] = val;
	}

	public int get(StatType statType) {
		return values[(int)statType];
	}

	public void change(StatType statType, int chval) {
		int val = values[(int)statType];

		val += chval;

		// value < 0
		if (val < 0)
			val = 0;

		// value <= maxValue
		if (statType == StatType.HP)
			val = Mathf.Min(val, values[(int)StatType.maxHP]);

		if (statType == StatType.MP)
			val = Mathf.Min(val, values[(int)StatType.maxMP]);

		if (statType == StatType.stamina)
			val = Mathf.Min(val, values[(int)StatType.maxStamina]);

		if (statType == StatType.hunger)
			val = Mathf.Min(val, values[(int)StatType.maxHunger]);


		values[(int)statType] = val;
	}

	public void changeEXP(int val) {
		values[(int)StatType.EXP] += val;

		if (values[(int)StatType.EXP] > values[(int)StatType.maxEXP]) {
			int rest = values[(int)StatType.EXP] - values[(int)StatType.maxEXP];
			values[(int)StatType.level]++;
			values[(int)StatType.EXP] = 0;
			values[(int)StatType.maxEXP] = getMaxEXP();

			changeEXP(rest);
		}
	}

	private int getMaxEXP() {
		return values[(int)StatType.level] * values[(int)StatType.level] * 100;
	}
}
