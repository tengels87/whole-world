using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalRuby.Tween;

public class StatsController : MonoBehaviour {
	public Stats baseStats;
	public Stats tempStats; // modified by temp. applied buffs

	public bool isReady = false;

	public Object healthBarPrefab;
	public Object statsEffectPrefab;

	private GameObject healthBar;

	void Start() {
		baseStats.init();
		tempStats.init();

		isReady = true;
	}

	void Update() {
		if (isReady) {

		}
	}

	public void changeBaseStat(StatType stype, int val) {
		baseStats.change(stype, val);
	}

	public void changeTempStat(StatType stype, int val) {
		tempStats.change(stype, val);
	}

	public float getHPpercentage() {
		int hp = baseStats.get(StatType.HP);
		int hp_max = baseStats.get(StatType.maxHP) + tempStats.get(StatType.maxHP);

		return (float)hp / (float)hp_max;
	}

	public float getMPpercentage() {
		int mp = baseStats.get(StatType.MP);
		int mp_max = baseStats.get(StatType.maxMP) + tempStats.get(StatType.maxMP);

		return (float)mp / (float)mp_max;
	}

	public int getMaxHP() {
		return baseStats.get(StatType.maxHP) + tempStats.get(StatType.maxHP);

	}

	public int getMaxMP() {
		return baseStats.get(StatType.maxMP) + tempStats.get(StatType.maxMP);

	}

	public int getAttackValue() {
		int att = baseStats.get(StatType.attack) + tempStats.get(StatType.attack);

		// check for critical hit
		System.Random rnd = new System.Random();
		int rInt = rnd.Next(0, 100);

		if (rInt <= baseStats.get(StatType.criticalChance)) {
			att = att * 2;
		}

		return att;
	}
}
