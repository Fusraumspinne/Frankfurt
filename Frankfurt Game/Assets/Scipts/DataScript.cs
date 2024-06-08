using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DataScript : MonoBehaviour
{
    public int money;
    public TMP_Text textMoney;

    public void Update()
    {
        if (PlayerPrefs.HasKey("Money"))
        {
            money = PlayerPrefs.GetInt("Money");

            textMoney.text = money.ToString();
        }
    }
}
