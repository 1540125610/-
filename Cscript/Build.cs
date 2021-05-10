using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Build : MonoBehaviour
{
    public GameObject barracks;         //房子

    public void Building()              //预创建
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Physics.Raycast(ray, out hit);
        Vector3 startPosition = new Vector3(hit.point.x, -1, hit.point.z);
        Instantiate(barracks, startPosition, Quaternion.Euler(new Vector3(-90, 0, 0)));
    }
}
