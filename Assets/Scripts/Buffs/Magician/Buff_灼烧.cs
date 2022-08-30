using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Buff_灼烧 : Buff
{
    public static new string intro = "灼烧：\n在回合开始时，受到灼烧层数一半的伤害，灼烧层数减少一半。";

    public Buff_灼烧(Character _target,int _num):base(_target,true,true,BuffType.Debuff, BattleUI.Instance.BuffSprites.sprites[0])
    {
        num = _num;   
    }

    public override void Effective()
    {
        base.Effective();
        BattleManager.Instance.phaseEvent += CheckTurn;
    }

    public void CheckTurn(BattlePhase phase)
    {
        if(phase == BattlePhase.EnemyStart)
        {
            Execute();
        }
    }

    public void Execute()
    {
        Combine_GO.transform.DOScale(Combine_GO.transform.localScale * 1.5f, 0.4f).From();
        int left = num / 2;
        int burn = num - left;
        if(BattleInfo.Instance.player.FindBuff<Buff_净化之炎>()!=null)
        {
            ActionManager.Instance.ActionAddToBottom(new MakeDamage(owner,
                new DamageInfo(null, num)));
        }
        else
        {
            ActionManager.Instance.ActionAddToBottom(new MakeDamage(owner,
                new DamageInfo(null, burn)));
        }
        
        Counter(-burn);
    }

    public override void Counter(int _num)
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

    public override string GetIntro()
    {
        return intro + "\n造成"+(num - num/2)+"伤害";
    }

    public override void RemoveBuff()
    {
        owner.buffs.Remove(this);
        BattleManager.Instance.phaseEvent -= CheckTurn;
        Object.Destroy(Combine_GO);
    }
}
