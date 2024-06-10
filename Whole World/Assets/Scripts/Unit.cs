using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class Unit : MonoBehaviour {
	public string unitName = "unnamed unit";
	//public Highlighting highlighting;
	public enum UnitType {
		NONE,
		TREE,
		BUSH,
		STONE_HEAP,
		ORE,

		ITEM,

		HUMAN,
		ANIMAL,
		UNDEAD
	}
	public enum UnitState {
		ALIVE,
		PAUSED,
		DEAD
	}
	public Unit.UnitType unitType;
	public Unit.UnitState unitState;
	public GameObject mainGO;

	[HideInInspector]
	public StatsController statsController;
	[HideInInspector]
	public LootController lootController;

	public virtual void Start() {

    }

	public virtual void Update() {
		
	}

	public virtual void init() {
		statsController = this.gameObject.GetComponent<StatsController>();
		if (statsController == null) {
			Debug.LogError(this.gameObject.name + ": StatsController Component not found");
		}

		lootController = this.gameObject.GetComponent<LootController>();
		if (lootController == null) {
			Debug.LogError(this.gameObject.name + ": LootController Component not found");
		}
	}

	public virtual void resetComponentPositions(Vector3 newPos) {
		this.transform.position = newPos;
		//mainGO.transform.localPosition = Vector3.zero;

		Rigidbody[] rbs = this.gameObject.GetComponentsInChildren<Rigidbody>();
		foreach (Rigidbody b in rbs) {
            b.isKinematic = true;
            b.useGravity = false;
			b.velocity = Vector3.zero;
			b.angularVelocity = Vector3.zero;
		}
	}

	public void setVisible(bool val) {
		MeshRenderer[] rlist = mainGO.GetComponentsInChildren<MeshRenderer>();;
        foreach (MeshRenderer r in rlist) {
            if (r != null) {
                r.enabled = val;
            }
        }
	}

	public virtual void highlight() {
		//print("mouse: hit " + unitName);
	}

	public virtual void clicked() {

	}

	public virtual void hit(int val) {
		statsController.changeBaseStat(StatType.HP, val);

		if (statsController.baseStats.get(StatType.HP) == 0) {
            kill();
		}
	}

    public virtual void kill() {
        unitState = UnitState.DEAD;
    }
}
