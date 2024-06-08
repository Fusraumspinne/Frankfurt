using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterBuilding : MonoBehaviour
{
    public GameObject gebäude;
    public GameObject innenRaum;

    public GameObject enter;
    public GameObject leave;

    public bool isColliding;

    public GameObject spawnPointDrinnen;
    public GameObject spawnPointDraussen;

    public GameObject player;

    public GameObject playerObject;

    public bool inHaus;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isColliding = true;

            if(gebäude.activeSelf)
            {
                enter.SetActive(true);
                leave.SetActive(false);
            }
            else
            {
                enter.SetActive(false);
                leave.SetActive(true);
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isColliding = false;

            enter.SetActive(false);
            leave.SetActive(false);
        }
    }

    public void Update()
    {
        if (isColliding)
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                if(!inHaus)
                {
                    inHaus = true;

                    player.SetActive(false);

                    playerObject.transform.localPosition = Vector3.zero;
                    player.transform.position = spawnPointDrinnen.transform.position;

                    player.SetActive(true);

                    gebäude.SetActive(false);
                    innenRaum.SetActive(true);

                    enter.SetActive(false);
                    leave.SetActive(false);
                }
                else
                {
                    inHaus = false;

                    player.SetActive(false);

                    playerObject.transform.localPosition = Vector3.zero;
                    player.transform.position = spawnPointDraussen.transform.position;

                    player.SetActive(true);

                    gebäude.SetActive(true);
                    innenRaum.SetActive(false);

                    enter.SetActive(false);
                    leave.SetActive(false);
                }
            }
        }
    }
}
