using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public Transform playerTransform;

    private Vector3 _cameraOffset;

    [Range(0.01f, 1.0f)]
    public float smoothFactor = 0.5f;

    public bool lookAtPlayer = false;

    public bool rotateAroundPlayer = true;
    public float rotationSpeed = 5.0f;

    void Start() {
        _cameraOffset = transform.position - playerTransform.position;
    }

    void LateUpdate() {

        if (rotateAroundPlayer) {
            Quaternion camTurnAngle =
                Quaternion.AngleAxis(Input.GetAxis("Mouse X") * rotationSpeed, Vector3.up);

            _cameraOffset = camTurnAngle * _cameraOffset;
        }

        Vector3 newPos = playerTransform.position + _cameraOffset;

        transform.position = Vector3.Slerp(transform.position, newPos, smoothFactor);

        if (lookAtPlayer || rotateAroundPlayer)
            transform.LookAt(playerTransform);
    }


}
