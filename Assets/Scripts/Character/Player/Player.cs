using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 作为属性、buff和部分方法持有者
/// </summary>
public class Player : Character
{
    [Header("Player")]
    public int costMax;
    public int cost;
    public int drawCardNum;

    void Start()
    {
        BattleManager.Instance.phaseEvent += PlayerTurnStart;
        BattleManager.Instance.phaseEvent += LoseBlockAtStart;
    }


    //回合开始自然消盾
    public override void LoseBlockAtStart(BattlePhase phase)
    {
        if (phase == BattlePhase.PlayerStart)
        {
            blocks = 0;
        }
    }

    public void PlayerTurnStart(BattlePhase phase)
    {
        Debug.Log("PlayerTurnCheck");
        if (phase ==(BattlePhase.PlayerTurn))
        {
            BattleManager.Instance.DrawCard(drawCardNum);
        }
        cost = costMax;
        BattleUI.Instance.RefreshEnergy();
    }

}
