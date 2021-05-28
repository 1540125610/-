using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardsSystem : MonoBehaviour
{
    public GameObject resourceSystem;
    public GameObject rewards;
    public int coins;
    public int minerals;
    public int humen;

    public Text coinsText;
    public Text mineralsText;
    public Text humenText;

    public GameObject human;
    public GameObject player1;
    void Start()
    {
        StartCoroutine("Rewards");
    }

    
    void Update()
    {
        coinsText.text = "获得金币" + coins;
        mineralsText.text = "获得矿石" + minerals;
        humenText.text = "获得士兵" + humen;
    }

    IEnumerator Rewards()    //每10秒给玩家一次奖励
    {
        while(true)
        {
            yield return new WaitForSeconds(10f);
            RandomRewards();
            rewards.SetActive(true);
        }
    }
    public void RandomRewards()     //随机奖励数量
    {
        int x = Random.Range(1, 3);
        coins = x * 100;
        int y = Random.Range(1, 3);
        minerals = y * 100;
        humen = Random.Range(1, 3);
    }

    public void CoinsRewards()    //金币奖励
    {
        resourceSystem.GetComponent<ResourceSystem>().coins += coins;
        rewards.SetActive(false);
    }

    public void MineralsRewards()   //矿石奖励
    {
        resourceSystem.GetComponent<ResourceSystem>().minerals += minerals;
        rewards.SetActive(false);
    }

    public void HumenRewards()    //士兵奖励
    {
        for (int i=0;i<humen;i++)
        {
            GameObject human_ = Instantiate(human, new Vector3(0, -1, 0), Quaternion.identity);
            human_.transform.parent = player1.transform;
            human_.GetComponent<HumanControl>().playerName = "Player1";
        }
        rewards.SetActive(false);
    }
}
