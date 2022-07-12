using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/// <summary>
/// 2-��һغ��У�5-���˻غ���
/// </summary>
public enum BattlePhase
{
    BattleStart, PlayerStart, PlayerTurn, PlayerEnd, EnemyStart, EnemyTurn, EnemyEnd
}


//���ڿ�������ս���غϵ�ת������ʱ���жϣ������ƿ�
public class BattleManager : Singleton<BattleManager>
{
    public BattlePhase battlePhase;

    public event Action<BattlePhase> phaseEvent;
    public GameObject cards_Hand, cards_Deck, cards_Thrown, cards_Destroyed, cards_Pack;

    /// <summary>
    /// ����ʱ��ʼ��
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
        //����Actionָ��
        //if (ActionManager.Instance.Count() > 0)
        //{
        //    ActionManager.Instance.RunAction();
        //}
    }

    #region �غϹ���

    //�غ�ת��(ͨ��)(����ָ���غ�ת��)
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

    //�غ�ת����ָ����
    /// <summary>
    /// �غ�ת�������ѻغ��¼������غϽ����õ�
    /// </summary>
    /// <param name="newPhase"></param>
    public void PhaseChange(BattlePhase newPhase)
    {
        battlePhase = newPhase;
        //ʹ���лغϷ�����Ч����ActionManager�����
        phaseEvent.Invoke(battlePhase);
        //���غ�ת�������ActionManager��
        if(newPhase != BattlePhase.PlayerTurn)
        {
            ActionManager.Instance.ActionAddToBotton(new EndPhase());
        }
    }

   //����Ƿ���Ŀ��غ�
    public bool CheckPhase(BattlePhase phase)
    {
        return (battlePhase == phase);
    }

    #endregion

    /// <summary>
    /// ����������鿨�Ѳ�ϴ��
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
    /// �����ƶѷ�����ƶѲ�ϴ��
    /// </summary>
    public void ShuffledCards()
    {
        //�����ƶѷ������
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

        //ϴ��
        for (int i = 2 * cards_Deck.transform.childCount; i > 0; i--)
        {
            var _c = cards_Deck.GetComponentsInChildren<Card>(true)[cards_Deck.transform.childCount / 2].transform;
            _c.SetSiblingIndex(UnityEngine.Random.Range(0, cards_Deck.transform.childCount));
        }
    }

    //�鿨
    public void DrawCard(int num)
    {
        for (int i = 0; i < num; i++)
        {
            if (cards_Deck.transform.childCount > 0)   //����
            {
                var _c = cards_Deck.GetComponentInChildren<Card>(true);
                _c.DrawCard();
            }
            else
            {
                if (cards_Thrown.transform.childCount>0) //�����������㣬����������
                {
                    ShuffledCards();
                    i--;
                    continue;
                }
                else           //û����
                {
                    break;
                }
            }
        }
    }

    public void DrawOneCard()
    {
        if (cards_Deck.transform.childCount > 0)   //����
        {
            var _c = cards_Deck.GetComponentInChildren<Card>(true);
            _c.DrawCard();
        }
        else
        {
            if (cards_Thrown.transform.childCount > 0) //�����������㣬����������
            {
                ShuffledCards();
                DrawOneCard();
            }
            else           //û����
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
    /// �����߹�����
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

