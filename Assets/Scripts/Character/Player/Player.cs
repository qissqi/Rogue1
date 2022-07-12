using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// ��Ϊ���ԡ�buff�Ͳ��ַ���������
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


    //�غϿ�ʼ��Ȼ����
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
