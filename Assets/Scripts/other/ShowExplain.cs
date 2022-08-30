using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShowExplain : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Func<string> infoSource = null;
    [HideInInspector]public string explainInfo;

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        if(infoSource!=null)
            GameManager.Instance.OpenExplainBox(infoSource(),eventData,transform.parent);
        else
        {
            GameManager.Instance.OpenExplainBox(explainInfo,eventData,transform.parent);
        }
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        GameManager.Instance.CloseExplainBox();
    }

    //public virtual void OnPointerMove(PointerEventData eventData)
    //{
    //    GameManager.Instance.ExplainBoxMove(eventData);
    //}
}
