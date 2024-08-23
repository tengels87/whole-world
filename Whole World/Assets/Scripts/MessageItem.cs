using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageItem : MonoBehaviour {
	public Image backdropElement;
	public Text titleElement;
	public Image iconElement;
	public Text messageElement;
	[HideInInspector]
	public string id;
	[HideInInspector]
	public Vector2 size;

	void Awake() {
		id = "" + Time.time + Random.value * 1000000;
		size = backdropElement.gameObject.GetComponent<RectTransform>().sizeDelta;
	}

	void Update() {
		
	}

	public void init(string title, Sprite iconSprite, string message, CustomEvent.MessageType mtype) {
		titleElement.text = title;

		if (mtype == CustomEvent.MessageType.INFO)
			titleElement.color = Color.yellow;

		messageElement.text = message;
		iconElement.sprite = iconSprite;
	}

	public void setAlpha(float alpha) {
		Color tmpCol = titleElement.color;
		tmpCol.a = alpha;
		titleElement.color = tmpCol;

		tmpCol = messageElement.color;
		tmpCol.a = alpha;
		messageElement.color = tmpCol;

		tmpCol = iconElement.color;
		tmpCol.a = alpha;
		iconElement.color = tmpCol;

		tmpCol = backdropElement.color;
		tmpCol.a = alpha;
		backdropElement.color = tmpCol;
	}
}
