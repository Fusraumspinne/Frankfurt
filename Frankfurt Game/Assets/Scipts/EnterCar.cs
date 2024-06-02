using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterCar : MonoBehaviour
{
    public Rigidbody body;
    public WheelColliders colliders;

    public MonoBehaviour car;
    public GameObject player;

    public GameObject playerObject;

    public bool enter = false;
    public bool inCar = false;

    public GameObject driver;

    public GameObject spawnPoint;

    public Camera firstPerson;
    public Camera thirdPerson;

    public AudioListener firstPersonAudio;
    public AudioListener thirdPersonAudio;


    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("F key pressed");
            if (enter && !inCar)
            {
                Debug.Log("Entering car");
                car.enabled = true;
                firstPerson.enabled = true;
                firstPersonAudio.enabled = true;
                thirdPerson.enabled = false;
                thirdPersonAudio.enabled = false;
                player.SetActive(false);

                inCar = true;

                driver.SetActive(true);
            }
            else if (inCar)
            {
                Debug.Log("Exiting car");
                car.enabled = false;
                firstPerson.enabled = false;
                firstPersonAudio.enabled = false;    
                thirdPerson.enabled = false;
                thirdPersonAudio.enabled = false;

                playerObject.transform.localPosition = Vector3.zero;

                player.transform.position = spawnPoint.transform.position;

                player.SetActive(true);

                inCar = false;

                driver.SetActive(false);

                StopCar();
            }
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            if(inCar)
            {
                if (firstPerson.enabled == true)
                {
                    thirdPerson.enabled = true; 
                    thirdPersonAudio.enabled = true;
                    firstPerson.enabled = false;
                    firstPersonAudio.enabled = false;
                } 
                else
                {
                    firstPerson.enabled = true;
                    firstPersonAudio.enabled = true;
                    thirdPerson.enabled = false;
                    thirdPersonAudio.enabled= false;

                }
            }
        }
    }

    void StopCar()
    {
        colliders.FRWheel.motorTorque = 0;
        colliders.FLWheel.motorTorque = 0;
        colliders.RRWheel.motorTorque = 0;
        colliders.RLWheel.motorTorque = 0;

        colliders.FRWheel.brakeTorque = Mathf.Infinity;
        colliders.FLWheel.brakeTorque = Mathf.Infinity;
        colliders.RRWheel.brakeTorque = Mathf.Infinity;
        colliders.RLWheel.brakeTorque = Mathf.Infinity;

        body.velocity = Vector3.zero;
        body.angularVelocity = Vector3.zero;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enter = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enter = false;
        }
    }
}