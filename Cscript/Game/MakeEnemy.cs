using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeEnemy : MonoBehaviour
{
    public GameObject enemy;
    public GameObject player2;
    public int enemyCount;      //敌人数量
    // Start is called before the first frame update
    void Start()
    {
        enemyCount = 2;
        StartCoroutine("Make");
    }
    IEnumerator Make()           //每2秒在地图内随机地点生成敌人
    {
        while(true)
        {
            yield return new WaitForSeconds(2f);
            float x = Random.Range(-50, 50);
            float z = Random.Range(-50, 50);
            GameObject theEnemy = Instantiate(enemy, new Vector3(x, -1, z), Quaternion.identity);
            theEnemy.transform.parent = player2.transform;
            theEnemy.GetComponent<HumanControl>().playerName = "Player2";
            enemyCount++;
            
        }
    }
}
