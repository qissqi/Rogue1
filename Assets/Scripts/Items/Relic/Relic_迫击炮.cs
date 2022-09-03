using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Relic_迫击炮 : Relic
{
    int mark = 0;

    public override void OnBattleLoad()
    {
        base.OnBattleLoad();
        BattleManager.Instance.phaseEvent += PhaseCheck;
    }

    public void PhaseCheck(BattlePhase phase)
    {
        if(phase == BattlePhase.PlayerStart)
        {
            mark = 0;
        }
    }

    public override void AfterCardUse(Card card)
    {
        int _m = 0;
        if(card.type == Card.CardType.Attack)
        {
            _m = 1;
        }
        else if(card.type == Card.CardType.Skill)
        {
            _m = 2;
        }

        if(mark!=0 &&_m!=0)
        {
            if(mark!=_m)
            {
                ActionManager.Instance.ActionAddToBottom(new AddBuff(new Buff_力量(BattleInfo.Instance.player, 1)));
                ActionManager.Instance.ActionAddToBottom(new AddBuff(new Buff_Chain_力量(BattleInfo.Instance.player,-1,false)));
            }
        }
        mark = _m;
    }

    public override string GetIntro()
    {
        var _intro = "迫击炮：\n每次交替打出技能卡和攻击卡时，获得1点临时力量";
        return _intro + explainInfo;
    }
}
