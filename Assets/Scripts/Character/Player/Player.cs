using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// ��Ϊ���ԡ�buff�Ͳ��ַ���������
/// </summary>
public class Player : Character
{

    [Header("Player")]
    public int maxCost;
    public int cost;
    public int drawCardNum;

    void Start()
    {
        BattleManager.Instance.phaseEvent += PlayerTurnStart;
        BattleManager.Instance.phaseEvent += LoseBlockAtStart;
        LoadFromSave();
    }


    //�غϿ�ʼ��Ȼ����
    public override void LoseBlockAtStart(BattlePhase phase)
    {
        if (phase == BattlePhase.PlayerTurn)
        {
            blocks = 0;
        }
    }

    public void PlayerTurnStart(BattlePhase phase)
    {
        if (phase ==(BattlePhase.PlayerTurn))
        {
            BattleManager.Instance.DrawCard(drawCardNum);
        }
        cost = maxCost;
        BattleUI.Instance.RefreshEnergy();
    }

    public void LoadFromSave()
    {
        maxHP = GameManager.Instance.playerInfo.maxHP;
        HP = GameManager.Instance.playerInfo.HP;
        maxCost = GameManager.Instance.playerInfo.maxCost;
    }

    public void SaveToInfo()
    {
        GameManager.Instance.playerInfo.maxHP = maxHP;
        GameManager.Instance.playerInfo.HP = HP;
        Debug.Log(JsonUtility.ToJson(GameManager.Instance.playerInfo));
    }

    public override void Die()
    {
        //���Խ׶�
        HP = 10;
        foreach (var _e in BattleInfo.Instance.enemies)
        {
            Destroy(_e.gameObject);
        }
        Destroy(gameObject);
        BattleManager.Instance.transform.parent.gameObject.SetActive(false);
        BattleManager.Instance.battleStart = false;
    }

    
}
