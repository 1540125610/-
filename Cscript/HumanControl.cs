﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class HumanControl : MonoBehaviour
{
    private Camera mainCamara;
    private Canvas canvas;
    private Vector3 aimPosition;            //目标地点
    private GridsControl gridsControl;      //导航脚本

    public GameObject currentEnemy;  //当前攻击目标
    public string playerName;  //所属玩家名称
    public bool isAttack=false;

    public int attack;      //攻击力
    public int maxHp;      //最大生命值
    public int moveSpeed;       //移动速度
    public int turningSpeed;    //转身速度
    private int currentHp;      //当前生命值


    public int mapIndex=-1;
    public int dir = 0;


    private Animator ani;
     enum state
    {
        Stand,    //待机
        Move,     //移动
        Pursue,   //追击
        Attack,   //攻击
        Die,      //阵亡
    }
    state humanState;

    void Start()
    {
        gridsControl = GameObject.Find("Grids Control").GetComponent<GridsControl>();

        ani = GetComponent<Animator>();
        mainCamara = Camera.main;
        //拿到是否被选中的圈，并关闭它
        canvas = GetComponentInChildren<Canvas>();
        canvas.gameObject.SetActive(false);

        humanState = state.Stand;    //初始状态为待机
        currentHp = maxHp;          //默认初始生命值最大
        moveSpeed = 1;                  //移动速度默认为1
        turningSpeed = 1; ;             //转身速度默认为1
    }

    
    void Update()
    {
        switch (humanState)
        {
            case state.Stand:       //待机
                Stand();
                break;
            case state.Pursue:      //追击
                Pursue();
                break;
            case state.Move:        //移动
                Move();
                break;
            case state.Attack:      //攻击
                Attack();
                break;
            case state.Die:         //死亡
                Die();
                break;
        }
        if(Input.GetKeyDown(KeyCode.S))    //按下“S”键进入待机状态
        {
            StopCoroutine("OnAttack");
            isAttack=false;
            humanState = state.Stand;
        }

        if(currentHp<=0)
        {
            humanState = state.Die;
        }

        if(currentEnemy ==null)      //攻击目标死亡时或者没有攻击目标停止攻击
        {
            StopCoroutine("OnAttack");
        }
        
    }



    //开启选框
    public void OnSelected(Color color)
    {
        canvas.GetComponentInChildren<Image>().color = color;   //改变选框颜色
        canvas.gameObject.SetActive(true);
    }

    //关闭选框
    public void OffSelected()
    {
        canvas.gameObject.SetActive(false);
    }

    void Stand()
    {
        if (!ani.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            ani.SetBool("isAttack", false);
            ani.SetBool("isWalk", false);
        }
    }

    void Move()             //移动
    {
        if(!ani.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
        {
            ani.SetBool("isWalk", true);
            ani.SetBool("isAttack", false);
        }
        isAttack = false;
        Vector3 direcion = new Vector3((dir-1)%3-1, 0,(dir-1)/3-1);           //方向转化以5为中心的坐标系(例如 1 转化为-1，-1)
        if(direcion == Vector3.zero)            //方向为5时
        {
            direcion = aimPosition - transform.position;
        }
        transform.Translate(direcion.normalized * moveSpeed *Time.deltaTime,Space.World);       //移动
        transform.rotation = Quaternion.LookRotation(direcion.normalized);          //转向


        if (Vector3.Distance(aimPosition, transform.position) < 0.1)            //到达终点
        {
            gridsControl.DeleteObj(gameObject, mapIndex);       //通知导航系统，清除自己

            humanState = state.Stand;               //切换到站立状态
            mapIndex = -1;                          //清零导航
        }

        
    }

    void Pursue()
    {
        StopCoroutine("OnAttack");
        isAttack = false;
        if (!ani.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
        {
            ani.SetBool("isWalk", true);
            ani.SetBool("isAttack", false);
        }
        if (currentEnemy != null)
        {

        }
    }

    void Attack()
    {
        if (currentEnemy == null)
        {
            humanState = state.Stand;
        }
        else
        {
            transform.LookAt(currentEnemy.transform);
        }
        if (!ani.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            ani.SetBool("isAttack", true);
        }
        if (!isAttack)
        {
            isAttack = true;
            StartCoroutine("OnAttack");
            
        }
        
    }

    void Die()
    {
        if (!ani.GetCurrentAnimatorStateInfo(0).IsName("Die"))
        {
            ani.SetTrigger("die");
        }
    }

    public void SetMove(Vector3 aimPoint,int newMapIndex)      //设置移动目的地
    {
        if (humanState!=state.Move)          //切换到移动状态
        {
            humanState = state.Move;
        }

        if(mapIndex == newMapIndex)         //是否使用同一张地图
        {
            aimPosition = aimPoint;             //更新最终地点
        }
        else
        {
            gridsControl.DeleteObj(gameObject, mapIndex);       //通知导航系统，清除自己

            aimPosition = aimPoint;             //获取最终地点
            mapIndex = newMapIndex;             //获取地图信息
        }
    }

    public void SetPursue()              //设置追击目标
    {
        humanState = state.Pursue;
    }
    public void SetEnemy(GameObject enemy)     //设置攻击目标
    {
        if (currentEnemy == null)
        {
            currentEnemy = enemy;
        }
        humanState = state.Attack;
    }
    public void Died()
    {
        Destroy(gameObject);
    }

    public void GetHurt(int damage)     //受到攻击
    {
        currentHp -= damage;
    }
    IEnumerator OnAttack()     //攻击协程
    {
        while(true)
        {
            yield return new WaitForSeconds(1f);
            currentEnemy.GetComponent<HumanControl>().GetHurt(attack);
        }

    }

    private void OnTriggerStay(Collider other)          //碰撞
    {
        if (mapIndex != -1)             //运动时
        {
            if (other.gameObject.layer == 10)           //当其为导航基点时
            {
                Vector3 pos = transform.position;

                pos.x = (((int)pos.x + 5) / 5) * 5 - 2.5f;
                pos.y = -1;
                pos.z = (((int)pos.z + 5) / 5) * 5 - 2.5f;

                if (pos == other.transform.position)
                {
                    dir = other.GetComponent<GridScript>().maps[mapIndex].direction;
                }
            }
        }
    }
}
