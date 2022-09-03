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
    private RectTransform RT;

    private void Awake()
    {
        Oposition = transform.position;
        GO_text = transform.GetChild(0).gameObject;
        button = GetComponent<Button>();
        RT = GetComponent<RectTransform>();
    }

    private void Start()
    {
        transform.DOMoveY(-16, 0.5f).From();
        button.onClick.AddListener(Exit);
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void Back()
    {
        RT.DOAnchorPos(new Vector2(0, 20), 0.5f);
        RT.DOSizeDelta(new Vector2(200, 40), 0.5f);
        GO_text.GetComponent<Text>().text = "ÍË³ö";

        start.UIBack();
        setting.UIBack();

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(Exit);
    }

    public void SettingSideway()
    {
        RT.DOAnchorPos(new Vector2(325, 400), 0.5f);
        RT.DOSizeDelta(new Vector2(150, 40), 0.5f);

        GO_text.GetComponent<Text>().text = "·µ»Ø";

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(Back);
    }



}
