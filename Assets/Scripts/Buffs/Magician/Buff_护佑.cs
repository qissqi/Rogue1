using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff_护佑 : Buff
{
    public static new string intro =
        "护佑：\n回合结束时，将护佑层数的一半转为等量的格挡。";

    public Buff_护佑(Character _target,int _num):base(_target,true,true,BuffType.Buff,GetBuffSprite("护佑"))
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
        if(phase == BattlePhase.PlayerEnd)
        {
            Execute();
        }
    }

    public void Execute()
    {
        int left = num / 2;
        int d = num - left;
        Combine_GO.transform.DOScale(Combine_GO.transform.localScale * 1.5f, 0.4f).From();
        ActionManager.Instance.ActionAddToBottom(new MakeDefend(owner,owner, d,false));
        if(owner.FindBuff<Buff_基础法阵>()!=null)
        {

        }
        else
        {
            Counter(-d);
        }
    }

    public override void Counter(int _num)
    {
        num += _num;
        if (num<=0)
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
        return intro+"\n获得"+(num-num/2)+"层格挡";
    }

    public override void RemoveBuff()
    {
        Object.Destroy(Combine_GO);
        BattleManager.Instance.phaseEvent -= CheckTurn;
        owner.buffs.Remove(this);
    }
}
