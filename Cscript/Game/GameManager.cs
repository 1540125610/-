using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public MakeEnemy makeEnemy;
    public int killedEnemies;
    // Start is called before the first frame update
    void Start()
    {
        killedEnemies = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(makeEnemy.enemyCount==20)
        {
            GameOver();
        }
    }
    public void GameOver()
    {
        SceneManager.LoadScene("GameOver");
    }
}
