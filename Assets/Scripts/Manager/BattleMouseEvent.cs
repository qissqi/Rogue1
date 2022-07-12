using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BattleMouseEvent : MonoBehaviour, IPointerClickHandler,IPointerMoveHandler
{

    private void Update()
    {

    }


    public void OnPointerClick(PointerEventData eventData)
    {
        
       
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        Debug.Log(eventData.pointerCurrentRaycast.gameObject.name);
    }
}
