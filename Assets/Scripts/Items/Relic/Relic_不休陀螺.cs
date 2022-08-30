using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Relic_不休陀螺 : Relic
{
    public override void AfterCardUse(Card card)
    {
            ActionManager.Instance.ActionAddToBottom(new Action_CommonUsed(Draw));
    }

    public void Draw()
    {
        if(BattleManager.Instance.cards_Hand.GetComponentsInChildren<Card>().Length == 0)
        {
            ActionManager.Instance.ActionAddToHead(new DrawCardAction(1));
        }
        ActionManager.Instance.ActionEnd();
    }

    public override string GetIntro()
    {
        var _intro = "不休陀螺：\n当手中没有牌时，抽两张牌";
        return _intro + explainInfo;
    }
}
