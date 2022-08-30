using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_法阵制作 : Card
{
    int huyou = 6;

    public override void CardEffect()
    {
        ActionManager.Instance.ActionAddToBottom(new AddBuff(
            new Buff_护佑(BattleInfo.Instance.player, huyou), BattleInfo.Instance.player, "Skill1"));
        ActionManager.Instance.ActionAddToBottom(new AddBuff(
            new Buff_基础法阵(BattleInfo.Instance.player, 1)));
    }

    public override void Initialize()
    {
        explain = Buff_护佑.intro+"\n"+Buff_基础法阵.intro;
    }

    public override void RefreshDescription()
    {

        text_description.text =
            "获得" + huyou + "层护佑，护佑在本回合不会减少";
    }
}
