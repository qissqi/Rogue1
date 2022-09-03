using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff_Chain_力量 : Buff
{
    bool JA;
    public Buff_Chain_力量(Character _target,int amount,bool justApplied) : base(_target,true,true,
        amount>0?BuffType.Buff:BuffType.Debuff,GetBuffSprite("Chain"))
    {
        num = amount;
        JA = justApplied;
    }

    public override void Counter(int _num)
    {
        num += _num;
        if (num == 0)
            RemoveBuff();
        buffType = num > 0 ? BuffType.Buff : BuffType.Debuff;
    }

    public override void Effective()
    {
        base.Effective();
        BattleManager.Instance.phaseEvent += PhaseCheck;
    }

    public override void PhaseCheck(BattlePhase phase)
    {
        if (phase == BattlePhase.PlayerStart&& !JA)
        {
            ActionManager.Instance.ActionAddToBottom(new AddBuff(new Buff_力量(owner, num)));
            RemoveBuff();
        }
    }

    public override void RemoveBuff()
    {
        base.RemoveBuff();
        BattleManager.Instance.phaseEvent -= PhaseCheck;
    }

    public override string GetIntro()
    {
        return $"回合结束时，{(num > 0 ? "获得" : "失去") }{Mathf.Abs(num)}点力量。";
    }
}
