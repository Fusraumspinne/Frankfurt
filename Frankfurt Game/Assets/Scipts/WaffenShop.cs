using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Unity.VisualScripting;
using UnityEngine;

public class WaffenShop : MonoBehaviour
{
    public int money;
    public int price;

    public string weaponName;
    public GameObject weapon;

    public string waffeFreigeschaltet;

    public bool isColliding;

    public GameObject e;

    public void Start()
    {
        if (PlayerPrefs.HasKey(weaponName))
        {
            waffeFreigeschaltet = PlayerPrefs.GetString(weaponName);

            if(waffeFreigeschaltet == "true")
            {
                EnableWeapon();
            }
        }
    }

    public void Update()
    {
        if(waffeFreigeschaltet == "true")
        {
            return;
        }

        GetData();
        ChangeData();
    }

    public void GetData()
    {
        if (PlayerPrefs.HasKey("Money"))
        {
            money = PlayerPrefs.GetInt("Money");
        }

        if (PlayerPrefs.HasKey(weaponName))
        {
            waffeFreigeschaltet = PlayerPrefs.GetString(weaponName);
        }
    }

    public void ChangeData()
    {
        if (Input.GetKeyDown(KeyCode.E) &&  isColliding)
        {
            if(money >= price)
            {
                int newMoney = money - price;

                PlayerPrefs.SetInt("Money", newMoney);
                PlayerPrefs.SetString(weaponName, "true");
                PlayerPrefs.Save();

                EnableWeapon();
            }
        }
    }

    public void EnableWeapon()
    {
        weapon.SetActive(true);
        e.SetActive(false);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isColliding = true;

            if (waffeFreigeschaltet == "false")
            {
                e.SetActive(true);
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isColliding = false;

            if (waffeFreigeschaltet == "false")
            {
                e.SetActive(false);
            }
        }
    }
}
