using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ScrollBox : MonoBehaviour
{
    public float offset,scale, speed;

    private float firstSet, height;

    private RectTransform rectT;

    private void Awake()
    {
        rectT = GetComponent<RectTransform>();
        firstSet = rectT.anchoredPosition.y;
        var par = transform.parent.GetComponent<RectTransform>();
        height = par.rect.height;
    }

    private void Update()
    {
        float scroll = Input.mouseScrollDelta.y;
        if (scroll != 0)
        {
            Scroll(scroll*speed); 
        }
    }


    public void Scroll(float scroll)
    {
        //Debug.Log("Scroll£∫" + scroll);
        rectT.anchoredPosition -= new Vector2(0, scroll);
        //Debug.Log(rectT.rect.height);
        //øÿ÷∆∂•≤øæ‡¿Î
        if(rectT.anchoredPosition.y<firstSet)
        {
            rectT.anchoredPosition = new Vector2(rectT.anchoredPosition.x, firstSet);
        }
        //øÿ÷∆µ◊≤øæ‡¿Î
        else if(rectT.rect.height*scale -rectT.anchoredPosition.y+offset<height)
        {
            rectT.anchoredPosition += new Vector2(0, scroll);
        }

    }


}
