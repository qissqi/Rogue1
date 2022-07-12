using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_Angry : Card
{
    int count;

    public override void CardEffect()
    {
        ActionManager.Instance.ActionAddToBotton(new MakeDamage(BattleInfo.Instance.ChosenEnemy,
            new DamageInfo(BattleInfo.Instance.player, 12)));
        ActionManager.Instance.ActionAddToBotton(new AddBuff(
            new Weak_Buff(BattleInfo.Instance.player, count)));
    }

    public override void RefreshDescription()
    {
        int _d = BattleInfo.Instance.CaculateCardDamage(12);
        text_description.text = "���" + _d + "���˺���\n���" + count + "������";
    }

    public override void initialize()
    {
        count = 2;
    }
}
