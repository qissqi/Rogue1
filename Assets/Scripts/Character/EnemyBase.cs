using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public abstract class EnemyBase : Character
{
    [Header("EnemyBase")]
    public int ATK;

    public SpriteList IntentList;
    public Image IntentImage;
    public Text IntentText;

    public delegate void enemySkillDele();
    public EnemySkill curSkill;

    [Header("Debug")]
    public GameObject GO_;
    public void Awake()
    {
        selectBox = GetComponentsInChildren<Image>(true)[1].gameObject;
        GO_ = gameObject;
    }

    /// <summary>
    /// 战斗开始：订阅响应，设置技能组，初始化
    /// </summary>
    public void Start()
    {
        //BattleManager.Instance.phaseEvent += RespondPhase;
        //BattleManager.Instance.phaseEvent += LoseBlockAtStart;
        //SetSkillGroup();
        //initialize();
    }

    public void BattleStart()
    {
        BattleManager.Instance.phaseEvent += RespondPhase;
        BattleManager.Instance.phaseEvent += LoseBlockAtStart;
        SetSkillGroup();
        initialize();
    }

    //回合开始自然消盾
    public override void LoseBlockAtStart(BattlePhase phase)
    {
        if(phase == BattlePhase.EnemyStart)
        {
            blocks = 0;
        }
    }

    /// <summary>
    /// 响应回合，意图
    /// </summary>
    public void RespondPhase(BattlePhase phase)
    {
        //敌人回合,进行行动
        if(phase== (BattlePhase.EnemyTurn))
        {
            TakeAction();
        }
        //玩家回合开始,设置并显示新意图
        else if(phase== (BattlePhase.PlayerStart))
        {
            SetCurrentIntent();
            ShowIntent();
        }
    }

    /// <summary>
    /// 敌人初始化，名字，攻击力，HP
    /// </summary>
    public abstract void initialize();

    /// <summary>
    /// 敌人回合的行动
    /// </summary>
    public abstract void TakeAction();

    /// <summary>
    /// 展示/刷新意图
    /// </summary>
    public abstract void ShowIntent(bool showNumber = true);


    /// <summary>
    /// 设置敌人技能组:List enemySkill  skillGroup
    /// </summary>
    public abstract void SetSkillGroup();

    /// <summary>
    /// 设置逻辑设定当前意图
    /// </summary>
    public abstract void SetCurrentIntent();

    public virtual void OnEnemyDie()
    {

    }

    public override void Die()
    {
        OnEnemyDie();
        BattleInfo.Instance.enemies.Remove(gameObject);
        Destroy(gameObject);
    }

}
/// <summary>
/// 0-未知,1-攻击,2-防御,3-Buff,4-Debuff,5-诅咒
/// </summary>
public enum EnemyIntentType
{
    Unknown,Attack,Defend,Buff,Debuff,Curse
}
