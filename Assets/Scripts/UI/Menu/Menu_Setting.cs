using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Menu_Setting : MonoBehaviour
{
    public Menu_Start start;
    public Menu_Exit exit;
    public GameObject GO_text;
    public GameObject SelectPanel;
    public GameObject SettingPanel;
    private Vector3 Oposition;

    private void Awake()
    {
        Oposition = transform.position;
        GO_text = transform.GetChild(0).gameObject;
    }

    private void Start()
    {
        transform.DOMoveY(-14, 0.5f).From();
    }

    public void Click()
    {
        
        SettingPanel.SetActive(true);
    }

    public void SettingSideway()
    {
        transform.DOMove(new Vector3(-7.2f, 4.57f), 0.5f);
        transform.DOScaleX(0.75f, 0.5f);
        GO_text.transform.DOScaleX(1.3f, 0.5f);

    }

    public void UIBack()
    {
        transform.DOMove(Oposition, 0.5f);
        transform.DOScaleX(1, 0.5f);
        GO_text.transform.DOScaleX(1, 0.5f);
    }

}
