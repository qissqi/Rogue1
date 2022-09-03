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
    private RectTransform RT;

    private float titleY;

    private void Awake()
    {
        RT = GetComponent<RectTransform>();
        titleY = title.transform.position.y;
        button = GetComponent<Button>();
        GO_text = transform.GetChild(0).gameObject;
    }

    private void Start()
    {
        SoundManager.Instance.PlayBGM("Title");
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

    public void GotoSelect()
    {
        //������ť��������
        _Continue.gameObject.SetActive(false);
        //ѡ������ʼ��
        SelectPanel.GetComponent<Image>().DOColor(new Color(1, 1,1, 0), 1f).From();
        SelectPanel.transform.GetChild(1).GetComponent<Image>().enabled = false;
        title.transform.DOMoveY(titleY+10, 0.5f);
        //��ť�������
        GO_text.GetComponent<Text>().text = "ѡ���ɫ";
        //GO_text.transform.DOScaleX(0.4f, 0.5f);
        button.interactable = false;

        RT.DOAnchorPosY(400, 0.5f);
        RT.DOSizeDelta(new Vector2(500, 40), 0.5f);
        //transform.DOScaleX(2.5f, 0.5f);

        setting.SettingSideway();
        exit.SettingSideway();
        SelectPanel.SetActive(true);

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(GameStart);
    }

    public void CanStart()
    {
        button.interactable = true;
        GO_text.GetComponent<Text>().text = "��ʼð��";
        GetComponent<Image>().color = new Color(0, 255, 255);
    }

    /// <summary>
    /// ѡ�ý�ɫ����Ϸ��ʼ
    /// </summary>
    public void GameStart()
    {
        Debug.Log("GameStart");
        GameManager.Instance.level = 1;
        GameManager.Instance.PlayerinfoClear();
        GameManager.Instance.ChangeScene(GameManager.GameScene.Map);
        GameManager.Instance.LoadNewMap();
    }

    public void UIBack()
    {
        GetComponent<RectTransform>().DOAnchorPosY(140, 0.5f);
        RT.DOAnchorPosY(140, 0.5f);
        RT.DOSizeDelta(new Vector2(200, 40), 0.5f);

        SelectPanel.GetComponent<Image>().sprite = panel;
        SelectPanel.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        SelectPanel.SetActive(false);
        //title.transform.DOMoveY(titleY, 0.5f);
        title.GetComponent<RectTransform>().DOAnchorPosY(-100, 0.5f);
        SelectPanel.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text = "";

        GO_text.GetComponent<Text>().text = "��ʼ";
        GetComponent<Image>().color = Color.white;
        button.interactable = true;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(Click);

    }


}
