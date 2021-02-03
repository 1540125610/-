using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject aim;   //攻击目标

    private int damage=10;    //伤害
    void Start()
    {
        Destroy(gameObject, 5f);
    }


    void Update()
    {
        if (aim != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, aim.transform.position, 1f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject==aim)
        {
            other.gameObject.GetComponent<HumanControl>().GetHurt(damage);
            Destroy(gameObject);
        }
    }
}
