using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour {
    public Image iconImage;
    public Text textMain;

    private GameObject mainGO;

    void Start() {
        mainGO = this.gameObject;
        hide();
    }

    void Update() {

    }

    public void show() {
        mainGO.SetActive(true);
    }

    public void hide() {
        mainGO.SetActive(false);
    }

    public void setPosition2D(Vector2 pos) {
        mainGO.transform.position = pos;
    }

    public void setIcon(Sprite sprite) {
        iconImage.sprite = sprite;
    }

    public void setText(string str) {
        textMain.text = str;
    }
}
