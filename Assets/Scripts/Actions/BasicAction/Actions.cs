using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// ����ģʽ������Ҫ���з����ľ���ʵ��
/// </summary>
public abstract class Actions
{
    public abstract void execute();

    public void EndCall()
    {
        ActionManager.Instance.onAction = false;
        
    }
}

#region �غϽ���
public class EndPhase : Actions
{
    public BattlePhase newPhase;
    public bool usePhase = false;

    public EndPhase(){ }

    public EndPhase(BattlePhase _newPhase) { newPhase = _newPhase;usePhase = true; }
    public override void execute()
    {
        if (!usePhase)
        {
            BattleManager.Instance.PhaseChange();
        }
        else
        {
            BattleManager.Instance.PhaseChange(newPhase);
        }
        EndCall();
    }
}

#endregion

#region ����
public class MakeDamage : Actions
{
    Character target;
    DamageInfo info;
    //int finnalDamage;

    public MakeDamage(Character _target,DamageInfo _info)
    {
        target = _target;
        //buff��Ӧ���˺�����ǰ
        _info.commonDamage = Mathf.FloorToInt(AtDamageGive(_info));
        info = _info;
    }
    
    public float AtDamageGive(DamageInfo info)
    {
        //float _damage = info.commonDamage;
        if(info.source.buffs.Count>0)
        {
            foreach (var _b in info.source.buffs)
            {
                info.commonDamage = (int)_b.AtDamageGive(info);
            }
        }
        return info.commonDamage;
    }

    public override void execute()
    {
        target.ReceiveDamage(info);
        info.source.transform.DOPunchPosition(-info.source.transform.position, 0.5f, 1).OnComplete(EndCall);
    }
}
public enum DamageType
{
    Normal,Thorn,LoseHP
}
public class DamageInfo
{
    public Character source;
    public int commonDamage;
    public DamageType damageType;
    public DamageInfo(Character _source,int _damage,DamageType _damageType = DamageType.Normal)
    {
        source = _source;
        commonDamage = _damage;
        damageType = _damageType;
    }
}

#endregion

#region ����
public class MakeDefend : Actions
{
    private int def;
    private Character target;
    public MakeDefend(Character _target,int num) { def = num;target = _target; }

    public override void execute()
    {
        target.blocks += def;
        EndCall();
    }


}

#endregion

