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
    string anim;
    //int finnalDamage;

    public MakeDamage(Character _target,DamageInfo _info,string _anim = null)
    {
        target = _target;
        _info.commonDamage = Mathf.FloorToInt(AtDamageGive(_info));
        info = _info;
        anim = _anim;
    }
    
    public float AtDamageGive(DamageInfo info)
    {
        if (info.source == null)
            return info.commonDamage;
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
        if(anim!=null)
        {
            info.source.animator.Play(anim,0,0f);
        }
        else
        {
            ActionManager.Instance.DelayActionEnd(0.5f);
        }
        
    }
}

public class AttackAllEnemies : Actions
{
    DamageInfo info;
    string anim;

    public AttackAllEnemies(DamageInfo _info,string _anim = null)
    {
        info = _info;
        anim = _anim;
    }

    public float AtDamageGive()
    {
        if (info.source == null)
            return info.commonDamage;
        if (info.source.buffs.Count > 0)
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
        AtDamageGive();

        foreach (var enemy in BattleInfo.Instance.enemies)
        {
            enemy.GetComponent<Character>().ReceiveDamage(info);
        }

        if (anim != null)
        {
            info.source.animator.Play(anim,0,0f);
        }
        else
        {
            ActionManager.Instance.DelayActionEnd(0.7f);
        }
    }
}


public enum DamageType
{
    Null,Normal,Thorn,LoseHP
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
    private Character target,source;
    private string anim;
    bool beInfluenced;
    public MakeDefend(Character _source,Character _target,int num,bool _beInfluenced,string _anim = null)
    {
        def = num;
        target = _target;
        source = _source;
        anim = _anim;
        beInfluenced = _beInfluenced;
    }

    public int OnDefendGiven(int _def)
    {
        float val = _def;
        foreach (var _buff in target.buffs)
        {
            val = _buff.OnDefendGiven(val);
        }
        return (int)val;
    }

    public override void execute()
    {
        BattleUI.Instance.PlayDefendEffect(target.transform);
        if(beInfluenced)
        {
            target.blocks += OnDefendGiven(def);
        }
        else
        {
            target.blocks += def;
        }

        if(anim!=null)
        {
            source.animator.Play(anim,0,0f);
        }
        else
        {
            ActionManager.Instance.DelayActionEnd(0.5f);
        }
    }


}

#endregion

