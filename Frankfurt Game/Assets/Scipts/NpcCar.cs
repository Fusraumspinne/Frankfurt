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

    public Transform initialWaypoint;
    private Transform currentWaypoint;

    private void Start()
    {
        currentWaypoint = initialWaypoint;
    }

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
        if (currentWaypoint == null)
            return;

        Vector3 target = currentWaypoint.position;
        Vector3 direction = target - transform.position;
        direction.y = 0; 

        float angle = Vector3.SignedAngle(transform.forward, direction, Vector3.up);
        steeringInput = Mathf.Clamp(angle / 45f, -1f, 1f);

        if (direction.magnitude < 1f)
        {
            Waypoint waypointComponent = currentWaypoint.GetComponent<Waypoint>();
            if (waypointComponent != null && waypointComponent.nextWaypoints.Count > 0)
            {
                int randomIndex = UnityEngine.Random.Range(0, waypointComponent.nextWaypoints.Count);
                Transform nextWaypoint = waypointComponent.nextWaypoints[randomIndex].transform;

                currentWaypoint = nextWaypoint;
            }
        }

        if (currentWaypoint.CompareTag("Stop"))
        {
            gasInput = 0f; 
            brakeInput = 1f; 
        }
        else if (currentWaypoint.CompareTag("Kurve"))
        {
            gasInput = 0.1f; 
            brakeInput = 0f; 
        }
        else if (currentWaypoint.CompareTag("VorKurve"))
        {
            gasInput = 0.1f; 
            brakeInput = 0.003f;
        }
        else if (currentWaypoint.CompareTag("Gerade"))
        {
            gasInput = 0.3f; 
            brakeInput = 0f; 
        }
        else
        {
            gasInput = 0.3f; 
            brakeInput = 0f; 
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
