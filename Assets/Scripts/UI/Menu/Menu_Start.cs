using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Menu_Start : MonoBehaviour
{
    public Menu_Setting setting;
    public Menu_Exit exit;
    public Menu_Continue _Continue;
    private Button button;
    public GameObject title;
    private GameObject GO_text;
    public GameObject AttentionPanel;
    public GameObject SelectPanel;
    public Sprite panel;

    private float titleY;
    private Vector3 Oposition;

    private void Awake()
    {
        titleY = title.transform.position.y;
        Oposition = transform.position;
        button = GetComponent<Button>();
        GO_text = transform.GetChild(0).gameObject;
    }

    private void Start()
    {
        button.onClick.AddListener(Click);
        transform.DOMoveY(-12, 0.5f).From();
    }

    public void Click()
    {
        if(_Continue.gameObject.activeSelf)
        {
            AttentionPanel.SetActive(true);
            AttentionPanel.transform.GetChild(0).DOMoveY(-10, 0.6f).From();
        }
        else
        {
            button.interactable = false;
            GotoSelect();
        }
    }

    public void SetSideWay()
    {
        transform.DOMove(new Vector3(3.9f, 4.57f), 0.5f);
        transform.DOScaleX(0.75f, 0.5f);
        GO_text.transform.DOScaleX(1.3f, 0.5f);
    }

    public void GotoSelect()
    {
        //继续按钮不再启用
        _Continue.gameObject.SetActive(false);
        //选角面板初始化
        SelectPanel.GetComponent<Image>().DOColor(new Color(1, 1,1, 0), 1f).From();
        title.transform.DOMoveY(titleY+10, 0.5f);
        //按钮组件更新
        GO_text.GetComponent<Text>().text = "选择角色";
        GO_text.transform.DOScaleX(0.4f, 0.5f);
        button.interactable = false;
        transform.DOMove(new Vector3(0, 4.57f), 0.5f);
        transform.DOScaleX(2.5f, 0.5f);

        setting.SettingSideway();
        exit.SettingSideway();
        SelectPanel.SetActive(true);

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(GameStart);
    }

    public void CanStart()
    {
        button.interactable = true;
        GO_text.GetComponent<Text>().text = "开始冒险";
        GetComponent<Image>().color = new Color(0, 255, 255);
    }

    /// <summary>
    /// 选好角色后游戏开始
    /// </summary>
    public void GameStart()
    {
        Debug.Log("GameStart");
        GameManager.Instance.LoadNewMap();
    }

    public void UIBack()
    {
        transform.DOMove(Oposition, 0.5f);
        transform.DOScaleX(1, 0.5f);
        GO_text.transform.DOScaleX(1, 0.5f);

        SelectPanel.GetComponent<Image>().sprite = panel;
        SelectPanel.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        SelectPanel.SetActive(false);
        title.transform.DOMoveY(titleY, 0.5f);
        SelectPanel.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text = "";

        GO_text.GetComponent<Text>().text = "开始";
        GetComponent<Image>().color = Color.white;
        button.interactable = true;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(Click);

    }


}
