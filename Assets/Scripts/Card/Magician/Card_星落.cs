using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_星落 : Card
{
    int damage = 3;
    int h = 4;
    public override void CardEffect()
    {
        ActionManager.Instance.ActionAddToBottom(new MakeDamage(BattleInfo.Instance.ChosenEnemy, new DamageInfo(
            BattleInfo.Instance.player, damage),"Skill1"));
        ActionManager.Instance.ActionAddToBottom(new AddBuff(new Buff_护佑(BattleInfo.Instance.player, h)));
    }

    public override void Initialize()
    {
        explain = Buff_护佑.intro;
    }

    public override void RefreshDescription()
    {
        int d = BattleInfo.Instance.CaculateCardDamage(damage);
        text_description.text =
            "对敌人造成" + d + "点伤害，获得"+h+"层护佑";
    }
}
