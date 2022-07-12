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
    [Header("卡牌属性")]
    public bool takeUp = false;
    RectTransform rect;
    public int handIndex;

    #region 卡牌属性
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

    [Header("卡牌特性")]
    public bool keepAfterTurn;
    public bool destroyAfterUse;
    public bool destroyAfterTurn;

    #region 卡面信息
    [Header("卡面")]
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
    /// 加载卡面信息
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
    /// 初始化：值values，指向target，[可略]花费cost，以及各类卡面信息
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
        //刷新界面信息
        BattleInfo.Instance.player.cost -= cost;
        BattleInfo.Instance.aim = null;
        BattleUI.Instance.RefreshEnergy();
        BattleManager.Instance.AllCardRefresh();
        HandGrid.Instance.SetCardposition();
        HandGrid.Instance.AllCardMove();

    }


    /// <summary>
    /// 卡牌效果，添加至ActionManager
    /// </summary>
    public abstract void CardEffect();

    #region 卡牌移动
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
        //抽卡后加入卡牌丢弃特性
        BattleManager.Instance.phaseEvent += phaseFunc;
    }

    public void CardMoveBack(float time = 0.3f)
    {
        transform.DOMove(targetPosition, time);
    }

    #endregion


    /// <summary>
    /// 更新卡牌描述信息
    /// </summary>
    public abstract void RefreshDescription();

    #region 回合事件

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


    #region 点击事件

    public void Choose()
    {

        //传递信息
        BattleInfo.Instance.ChosenCard = this;
        BattleUI.Instance.RefreshEnergy(cost);
        HandGrid.Instance.choosing = true;
        //卡牌抬起，关闭组件影响
        //BattleUI.Instance.SetInfluencedUI(false);
        var p = transform.position;
        transform.position = p + new Vector3(0, 1, -1);

        //脱离牌区优先渲染，记住序号
        //handIndex = transform.GetSiblingIndex();
        //transform.SetParent(transform.parent.parent);

        switch (target)
        {
            case Target.None:
                Drop();
                break;

            case Target.Enemy:
                //敌人可选开启
                BattleUI.Instance.SetCursorAttack();
                foreach (var _e in BattleInfo.Instance.enemies)
                {
                    _e.CanChoose = true;
                }
                break;

            case Target.NoNeed:
                //协程跟随移动
                takeUp = true;
                StartCoroutine(CardOnHand());
                break;

            case Target.PlayerNEnemy:
                //可选全开启
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
        //费用不足放下
        if (BattleInfo.Instance.player.cost < cost)
        {
            StartCoroutine(BattleUI.Instance.StartFloatingTips("能量不足"));
            Drop();
        }

    }

    public void Drop()
    {
        //传递信息
        BattleInfo.Instance.ChosenCard = null;
        BattleUI.Instance.RefreshEnergy();
        HandGrid.Instance.choosing = false;
        //卡牌放下，同时启用组件
        var p = transform.position;
        BattleUI.Instance.SetCursorNormal();
        transform.position = p + new Vector3(0, -1, 1);
        takeUp = false;
        UnFocus();
        //回归指定牌区
        //transform.SetParent(BattleManager.Instance.cards_Hand.transform);
        transform.SetSiblingIndex(handIndex);

        //可选全关
        BattleInfo.Instance.player.CanChoose = false;
        foreach (var _e in BattleInfo.Instance.enemies)
        {
            _e.CanChoose = false;
        }

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        BattleInfo infoSys = BattleInfo.Instance;
        //左键点击选择，抬起，待选择指定目标
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            //单击抬起
            if (infoSys.ChosenCard != this)
            {
                //放下其他卡
                if(infoSys.ChosenCard!=null)
                {
                    infoSys.ChosenCard.Drop();
                }
                //再抬起
                Choose();
            }
            //技能卡复选检查是否释放
            else if(target == Target.NoNeed && transform.position.y>-1)
            {
                takeUp = false;
                UseCard();
            }
            //非技能卡复选放下
            else
            {
                Drop();
            }
        }
        //右键放下 移动至battleInfo脚本

        //右键放下(对已选择卡生效)
        //else if(infoSys.ChosenCard!=null)
        //{
        //    BattleInfo.Instance.ChosenCard.Drop();
        //    BattleInfo.Instance.ChosenCard = null;
        //}

    }

    public IEnumerator CardOnHand()
    {

        //位置偏下，但是要保持向面前抬
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

        //回归指定牌区
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
