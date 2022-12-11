using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using TMPro;

public abstract class Card : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [HideInInspector]public bool scaled;
    [HideInInspector]public Vector3 targetPosition;
    [HideInInspector]public bool takeUp = false;
    [HideInInspector]public int handIndex;
    [HideInInspector]public string explain;
    [HideInInspector] public bool isTemp;
    public delegate void Call(Card card);
    public Call CardRespone;

    private Vector3 oriScaleValue;
    private Vector3 endSclaeValue;

    #region ��������
    public enum CardType
    {
        Attack, Skill, Power, Status, Curse
    }
    public enum Target
    {
        None, Enemy, NoNeed, PlayerNEnemy,Player,AllEnemies
    }
    public enum Rare
    {
        Unknown, Basi, Common, Rare, SuperRare
    }
    public enum Position
    {
        Detail, Hand, Uncaught, Thrown, Destroyed
    }
    [Header("��������")]
    public bool hasExplain;
    public CardType type;
    public Target target;
    public Rare rare;
    public int price = 0;
    public bool inReward = false;
    public bool inSelect = false;
    public bool inShow = false;
    #endregion

    [Header("��������")]
    public bool keepAfterTurn;
    public bool destroyAfterUse;
    public bool destroyAfterTurn;

    [HideInInspector] public Button button_Get;
    #region ������Ϣ
    [Header("����")]
    [HideInInspector]public TMP_Text text_cardName;
    [HideInInspector]public TMP_Text text_description;
    [HideInInspector]public Text text_cost;
    [HideInInspector]public Transform type_icon;



    public string cardName;
    //[TextArea]
    //public string description;
    [Range(0, 10)]
    public int cost;

    #endregion


    /// <summary>
    /// ���ؿ�����Ϣ
    /// </summary>
    public virtual void Awake()
    {
        oriScaleValue = transform.localScale;
        endSclaeValue = transform.localScale * 1.2f;
        //button_Get = transform.Find("Get").GetComponent<Button>();
        text_cardName = transform.Find("Name").GetComponent<TMP_Text>();
        text_description = transform.Find("Description").GetComponent<TMP_Text>();
        text_cost = transform.Find("Cost").GetComponent<Text>();
        //rect = GetComponent<RectTransform>();
        CardFaceFix();
        Initialize();
    }

    private void Reset()
    {
        text_cardName = transform.Find("Name").GetComponent<TMP_Text>();
        text_description = transform.Find("Description").GetComponent<TMP_Text>();
        text_cost = transform.Find("Cost").GetComponent<Text>();
        type_icon = transform.Find("Type");
    }

    private void Start()
    {
        RefreshDescription();
    }

    public void CardFaceFix()
    {
        text_cardName.text = cardName;
        RefreshDescription();
        text_cost.text = cost.ToString();
        type_icon = transform.Find("Type");
        type_icon.GetChild((int)type)?.gameObject.SetActive(true);
        
    }

    /// <summary>
    /// ��ʼ������Ҫ�ĳ�ʼֵ���Լ�explain
    /// </summary>
    public abstract void Initialize();

    public virtual void UseCardFromHand()
    {
        if(!CheckUse())
        {
            if(isTemp)
            {
                Destroy(gameObject);
            }
            else
            {
                Drop();
            }
            return;
        }

        UsingCard();

        Drop();
        if (destroyAfterUse)
        {
            CardDestroy();
        }
        else
        {
            CardThrow();
        }
        //ˢ�½�����Ϣ
        BattleInfo.Instance.player.cost -= cost;
        BattleInfo.Instance.aim = null;
        BattleUI.Instance.RefreshEnergy();
        BattleManager.Instance.AllCardRefresh();
        HandGrid.Instance.SetCardposition();
        HandGrid.Instance.AllCardMove();
        GameManager.Instance.CloseExplainBox();
    }

    public void UsingCard()
    {
        CardEffect();
        AfterCardUse();
        if(isTemp)
        {
            Destroy(gameObject);
        }
    }

    public virtual bool CheckUse()
    {
        return true;
    }

    public virtual void AfterCardUse()
    {
        foreach (var item in BattleInfo.Instance.player.buffs)
        {
            item.AfterCardUse(this);
        }
        if(type == CardType.Power || isTemp)
        {
            Destroy(gameObject);
        }
    }

    /// <summary> ����Ч���������ActionManager </summary>
    public abstract void CardEffect();
    
    //����λ��

    public void AddToPack()
    {
        gameObject.transform.SetParent(BattleManager.Instance.cards_Pack.transform);
        gameObject.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
        gameObject.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
        transform.localScale = new Vector3(1.5f, 1.5f,1);
        inReward = false;
        inSelect = false;
        inShow = true;
        GameManager.Instance.CloseExplainBox();
    }

    public void CardThrow()
    {
        UnFocus();
        gameObject.transform.SetParent(BattleManager.Instance.cards_Thrown.transform, false);
        //gameObject.SetActive(false);
        transform.position = BattleManager.Instance.cards_Thrown.transform.position;
        BattleManager.Instance.phaseEvent -= phaseFunc;
        inShow = true;
    }

    public void CardDestroy()
    {
        UnFocus();
        gameObject.transform.SetParent(BattleManager.Instance.cards_Destroyed.transform, false);
        //gameObject.SetActive(false);
        transform.position = BattleManager.Instance.cards_Destroyed.transform.position;
        BattleManager.Instance.phaseEvent -= phaseFunc;
        inShow = true;
    }

    public void Back2Deck()
    {
        UnFocus();
        gameObject.transform.SetParent(BattleManager.Instance.cards_Deck.transform, false);
        //gameObject.SetActive(false);
        transform.position = BattleManager.Instance.cards_Deck.transform.position;
        BattleManager.Instance.phaseEvent -= phaseFunc;
        inShow = true;
    }

    public void DrawCard()
    {
        transform.rotation = Quaternion.Euler(0, 180, 0);
        transform.position = new Vector3(0, 0,100);
        GameObject _back = transform.Find("Back").gameObject;
        _back.SetActive(true);
        //UnFocus();
        gameObject.transform.SetParent(BattleManager.Instance.cards_Hand.transform, false);
        //gameObject.SetActive(true);
        inShow = false;
        HandGrid.Instance.SetCardposition();
        RefreshDescription();
        HandGrid.Instance.AllCardMove();
        //�鿨����뿨�ƶ�������
        BattleManager.Instance.phaseEvent += phaseFunc;
    }

    public void CardMoveBack(float time = 0.3f)
    {
        //transform.DOMove(targetPosition, time);
        GetComponent<RectTransform>().DOAnchorPos(targetPosition, time).OnComplete(()=>
        {
            if(Mathf.Abs( transform.rotation.eulerAngles.y)==180)
            {
                transform.DORotate(new Vector3(0, 90, 0), time/2).OnComplete(()=> 
                {
                    transform.Find("Back").gameObject.SetActive(false);
                    transform.DORotate(new Vector3(0, 0, 0), time / 2);
                });
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
                transform.Find("Back").gameObject.SetActive(false);
            }
        });
    }


    /// <summary>
    /// ���¿���������Ϣ
    /// </summary>
    public abstract void RefreshDescription();

    #region �غ��¼�

    public virtual void phaseFunc(BattlePhase phase)
    {
        if(phase == BattlePhase.PlayerEnd)
        {
            if(destroyAfterTurn)
            {
                CardDestroy();
            }
            else if(keepAfterTurn)
            {

            }
            else
            {
                CardThrow();
            }
        }
    }

    #endregion


    #region ����¼�

    public virtual void Choose()
    {

        //������Ϣ
        BattleInfo.Instance.ChosenCard = this;
        BattleUI.Instance.RefreshEnergy(cost);
        HandGrid.Instance.choosing = true;
        //����̧�𣬹ر����Ӱ��
        //BattleUI.Instance.SetInfluencedUI(false);
        var p = transform.position;
        transform.position = p + new Vector3(0, 1, -1);

        switch (target)
        {
            case Target.None:
                Drop();
                break;

            case Target.Enemy:
                //���˿�ѡ����
                BattleUI.Instance.SetCursorAttack();
                foreach (var _e in BattleInfo.Instance.enemies)
                {
                    _e.GetComponent<EnemyBase>().CanChoose = true;
                }
                break;

            case Target.Player:
                //Э�̸����ƶ�
                takeUp = true;
                StartCoroutine(CardOnHand());
                BattleInfo.Instance.player.selectBox.SetActive(true);
                break;

            case Target.AllEnemies:
                //Э�̸����ƶ�
                takeUp = true;
                StartCoroutine(CardOnHand());
                foreach (var _e in BattleInfo.Instance.enemies)
                {
                    _e.GetComponent<EnemyBase>().selectBox.SetActive(true);
                }
                break;

            case Target.PlayerNEnemy:
                //��ѡȫ����
                BattleInfo.Instance.player.CanChoose = true;
                foreach (var _e in BattleInfo.Instance.enemies)
                {
                    _e.GetComponent<EnemyBase>().CanChoose = true;
                }
                break;

            default:
                Drop();
                break;
        }
        //���ò������
        if (BattleInfo.Instance.player.cost < cost)
        {
            BattleUI.Instance.ShowTipInfo("��������");
            Drop();
        }
        else if(!CheckUse())
        {
            Drop();
        }

    }

    public void Drop()
    {
        //������Ϣ
        BattleInfo.Instance.ChosenCard = null;
        BattleUI.Instance.RefreshEnergy();
        HandGrid.Instance.choosing = false;
        //���Ʒ��£�ͬʱ�������
        var p = transform.position;
        BattleUI.Instance.SetCursorNormal();
        transform.position = p + new Vector3(0, -1, 1);
        takeUp = false;
        UnFocus();
        //�ع�ָ������
        //transform.SetParent(BattleManager.Instance.cards_Hand.transform);
        transform.SetSiblingIndex(handIndex);

        //��ѡȫ��
        BattleInfo.Instance.player.CanChoose = false;
        BattleInfo.Instance.player.selectBox.SetActive(false);
        foreach (var _e in BattleInfo.Instance.enemies)
        {
            _e.GetComponent<EnemyBase>().CanChoose = false;
            _e.GetComponent<EnemyBase>().selectBox.SetActive(false);
        }

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(inReward)
        {
            AddToPack();
            BattleManager.Instance.ClearRewardCard();
            return;
        }
        if(inSelect)
        {
            CardRespone(this);
            return;
        }
        if(inShow)
        {
            return;
        }

        BattleInfo infoSys = BattleInfo.Instance;
        //������ѡ��̧�𣬴�ѡ��ָ��Ŀ��
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            //����̧��
            if (infoSys.ChosenCard != this)
            {
                SoundManager.Instance.PlaySE("CardPick");
                //����������
                if(infoSys.ChosenCard!=null)
                {
                    infoSys.ChosenCard.Drop();
                }
                //��̧��
                Choose();
            }
            //���ܿ���ѡ ����Ƿ��ͷ�
            else if((target == Target.NoNeed|| 
                target == Target.Player ||
                target == Target.AllEnemies) && 
                GetComponent<RectTransform>().anchoredPosition.y>150)
            {
                takeUp = false;
                UseCardFromHand();
            }
            //�Ǽ��ܿ���ѡ����
            else
            {
                Drop();
            }
        }

    }

    public IEnumerator CardOnHand()
    {

        //λ��ƫ�£�����Ҫ��������ǰ̧
        transform.position = transform.position + new Vector3(0, -0.5f, 0);
        Vector3 oPos = transform.position;
        for (; takeUp;)
        {
            Vector3 pos1 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            yield return null;
            Vector3 pos2 = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Vector3 delt = (pos2 - pos1);
            transform.position += delt;
        }
        transform.position = oPos + new Vector3(0, -0.5f, 1);

        //�ع�ָ������
        //transform.SetParent(BattleManager.Instance.cards_Hand.transform);
        //transform.SetSiblingIndex(handIndex);
        //BattleUI.Instance.SetInfluencedUI(true);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if(!inReward&&!inSelect&&!inShow)
            UnFocus();

        if(hasExplain)
        {
            GameManager.Instance.CloseExplainBox();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(!inReward&&!inSelect&&!inShow)
            Focus();

        if(hasExplain)
        {
            GameManager.Instance.OpenExplainBox(explain, eventData,transform.parent, gameObject);
        }
    }

    public void Focus()
    {
        if(HandGrid.Instance.choosing)
        {
            return;
        }
        //StartCoroutine(Emove());
        HandGrid.Instance.Focuses++;
        HandGrid.Instance.focusing = true;
        HandGrid.Instance.FocusCard = this;

        //handIndex = transform.GetSiblingIndex();
        HandGrid.Instance.FocusMove(this);
        transform.SetAsLastSibling();
        if (!scaled)
        {
            //transform.DOScale(1.1f, 0.1f);
            transform.DOScale(endSclaeValue, 0.1f);
            scaled = true;
        }
    }

    public void UnFocus()
    {
        if(HandGrid.Instance.choosing)
        {
            return;
        }
        HandGrid.Instance.Focuses--;

        transform.SetSiblingIndex(handIndex);
        //HandGrid.Instance.FocusMove(this, true);
        if(scaled)
        {
            scaled = false;
            //transform.DOScale(new Vector3(0.88f,0.88f), 0.1f);
            transform.DOScale(oriScaleValue, 0.1f);
        }
    }


    #endregion

}
