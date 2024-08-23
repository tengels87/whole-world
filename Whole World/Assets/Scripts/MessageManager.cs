using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DigitalRuby.Tween;

public class MessageManager : MonoBehaviour {
	public Object messagePrefab;
	public Transform container;

	public List<GameObject> messageList = new List<GameObject>();

    void OnEnable() {
        CustomEvent.messageCallback += showMessage;
    }

    void OnDisable() {
        CustomEvent.messageCallback -= showMessage;
    }

	void Update() {
		/*
		if (Input.GetKeyDown(KeyCode.Space)) {
			CustomEvent.SendMessage("First Message", null, "sjfsasajfsa jsafsa flsaj sajlfjsaifsai isajfsj sfa", CustomEvent.MessageType.INFO);
		}
		*/
	}

	private void showMessage(string title, Sprite iconSprite, string message, CustomEvent.MessageType mtype) {
		GameObject go = (GameObject)Object.Instantiate(messagePrefab);

		go.transform.SetParent(container, false);

		MessageItem mScript = go.GetComponent<MessageItem>();

		mScript.init(title, iconSprite, message, mtype);

		float newPosY = - messageList.Count * mScript.size.y;

		messageList.Add(go);

		go.Tween("MessageShow" + mScript.id, 1, 1, 5.0f, TweenScaleFunctions.QuadraticEaseInOut, (t) => {
			// progress
			mScript.setAlpha(t.CurrentValue);
		}, (t) => {
			// completion
			removeMessage(go);
		});

		go.Tween("MessageFly" + mScript.id, go.GetComponent<RectTransform>().localPosition.y, newPosY, 1.0f, TweenScaleFunctions.QuadraticEaseInOut, (t) => {
			// progress
			Vector3 pos = go.GetComponent<RectTransform>().localPosition;
			pos.y = t.CurrentValue;
			go.GetComponent<RectTransform>().localPosition = pos;
		}, (t) => {
			// completion
		});
	}

	private void removeMessage(GameObject g) {
		messageList.Remove(g);

		// fade
		g.Tween("MessageHide" + g.GetComponent<MessageItem>().id, 1, 0, 0.4f, TweenScaleFunctions.QuadraticEaseInOut, (t) => {
			// progress
			g.GetComponent<MessageItem>().setAlpha(t.CurrentValue);
		}, (t) => {
			// completion
			Object.Destroy(g);
		});

		// wrap up
		for (int i=0; i<messageList.Count; i++) {
			float oldPosY = messageList[i].GetComponent<RectTransform>().localPosition.y;
			float newPosY = oldPosY + messageList[i].GetComponent<MessageItem>().size.y;
			GameObject goo = messageList[i];

			goo.Tween("MessageWrap" + goo.GetComponent<MessageItem>().id, oldPosY, newPosY, 0.5f, TweenScaleFunctions.QuadraticEaseInOut, (t) => {
                // progress
                if (goo != null) {
                    Vector3 pos = goo.GetComponent<RectTransform>().localPosition;
                    pos.y = t.CurrentValue;
                    goo.GetComponent<RectTransform>().localPosition = pos;
                }
			}, (t) => {
				// completion
			});
		}
	}
}
