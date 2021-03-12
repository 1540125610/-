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
    public bool isAttack=false;

    public int attack;      //攻击力
    public int maxHp;      //最大生命值
    private int currentHp;  //当前生命值

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
        ani = GetComponent<Animator>();
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

        int x=0, z=0;
        switch (dir)
        {
            case 1:
                x = -1; z = -1;
                break;
            case 2:
                x = 0; z = -1; 
                break;
            case 3:
                x = 1; z = -1;
                break;
            case 4:
                x = -1; z = 0;
                break;
            case 5:
                x = 0; z = 0;
                break;
            case 6:
                x = 1; z = 0;
                break;
            case 7:
                x = -1; z = 1;
                break;
            case 8:
                x = 0; z = 1;
                break;
            case 9:
                x = 1; z = 1;
                break;
        }
        Vector3 direcion = new Vector3(x, 0, z);
        transform.Translate(direcion.normalized *Time.deltaTime);
        //transform.Rotate(direcion*90);
        
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
            agent.SetDestination(currentEnemy.transform.position);
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
        if(humanState!=state.Move)
        {
            humanState = state.Move;
        }


        mapIndex = newMapIndex;
        
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

    private void OnTriggerStay(Collider other)
    {
        if (mapIndex != -1)
        {
            Vector3 pos = transform.position;
            if (pos.x >= 0)
            {
                pos.x=(int)((int)(pos.x + 1) % 2 + pos.x);
            }
            else
            {
                pos.x = (int)((int)(pos.x - 1) % 2 + pos.x);
            }

            if (pos.z >= 0)
            {
                pos.z = (int)((int)(pos.z + 1) % 2 + pos.z);
            }
            else
            {
                pos.z = (int)((int)(pos.z - 1) % 2 + pos.z);
            }

            pos.y = -1;

            if (other.gameObject.layer == 10)
            {
                if(pos == other.transform.position)
                {
                    dir = other.GetComponent<GridScript>().maps[mapIndex].direction;
                }
            }
        }
    }
}
