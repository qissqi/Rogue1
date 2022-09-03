using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using DG.Tweening;

public abstract class EnemyBase : Character
{
    [Header("EnemyBase")]

    public Image IntentImage;
    public Text IntentText;

    public delegate void enemySkillDele();
    public EnemySkill curSkill;

    [Header("Debug")]
    public GameObject GO_;
    public void Awake()
    {
        //selectBox = GetComponentsInChildren<Image>(true)[1].gameObject;
        GO_ = gameObject;
        hp = maxHP;
    }

    /// <summary>
    /// 战斗开始：订阅响应，设置技能组，初始化
    /// </summary>

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
        if(phase == BattlePhase.EnemyTurn)
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
            SetCurrentSkill();
            ShowIntent();
        }
    }

    /// <summary>
    /// 敌人初始化
    /// </summary>
    public abstract void initialize();

    /// <summary>
    /// 敌人回合的行动
    /// </summary>
    public virtual void TakeAction()
    {
        curSkill.TakeSkill();
    }

    public virtual void SetCurrentSkill(EnemySkill skill)
    {
        curSkill = skill;
    }

    /// <summary>
    /// 展示/刷新意图
    /// </summary>
    public virtual void ShowIntent(bool showNumber = true)
    {
        if (curSkill == null)
            return;
        IntentText.text = "";
        UnityEngine.UI.Image image = IntentImage;
        SpriteList _sp = BattleUI.Instance.EnemyIntentSprites;
        bool isAttack = false;
        switch (curSkill.intentType)
        {
            case EnemyIntentType.Unknown:
                IntentImage.sprite = _sp.sprites[15];
                break;

            case EnemyIntentType.Attack:
                IntentImage.sprite = _sp.sprites[0];isAttack = true;
                break;
            case EnemyIntentType.AttackBuff:
                IntentImage.sprite = _sp.sprites[1];isAttack = true;break;
            case EnemyIntentType.AttackDebuff:
                IntentImage.sprite = _sp.sprites[2];isAttack = true;break;
            case EnemyIntentType.AttackDefend:
                IntentImage.sprite = _sp.sprites[3];isAttack = true;break;

            case EnemyIntentType.Defend:
                IntentImage.sprite = _sp.sprites[7];
                break;

            case EnemyIntentType.Buff:
                IntentImage.sprite = _sp.sprites[4];
                break;

            case EnemyIntentType.Debuff:
                IntentImage.sprite = _sp.sprites[5];
                break;

            case EnemyIntentType.Curse:
                IntentImage.sprite = _sp.sprites[6];
                break;

            case EnemyIntentType.DefendBuff:
                IntentImage.sprite = _sp.sprites[8];
                break;

            case EnemyIntentType.Magic:
                IntentImage.sprite = _sp.sprites[10];
                break;

            case EnemyIntentType.Sleep:
                IntentImage.sprite = _sp.sprites[12];
                break;

            case EnemyIntentType.Stun:
                IntentImage.sprite = _sp.sprites[14];
                break;


            default:
                IntentImage.sprite = _sp.sprites[15];
                break;
        }

        if(isAttack)
        {
            var _d = BattleInfo.Instance.CaculateEnemyDamage(curSkill.basicDamage, this);
            IntentText.text = _d.ToString();
            if (curSkill.times > 1)
            {
                IntentText.text += "x" + curSkill.times;
            }
        }

        image.SetNativeSize();
    }



    /// <summary>
    /// 设置敌人技能组:List enemySkill  skillGroup
    /// </summary>
    public abstract void SetSkillGroup();

    /// <summary>
    /// 设置逻辑设定当前技能意图
    /// </summary>
    public abstract void SetCurrentSkill();

    public override void Die()
    {
        BattleManager.Instance.phaseEvent -= RespondPhase;
        BattleManager.Instance.phaseEvent -= LoseBlockAtStart;
        BattleInfo.Instance.enemies.Remove(gameObject);
        for (int i = buffs.Count-1; i >=0; i--)
        {
            if(buffs[i] is Buff)
            {
                var b = buffs[i] as Buff;
                b.RemoveBuff();
            }
        }
        buffs.Clear();
        dead = true;

        Destroy(IntentImage.gameObject);
        Destroy(blockText.transform.parent.gameObject);
        Destroy(selectBox);

        GetComponentInChildren<Image>().DOColor(new Color(0, 0, 0, 0), 1f);
        transform.DOShakePosition(1, transform.right).OnComplete(()=>
        {
            BattleInfo.Instance.enemyNum--;
            DestroyImmediate(gameObject);
        });
    }

}
/// <summary>
/// 0-未知,1-攻击,2-防御,3-Buff,4-Debuff,5-诅咒
/// </summary>
public enum EnemyIntentType
{
    Unknown,Attack,Defend,Buff,Debuff,Curse,AttackBuff,AttackDebuff,AttackDefend,DefendBuff,Sleep,Stun,Magic
}
