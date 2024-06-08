using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public Transform carTransform;
    public Rigidbody carRigidbody;
    public float followSpeed = 2;
    public float lookSpeed = 5;
    public float baseDistanceBehindCar = 5.0f; // Base distance behind the car
    public float heightAboveCar = 2.0f;        // Height above the car
    public float maxDistanceBehindCar = 7.0f;  // Max distance when the car is at top speed
    public float speedToMaxDistance = 10.0f;   // Speed at which max distance is reached

    void FixedUpdate()
    {
        // Calculate car speed
        float carSpeed = carRigidbody.velocity.magnitude;

        // Calculate dynamic distance based on speed
        float dynamicDistance = Mathf.Lerp(baseDistanceBehindCar, maxDistanceBehindCar, carSpeed / speedToMaxDistance);

        // Look at car
        Vector3 _lookDirection = carTransform.position - transform.position;
        Quaternion _rot = Quaternion.LookRotation(_lookDirection, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, _rot, lookSpeed * Time.deltaTime);

        // Calculate target position behind the car
        Vector3 offset = -carTransform.forward * dynamicDistance + Vector3.up * heightAboveCar;
        Vector3 _targetPos = carTransform.position + offset;

        // Move to target position
        transform.position = Vector3.Lerp(transform.position, _targetPos, followSpeed * Time.deltaTime);
    }
}
