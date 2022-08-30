using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defend : Card
{
    int Defendval;

    public override void Initialize()
    {
        Defendval = 5;
    }

    public override void CardEffect()
    {
        ActionManager.Instance.ActionAddToBottom(new MakeDefend(BattleInfo.Instance.player, BattleInfo.Instance.player, Defendval,true,"Skill1"));
    }

    public override void RefreshDescription()
    {
        int d = BattleInfo.Instance.CaculateDefendValue(Defendval, BattleInfo.Instance.player);
        text_description.text =
            "获得" + d + "点格挡";
    }
}
