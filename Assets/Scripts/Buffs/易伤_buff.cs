using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 易伤_buff : Buff
{
    bool justApplied;
    public 易伤_buff(Character target,int counts,bool jp=false)
        :base(target,true,BuffType.Debuff,BattleUI.Instance.BuffSprites.sprites[1])
    {
        num = counts;
        justApplied = jp;
    }

    public override void Effective()
    {
        base.Effective();
        BattleManager.Instance.phaseEvent += PhaseCount;

    }

    public override float AtDamageReceive(DamageInfo info)
    {
        return info.commonDamage * 1.5f;
    }

    public void PhaseCount(BattlePhase phase)
    {
        if (phase == BattlePhase.EnemyEnd)
        {
            if (justApplied)
            {
                justApplied = false;
            }
            else
            {
                Counter(-1);
            }
        }
    }

    public override void Counter(int _num)
    {
        num += _num;
        BuffSystem.RefreshBuffUICounter(this);
        if(num<=0)
        {
            RemoveBuff();
        }
    }

    public override void RemoveBuff()
    {
        BattleManager.Instance.phaseEvent -= PhaseCount;
        Object.Destroy(Combine_GO);
        owner.buffs.Remove(this);
    }
}
