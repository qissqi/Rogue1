using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Relic_不休陀螺 : Relic
{
    public override void AfterCardUse(Card card)
    {
        if(BattleManager.Instance.cards_Hand.GetComponentsInChildren<Card>().Length == 0)
        {
            ActionManager.Instance.ActionAddToBotton(new DrawCardAction(1));
        }
    }
}
