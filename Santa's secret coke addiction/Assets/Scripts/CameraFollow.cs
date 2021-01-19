using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform followTarget;
    private float offsetDistance = 2.5f;
    private float currentX = 0f;
    private float currentY = 0f;
    private float mouseSensitivity;
    private Vector3 offset;
    private float yAngleMin = -89.9f;
    private float yAngleMax = 89.9f;
    private Vector3 desiredPosition;
    private float hitOffset;
    private void Awake()
    {
        followTarget = GameObject.FindGameObjectWithTag("Player").transform;
        mouseSensitivity = PlayerPrefs.GetFloat("MouseSensitivity", 1f);
        offset = new Vector3(0, 0, offsetDistance);
    }
    private void Update()
    {

        currentX += Input.GetAxis("Mouse X") * Time.deltaTime * mouseSensitivity * 600;
        if (currentX < -360)
            currentX += 360;
        else if (currentX > 360)
            currentX -= 360;

        currentY += Input.GetAxis("Mouse Y") * Time.deltaTime * mouseSensitivity * 600;
        currentY = Mathf.Clamp(currentY, yAngleMin, yAngleMax);
    }
    private void LateUpdate()
    {
        offsetDistance = 2.5f;
        MoveCamera();
        transform.LookAt(followTarget);
        DebugMovement();
    }

    void DebugMovement()
    {
        if(offsetDistance < 0.1f)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<Movement>().enabled = false;
            this.enabled = false;
        }
    }
    void MoveCamera()
    {
        offset = new Vector3(0, 0, offsetDistance);
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        desiredPosition = followTarget.position + rotation * offset;
        CheckCamera(desiredPosition);
        offset = new Vector3(0, 0, offsetDistance);
        rotation = Quaternion.Euler(currentY, currentX, 0);
        desiredPosition = followTarget.position + rotation * offset;
        transform.position = desiredPosition;
    }

    void CheckCamera(Vector3 wantedPosition)
    {
        RaycastHit hit;
        Vector3 direction = wantedPosition - followTarget.position;
        if (Physics.Raycast(followTarget.position, direction, out hit, maxDistance: 2.5f) && hit.collider.tag != "Player")
        {
            offsetDistance = hit.distance - 0.1f;      
        }
        else
        {
            offsetDistance = 2.5f;
        }
    }
}
