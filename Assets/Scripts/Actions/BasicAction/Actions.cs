using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// 命令模式，类中要进行方法的具体实现
/// </summary>
public abstract class Actions
{
    public abstract void execute();

    public void EndCall()
    {
        ActionManager.Instance.onAction = false;
        
    }
}

#region 回合结束
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

#region 攻击
public class MakeDamage : Actions
{
    Character target;
    DamageInfo info;
    //int finnalDamage;

    public MakeDamage(Character _target,DamageInfo _info)
    {
        target = _target;
        //buff响应，伤害给予前
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

#region 防御
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

