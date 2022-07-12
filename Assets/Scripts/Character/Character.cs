using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using DG.Tweening;

public abstract class Character : MonoBehaviour, IPointerClickHandler,IPointerEnterHandler,IPointerExitHandler
{
    [Header("Character-手动赋值")]
    public Slider bloodBar;
    public Text hpText, blockText;
    public GameObject selectBox;
    public GameObject GOname;
    public int HPmax;
    public Action<DamageInfo> AfterAttacked;
    public GameObject BuffArea;
    [Header("Debug")]
    public int HP;
    public int blocks;
    public float attackInfluence = 1;
    public float defenceInfluence = 1;
    public int power=0, defence=0;

    public bool CanChoose;

    public List<Buff> buffs = new List<Buff>();

    protected void Update()
    {
        //进行一个懒的偷
        FreshAllUI();

    }

    public abstract void LoseBlockAtStart(BattlePhase phase);

    #region 伤害事件

    public float AtDamageReceive(DamageInfo info)
    {
        float _damage = info.commonDamage;
        if(buffs.Count>0)
        {
            foreach (var _b in buffs)
            {
                _damage = (int)_b.AtDamageReceive(info);
            }
        }
        return _damage;
    }

    public virtual void ReceiveDamage(DamageInfo info)
    {
        //buff响应，收到伤害前
        info.commonDamage = (int)AtDamageReceive(info);

        //盾大于伤害，只打盾
        if (blocks >= info.commonDamage)
        {
            HitBlock(info.commonDamage);
        }
        //盾小于伤害，消盾打血
        else if (blocks > 0)
        {
            int blocked = blocks;
            HitBlock(blocks);
            LoseHP(info.commonDamage - blocked);
        }
        //没盾，打血
        else
        {
            LoseHP(info.commonDamage);
        }
        //buff响应，收到伤害后
        transform.DOPunchPosition(transform.position, 0.3f,1);
    }

    public void HitBlock(int defend)
    {
        blocks -= defend;
    }

    public void LoseHP(int hurtHP)
    {
        HP -= hurtHP;
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
        hpText.text = HP + "/" + HPmax;
        bloodBar.value =(float) HP / HPmax;
    }

    public void FreshBlock()
    {
        blockText.text = blocks.ToString();
        if(blocks<=0)
        {
            blockText.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            blockText.transform.parent.gameObject.SetActive(true);
        }
    }

    #endregion


    #region 光标事件
    public void OnPointerClick(PointerEventData eventData)
    {
        if(CanChoose && eventData.pointerId == -1)
        {
            BattleInfo.Instance.ChosenEnemy = this;
            BattleInfo.Instance.ChosenCard.UseCard();
        }
        selectBox.SetActive(false);
        BattleInfo.Instance.aim = null;
        
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        GOname.SetActive(true);
        if(CanChoose)
        {
            selectBox.SetActive(true);
            //有选择目标，更新描述
            BattleInfo.Instance.aim = this;
            if(BattleInfo.Instance.ChosenCard!=null)
            {
                BattleInfo.Instance.ChosenCard.RefreshDescription();
            }
        }
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        GOname.SetActive(false);
        selectBox.SetActive(false);
        //更新描述
        BattleInfo.Instance.aim = null;
        if(BattleInfo.Instance.ChosenCard!=null)
            BattleInfo.Instance.ChosenCard.RefreshDescription();
    }


    #endregion
}
