using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_魔力冲击 : Card
{
    int extra = 0;
    public override void CardEffect()
    {
        int count = 0;
        var h = BattleInfo.Instance.player.FindBuff<Buff_护佑>();
        if(h!=null)
        {
            count = h.num;
            ActionManager.Instance.ActionAddToBottom(new AddBuff(
                new Buff_护佑(BattleInfo.Instance.player, -count), BattleInfo.Instance.player, "Skill1"));
            ActionManager.Instance.ActionAddToBottom(new MakeDefend(BattleInfo.Instance.player, BattleInfo.Instance.player, count-count/2,false));

            foreach (var enemy in BattleInfo.Instance.enemies)
            {
                ActionManager.Instance.ActionAddToBottom(new AddBuff(
                    new Buff_灼烧(enemy.GetComponent<Character>(), count)));
            }
        }
    }

    public override void Initialize()
    {
        explain = Buff_护佑.intro + "\t" + Buff_灼烧.intro;
    }

    public override void RefreshDescription()
    {
        text_description.text =
            "将自身所有的护佑转为灼烧施加给所有敌人，自身获得一半层数的格挡。";
    }
}
