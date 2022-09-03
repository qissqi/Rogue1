using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using DG.Tweening;


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
    public GameObject BuffBlockPre;
    [HideInInspector]public BattlePhase battlePhase;
    public GameObject RewardPanel;
    [HideInInspector]public bool battleStart;
    public event Action<BattlePhase> phaseEvent;
    public GameObject cards_Hand, cards_Deck, cards_Thrown, cards_Destroyed, cards_Pack;
    public Transform Equipment, Consumables, Relic;
    public Transform CardRewardTr;
    [HideInInspector]public Enemy_OnMap EoM;
    public Text GoldsRewardText;
    public GameObject FailPanel;
    public GameObject TutorialPanel;
    private int goldsReward;

    //public event Action InBattleStart, InBattleEnd;

    /// <summary>
    /// 启用时初始化
    /// </summary>
    public void Start()
    {
        //cards_Hand = GameObject.FindGameObjectWithTag("Hand");
        //cards_Deck = GameObject.FindGameObjectWithTag("Deck");
        //cards_Deck.SetActive(false);
        //cards_Thrown = GameObject.FindGameObjectWithTag("Thrown");
        //cards_Thrown.SetActive(false);
        //cards_Destroyed = GameObject.FindGameObjectWithTag("Destroyed");
        //cards_Destroyed.SetActive(false);
        //cards_Pack = GameObject.FindGameObjectWithTag("CardPack").transform.GetChild(0).GetChild(0).gameObject;
        //cards_Pack = GameObject.Find("MapUI").transform.GetChild(1).gameObject;
        transform.parent.gameObject.SetActive(false);
    }

    /// <summary>
    /// 获取基本组件信息与生成
    /// </summary>
    /// 

    public void ReadyStart(Enemy_OnMap _eom)
    {
        EoM = _eom;
        SoundManager.Instance.PlaySE("BattleStart");
        SoundManager.Instance.PlayBGM("Anxiety_Front");
        GameManager.Instance.inBattle = true;
        GameManager.Instance.ChangeScene(GameManager.GameScene.Battle);

        Camera.main.DOOrthoSize(2.5f, 1).SetDelay(0.3f).OnComplete(() =>
        {
            Camera.main.orthographicSize = 5;
            BattleStart();
        });
    }

    public void BattleStart()
    {
        GameManager.Instance.currentCharacter.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        var battleCanvas = Instance.transform.parent.gameObject;
        battleCanvas.SetActive(true);
        battleCanvas.GetComponent<Canvas>().worldCamera = Camera.main;
        var PP = battleCanvas.transform.Find("BattleScene/Player");
        var EP = battleCanvas.transform.Find("BattleScene/Enemies");
        Instantiate(GameManager.Instance.currentCharacter.BattlePlayer, PP);

        BattleInfo.Instance.enemyNum = EoM.combineEnemyPre.Length;
        for (int i = 0; i < EoM.combineEnemyPre.Length; i++)
        {

            var e = Instantiate(EoM.combineEnemyPre[i], EP);
            e.GetComponent<RectTransform>().anchoredPosition = new Vector2(-i * 150, 0);
            if(i>=2)
            {
                e.GetComponent<RectTransform>().anchoredPosition = new Vector2(-(4 - i) * 150, 150);
            }
            e.GetComponent<EnemyBase>().BattleStart();
        }

        Instance.InitBattle();
    }

    /// <summary>
    /// 初始化敌人，玩家与卡牌
    /// </summary>
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
        ActionManager.Instance.ActionAddToBottom(new EndPhase(BattlePhase.BattleStart));
        battleStart = true;

        if(!GameManager.Instance.tutorial)
        {
            GameManager.Instance.tutorial = true;
            TutorialPanel.SetActive(true);
        }

    }


    private void Update()
    {
        if (!battleStart)
            return;
        DeveloperMode();
        if(BattleInfo.Instance.enemyNum == 0)
        {
            BattleVictory();
        }

    }

    public void BattleVictory()
    {
        BattleInfo.Instance.player.SaveToInfo();
        battleStart = false;
        DG.Tweening.DOTween.KillAll();
        EoM.BattleEnd(true);

        //生成奖励
        InitRewardCard(3);
        InitGolds();

        CardRewardTr.gameObject.SetActive(true);
        RewardPanel.SetActive(true);

        //关闭标记
        MapManager.Instance.currentRoom.Mark.transform.Find("Enemy").gameObject.SetActive(false);
        MapManager.Instance.currentRoom.Mark.transform.Find("Elite").gameObject.SetActive(false);
        //MapManager.Instance.currentRoom.Mark.transform.Find("Boss").gameObject.SetActive(false);

        //音效
        SoundManager.Instance.PlaySE("Clear");
        SoundManager.Instance.StopBGM();
    }

    #region 背包导入
    public void ItemLoad()
    {
        InventoryManager inv = InventoryManager.Instance;
        foreach (var item in inv.EquipmentItem)
        {
            var scale = item.transform.parent.localScale;
            item.transform.parent.SetParent(Equipment);
            item.transform.parent.localScale = scale;
            item.Dragable = false;
            var eq = item as Equipment;
            BattleInfo.Instance.player.buffs.Add(eq);
            item.GetComponent<Equipment>().OnBattleLoad();
        }
        foreach (var item in inv.ConsumablesItem)
        {
            var scale = item.transform.parent.localScale;
            item.transform.parent.SetParent(Consumables);
            item.transform.parent.localScale = scale;
            item.Dragable = false;
            item.GetComponent<ConsumableItem>().OnBattleLoad();
        }
        foreach (var item in inv.InventoryItems)
        {
            if(item.type == Item.ItemType.Relic)
            {

                var scale = item.transform.parent.localScale;
                item.transform.parent.SetParent(Relic);
                item.transform.parent.localScale = scale;
                item.Dragable = false;
                var rl = item as Relic;
                BattleInfo.Instance.player.buffs.Add(rl);
                item.GetComponent<Relic>().OnBattleLoad();
            }
        }
    }

    public void ItemBack()
    {
        InventoryManager inv = InventoryManager.Instance;
        for (int i = Equipment.childCount; i >0 ; i--)
        {
            var item = Equipment.GetChild(0);
            item.GetChild(0).GetComponent<Equipment>().OnBattleEnd();
            var scale = item.localScale;
            item.SetParent(inv.EquipmentPanel.transform);
            item.localScale = scale;

        }
        for (int i = Consumables.childCount; i >0; i--)
        {
            var item = Consumables.GetChild(0);
            item.GetChild(0)?.GetComponent<ConsumableItem>()?.OnBattleEnd();
            var scale = item.localScale;
            item.SetParent(inv.CarryPanel.transform);
            item.localScale = scale;
            item.GetComponent<Item>().OriginParentReset();
        }
        for (int i = Relic.childCount; i >0 ; i--)
        {
            var item = Relic.GetChild(0);
            item.GetChild(0).GetComponent<Relic>().OnBattleEnd();
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

    #endregion

    /// <param name="EnemyLevel"> 1-普通，2-精英，3-boss </param>
    public void InitRewardCard(int num)
    {
        int LA, LB;
        int _t = 1;
        switch (/*EoM.EnemyLevel*/_t)
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

        var SO = GameManager.Instance.currentCharacter.combineCards;
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

            var _cards = SO.objects[_r].gameObjects;

            int _r2 = UnityEngine.Random.Range(0, _cards.Count);
            var C = _cards[_r2];

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

    public void InitGolds()
    {
        goldsReward = EoM.EnemyLevel * 25 + UnityEngine.Random.Range(0, 40);
        GoldsRewardText.text = goldsReward.ToString();
        GoldsRewardText.transform.parent.gameObject.SetActive(true);
    }
    public void GetGolds()
    {
        GameManager.Instance.playerInfo.Golds += goldsReward;
        GoldsRewardText.transform.parent.gameObject.SetActive(false);
    }


    //由奖励界面关闭按钮调用
    public void BattleEnd()
    {
        phaseEvent = (battlePhase)=> { };
        ActionManager.Instance.ClearActions();
        GameManager.Instance.inBattle = false;
        GameManager.Instance.ChangeScene(GameManager.GameScene.Map);
        
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

        MapManager.Instance.RefreshUI();
        SaveManager.Instance.SaveBasic();
        SoundManager.Instance.PlayBGM("F1_Loop");
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
            ActionManager.Instance.ActionAddToBottom(new EndPhase());
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
            var card = Instantiate(_c.gameObject, cards_Deck.transform);
            card.GetComponent<Card>().inShow = true;

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

    public void BackToTitle()
    {
        GameManager.Instance.ChangeScene(GameManager.GameScene.Menu);
        GameManager.AsyncLoadScene(1);
    }

    public void OpenFailPanel()
    {
        FailPanel.SetActive(true);
        FailPanel.transform.GetChild(0).DOScale(0, 0.25f).From();
    }


    /// <summary>
    /// 开发者功能区
    /// </summary>
    public void DeveloperMode()
    {
        

    }


}

