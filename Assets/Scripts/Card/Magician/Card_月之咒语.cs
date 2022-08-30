using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_月之咒语 : Card
{
    int block = 12;
    public override void CardEffect()
    {
        ActionManager.Instance.ActionAddToBottom(new MakeDefend(BattleInfo.Instance.player, BattleInfo.Instance.player, block, true, "Skill1"));
        ActionManager.Instance.ActionAddToBottom(new AddBuff(new Buff_护佑(
            BattleInfo.Instance.player, -4)));
    }

    public override bool CheckUse()
    {
        var buff = BattleInfo.Instance.player.FindBuff<Buff_护佑>();
        if(buff==null)
        {
            return false;
        }
        else if(buff.num<4)
        {
            return false;
        }
        return true;
    }

    public override void Initialize()
    {
        explain = Buff_护佑.intro;
    }

    public override void RefreshDescription()
    {
        int _b = BattleInfo.Instance.CaculateDefendValue(block, BattleInfo.Instance.player);
        text_description.text =
            "消耗4层护佑,获得" + _b + "层格挡";
    }
}
