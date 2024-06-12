using UnityEngine;
using System.Collections;

public class HighlightObject : MonoBehaviour {
	public Color highlightColor = new Color(0.8f, 0.6f, 0.0f, 0.5f);

	private Material highlightMaterial;
	private GameObject targetObject;

	void Start() {
		
	}

	void Update() {
		/*
		// Test
		if (Input.GetMouseButton(1)) {
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			if (Physics.Raycast(ray, out hit, 1000, LayerMask.NameToLayer("InveractiveUnit"))) {
				setTarget(hit.collider.gameObject);
			}
		} else {
			setTarget(null);
		}
		*/
	}

	public void OnPostRender() {
		updateHighlight();
	}

	private Material getHighlightMaterial() {
		if (highlightMaterial != null)
			return highlightMaterial;

		// create from Shader
		Shader tempShader = Shader.Find("Custom/ShineColor"); // Resource
		if (tempShader != null) {
			highlightMaterial = new Material(tempShader);
			highlightMaterial.SetColor("_Color", highlightColor);

			return highlightMaterial;
		}

		return null;
	}

	public void setTarget(GameObject go) {
		if (go != null) {
			targetObject = go;
		} else { // reset targetObject
			targetObject = null;
		}
	}

	public void setColor(Color color) {
		highlightColor = color;
		getHighlightMaterial().SetColor("_Color", highlightColor);
	}

	private void updateHighlight() {
		if (targetObject != null) {
			getHighlightMaterial().SetPass(0);

			MeshFilter[] meshes = targetObject.GetComponentsInChildren<MeshFilter>();
			foreach (MeshFilter m in meshes) {
				Graphics.DrawMeshNow(m.sharedMesh, Matrix4x4.TRS(m.transform.position, m.transform.rotation, m.transform.lossyScale));
			}
		}
	}
}
