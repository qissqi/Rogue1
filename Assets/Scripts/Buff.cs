using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// buff���Ʋο���ɱ¾����
/// </summary>
public abstract class Buff:IBattleActive
{

    public static string intro = "Buff";

    public abstract string GetIntro();


    public enum BuffType
    {
        Fixed, Buff, Debuff
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="target">buff�ͷ�Ŀ��</param>
    /// <param name="_increasable">�Ƿ�ɵ���</param>
    /// <param name="_buffType">buff����</param>
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

    //����������BuffSystemʵ�� 

    protected bool effective;
    public GameObject Combine_GO;

    public virtual bool Prior()
    {
        return false;
    }


    public abstract void RemoveBuff();

    /// <summary>
    /// Effective����ֻ�����һ���ֹ��ܣ����Ϊ�غ��¼���ע��bug
    /// </summary>
    public virtual void Effective() { effective = true; }

    public virtual void Counter(int _num){ num += _num; }

    #region �˺��¼���Ӧ

    
    public virtual float AtDamageReceive(DamageInfo info)
    { return info.commonDamage; }    

    

    public virtual float AtDamageGive(DamageInfo info)
    { return info.commonDamage; }

    public void AfterCardUse(Card card)
    {
        
    }

    #endregion


}
