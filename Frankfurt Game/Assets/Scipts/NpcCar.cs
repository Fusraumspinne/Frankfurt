using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcCar : MonoBehaviour
{
    public Rigidbody body;
    public WheelColliders colliders;
    public WheelMeshes meshes;
    public float gasInput;
    public float brakeInput;
    public float steeringInput;

    public float motorPower;
    public float brakePower;
    public float slipAngle;
    public float speed;
    public AnimationCurve steeringCurve;

    public Transform[] waypoints;
    private int currentWaypointIndex = 0;

    private void Update()
    {
        speed = body.velocity.magnitude;
        ApplyWheelPosition();
        FollowWaypoints();
        ApplyMotor();
        ApplySteering();
        ApplyBrake();
    }

    void FollowWaypoints()
    {
        if (waypoints.Length == 0)
            return;

        Transform currentWaypoint = waypoints[currentWaypointIndex];
        Vector3 target = currentWaypoint.position;
        Vector3 direction = target - transform.position;
        direction.y = 0; // Ignore the height difference

        // Get the angle between the forward direction and the target direction
        float angle = Vector3.SignedAngle(transform.forward, direction, Vector3.up);
        steeringInput = Mathf.Clamp(angle / 45f, -1f, 1f);

        if (direction.magnitude < 1f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }

        // Check if the waypoint has specific tags
        if (currentWaypoint.CompareTag("Stop"))
        {
            gasInput = 0f; // Stop the car
            brakeInput = 1f; // Apply full brakes
        }
        else if (currentWaypoint.CompareTag("Kurve"))
        {
            gasInput = 0.1f; // Reduce speed for the curve
            brakeInput = 0f; // Apply some brakes
        }
        else if (currentWaypoint.CompareTag("VorKurve"))
        {
            gasInput = 0.1f; // Reduce speed for the curve
            brakeInput = 0.003f; // Apply some brakes
        }
        else if (currentWaypoint.CompareTag("Gerade"))
        {
            gasInput = 0.3f; // Increase speed on straight paths
            brakeInput = 0f; // No braking
        }
        else
        {
            gasInput = 0.3f; // Default acceleration
            brakeInput = 0f; // No braking for now
        }
    }



    void ApplyBrake()
    {
        colliders.FRWheel.brakeTorque = brakeInput * brakePower * 0.7f;
        colliders.FLWheel.brakeTorque = brakeInput * brakePower * 0.7f;

        colliders.RRWheel.brakeTorque = brakeInput * brakePower * 0.3f;
        colliders.RLWheel.brakeTorque = brakeInput * brakePower * 0.3f;
    }

    void ApplyMotor()
    {
        colliders.RRWheel.motorTorque = motorPower * gasInput;
        colliders.RLWheel.motorTorque = motorPower * gasInput;
    }

    void ApplySteering()
    {
        float steeringAngle = steeringInput * steeringCurve.Evaluate(speed);
        colliders.FRWheel.steerAngle = steeringAngle;
        colliders.FLWheel.steerAngle = steeringAngle;
    }

    void ApplyWheelPosition()
    {
        UpdateWheel(colliders.FRWheel, meshes.FRWheel);
        UpdateWheel(colliders.FLWheel, meshes.FLWheel);
        UpdateWheel(colliders.RRWheel, meshes.RRWheel);
        UpdateWheel(colliders.RLWheel, meshes.RLWheel);
    }

    void UpdateWheel(WheelCollider collider, MeshRenderer renderer)
    {
        Quaternion quat;
        Vector3 position;

        collider.GetWorldPose(out position, out quat);
        renderer.transform.position = position;
        renderer.transform.rotation = quat;
    }
}
