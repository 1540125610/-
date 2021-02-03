using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    private string playerName;
    void Start()
    {
        playerName = GetComponentInParent<HumanControl>().playerName;

    }
    private void OnTriggerEnter(Collider other)
    {
        try
        {
            string otherPlayer = other.GetComponentInParent<HumanControl>().playerName;
            if (otherPlayer != playerName)
            {
                GetComponentInParent<HumanControl>().SetEnemy(other.gameObject);
            }
        }
        catch
        {

        }
    }
    private void OnTriggerExit(Collider other)
    {
        try
        {
            string otherPlayer = other.GetComponentInParent<HumanControl>().playerName;
            if (otherPlayer != playerName)
            {
                GetComponentInParent<HumanControl>().SetPursue();
            }
        }
        catch
        {

        }
    }
}
