using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment_破碎之镜 : Equipment
{
    public override void OnBattleLoad()
    {
        base.OnBattleLoad();
        num = 1;
        RefreshNumber();
        useable = true;
    }

    public override void OnBattleEnd()
    {
        base.OnBattleEnd();
        useable = false;
        active = false;
    }

    public override void Use()
    {
        if (num > 0)
        {
            active = true;
            num = 0;
            RefreshNumber();
        }
    }

    public override void AfterCardUse(Card card)
    {
        if(active)
        {
            ActionManager.Instance.ActionAddToBottom(new CopyCard(card, Use));
            active = false;
        }
    }

    public void Use(GameObject card)
    {
        var c = card.GetComponent<Card>();
        c.isTemp = true;
        c.UsingCard();

    }

    public override string GetIntro()
    {
        var _intro = "破碎之镜：\n每场战斗仅能使用一次\n使用后，下一张打出的牌将生效两次。";
        return _intro + explainInfo;
    }
}
