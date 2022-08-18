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
        ActionManager.Instance.ActionAddToBotton(new MakeDefend(BattleInfo.Instance.player, Defendval));
    }

    public override void RefreshDescription()
    {
        
    }
}
