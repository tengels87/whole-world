using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
	public GameObject cameraTarget;
	public float height = 6.0f;
	public float distance = 5.0f;
	public float smoothing = 10.0f;

	private Vector3 lastPosition;
	private Vector3 offset;

	private void Start() {
		if (!cameraTarget) {
			Debug.LogError("No target selected for Camera.");
		} else {
			SetStartPosition();
		}
	}

	private void SetStartPosition() {
		offset = new Vector3(cameraTarget.transform.position.x,
			cameraTarget.transform.position.y + height,
			cameraTarget.transform.position.z - distance);

		lastPosition = new Vector3(cameraTarget.transform.position.x,
			cameraTarget.transform.position.y + height,
			cameraTarget.transform.position.z - distance);

		transform.position = lastPosition;
	}

	private void Update() {
		if (!cameraTarget) { return; }

		Follow();
	}

	private void Follow() {

		transform.position = new Vector3(Mathf.Lerp(lastPosition.x, cameraTarget.transform.position.x, smoothing * Time.deltaTime),
			Mathf.Lerp(lastPosition.y, cameraTarget.transform.position.y + height, smoothing * Time.deltaTime),
			Mathf.Lerp(lastPosition.z, cameraTarget.transform.position.z - distance, smoothing * Time.deltaTime));

		// Set cameraTargetOffset as cameraTarget + cameraTargetOffsetY.
		float cameraTargetOffsetY = 2f;
		Vector3 cameraTargetOffset = cameraTarget.transform.position + new Vector3(0, cameraTargetOffsetY, 0);

		// Smoothly look at cameraTargetOffset.
		transform.rotation = Quaternion.Slerp(transform.rotation,
			Quaternion.LookRotation(cameraTargetOffset - transform.position),
			Time.deltaTime * smoothing);
	}

	private void LateUpdate() { lastPosition = transform.position; }
}
