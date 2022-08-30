using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumable_简易炸弹 : ConsumableItem
{
    private int amount = 10;
    public override void OnBattleLoad()
    {
        useable = true;
    }

    public override void OnBattleEnd()
    {
        useable = false;
    }

    public override void MakeEffect()
    {
        ActionManager.Instance.ActionAddToBottom(new AttackAllEnemies(new DamageInfo(null, amount)));
    }

    public override string GetIntro()
    {
        var _intro = "简易炸弹：战斗内外可用\n使用后对所有敌人造成10点伤害" +
            "\n在战斗外使用有其他效果？\n开发者：现在还没有其他效果捏";
        return _intro + explainInfo;
    }
}
