using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action_AnimController : MonoBehaviour
{
    public void ActionEnd()
    {
        ActionManager.Instance.ActionEnd();
    }
}
