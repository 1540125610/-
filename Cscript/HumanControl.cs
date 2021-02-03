using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class HumanControl : MonoBehaviour
{
    private Camera mainCamara;
    private Canvas canvas;
    private Vector3 aimPosition;
    private NavMeshAgent agent;     //设置导航代理
    public GameObject currentEnemy;  //当前攻击目标
    public string playerName;  //所属玩家名称
    public GameObject basicBullet;  //子弹
    public bool isAttack=false;


    public int maxHp;      //最大生命值
    private int currentHp;  //当前生命值
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
        agent = GetComponent<NavMeshAgent>();
        mainCamara = Camera.main;
        //拿到是否被选中的圈，并关闭它
        canvas = GetComponentInChildren<Canvas>();
        canvas.gameObject.SetActive(false);

        humanState = state.Stand;    //初始状态为待机
        currentHp = maxHp;
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
            agent.SetDestination(transform.position);
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
        //Debug.Log(humanState);
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
        
    }

    void Move()
    {
        agent.SetDestination(aimPosition);
    }

    void Pursue()
    {
        if (currentEnemy != null)
        {
            agent.SetDestination(currentEnemy.transform.position);
        }
    }

    void Attack()
    {
        if(!isAttack)
        {
            isAttack = true;
            StartCoroutine("OnAttack");
            
        }

    }

    void Die()
    {
        Destroy(gameObject);
    }

    public void SetMove(Vector3 aimPoint)      //设置移动目的地
    {
        if(humanState!=state.Move)
        {
            humanState = state.Move;
        }
        aimPosition = aimPoint;
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


    public void GetHurt(int damage)     //受到攻击
    {
        currentHp -= damage;
    }
    IEnumerator OnAttack()     //攻击协程
    {
        while(true)
        {
            yield return new WaitForSeconds(1f);
            GameObject bullet = Instantiate(basicBullet, transform.position, Quaternion.identity);
            bullet.GetComponent<Bullet>().aim = currentEnemy;
        }

    }
}
