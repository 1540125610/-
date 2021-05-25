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

    void Start()
    {

    }

    void Update()
    {
        mineralsCounts.text = "矿石:" + minerals;
        coinsCounts.text = "金币:" + coins;

    }
}
