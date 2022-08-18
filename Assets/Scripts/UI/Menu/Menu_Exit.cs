using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Menu_Exit : MonoBehaviour
{
    public Menu_Start start;
    public Menu_Setting setting;
    public GameObject GO_text;
    private Button button;
    private Vector3 Oposition;

    private void Awake()
    {
        Oposition = transform.position;
        GO_text = transform.GetChild(0).gameObject;
        button = GetComponent<Button>();
    }

    private void Start()
    {
        transform.DOMoveY(-16, 0.5f).From();
        button.onClick.AddListener(Exit);
        
    }

    public void Exit()
    {

    }

    public void Back()
    {
        transform.DOMove(Oposition, 0.5f);
        transform.DOScaleX(1, 0.5f);
        GO_text.GetComponent<Text>().text = "ÍË³ö";
        GO_text.transform.DOScaleX(1, 0.5f);

        start.UIBack();
        setting.UIBack();

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(Exit);
    }

    public void SettingSideway()
    {
        transform.DOMove(new Vector3(7.2f, 4.57f), 0.5f);
        transform.DOScaleX(0.75f, 0.5f);
        GO_text.GetComponent<Text>().text = "·µ»Ø";
        GO_text.transform.DOScaleX(1.3f, 0.5f);

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(Back);
    }



}
