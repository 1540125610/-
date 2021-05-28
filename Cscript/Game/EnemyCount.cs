using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyCount : MonoBehaviour
{
    public GameObject makeEnemy;
    public Text enemyCount;
    // Update is called once per frame
    void Update()
    {
        enemyCount.text = "敌人数量：" + makeEnemy.GetComponent<MakeEnemy>().enemyCount;
    }
}
