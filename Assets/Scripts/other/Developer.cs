using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Developer : MonoBehaviour
{

    private void Awake()
    {
        Debug.Log("Awake");
    }

    private void Start()
    {
        Debug.Log("Start");
    }

    private void OnEnable()
    {
        Debug.Log("Enable");
    }


    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            print(Input.mousePosition + "\nWorld Position: " + Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
    }



    private void OnDisable()
    {
        Debug.Log("Disable");
    }

}
