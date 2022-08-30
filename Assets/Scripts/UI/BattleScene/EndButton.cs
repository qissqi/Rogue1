using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndButton : MonoBehaviour
{
    Button button;
    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        if (!GameManager.Instance.inBattle)
            return;
        BattleManager.Instance.phaseEvent += PTstart;
    }

    public void PTstart(BattlePhase phase)
    {
        if(phase==(BattlePhase.PlayerTurn))
        {
            button.interactable = true;
        }
    }

    public void ButtonDown()
    {
        ActionManager.Instance.ActionAddToBottom(new EndPhase());
        BattleInfo.Instance.ChosenCard = null;
        button.interactable = false;
    }
}
