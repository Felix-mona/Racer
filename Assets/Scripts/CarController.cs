using System.Collections;
using System.Collections.Generic;
using System.Net.Security;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class CarController : MonoBehaviour
{
    //All the WheelCollider objects from the car
    public WheelCollider frontLC;
    public WheelCollider frontRC;
    public WheelCollider backLC;
    public WheelCollider backRC;


    //All the Transform objects from the wheels - used in the spinning and animation of the Wheels
    public Transform frontLT;
    public Transform frontRT;
    public Transform backLT;
    public Transform backRT;

    //set values for the characteristics of how the car will drive
    public float acceleration;
    public float brakingForce;
    public float maxTurnAngle;

    //the current variables of the car, these will be changed when the car is moving
    private float currentAcceleration;
    private float currentBrakeForce;
    private float currentTurnAngle;
    private float vAxis;
    private float hAxis;






    public void FixedUpdate()
    {
        //while the start timer has finished and the game hasn't ended, but the code for seeing if the game has ended hasn't been made yet
        UserInput();
        //if the controls are NOT frozen then the car should be able to move
        DriveHandling();
        TurnHandling();

        //update the wheels 
        UpdateWheel(frontLC, frontLT);
        UpdateWheel(frontRC, frontRT);
        UpdateWheel(backLC, backLT);
        UpdateWheel(backRC, backRT);
    }

    public void UserInput()
    {
        //gets the input values from the user inputs, uses w,s or up and down keys
        vAxis = Input.GetAxis("Vertical");
        //gets the input values from the user inputs, uses a,d or left and right keys
        hAxis = Input.GetAxis("Horizontal");
    }

    public void DriveHandling()
    {

        //sets the new acceleration of the car
        currentAcceleration = acceleration * vAxis;
        frontLC.motorTorque = currentAcceleration;
        frontRC.motorTorque = currentAcceleration;

        //Tells if the space key is being pressed, if it is then apply the brakes
        if (Input.GetKey(KeyCode.Space))
        {
            currentBrakeForce = brakingForce;
        }
        else
        {
            currentBrakeForce = 0f;
        }
        frontLC.brakeTorque = currentBrakeForce;
        frontRC.brakeTorque = currentBrakeForce;
        backLC.brakeTorque = currentBrakeForce;
        backRC.brakeTorque = currentBrakeForce;


    }

    public void TurnHandling()
    {
        //set shte angles of the wheels so that the user is able to turn
        currentTurnAngle = maxTurnAngle * hAxis;
        frontLC.steerAngle = currentTurnAngle;
        frontRC.steerAngle = currentTurnAngle;
    }

    public void UpdateWheel(WheelCollider wColl, Transform wTrans)
    {

        Vector3 position;
        Quaternion rotation;
        wColl.GetWorldPose(out position, out rotation);

        //setting the new rotations and positions of the wheel colliders
        wTrans.position = position;
        wTrans.rotation = rotation;
    }
}
