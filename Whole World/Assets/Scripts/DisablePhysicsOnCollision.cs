using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisablePhysicsOnCollision : MonoBehaviour {
    private Rigidbody rb;

    void Start() {
        rb = this.gameObject.GetComponent<Rigidbody>();
    }

    void Update() {

    }

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Terrain")) {
            rb.useGravity = false;
            rb.isKinematic = true;
        }
    }
}
