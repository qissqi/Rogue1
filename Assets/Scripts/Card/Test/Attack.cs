using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : Card
{
    int baseDamage;

    private void Reset()
    {
        target = Target.Enemy;
        inShow = true;
        rare = Rare.Basi;
    }

    public override void Initialize()
    {
        baseDamage = 6;
    }
    public override void CardEffect()
    {
        ActionManager.Instance.ActionAddToBottom(new MakeDamage(BattleInfo.Instance.ChosenEnemy,
            new DamageInfo(BattleInfo.Instance.player, baseDamage, DamageType.Normal),"Attack1"));
    }

    public override void RefreshDescription()
    {
        int d = BattleInfo.Instance.CaculateCardDamage(baseDamage);
        text_description.text = "对敌人造成" + d + "点伤害";
    }

}

