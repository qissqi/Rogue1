using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndButton : MonoBehaviour
{
    Button button;
    bool canUse;
    private void Awake()
    {
        canUse = false;
        button = GetComponent<Button>();
    }

    private void Start()
    {
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
        ActionManager.Instance.ActionAddToBotton(new EndPhase());
        BattleInfo.Instance.ChosenCard = null;
        button.interactable = false;
    }
}
