using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : Card
{
    int baseDamage;
    int FinalDamage;

    public override void initialize()
    {
        baseDamage = 5;
        FinalDamage = baseDamage;
        
    }
    public override void CardEffect()
    {
        ActionManager.Instance.ActionAddToBotton(new MakeDamage(BattleInfo.Instance.ChosenEnemy,
            new DamageInfo(BattleInfo.Instance.player, baseDamage, DamageType.Normal)));
    }

    public override void RefreshDescription()
    {
        int d = BattleInfo.Instance.CaculateCardDamage(5);
        text_description.text = "对敌人造成" + d + "点伤害";
    }

}

