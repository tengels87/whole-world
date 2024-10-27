using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

using RPGCharacterAnims;
using RPGCharacterAnims.Lookups;

public class NpcUnit : Unit {
	public enum KIstate {
		IDLE,
		WALK,
		RUN,
		ATTACK,
		HIT,
		DEAD
	}
	public enum Bodyparts {
		HEAD = 0,
		ARM_RIGHT = 1,
		ARM_LEFT = 2,
		TORSO = 3,
		LEGS = 4
	}
	public KIstate kiState;
	public float followDistance = 1f;
	public ItemUnit[] equipment = new ItemUnit[5];	// 5 bodyparts

	private NavMeshAgent agent;
	private RPGCharacterController characterController;


	public override void Start() {
		init();
	}

	public override void Update() {
		base.Update();
	}

	public override void init() {
		base.init();

		agent = this.gameObject.GetComponent<NavMeshAgent>();
		if (agent == null)
			Debug.LogError(this.gameObject.name + ": NavMeshAgent Component not found");

		characterController = this.gameObject.GetComponentInParent<RPGCharacterController>();
		if (characterController == null)
			Debug.LogError(this.gameObject.name  + ": RPGCharacterController Component not found");

		// only use this agent to validate if a destination is reachable on the nav mesh
		agent.isStopped = true;
	}

	public Vector3 setMoveTarget(Vector3 pos) {
		agent.SetDestination(pos);

		return agent.destination;
	}

	public void setAttackTarget(Unit target) {

	}

	private int getAttackValue() {
		int a_base = statsController.getAttackValue();
		int a_melee_weapon = 0;

		ItemUnit weapon = equipment[(int)Bodyparts.ARM_RIGHT];
		if (weapon != null) {
			a_melee_weapon = ((WeaponUnit)weapon).getActionValue();
		}

		return a_base + a_melee_weapon;
	}

	public virtual bool Attack(Unit target) {
		int attackVal = getAttackValue();

		WeaponUnit weapon = (WeaponUnit)equipment[(int)Bodyparts.ARM_RIGHT];

		if (target.unitType == Unit.UnitType.TREE) {
			if (weapon != null) {
				if (weapon.itemType == ItemUnit.ItemType.AXE) {
					target.hit(attackVal);

					return true;
				}
			}
		} else if (target.unitType == Unit.UnitType.STONE_HEAP || target.unitType == Unit.UnitType.ORE) {
			if (weapon != null) {
				if (weapon.itemType == ItemUnit.ItemType.PICKAXE) {
					target.hit(attackVal);

					return true;
				}
			}
		} else {
			target.hit(attackVal);

			return true;
		}

		return false;
	}

	public override void hit(int val) {
		base.hit(val);
	}

	public void consumeItem(ItemUnit item) {
		GameObject g = this.gameObject;
		Buff[] buffsOnItem = item.gameObject.GetComponents<Buff>();
		BuffController buffController = g.GetComponent<BuffController>();
		if (buffController == null) {
			buffController = g.AddComponent<BuffController>();
        }

		foreach (Buff buff in buffsOnItem) {
			buffController.addBuff(buff, g);
		}
	}

	public void equipItem(ItemUnit item) {
		equipment[(int)item.bodyPart] = item;

		animate_switchWeapon(Weapon.TwoHandSword);
    }

	public bool unequipItem(ItemUnit item) {
		for (int i = 0; i < equipment.Length; i++) {
			if (equipment[i] != null) {
				if (equipment[i].Equals(item)) {
					equipment[i] = null;

					animate_switchWeapon(Weapon.Unarmed);

					return true;
				}
			}
		}

		return false;
	}

	public ItemUnit getItemAtBodypart(Bodyparts _bodypart) {
		if (equipment[(int)_bodypart] != null) {
			return equipment[(int)_bodypart];
		}

		return null;
	}

	private void animate_switchWeapon(Weapon weaponAnim) {
		if (!characterController.HandlerExists(HandlerTypes.SwitchWeapon)) { return; }

		var switchWeaponContext = new RPGCharacterAnims.Actions.SwitchWeaponContext();
		bool doSwitch = false;

		// Unarmed
		if (weaponAnim == Weapon.Unarmed) {
			if (characterController.rightWeapon != Weapon.Unarmed
				|| characterController.leftWeapon != Weapon.Unarmed) {
				doSwitch = true;

				switchWeaponContext.type = "Switch";
				switchWeaponContext.side = "Both";
				switchWeaponContext.leftWeapon = Weapon.Unarmed;
				switchWeaponContext.rightWeapon = Weapon.Unarmed;
			}
		} else {	// weapons
			if (characterController.rightWeapon != weaponAnim) {
				doSwitch = true;

				switchWeaponContext.type = "Switch";
				switchWeaponContext.side = "None";
				switchWeaponContext.leftWeapon = Weapon.Unarmed;
				switchWeaponContext.rightWeapon = weaponAnim;
			}
		}

		if (doSwitch) {
			characterController.TryStartAction(HandlerTypes.SwitchWeapon, switchWeaponContext);
		}
	}
}
