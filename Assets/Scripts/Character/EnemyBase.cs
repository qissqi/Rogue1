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
    /// ս����ʼ��������Ӧ�����ü����飬��ʼ��
    /// </summary>

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
        if(phase == BattlePhase.EnemyTurn)
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
            SetCurrentSkill();
            ShowIntent();
        }
    }

    /// <summary>
    /// ���˳�ʼ��
    /// </summary>
    public abstract void initialize();

    /// <summary>
    /// ���˻غϵ��ж�
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
    /// չʾ/ˢ����ͼ
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
    /// ���õ��˼�����:List enemySkill  skillGroup
    /// </summary>
    public abstract void SetSkillGroup();

    /// <summary>
    /// �����߼��趨��ǰ������ͼ
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
/// 0-δ֪,1-����,2-����,3-Buff,4-Debuff,5-����
/// </summary>
public enum EnemyIntentType
{
    Unknown,Attack,Defend,Buff,Debuff,Curse,AttackBuff,AttackDebuff,AttackDefend,DefendBuff,Sleep,Stun,Magic
}
