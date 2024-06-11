using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffController : MonoBehaviour {
	public StatsController statsController;
	public SkillManager skillManager;

	public List<Buff> buffs = new List<Buff>();

    void Start() {

    }

	void Update() {
		for (int i = 0; i < buffs.Count; i++) {

			// periodically applied or permanent with duration > 0
			if (buffs[i].isReady) {
				if (buffs[i].applyMode != ApplyMode.applyOnce) {
					if (Time.time - buffs[i].startEffectTime <= buffs[i].duration) {

						if (buffs[i].applyMode == ApplyMode.applyPeriodically) {
							buffs[i].effectTimer += Time.deltaTime;
							if (buffs[i].effectTimer >= (1.0f / buffs[i].rate)) {
								buffs[i].effectTimer = 0;

								if (buffs[i].applyOn == ApplicationField.Stat) {
									statsController.changeBaseStat(buffs[i].applyOnStat, buffs[i].value);
								} else if (buffs[i].applyOn == ApplicationField.Skill) {
									skillManager.getSkill(buffs[i].applyOnSkill).changeLevelBonus((int)buffs[i].value);
								}
							}
						} else if (buffs[i].applyMode == ApplyMode.applyPersistent) {
							// persistant, applied on tempStats in activate(),
							// with limited duration, removed in destroyBuff()
						}
					} else {
						buffs[i].destroy();
					}
				}
			}
		}

		// clean up list when buff is destroyed
		for (int i = buffs.Count - 1; i >= 0; i--) {
			if (buffs[i].isDestroyed) {
				buffs.RemoveAt(i);
			}
		}
	}

	public void addBuff(Buff buff, GameObject targetGO) {
		statsController = targetGO.GetComponent<StatsController>();
		if (statsController == null) {
			Debug.LogError("no StatsController found to apply buff on (" + buff.buffName + ")");
		} else {

			// start immediatly, if applied once
			if (buff.applyMode == ApplyMode.applyOnce) {
				if (buff.applyOn == ApplicationField.Stat) {
					statsController.changeBaseStat(buff.applyOnStat, buff.value);
				} else if (buff.applyOn == ApplicationField.Skill) {
					skillManager.getSkill(buff.applyOnSkill).changeLevel((int)buff.value);
				}
			}

			if (buff.applyMode == ApplyMode.applyPersistent) {
				if (buff.applyOn == ApplicationField.Stat) {
					statsController.changeTempStat(buff.applyOnStat, buff.value);
				} else if (buff.applyOn == ApplicationField.Skill) {
					skillManager.getSkill(buff.applyOnSkill).changeLevelBonus((int)buff.value);
				}
			}

			buff.activate();
		}
	}

	public void removeBuff(Buff buff) {

		// remove persistant effect after time expired
		if (buff.applyMode == ApplyMode.applyPersistent) {
			if (buff.applyOn == ApplicationField.Stat) {
				statsController.changeTempStat(buff.applyOnStat, -buff.value);
			} else if (buff.applyOn == ApplicationField.Skill) {
				skillManager.getSkill(buff.applyOnSkill).changeLevelBonus((int)(-buff.value));
			}
		}

		buff.destroy();
	}
}
