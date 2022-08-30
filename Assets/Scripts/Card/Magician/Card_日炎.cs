using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_日炎 : Card
{
    int burn = 5;
    public override void CardEffect()
    {
        int i = 0;
        foreach (var e in BattleInfo.Instance.enemies)
        {
            if(i==0)
            {
                i++;
                ActionManager.Instance.ActionAddToBottom(new AddBuff(
                new Buff_灼烧(e.GetComponent<Character>(), burn),BattleInfo.Instance.player,"Skill1"));
            }
            else
            {
                ActionManager.Instance.ActionAddToBottom(new AddBuff(
                    new Buff_灼烧(e.GetComponent<Character>(), burn)));
            }
        }
    }

    public override void Initialize()
    {
        explain = Buff_灼烧.intro;
    }

    public override void RefreshDescription()
    {
        text_description.text =
            "对所有敌人施加" + burn + "层灼烧。";
    }
}
