using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Relic_迷你剪刀 : Relic
{
    public override void AfterCardUse(Card card)
    {
        if(card.type == Card.CardType.Skill)
        {
            Counter(1);
        }
    }

    public override void OnBattleLoad()
    {
        base.OnBattleLoad();
        BattleManager.Instance.phaseEvent += PhaseCheck;
    }

    public void PhaseCheck(BattlePhase phase)
    {
        if(phase == BattlePhase.PlayerEnd)
        {
            num = 0;
            RefreshNumber();
        }
    }

    public void Execute()
    {
        ActionManager.Instance.ActionAddToHead(new AttackAllEnemies(new DamageInfo(null, 3)));
    }

    public void Counter(int _num)
    {
        num += _num;
        if(num == 3)
        {
            num = 0;
            Execute();
        }
        RefreshNumber();
    }

    public override string GetIntro()
    {
        var _intro = "迷你剪刀：\n在你的回合中，每打出3张技能卡，对所有敌人造成3点伤害";
        return _intro + explainInfo;
    }
}
