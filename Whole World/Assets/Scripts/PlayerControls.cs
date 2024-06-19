using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour {
    public RPGCharacterController charController;
    public Transform targetItem;

    void Start() {

    }

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            //walkToMousePos();
        }
    }

    private void walkToMousePos() {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, float.PositiveInfinity, LayerMask.GetMask("Terrain"))) {
            charController.navMeshAgent.destination = hit.point;
        }
    }
}
