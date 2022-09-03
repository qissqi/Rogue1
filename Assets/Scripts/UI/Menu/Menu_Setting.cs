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
    private RectTransform RT;

    private void Awake()
    {
        Oposition = transform.position;
        GO_text = transform.GetChild(0).gameObject;
        RT = GetComponent<RectTransform>();
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
        RT.DOAnchorPos(new Vector2(-325, 400), 0.5f);
        RT.DOSizeDelta(new Vector2(150, 40), 0.5f);

    }

    public void UIBack()
    {
        RT.DOAnchorPos(new Vector2(0, 80), 0.5f);
        RT.DOSizeDelta(new Vector2(200, 40), 0.5f);
    }

}
