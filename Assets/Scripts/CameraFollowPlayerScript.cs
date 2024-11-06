using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.AnimatedValues;
using UnityEngine;

public class CameraFollowPlayerScript : MonoBehaviour
{
    //object that the camera follows
    [SerializeField] GameObject playerCar;

    //how far back the camera will sit when it has  caught up to the car
    public Vector3 offset;

    //Two values that are used for the smoothing and speed of the camera's movement
    public float smoothSpeed = 0.125f;
    public float followSpeed = 5.0f;
    private Vector3 velocity = Vector3.zero;

    // Update is called once per frame
    void Update()
    {
        //find the instantiated car prefab
        playerCar = GameObject.FindGameObjectWithTag("Player Car");
        //Get the 'Desired' position of the car ready to move the camera's position to it
        Vector3 desiredPosition = playerCar.transform.position - playerCar.transform.forward * 5.0f + playerCar.transform.up * 2.0f;
        //transform the desired position to make the camera's vector move with a smoother movement
        Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothSpeed, followSpeed);
        //Move the camera and change the way it is facing (at the car)
        transform.position = smoothedPosition;
        transform.LookAt(playerCar.transform);
    }

}
