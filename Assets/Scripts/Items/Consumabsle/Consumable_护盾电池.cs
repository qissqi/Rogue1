using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumable_护盾电池 : ConsumableItem
{
    private int amount = 15;

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
        ActionManager.Instance.ActionAddToBottom(new MakeDefend(null, BattleInfo.Instance.player, amount, false));
    }

    public override string GetIntro()
    {
        var _intro = "护盾电池：战斗中可用\n使用后获得15层格挡。";
        return _intro + explainInfo;
    }
}
