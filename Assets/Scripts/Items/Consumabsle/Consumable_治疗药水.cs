using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumable_治疗药水 : ConsumableItem
{
    int amount = 10;

    public override void Add()
    {
        base.Add();
        useable = true;
    }

    public override string GetIntro()
    {
        var _intro = "治疗药水：战斗内外可用\n使用后恢复10点HP。";
        return _intro + explainInfo;
    }

    public override void MakeEffect()
    {
        if(GameManager.Instance.inBattle)
        {
            ActionManager.Instance.ActionAddToBottom(new HealAction(null, BattleInfo.Instance.player, amount));
        }
        else
        {
            GameManager.Instance.playerInfo.HP += amount;
        }
    }
}
