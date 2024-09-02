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
		// Test
		//if (Input.GetKeyDown(KeyCode.LeftControl))
		//	dropItems();
	}

	public void dropItems() {

		// create final item list
		List<Object> allItems = new List<Object>();

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
		float dAngle = 360.0f / allItems.Count;
		float angle = 0;
		float radius = 1;

		for (int i=0; i<allItems.Count; i++) {
			angle += (dAngle + Random.Range(-dAngle/3, dAngle/3+1));

			GameObject go = (GameObject)Object.Instantiate(allItems[i], GameObject.Find(WorldConstants.objName_world_items).transform);
			go.transform.position = this.transform.position + Vector3.up * 5;
			go.transform.Rotate(new Vector3(0, angle, 0));
			go.transform.position += go.transform.forward * radius;

			// add circular force
			Vector3 force = go.transform.forward;
			Rigidbody rb = go.GetComponentInChildren<Rigidbody>();
			if (rb != null) {
				rb.AddForce(go.transform.forward*10);
			}
		}
	}
}
