using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public abstract class Card : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public bool scaled;
    public Vector3 targetPosition;
    [Header("��������")]
    public bool takeUp = false;
    RectTransform rect;
    public int handIndex;

    #region ��������
    public enum CardType
    {
        Attack, Skill, Ability, Status, Curse
    }
    public enum Target
    {
        None, Enemy, NoNeed, PlayerNEnemy
    }
    public enum Rare
    {
        Unknown, Basi, Common, Rare, SuperRare
    }
    public enum Position
    {
        Detail, Hand, Uncaught, Thrown, Destroyed
    }
    public CardType type;
    public Target target;
    public Rare rare;
    #endregion

    [Header("��������")]
    public bool keepAfterTurn;
    public bool destroyAfterUse;
    public bool destroyAfterTurn;

    #region ������Ϣ
    [Header("����")]
    //public Image image;
    public Text text_cardName;
    public Text text_description;
    public Text text_cost;
    //public Sprite sprite;


    public string cardName;
    [TextArea]
    public string description;
    [Range(0, 10)]
    public int cost;

    #endregion
    /// <summary>
    /// ���ؿ�����Ϣ
    /// </summary>
    public virtual void Awake()
    {
        rect = GetComponent<RectTransform>();
        text_cardName.text = cardName;
        text_description.text = description;
        text_cost.text = cost.ToString();
        initialize();
    }


    /// <summary>
    /// ��ʼ����ֵvalues��ָ��target��[����]����cost���Լ����࿨����Ϣ
    /// </summary>
    public abstract void initialize();


    public void UseCard()
    {
        CardEffect();
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

    }


    /// <summary>
    /// ����Ч���������ActionManager
    /// </summary>
    public abstract void CardEffect();

    #region �����ƶ�
    public void CardThrow()
    {
        UnFocus();
        gameObject.transform.SetParent(BattleManager.Instance.cards_Thrown.transform, false);
        gameObject.SetActive(false);
        transform.position = BattleManager.Instance.cards_Thrown.transform.position;
        BattleManager.Instance.phaseEvent -= phaseFunc;
    }

    public void CardDestroy()
    {
        UnFocus();
        gameObject.transform.SetParent(BattleManager.Instance.cards_Destroyed.transform, false);
        gameObject.SetActive(false);
        transform.position = BattleManager.Instance.cards_Destroyed.transform.position;
        BattleManager.Instance.phaseEvent -= phaseFunc;
    }

    public void Back2Deck()
    {
        UnFocus();
        gameObject.transform.SetParent(BattleManager.Instance.cards_Deck.transform, false);
        gameObject.SetActive(false);
        transform.position = BattleManager.Instance.cards_Deck.transform.position;
        BattleManager.Instance.phaseEvent -= phaseFunc;
    }

    public void DrawCard()
    {
        UnFocus();
        gameObject.transform.SetParent(BattleManager.Instance.cards_Hand.transform, false);
        HandGrid.Instance.SetCardposition();
        gameObject.SetActive(true);
        RefreshDescription();
        HandGrid.Instance.AllCardMove();
        //�鿨����뿨�ƶ�������
        BattleManager.Instance.phaseEvent += phaseFunc;
    }

    public void CardMoveBack(float time = 0.3f)
    {
        transform.DOMove(targetPosition, time);
    }

    #endregion


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

    public void Choose()
    {

        //������Ϣ
        BattleInfo.Instance.ChosenCard = this;
        BattleUI.Instance.RefreshEnergy(cost);
        HandGrid.Instance.choosing = true;
        //����̧�𣬹ر����Ӱ��
        //BattleUI.Instance.SetInfluencedUI(false);
        var p = transform.position;
        transform.position = p + new Vector3(0, 1, -1);

        //��������������Ⱦ����ס���
        //handIndex = transform.GetSiblingIndex();
        //transform.SetParent(transform.parent.parent);

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
                    _e.CanChoose = true;
                }
                break;

            case Target.NoNeed:
                //Э�̸����ƶ�
                takeUp = true;
                StartCoroutine(CardOnHand());
                break;

            case Target.PlayerNEnemy:
                //��ѡȫ����
                BattleInfo.Instance.player.CanChoose = true;
                foreach (var _e in BattleInfo.Instance.enemies)
                {
                    _e.CanChoose = true;
                }
                break;

            default:
                Drop();
                break;
        }
        //���ò������
        if (BattleInfo.Instance.player.cost < cost)
        {
            StartCoroutine(BattleUI.Instance.StartFloatingTips("��������"));
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
        foreach (var _e in BattleInfo.Instance.enemies)
        {
            _e.CanChoose = false;
        }

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        BattleInfo infoSys = BattleInfo.Instance;
        //������ѡ��̧�𣬴�ѡ��ָ��Ŀ��
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            //����̧��
            if (infoSys.ChosenCard != this)
            {
                //����������
                if(infoSys.ChosenCard!=null)
                {
                    infoSys.ChosenCard.Drop();
                }
                //��̧��
                Choose();
            }
            //���ܿ���ѡ����Ƿ��ͷ�
            else if(target == Target.NoNeed && transform.position.y>-1)
            {
                takeUp = false;
                UseCard();
            }
            //�Ǽ��ܿ���ѡ����
            else
            {
                Drop();
            }
        }
        //�Ҽ����� �ƶ���battleInfo�ű�

        //�Ҽ�����(����ѡ����Ч)
        //else if(infoSys.ChosenCard!=null)
        //{
        //    BattleInfo.Instance.ChosenCard.Drop();
        //    BattleInfo.Instance.ChosenCard = null;
        //}

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
            Vector3 delt = pos2 - pos1;
            //rect.anchoredPosition += delt;
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
        UnFocus();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Focus();
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
            transform.DOScale(1.1f, 0.1f);
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
            transform.DOScale(new Vector3(0.88f,0.88f), 0.1f);
        }
    }


    #endregion

}
