using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public sealed class CustomEvent {
	public enum MessageType {
		INFO
	}

	public delegate void MessageCallback(string title, Sprite iconSprite, string message, MessageType mtype);
	public static event MessageCallback messageCallback;

	public static void SendMessage(string title, Sprite iconSprite, string message, MessageType mtype) {
		if (messageCallback != null)
			messageCallback(title, iconSprite, message, mtype);
	}

	static readonly CustomEvent _instance = new CustomEvent();
	public static CustomEvent Instance {
		get {
			return _instance;
		}
	}

	private CustomEvent() {

	}
}
