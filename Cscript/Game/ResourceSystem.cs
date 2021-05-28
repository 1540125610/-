using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceSystem : MonoBehaviour
{
    public int coins;    //金币数

    public int minerals;  //矿石数
    public int human;     //人口
    public Text mineralsCounts;
    public Text coinsCounts;
    public Text humanCounts;

    float i = 0;

    void Start()
    {

    }

    void Update()
    {
        mineralsCounts.text = "矿石:" + minerals;
        coinsCounts.text = "金币:" + coins;

        if (i < 0.5f)
        {
            i += Time.deltaTime;
        }
        else
        {
            coins += 1;
            i = 0;
        }
        
    }
}
