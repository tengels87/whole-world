using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour {
    public Image image_hp;

    private GameObject targetGO;
    private float lastSpawnTime;

    void Start() {

    }

    void Update() {
        if (Time.time <= lastSpawnTime + 5.0f) {
            this.transform.position = Camera.main.WorldToScreenPoint(targetGO.transform.position + Vector3.forward * 5);
        } else {
            hide();
        }
    }

    public void show(GameObject target) {
        lastSpawnTime = Time.time;

        this.targetGO = target;
        this.gameObject.SetActive(true);
    }

    public void hide() {
        lastSpawnTime = 0;

        this.gameObject.SetActive(false);
    }

    public void setFillAmount(float floatVal) {
        image_hp.fillAmount = floatVal;
    }
}
