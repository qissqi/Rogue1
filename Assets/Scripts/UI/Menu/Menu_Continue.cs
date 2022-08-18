using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class Menu_Continue : MonoBehaviour
{
    void Start()
    {
        if(!SaveManager.Instance.HasSave)
        {
            GetComponent<Button>().interactable = false;
            gameObject.SetActive(false);
        }
        else
        {
            transform.DOMoveY(-10, 0.5f).From();

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
