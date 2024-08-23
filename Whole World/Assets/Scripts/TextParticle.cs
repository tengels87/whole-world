using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DigitalRuby.Tween;

public class TextParticle : MonoBehaviour {
	public Text textElement;
	[HideInInspector]
	public string id;
	[HideInInspector]
	public Vector2 size;

	private RectTransform rect;
	private RectTransform canvasRT;

	private Camera cam_main;
	private Vector3 pos0_world;

	void Awake() {
		id = "" + Time.time + Random.value * 1000000;
	}

	void Start() {
		rect = this.gameObject.GetComponent<RectTransform>();
		canvasRT = WorldConstants.Instance.getMainCanvas().gameObject.GetComponent<RectTransform>();

		cam_main = WorldConstants.Instance.getMainCamera();
	}

	void Update() {
		Vector2 screenPosition = WorldToCanvasPosition(canvasRT, cam_main, pos0_world);

		rect.localPosition = screenPosition;
	}

	private Vector2 WorldToCanvasPosition(RectTransform canvas, Camera camera, Vector3 position) {
		Vector2 temp = camera.WorldToViewportPoint(position);

		temp.x *= canvas.sizeDelta.x;
		temp.y *= canvas.sizeDelta.y;

		temp.x -= canvas.sizeDelta.x * canvas.pivot.x;
		temp.y -= canvas.sizeDelta.y * canvas.pivot.y;

		return temp;
	}

	public void init(string str, Color color, Vector3 worldPos) {
		textElement.text = str;

		textElement.color = color;

		GameObject go = this.gameObject;

		pos0_world = worldPos;

		Vector3 pos0 = Camera.main.WorldToScreenPoint(pos0_world);
		
		Vector3 pos1_world = Camera.main.ScreenToWorldPoint(pos0);

		float newPosY = Camera.main.WorldToScreenPoint(pos1_world).y;

		go.Tween("TextParticleShow" + id, 1, 1, 0.4f, TweenScaleFunctions.QuadraticEaseInOut, (t) => {
			// progress
			setAlpha(t.CurrentValue);
		}, (t) => {
			// completion
		});
		
		go.Tween("TextParticleFly" + id, go.GetComponent<RectTransform>().localPosition.y, newPosY, 1.0f, TweenScaleFunctions.Linear, (t) => {
			// progress
			Vector3 pos = pos0;
			//pos.y = t.CurrentValue;
			//go.GetComponent<RectTransform>().localPosition = pos;
		}, (t) => {
			// completion
			destroyText(go);
		});

	}

	public void setAlpha(float alpha) {
		Color tmpCol = textElement.color;
		tmpCol.a = alpha;
		textElement.color = tmpCol;
	}

	private void destroyText(GameObject go) {
		go.Tween("TextParticleHide" + id, 1, 0, 0.2f, TweenScaleFunctions.QuadraticEaseInOut, (t) => {
			// progress
			setAlpha(t.CurrentValue);
		}, (t) => {
			// completion
			Object.Destroy(go);
		});
	}
}
