using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;


public class Notification : MonoBehaviour
{
    private int Reward;
    private float Timer;
    private string AdressName = "";

    public GameObject amount;
    public GameObject timer;
    public GameObject address;

    public bool Pizza;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Timer > 0)
        {
            Timer -= Time.deltaTime;
        }
        string minutes = TimeSpan.FromSeconds((((int)Timer) + 1)).Minutes.ToString();
        int secondsInt = TimeSpan.FromSeconds((((int)Timer) + 1)).Seconds;

        string seconds = "";
        if (secondsInt < 10)
        {
            seconds = ("0" + secondsInt.ToString());
        }
        else
        {
            seconds = secondsInt.ToString();
        }
        timer.GetComponent<TextMeshProUGUI>().text = (minutes + ":" + seconds);
        amount.GetComponent<TextMeshProUGUI>().text = (((int)(Reward + Timer)).ToString() + "$");
    }

    public void setValues(int reward, int time, string addressName)
    {
        Reward = reward;
        Timer = time;
        AdressName = addressName;
        amount.GetComponent<TextMeshProUGUI>().text = (((int)(Reward + time)).ToString() + "$");
        if (Pizza)
        {
            address.GetComponent<TextMeshProUGUI>().text = AdressName;
        }
        string minutes = TimeSpan.FromSeconds((((int)Timer) + 1)).Minutes.ToString();
        int secondsInt = TimeSpan.FromSeconds(Timer).Seconds;

        string seconds = "";
        if (secondsInt < 10)
        {
            seconds = ("0" + secondsInt.ToString());
        }
        else
        {
            seconds = secondsInt.ToString();
        }

        timer.GetComponent<TextMeshProUGUI>().text = (minutes + ":" + seconds);
    }
}
