using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_火花箭 : Card
{
    private int damage = 4;
    private int burn = 3;
    public override void CardEffect()
    {
        ActionManager.Instance.ActionAddToBottom(new MakeDamage(BattleInfo.Instance.ChosenEnemy,
            new DamageInfo(BattleInfo.Instance.player, damage), "Skill1"));
        ActionManager.Instance.ActionAddToBottom(new AddBuff(new Buff_灼烧(BattleInfo.Instance.ChosenEnemy, burn)));
    }

    public override void Initialize()
    {
        explain = Buff_灼烧.intro;
    }

    public override void RefreshDescription()
    {
        int d = BattleInfo.Instance.CaculateCardDamage(damage);
        text_description.text =
            "对敌人造成" + d + "点伤害，\n同时附加" + burn + "层灼烧";
    }
}
