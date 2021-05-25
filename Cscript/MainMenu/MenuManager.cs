using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public Animator ani_1;
    public Animator ani_2;
    public void StartGame()      //开始游戏进入游戏场景
    {
        SceneManager.LoadScene("Game");
    }
    public void BeforeStart()     //先播放角色战斗动画再开始游戏
    {
        ani_1.SetBool("isAttack", true);
        ani_2.SetBool("isAttack", true);
        Invoke("StartGame", 2f);
    }
    public void ExitGame()       //离开游戏关闭软件
    {
        Application.Quit();
    }
}
