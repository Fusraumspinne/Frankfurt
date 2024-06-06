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

    public bool crash;

    public float cooldownTime;
    private float lastTriggerTime;
    public bool stop‹berschreiben;
    public bool inVorKurve;
    public bool isTiming;

    private void Start()
    {
        initialWaypoint = FindNearestWaypointWithTag("Gerade");
        currentWaypoint = initialWaypoint;
    }

    Transform FindNearestWaypointWithTag(string tag)
    {
        GameObject[] waypoints = GameObject.FindGameObjectsWithTag(tag);
        Transform nearestWaypoint = null;
        float nearestDistance = Mathf.Infinity;

        foreach (GameObject waypoint in waypoints)
        {
            float distance = Vector3.Distance(transform.position, waypoint.transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestWaypoint = waypoint.transform;
            }
        }

        return nearestWaypoint;
    }

    private void Update()
    {
        speed = body.velocity.magnitude;
        ApplyWheelPosition();
        FollowWaypoints();
        ApplyMotor();
        ApplySteering();
        ApplyBrake();

        if (Time.time > lastTriggerTime + cooldownTime && inVorKurve)
        {
            stop‹berschreiben = true;
        }
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
            inVorKurve = false;
            isTiming = false;
            stop‹berschreiben = false;
        }
        else if (currentWaypoint.CompareTag("Kurve"))
        {
            if (!crash)
            {
                gasInput = 0.1f;
                brakeInput = 0f;
                inVorKurve = false;
                isTiming = false;
                stop‹berschreiben = false;
            }
        }
        else if (currentWaypoint.CompareTag("Bergauf"))
        {
            if (!crash)
            {
                gasInput = 0.4f;
                brakeInput = 0f;
                inVorKurve = false;
                isTiming = false;
                stop‹berschreiben = false;
            }
        }
        else if (currentWaypoint.CompareTag("Steil"))
        {
            if (!crash)
            {
                gasInput = 1.25f;
                brakeInput = 0f;
                inVorKurve = false;
                isTiming = false;
                stop‹berschreiben = false;
            }
        }
        else if (currentWaypoint.CompareTag("VorKurve"))
        {
            inVorKurve = true;

            if (!crash)
            {
                if (!stop‹berschreiben)
                {
                    gasInput = 0f;
                    brakeInput = 0.1f;

                    if (!isTiming)
                    {
                        isTiming = true;
                        lastTriggerTime = Time.time;
                    }
                }
                else
                {
                    gasInput = 0.1f;
                    brakeInput = 0f;
                }
            }
        }
        else if (currentWaypoint.CompareTag("Gerade"))
        {
            if (!crash)
            {
                gasInput = 0.15f;
                brakeInput = 0f;
                inVorKurve = false;
                isTiming = false;
                stop‹berschreiben = false;
            }
        }
        else
        {
            if (!crash)
            {
                gasInput = 0.15f;
                brakeInput = 0f;
                inVorKurve = false;
                isTiming = false;
                stop‹berschreiben = false;
            }
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

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Car"))
        {
            crash = true;
            brakeInput = 1f;
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Car"))
        {
            crash = true;
            brakeInput = 1f;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Car"))
        {
            crash = false;
            brakeInput = 0f;
        }
    }
}
