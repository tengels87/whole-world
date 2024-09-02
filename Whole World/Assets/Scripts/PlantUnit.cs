using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantUnit : Unit {
	public enum State {
		SEED,
		PLANT,
		FROZEN
	}
	public PlantUnit.State growthState;
	public bool initAsMature = true;
	public float timeUntilNextLevel = 3.0f; // [seconds]
	public GameObject[] growStateGO;

	private float lastTimeStamp;
	private int growthLevel = 0;

    public override void Start() {
        base.Start();

		init();

		if (initAsMature)
			setMature();
	}

	public override void Update() {
		base.Update();

		if (growthState != State.FROZEN) {
			if (growthLevel + 1 < growStateGO.Length)
				grow();
		}
	}

	public override void init() {
		base.init();

		setSeed();

		lastTimeStamp = Time.time;
	}

    public override void kill() {
        base.kill();

		// drop loot
		LootController lootController = this.GetComponent<LootController>();
		lootController.dropItems();

        this.gameObject.SetActive(false);

    }

    public void setSeed() {
		setGrowthLevel(0);
	}

	public void setMature() {
		setGrowthLevel(growStateGO.Length - 1);
	}

	private void grow() {
		float currentTime = Time.time;

		if (lastTimeStamp + timeUntilNextLevel < currentTime) {
			lastTimeStamp = currentTime;

			growthLevel++;
			setGrowthLevel(growthLevel);
		}
	}

	public void setGrowthLevel(int lvl) {
		growthLevel = lvl;

		if (growthLevel > 0)
			growthState = State.PLANT;
		else
			growthState = State.SEED;

		updatePlantMesh(growthLevel);
	}

	private void updatePlantMesh(int level) {
		for (int i=0; i<growStateGO.Length; i++) {
			growStateGO[i].SetActive(false);
		}
		growStateGO[level].SetActive(true);
	}
}
