using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private CharacterController cc;
    private Transform cameraTransform;
    private Vector3 movePosition;
    private float movementSpeed = 10f;
    private Vector3 finalPosition;
    private Vector3 direction;

    private const float gravitationalConstant = 9.81f;
    private float timeInAir = 0f;
    private float jumpHeight = 400f;
    private Vector3 jumpForce = new Vector3(0, 0, 0);
    private Vector3 verticalForce;

    private void Awake()
    {
        timeInAir = 0f;
        cc = gameObject.GetComponent<CharacterController>();
        cameraTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
    }
    private void Update()
    {
        movePosition = new Vector3(0, 0, 0);
        if(Input.GetKey("w"))
        {
            movePosition += Vector3.forward;
        }
        if (Input.GetKey("s"))
        {
            movePosition += Vector3.back;
        }
        if (Input.GetKey("d"))
        {
            movePosition += Vector3.right;
        }
        if (Input.GetKey("a"))
        {
            movePosition += Vector3.left;
        }
        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            movementSpeed *= 2;
        }
        if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            movementSpeed /= 2;
        }
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }


        ApplyVerticalForce();
        CalcFinalPosition();
        MoveCharacter();
    }


    void Jump()
    {
        cc.Move(new Vector3(0, -0.01f, 0)) ;
        if(cc.isGrounded)
        {
            Debug.Log("Jumping");
            jumpForce.y = jumpHeight;
        }
    }

    void ApplyVerticalForce()
    {
        if (cc.isGrounded)
            timeInAir = 0;
        else
         timeInAir += Time.deltaTime;

        verticalForce = new Vector3(0, 0, 0);
        verticalForce.y = (-gravitationalConstant * timeInAir) + (jumpForce.z * timeInAir);
        cc.Move(verticalForce);

    }

    void CalcFinalPosition()
    {
        Quaternion cameraMoveRotation = Quaternion.Euler(0, cameraTransform.rotation.eulerAngles.y, 0);
        finalPosition = cameraMoveRotation * movePosition;
    }
    void RotateCharacter()
    {
        if (movePosition != Vector3.zero)
        {
            direction = (transform.position + finalPosition) - (transform.position);
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }

    void MoveCharacter()
    {
        RotateCharacter();
        cc.Move(finalPosition * Time.deltaTime * movementSpeed);
    }

    
}
