using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// buff机制参考自杀戮尖塔
/// </summary>
public abstract class Buff
{
    public enum BuffType
    {
        Fixed, Buff, Debuff
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="target">buff释放目标</param>
    /// <param name="_increasable">是否可叠加</param>
    /// <param name="_buffType">buff种类</param>
    public Buff(Character target,bool _increasable,BuffType _buffType,Sprite _buffSprite)
    {
        owner = target;
        increasable = _increasable;
        buffType = _buffType;
        buffSprite = _buffSprite;
    }
   
    public bool increasable;
    public Character owner;
    public BuffType buffType;
    public Sprite buffSprite;
    
    public int num;

    //以下属性由BuffSystem实现 

    protected bool effective;
    public GameObject Combine_GO;

    public abstract void RemoveBuff();

    /// <summary>
    /// Effective可能只会关联一部分功能，大多为回合事件，注意bug
    /// </summary>
    public virtual void Effective() { effective = true; }

    public virtual void Counter(int _num){ num += _num; }

    #region 伤害事件响应

    /// <summary>
    /// 收到伤害前
    /// </summary>
    public virtual float AtDamageReceive(DamageInfo info)
    { return info.commonDamage; }    

    

    /// <summary>
    /// 伤害给予前
    /// </summary>
    public virtual float AtDamageGive(DamageInfo info)
    { return info.commonDamage; }

    #endregion


}
