using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class HumanControl : MonoBehaviour
{
    private Camera mainCamara;
    private bool isSelect = false;
    private Canvas canvas;
    
    private NavMeshAgent agent;     //设置导航代理
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
    }

    
    void Update()
    {
        switch (humanState)
        {
            case state.Stand:       //待机
                Stand();
                break;
            case state.Pursue:      //
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
    }



    //开启选框
    public void OnSelected(Color color)
    {
        isSelect = true;
        canvas.GetComponentInChildren<Image>().color = color;   //改变选框颜色
        canvas.gameObject.SetActive(true);
    }

    //关闭选框
    public void OffSelected()
    {
        isSelect = false;
        canvas.gameObject.SetActive(false);
    }

    void Stand()
    {
        if(isSelect&&Input.GetMouseButtonDown(1))
        {
            Ray ray = mainCamara.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit = new RaycastHit();
            if(Physics.Raycast(ray,out hit))
            {
                if (hit.collider.tag == "Ground")
                {
                    agent.SetDestination(hit.point);   //设置寻路目标
                    humanState = state.Move;
                }
            }
            
        }
    }

    void Move()
    {
        
        if (isSelect && Input.GetMouseButtonDown(1))
        {
            Ray ray = mainCamara.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.tag == "Ground")
                {
                    agent.SetDestination(hit.point);
                }
            }

        }
    }

    void Pursue()
    {

    }

    void Attack()
    {

    }

    void Die()
    {

    }
}
