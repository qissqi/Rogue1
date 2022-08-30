using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class 全力一击 : Card
{
    int damage = 12;
    int count = 2;

    public override void CardEffect()
    {
        ActionManager.Instance.ActionAddToBottom(new MakeDamage(BattleInfo.Instance.ChosenEnemy,
            new DamageInfo(BattleInfo.Instance.player, damage)));
        ActionManager.Instance.ActionAddToBottom(new AddBuff(
            new 易伤_buff(BattleInfo.Instance.player, count)));
    }



    public override void Initialize()
    {
        explain = 易伤_buff.intro;
    }

    public override void RefreshDescription()
    {
        int _d = BattleInfo.Instance.CaculateCardDamage(damage);
        text_description.text =
            "攻击造成" + _d + "点伤害\n获得" + count + "层易伤";
    }


}
