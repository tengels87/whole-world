using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ApplicationField {
	Stat,
	Skill
}

public enum ApplyMode {
	applyOnce,			// e.q. health potiom
	applyPeriodically,	// e.q. healing aura, poisened
	applyPersistent		// e.q. increase HP for amount of time, increased defense value when equipped
}

public class Buff : MonoBehaviour {
	public string buffName = "unnamed buff";
	public int value = 0;
	public bool isPercentageValue = false;
	public float duration = -1f; // time until destruction [seconds], -1=infinite
	public float rate = 1.0f; // [per second]
	public ApplicationField applyOn;
	public StatType applyOnStat;
	public Skill.Type applyOnSkill;
	public ApplyMode applyMode;

	public bool isReady = false;
	public float startEffectTime;
	public float effectTimer;
	public bool isDestroyed = false;

	public void activate() {
		if (applyMode == ApplyMode.applyPeriodically ||
			applyMode == ApplyMode.applyPersistent) {
			startEffectTime = Time.time;
			effectTimer = 0;
		} else if (applyMode == ApplyMode.applyOnce) {
			destroy();
		}

		isReady = true;
	}

	public void destroy() {
		isReady = false;
		isDestroyed = true;
	}

	public string getBuffText() {
		if (value == 0) {
			return "";
        }

		Color color = Color.green;
		if (value < 0) {
			color = Color.red;
        }

		string how = "";
		string what = "";
		string timing = "";

		if (applyMode == ApplyMode.applyOnce) {
			if (value > 0) {
				how = "+";
			} else {
				how = "-";
			}
        } else if (applyMode == ApplyMode.applyPersistent) {
			if (value > 0) {
				how = "+";
			} else {
				how = "-";
			}
			if (duration > 0) {
				timing = "for " + duration + " s";
            }
		} else if (applyMode == ApplyMode.applyPeriodically) {
			if (value > 0) {
				how = "+";
			} else {
				how = "-";
			}
			timing = "every " + (1.0f / rate) + " s";
			if (duration > 0) {
				timing = timing + " for " + duration + " s";
			}
		}

		if (applyOn == ApplicationField.Stat) {
			what = applyOnStat.ToString();
		} else if (applyOn == ApplicationField.Skill) {
			what = applyOnSkill.ToString() + " level";
        }

		string ret = how + "" + value + "" + what + " " + timing;

		return ret;

	}

	/*
	public Buff CopyBuffComponent(GameObject target) {
		// * usage:
		//    GameObject g = GameObject.Find("testObj1");
		//	Buff bb = CopyBuffComponent(g);
		//	bb.activate();

		Buff newBuff = target.AddComponent<Buff>();

		if (UnityEditorInternal.ComponentUtility.CopyComponent(this)) {
			if (UnityEditorInternal.ComponentUtility.PasteComponentValues(newBuff)) {
				print("copied buff component: " + buffName);

				return newBuff;
			}
		}

		return null;
	}
	*/
}
