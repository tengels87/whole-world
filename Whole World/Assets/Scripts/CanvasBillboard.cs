using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasBillboard : MonoBehaviour
{
    public Transform targetPos;
    Vector3 offset;

    void Start() {
        offset = transform.position - targetPos.position;
    }
    void Awake() {
        offset = transform.position - targetPos.position;
    }

    void LateUpdate() {
        transform.position = targetPos.position + offset;
        transform.LookAt(Camera.main.transform.position);
        Vector3 rot = transform.rotation.eulerAngles;
        rot.y = 0;
        transform.rotation = Quaternion.Euler(rot.x, rot.y, rot.z);
    }
}
