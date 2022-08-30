using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff_基础法阵 : Buff
{
    public static new string intro =
        "基础法阵：\n护佑在本回合结束时不会减少";

    public Buff_基础法阵(Character _target,int _num) : base(_target, true,false, BuffType.Buff, BattleUI.Instance.BuffSprites.sprites[0])
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
        if(phase == BattlePhase.PlayerStart)
        {
            BattleManager.Instance.phaseEvent -= CheckTurn;
            RemoveBuff();
        }
    }

    

    public override string GetIntro()
    {
        return intro;
    }
}
