using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// buff���Ʋο���ɱ¾����
/// </summary>
public abstract class Buff:IBattleActive
{

    public static string intro = "Buff";
    public bool showNum;

    public static Sprite GetBuffSprite(int index)
    {
        return BattleUI.Instance.BuffSprites.sprites[index];
    }

    public abstract string GetIntro();


    public enum BuffType
    {
        Fixed, Buff, Debuff
    }


    /// <param name="target">buff�ͷ�Ŀ��</param>
    /// <param name="_increasable">�Ƿ�ɵ���</param>
    /// <param name="_buffType">buff����</param>
    public Buff(Character target,bool _increasable,bool _showNum,BuffType _buffType,Sprite _buffSprite)
    {
        owner = target;
        increasable = _increasable;
        buffType = _buffType;
        buffSprite = _buffSprite;
        showNum = _showNum;
    }
   
    public bool increasable;
    public Character owner;
    public BuffType buffType;
    public Sprite buffSprite;

    public int num;

    //����������BuffSystemʵ�� 

    protected bool effective;
    public GameObject Combine_GO;

    public virtual bool Prior()
    {
        return false;
    }


    public virtual void RemoveBuff()
    {
        Object.Destroy(Combine_GO);
        owner.buffs.Remove(this);
    }

    /// <summary>
    /// Effective����ֻ�����һ���ֹ��ܣ����Ϊ�غ��¼���ע��bug
    /// </summary>
    public virtual void Effective() { effective = true; }

    public virtual void PhaseCheck(BattlePhase phase) { }

    public virtual void Counter(int _num)
    {
        num += _num;
        if (num <= 0)
        {
            RemoveBuff();
        }
        else
        {
            BuffSystem.RefreshBuffUICounter(this);
        }
    }

    #region �˺��¼���Ӧ

    
    public virtual float AtDamageReceive(DamageInfo info)
    { return info.commonDamage; }    

    

    public virtual float AtDamageGive(DamageInfo info)
    { return info.commonDamage; }

    public void AfterCardUse(Card card)
    {
        
    }


    #endregion
    public virtual float OnDefendGiven(float defend, bool caculate = false)
    {
        return defend;
    }

    public virtual void OnDie()
    {
        
    }
}
