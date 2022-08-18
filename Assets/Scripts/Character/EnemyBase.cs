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
    /// ս����ʼ��������Ӧ�����ü����飬��ʼ��
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

    //�غϿ�ʼ��Ȼ����
    public override void LoseBlockAtStart(BattlePhase phase)
    {
        if(phase == BattlePhase.EnemyStart)
        {
            blocks = 0;
        }
    }

    /// <summary>
    /// ��Ӧ�غϣ���ͼ
    /// </summary>
    public void RespondPhase(BattlePhase phase)
    {
        //���˻غ�,�����ж�
        if(phase== (BattlePhase.EnemyTurn))
        {
            TakeAction();
        }
        //��һغϿ�ʼ,���ò���ʾ����ͼ
        else if(phase== (BattlePhase.PlayerStart))
        {
            SetCurrentIntent();
            ShowIntent();
        }
    }

    /// <summary>
    /// ���˳�ʼ�������֣���������HP
    /// </summary>
    public abstract void initialize();

    /// <summary>
    /// ���˻غϵ��ж�
    /// </summary>
    public abstract void TakeAction();

    /// <summary>
    /// չʾ/ˢ����ͼ
    /// </summary>
    public abstract void ShowIntent(bool showNumber = true);


    /// <summary>
    /// ���õ��˼�����:List enemySkill  skillGroup
    /// </summary>
    public abstract void SetSkillGroup();

    /// <summary>
    /// �����߼��趨��ǰ��ͼ
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
/// 0-δ֪,1-����,2-����,3-Buff,4-Debuff,5-����
/// </summary>
public enum EnemyIntentType
{
    Unknown,Attack,Defend,Buff,Debuff,Curse
}
