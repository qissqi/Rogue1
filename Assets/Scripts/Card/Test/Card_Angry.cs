using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_Angry : Card
{
    int count;

    public override void CardEffect()
    {
        ActionManager.Instance.ActionAddToBottom(new MakeDamage(BattleInfo.Instance.ChosenEnemy,
            new DamageInfo(BattleInfo.Instance.player, 12)));
        ActionManager.Instance.ActionAddToBottom(new AddBuff(
            new Weak_Buff(BattleInfo.Instance.player, count)));
    }

    public override void RefreshDescription()
    {
        int _d = BattleInfo.Instance.CaculateCardDamage(12);
        text_description.text = "造成" + _d + "点伤害，\n获得" + count + "层虚弱";
    }

    public override void Initialize()
    {
        count = 2;
        explain = Weak_Buff.intro;
    }
}
