using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;


public class SelectCharacter : MonoBehaviour
{
    public GameObject character;
    public Sprite sprite;
    public Menu_Start start;
    public bool Unlocked;
    public Button button;
    public TMP_Text TMP;
    [TextArea]
    public string intro;

    private void Awake()
    {
        TMP = transform.parent.GetChild(0).GetComponent<TMP_Text>();
        button = GetComponent<Button>();
        if(!Unlocked)
        {
            button.interactable = false;
        }
        start = FindObjectOfType<Menu_Start>();
    }

    private void Start()
    {
        button.onClick.AddListener(Click);
    }

    void OnEnable()
    {
        transform.DOMoveY(-20, 0.5f).From();
    }

    public void Click()
    {
        start.CanStart();
        GameManager.Instance.currentCharacter = character;
        transform.parent.GetComponent<Image>().sprite = sprite;
        TMP.text = intro;
    }


}
