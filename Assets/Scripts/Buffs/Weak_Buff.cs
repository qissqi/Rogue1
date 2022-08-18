using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weak_Buff : Buff
{
    public bool justApplied;
    public new static string intro = "虚弱：造成的伤害减少25%";

    public Weak_Buff(Character target, int count, bool jp=false)
        :base(target,true,BuffType.Debuff,BattleUI.Instance.BuffSprites.sprites[0])
    {
        num = count;
        justApplied = jp;
    }

    public override void Effective()
    {
        base.Effective();
        BattleManager.Instance.phaseEvent += Counter;
    }

    public override float AtDamageGive(DamageInfo info)
    {
        return info.commonDamage * 0.75f;
    }
    public void Counter(BattlePhase phase)
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
        Debug.Log("Buff Left:\n" + num);
        if(num<=0)
        {
            Debug.Log("Remove Buff");
            RemoveBuff();
        }
        BuffSystem.RefreshBuffUICounter(this);
    }

    public override void RemoveBuff()
    {
        owner.buffs.Remove(this);
        BattleManager.Instance.phaseEvent -= Counter;
        Object.Destroy(Combine_GO);
    }

    public override string GetIntro()
    {
        return intro;
    }
}
