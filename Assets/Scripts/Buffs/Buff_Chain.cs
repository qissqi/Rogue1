using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff_Chain : Buff
{
    public string targetBuff;
    private bool JA;

    public Buff_Chain(Character _target,string aimBuff,int amount,bool _justApplied) : base(_target,false,true,amount>0?BuffType.Buff:BuffType.Debuff,
        GetBuffSprite(0))
    {
        num = amount;
        targetBuff = aimBuff;
        JA = _justApplied;
    }


    public override void Effective()
    {
        base.Effective();
        BattleManager.Instance.phaseEvent += PhaseCheck;
        foreach (var buff in owner.buffs)
        {
            
            if(buff.GetType() == GetType() && buff!=this)
            {
                var chain = buff as Buff_Chain;
                //找到重复
                if(chain.targetBuff == targetBuff)
                {
                    chain.Counter(num);
                    BattleManager.Instance.phaseEvent -= PhaseCheck;
                    owner.buffs.Remove(this);
                    owner = null;
                    return;
                }
            }
        }
    }

    public override void PhaseCheck(BattlePhase phase)
    {
        if(phase == BattlePhase.PlayerStart)
        {
            if(JA)
            {
                JA = false;
            }
            else
            {
                owner.FindBuff(targetBuff)?.Counter(num);
                RemoveBuff();
            }
        }
    }

    public override void RemoveBuff()
    {
        BattleManager.Instance.phaseEvent -= PhaseCheck;
        base.RemoveBuff();
        
    }

    public override void Counter(int _num)
    {
        num += _num;
        if (num == 0)
        {
            RemoveBuff();
        }
        else
        {
            if (num > 0)
            {
                buffType = BuffType.Buff;
            }
            else
            {
                buffType = BuffType.Debuff;
            }
            BuffSystem.RefreshBuffUICounter(this);
        }

    }

    public override string GetIntro()
    {
        var name = targetBuff.Split('_')[1];
        var b = num > 0 ? "增加" : "减少";
        var str = "回合结束时" + b + Mathf.Abs(num) + "层" + name;
        return str;
    }
}
