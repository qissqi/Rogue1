using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using DG.Tweening;

public abstract class Character : MonoBehaviour, IPointerClickHandler,IPointerEnterHandler,IPointerExitHandler
{
    [Header("Character-�ֶ���ֵ")]
    public Animator animator;
    public Slider bloodBar;
    public Text hpText, blockText;
    public GameObject selectBox;
    public GameObject GOname;
    public int maxHP;
    public Action<DamageInfo> AfterAttacked;
    public GameObject BuffArea;
    [Header("Debug")]
    public int hp;
    public int HP
    {
        set { hp = value > maxHP ? maxHP : value; }
        get => hp;
    }
    public int blocks;
    //public float attackInfluence = 1;
    //public float defenceInfluence = 1;
    //public int power=0, defence=0;

    public bool CanChoose;
    public bool dead;

    public List<IBattleActive> buffs = new List<IBattleActive>();

    protected void Update()
    {
        //����һ������͵
        FreshAllUI();

    }
    public Buff FindBuff(Buff _buff)
    {
        foreach (var _b in buffs)
        {
            if (_b.GetType() == _buff.GetType())
            {
                return (Buff)_b;
            }
        }
        return null;
    }

    public Buff FindBuff(string buff_fullName)
    {
        foreach (var _b in buffs)
        {
            if (_b.GetType().ToString() == buff_fullName)
            {
                return (Buff)_b;
            }
        }
        return null;
    }
    public Buff FindBuff<T>() where T:Buff
    {
        foreach (var _b in buffs)
        {
            if (_b is T)
            {
                return (Buff)_b;
            }
        }
        return null;
    }

    public void ActionEnd()
    {
        ActionManager.Instance.ActionEnd();
        
    }

    public abstract void LoseBlockAtStart(BattlePhase phase);

    #region �˺��¼�

    public float AtDamageReceive(DamageInfo info)
    {
        //float _damage = info.commonDamage;
        if(buffs.Count>0)
        {
            foreach (var _b in buffs)
            {
                info.commonDamage = Mathf.FloorToInt(_b.AtDamageReceive(info));
            }
        }
        return info.commonDamage;
    }

    public virtual void ReceiveDamage(DamageInfo info)
    {
        
        //buff��Ӧ���յ��˺�ǰ
        info.commonDamage = Mathf.FloorToInt(AtDamageReceive(info));
        if (info.commonDamage < 0)
            info.commonDamage = 0;

        BattleUI.Instance.ShowHit(transform, info.commonDamage.ToString(), Color.red);
        animator.Play("Hurt");
        SoundManager.Instance.PlaySE("Hit");
        //�ܴ����˺���ֻ���
        if (blocks >= info.commonDamage)
        {
            HitBlock(info.commonDamage);
        }
        //��С���˺������ܴ�Ѫ
        else if (blocks > 0)
        {
            int blocked = blocks;
            HitBlock(blocks);
            LoseHP(info.commonDamage - blocked);
        }
        //û�ܣ���Ѫ
        else
        {
            LoseHP(info.commonDamage);
        }

        
    }

    public void HitBlock(int defend)
    {
        blocks -= defend;
    }

    public void LoseHP(int hurtHP)
    {
        HP -= hurtHP;
        if(hp<=0)
        {
            OnDie();
        }
    }

    public virtual void OnDie()
    {
        for (int i=0;i<buffs.Count;i++)
        {
            if(hp<=0)
            {
                buffs[i].OnDie();
            }
            else
            {
                break;
            }
        }

        if(hp<=0)
        { 
            Die();
        }
    }

    #endregion

    #region UI

    public void FreshAllUI()
    {
        FreshHP();
        FreshBlock();
    }

    public void FreshHP()
    {
        hpText.text = HP + "/" + maxHP;
        bloodBar.value =(float) HP / maxHP;
        if (dead)
            hpText.text = "������";
    }

    public void FreshBlock()
    {
        blockText.text = blocks.ToString();
        if (dead)
            return;
        if(blocks<=0)
        {
            blockText.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            blockText.transform.parent?.gameObject.SetActive(true);
        }
    }

    #endregion

    public abstract void Die();

    

    #region ����¼�
    public void OnPointerClick(PointerEventData eventData)
    {
        if(CanChoose && eventData.pointerId == -1)
        {
            BattleInfo.Instance.ChosenEnemy = this;
            BattleInfo.Instance.ChosenCard.UseCardFromHand();
        }
        selectBox.SetActive(false);
        BattleInfo.Instance.aim = null;
        
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        if (dead)
            return;
        GOname.SetActive(true);
        if(CanChoose)
        {
            selectBox.SetActive(true);
            //��ѡ��Ŀ�꣬��������
            BattleInfo.Instance.aim = this;
            if(BattleInfo.Instance.ChosenCard!=null)
            {
                BattleInfo.Instance.ChosenCard.RefreshDescription();
            }
        }
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        if (dead)
            return;
        GOname.SetActive(false);
        selectBox.SetActive(false);
        //��������
        BattleInfo.Instance.aim = null;
        if(BattleInfo.Instance.ChosenCard!=null)
            BattleInfo.Instance.ChosenCard.RefreshDescription();
    }


    #endregion
}
