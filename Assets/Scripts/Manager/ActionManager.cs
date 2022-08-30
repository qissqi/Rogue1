using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionManager : Singleton<ActionManager>
{
    private List<Actions> actions = new List<Actions>();
    public bool onAction;



    private void Update()
    {
        if(!onAction&&actions.Count>0)
        {
            onAction = true;
            RunAction();
        }
    }

    public void ActionAddToHead(Actions action)
    {
        //actions.Insert(0, ActionEnd);
        actions.Insert(0, action);
    }

    public void ActionAddToBottom(Actions action)
    {
        actions.Insert(actions.Count, action);
        //actions.Insert(actions.Count, ActionEnd);
    }

    public void ActionEnd()
    {
        onAction = false;
    }

    public void DelayActionEnd(float _d)
    {
        Invoke("ActionEnd", _d);
    }

    public void RemoveAction()
    {
        actions.RemoveAt(0);
    }

    public void RunAction()
    {
        Actions act = actions[0];
        actions.RemoveAt(0);
        act.execute();
    }

    public void ClearActions()
    {
        actions.Clear();
        onAction = false;
    }

}
