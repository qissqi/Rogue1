using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_强焰 : Card
{
    public override void CardEffect()
    {
        ActionManager.Instance.ActionAddToBottom(new AddBuff(
            new Buff_净化之炎(BattleInfo.Instance.player), BattleInfo.Instance.player, "Skill1"));
    }

    public override void Initialize()
    {
        explain = Buff_净化之炎.intro;
    }

    public override void RefreshDescription()
    {
        text_description.text =
            "获得净化之炎";
    }
}
