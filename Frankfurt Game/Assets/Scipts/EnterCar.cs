using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterCar : MonoBehaviour
{
    public MonoBehaviour car;
    public GameObject carCamera;
    public GameObject player;

    public bool enter = false;
    public bool inCar = false;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("F key pressed");
            if (enter && !inCar)
            {
                Debug.Log("Entering car");
                car.enabled = true;
                carCamera.SetActive(true);
                player.SetActive(false);

                inCar = true;
            }
            else if (inCar)
            {
                Debug.Log("Exiting car");
                car.enabled = false;
                carCamera.SetActive(false);
                player.SetActive(true);

                inCar = false;
            }
        }
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
