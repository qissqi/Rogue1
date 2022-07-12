using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/// <summary>
/// 2-玩家回合中，5-敌人回合中
/// </summary>
public enum BattlePhase
{
    BattleStart, PlayerStart, PlayerTurn, PlayerEnd, EnemyStart, EnemyTurn, EnemyEnd
}


//用于控制整场战斗回合的转换、计时与判断，管理牌库
public class BattleManager : Singleton<BattleManager>
{
    public BattlePhase battlePhase;

    public event Action<BattlePhase> phaseEvent;
    public GameObject cards_Hand, cards_Deck, cards_Thrown, cards_Destroyed, cards_Pack;

    /// <summary>
    /// 启用时初始化
    /// </summary>
    public void Start()
    {
        BattleInfo.Instance.player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        var _e = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < _e.Length; i++)
        {
            BattleInfo.Instance.enemies.Add(_e[i].GetComponent<EnemyBase>());
        }

        cards_Hand = GameObject.FindGameObjectWithTag("Hand");
        cards_Deck = GameObject.FindGameObjectWithTag("Deck");
        cards_Thrown = GameObject.FindGameObjectWithTag("Thrown");
        cards_Destroyed = GameObject.FindGameObjectWithTag("Destroyed");
        cards_Pack = GameObject.FindGameObjectWithTag("CardPack");

        //phaseEvent += PhaseEnd;

        CardPackLoad();
        ActionManager.Instance.ActionAddToBotton(new EndPhase(BattlePhase.BattleStart));

    }


    private void Update()
    {
        DeveloperMode();
        //运行Action指令
        //if (ActionManager.Instance.Count() > 0)
        //{
        //    ActionManager.Instance.RunAction();
        //}
    }

    #region 回合功能

    //回合转换(通常)(调用指定回合转换)
    public void PhaseChange()
    {
        if (battlePhase != BattlePhase.EnemyEnd)
        {
            PhaseChange(battlePhase + 1);
        }
        else
        {
            PhaseChange(BattlePhase.PlayerStart);
        }
    }

    //回合转换（指定）
    /// <summary>
    /// 回合转换，唤醒回合事件，将回合结束置底
    /// </summary>
    /// <param name="newPhase"></param>
    public void PhaseChange(BattlePhase newPhase)
    {
        battlePhase = newPhase;
        //使所有回合方法生效，向ActionManager中填充
        phaseEvent.Invoke(battlePhase);
        //将回合转换添加至ActionManager底
        if(newPhase != BattlePhase.PlayerTurn)
        {
            ActionManager.Instance.ActionAddToBotton(new EndPhase());
        }
    }

   //检查是否处于目标回合
    public bool CheckPhase(BattlePhase phase)
    {
        return (battlePhase == phase);
    }

    #endregion

    /// <summary>
    /// 将卡包放入抽卡堆并洗牌
    /// </summary>
    public void CardPackLoad()
    {
        foreach (var _c in cards_Pack.GetComponentsInChildren<Card>(true))
        {
            Instantiate(_c.gameObject, cards_Deck.transform);
        }
        ShuffledCards();


    }

    /// <summary>
    /// 将弃牌堆放入抽牌堆并洗牌
    /// </summary>
    public void ShuffledCards()
    {
        //将弃牌堆放入抽牌
        for (; ; )
        {
            if (cards_Thrown.transform.childCount > 0)
            {
                cards_Thrown.GetComponentInChildren<Card>(true).Back2Deck();
            }
            else
            {
                break;
            }
        }

        //洗牌
        for (int i = 2 * cards_Deck.transform.childCount; i > 0; i--)
        {
            var _c = cards_Deck.GetComponentsInChildren<Card>(true)[cards_Deck.transform.childCount / 2].transform;
            _c.SetSiblingIndex(UnityEngine.Random.Range(0, cards_Deck.transform.childCount));
        }
    }

    //抽卡
    public void DrawCard(int num)
    {
        for (int i = 0; i < num; i++)
        {
            if (cards_Deck.transform.childCount > 0)   //抽牌
            {
                var _c = cards_Deck.GetComponentInChildren<Card>(true);
                _c.DrawCard();
            }
            else
            {
                if (cards_Thrown.transform.childCount>0) //若抽牌区不足，弃牌区有余
                {
                    ShuffledCards();
                    i--;
                    continue;
                }
                else           //没牌了
                {
                    break;
                }
            }
        }
    }

    public void DrawOneCard()
    {
        if (cards_Deck.transform.childCount > 0)   //抽牌
        {
            var _c = cards_Deck.GetComponentInChildren<Card>(true);
            _c.DrawCard();
        }
        else
        {
            if (cards_Thrown.transform.childCount > 0) //若抽牌区不足，弃牌区有余
            {
                ShuffledCards();
                DrawOneCard();
            }
            else           //没牌了
            {

            }
        }
    }

    public void AllCardRefresh()
    {
        foreach (var _c in cards_Hand.GetComponentsInChildren<Card>())
        {
            _c.RefreshDescription();
        }
    }

    /// <summary>
    /// 开发者功能区
    /// </summary>
    public void DeveloperMode()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            Debug.Log("Tab");
            StartCoroutine(DeveloperCounter());
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            HandGrid.Instance.SetCardposition();
            HandGrid.Instance.AllCardMove();
        }

        if(Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log(cards_Hand.GetComponentInChildren<Card>().transform.localScale.x*100000);
        }

    }
    public IEnumerator DeveloperCounter()
    {
        float t = 1f;
        while (t>0)
        {
            yield return null;
            t -= Time.deltaTime;
            Debug.Log(t);
        }
        BattleInfo.Instance.ChosenCard.transform.position = cards_Hand.transform.position;
    }


}

