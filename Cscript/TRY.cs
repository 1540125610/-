using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TRY : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        int i = GetComponent<GridScript>().maps[0].direction;
        Debug.Log(i);
    }
}
