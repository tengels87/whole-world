using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootController : MonoBehaviour {
	public Object[] primaryItems;
	[Range(0, 1)]
	public float ProbabilityPrimItem = 1.0f;

	public Object[] secondaryItems;
	[Range(0, 1)]
	public float ProbabilitySecItem = 1.0f;

	public Object[] bonusSkillItems; // usually filled by SkillManager
	[Range(0, 1)]
	public float ProbabilityBonusItem = 1.0f;
	
	void Start() {
		
	}

	void Update() {
		if (Input.GetKeyDown(KeyCode.LeftControl))
			dropItems();
	}

	public void dropItems() {

		// create final item list
		List<Object> allItems = new List<Object>(primaryItems.Length + secondaryItems.Length + bonusSkillItems.Length);

		foreach (Object it in primaryItems) {
			if (Random.value <= ProbabilityPrimItem)
				allItems.Add(it);
		}
		foreach (Object it in secondaryItems) {
			if (Random.value <= ProbabilitySecItem)
				allItems.Add(it);
		}
		foreach (Object it in bonusSkillItems) {
			if (Random.value <= ProbabilityBonusItem)
				allItems.Add(it);
		}

		// instantiate and position items in world
		
	}
}
