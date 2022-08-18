using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 挠胳肢窝 : Card
{

    public override void CardEffect()
    {
        ActionManager.Instance.ActionAddToBotton(new AddBuff(
            new Weak_Buff(BattleInfo.Instance.ChosenEnemy, 1)));
        ActionManager.Instance.ActionAddToBotton(new AddBuff(
            new 易伤_buff(BattleInfo.Instance.ChosenEnemy, 1)));
        ActionManager.Instance.ActionAddToBotton(new DrawCardAction(2));
    }

    public override void Initialize()
    {
        explain = Weak_Buff.intro + "\n" + 易伤_buff.intro;
    }

    public override void RefreshDescription()
    {
        text_description.text =
            "给予1层虚弱，给予1层易伤\n抽2张牌";
    }
}
