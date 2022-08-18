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
    [HideInInspector]public BattlePhase battlePhase;
    public GameObject RewardPanel;
    [HideInInspector]public bool battleStart;
    public event Action<BattlePhase> phaseEvent;
    [HideInInspector]public GameObject cards_Hand, cards_Deck, cards_Thrown, cards_Destroyed, cards_Pack;
    public Transform Equipment, Consumables, Relic;
    public Transform CardRewardTr;
    [HideInInspector]public Enemy_OnMap EoM;
    /// <summary>
    /// 启用时初始化
    /// </summary>
    public void Start()
    {
        cards_Hand = GameObject.FindGameObjectWithTag("Hand");
        cards_Deck = GameObject.FindGameObjectWithTag("Deck");
        cards_Deck.SetActive(false);
        cards_Thrown = GameObject.FindGameObjectWithTag("Thrown");
        cards_Thrown.SetActive(false);
        cards_Destroyed = GameObject.FindGameObjectWithTag("Destroyed");
        cards_Destroyed.SetActive(false);
        cards_Pack = GameObject.FindGameObjectWithTag("CardPack");
        transform.parent.gameObject.SetActive(false);
    }

    public void BattleStart(Enemy_OnMap enemy_On)
    {
        EoM = enemy_On;
        GameManager.Instance.inBattle = true;
        GameManager.Instance.currentscene = GameManager.GameScene.Battle;
        GameManager.Instance.currentCharacter.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        var battleCanvas = Instance.transform.parent.gameObject;
        battleCanvas.SetActive(true);
        battleCanvas.GetComponent<Canvas>().worldCamera = Camera.main;
        var PP = battleCanvas.transform.Find("BattleScene/Player");
        var EP = battleCanvas.transform.Find("BattleScene/Enemies");
        Instantiate(GameManager.Instance.currentCharacter.GetComponent<CharacterControl>().BattlePlayer, PP);
        foreach (var _e in enemy_On.combineEnemyPre)
        {
            var _E = Instantiate(_e, EP);
            _E.GetComponent<EnemyBase>().BattleStart();
        }

        Instance.InitBattle();
    }

    public void InitBattle()
    {
        BattleInfo.Instance.player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        var _e = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < _e.Length; i++)
        {
            BattleInfo.Instance.enemies.Add(_e[i]);
        }

        CardPackLoad();
        ItemLoad();
        BattleUI.Instance.Item_Reset();
        ActionManager.Instance.ActionAddToBotton(new EndPhase(BattlePhase.BattleStart));
        battleStart = true;
    }


    private void Update()
    {
        if (!battleStart)
            return;
        DeveloperMode();
        if(BattleInfo.Instance.enemies.Count == 0)
        {
            BattleVictory();
        }

    }

    public void BattleVictory()
    {

        battleStart = false;
        DG.Tweening.DOTween.KillAll();
        EoM.BattleEnd(true);

        //生成奖励
        InitRewardCard(3);

        CardRewardTr.gameObject.SetActive(true);
        RewardPanel.SetActive(true);

        //关闭标记
        MapManager.Instance.currentRoom.Mark.transform.Find("Enemy").gameObject.SetActive(false);
        MapManager.Instance.currentRoom.Mark.transform.Find("Elite").gameObject.SetActive(false);
        MapManager.Instance.currentRoom.Mark.transform.Find("Boss").gameObject.SetActive(false);
    }

    public void ItemLoad()
    {
        InventoryManager inv = InventoryManager.Instance;
        foreach (var item in inv.EquipmentItem)
        {
            var scale = item.transform.parent.localScale;
            item.transform.parent.SetParent(Equipment);
            item.transform.parent.localScale = scale;
            item.Dragable = false;
        }
        foreach (var item in inv.ConsumablesItem)
        {
            var scale = item.transform.parent.localScale;
            item.transform.parent.SetParent(Consumables);
            item.transform.parent.localScale = scale;
            item.Dragable = false;
        }
        foreach (var item in inv.InventoryItems)
        {
            if(item.type == Item.ItemType.Relic)
            {
                var scale = item.transform.parent.localScale;
                item.transform.parent.SetParent(Relic);
                item.transform.parent.localScale = scale;
                item.Dragable = false;
            }
        }
    }

    public void ItemBack()
    {
        InventoryManager inv = InventoryManager.Instance;
        for (int i = Equipment.childCount; i >0 ; i--)
        {
            var item = Equipment.GetChild(0);
            var scale = item.localScale;
            item.SetParent(inv.EquipmentPanel.transform);
            item.localScale = scale;

        }
        for (int i = Consumables.childCount; i >0; i--)
        {
            var item = Consumables.GetChild(0);
            var scale = item.localScale;
            item.SetParent(inv.CarryPanel.transform);
            item.localScale = scale;
        }
        for (int i = Relic.childCount; i >0 ; i--)
        {
            var item = Relic.GetChild(0);
            var scale = item.localScale;
            item.SetParent(inv.InventoryPanel.transform);
            item.localScale = scale;
        }
        //复原位置
        foreach (var item in inv.InventoryItems)
        {
            if(item.type == Item.ItemType.Relic)
            {
                item.Dragable = true;
                item.transform.parent.SetSiblingIndex(item.OriginSiblingIndex);
            }
        }
        foreach (var item in inv.ConsumablesItem)
        {
            item.Dragable = true;
        }
        foreach (var item in inv.EquipmentItem)
        {
            item.Dragable = true;
        }
    }


    /// <param name="EnemyLevel"> 1-普通，2-精英，3-boss </param>
    public void InitRewardCard(int num)
    {
        int LA, LB;
        switch (EoM.EnemyLevel)
        {
            case 1:
                LA = 3;
                LB = 37;
                break;

            case 2:
                LA = 10;
                LB = 40;
                break;

            default:
                LA = 100;
                LB = 0;
                break;
        }

        var SO = GameManager.Instance.currentCharacter.GetComponent<CharacterControl>().combineCards;
        var initCards = new List<GameObject>();

        for (int i = 0; i < num; i++)
        {
            //按稀有度随机
            int _r = UnityEngine.Random.Range(0, 100);
            if (_r < LA)
                _r = 2;
            else if (_r < LA + LB)
                _r = 1;
            else
                _r = 0;

            Debug.Log("Rare:" + _r);
            var _cards = SO.objects[_r].gameObjects;

            int _r2 = UnityEngine.Random.Range(0, _cards.Count);
            var C = _cards[_r2];
            Debug.Log("Index:" + _r2);

            //确保无重复
            if (initCards.Contains(C))
            {
                i--;
                continue;
            }
            initCards.Add(C);
        }

        //生成
        foreach (var _c in initCards)
        {
            var _newc = Instantiate(_c, CardRewardTr).GetComponent<Card>();
            _newc.inReward = true;
            
        }
    }

    public void ClearRewardCard()
    {
        foreach (var _i in CardRewardTr.GetComponentsInChildren<Card>(true))
        {
            Destroy(_i.gameObject);
        }
        CardRewardTr.gameObject.SetActive(false);
    }


    //由奖励界面关闭按钮调用
    public void BattleEnd()
    {
        phaseEvent = (battlePhase)=> { };
        ActionManager.Instance.ClearActions();
        GameManager.Instance.inBattle = false;
        GameManager.Instance.currentscene = GameManager.GameScene.Map;
        Destroy(BattleInfo.Instance.player.gameObject);
        transform.parent.gameObject.SetActive(false);
        GameManager.Instance.currentCharacter.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        ItemBack();
        

        foreach (var _c in cards_Deck.transform.GetComponentsInChildren<Card>())
        {
            Destroy(_c.gameObject);
        }
        foreach (var item in cards_Hand.transform.GetComponentsInChildren<Card>())
        {
            Destroy(item.gameObject);
        }
        foreach (var item in cards_Thrown.transform.GetComponentsInChildren<Card>())
        {
            Destroy(item.gameObject);
        }
        foreach (var item in cards_Destroyed.transform.GetComponentsInChildren<Card>())
        {
            Destroy(item.gameObject);
        }
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
        

    }


}

