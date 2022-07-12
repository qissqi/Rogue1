using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HandGrid : Singleton<HandGrid>
{
    private int focuses = 0;
    public bool focusing, choosing;
    private Card focusCard;
    public GameObject Hand;
    public float space;
    private float leaveCD = 0.1f;

    public int Focuses { get => focuses;
        set
        {
            focuses = value;
            if(focuses<0)
            {
                focuses = 0;
            }
        }
    }

    public Card FocusCard { get => focusCard;
        set
        {
            focusCard = value;
        }
    }

    void Start()
    {
        Hand = gameObject;
        space = 2;
    }


    void Update()
    {

        //脱离时计时，时间到则归位
        if(focuses ==0 && focusing && !choosing)
        {
            if(leaveCD<0)
            {
                focusing = false;
                leaveCD = 0.1f;
                AllCardMove();
            }
            else
            {
                leaveCD -= Time.deltaTime;
            }
        }
        else
        {
            leaveCD = 0.1f;
        }

    }



    public void SetCardposition()
    {

        float Fspace = space;
        List<Card> cards = new List<Card>();
        cards.AddRange(Hand.GetComponentsInChildren<Card>(true));
        Vector3 midPos = Hand.transform.position;

        if(cards.Count>5)
        {
            Fspace =(float) 10 / cards.Count;
        }

        //奇数
        if(cards.Count%2==1)
        {
            int midCount = (cards.Count + 1) / 2;
            for (int i = 0; i < cards.Count; i++)
            {
                cards[i].handIndex = i;
                int ind = i + 1;
                cards[i].targetPosition = (ind - midCount) * (new Vector3(Fspace, 0)) + midPos;
            }
        }
        //偶数
        else
        {
            int midCount = cards.Count / 2;
            for (int i = 0; i < cards.Count; i++)
            {
                cards[i].handIndex = i;
                int ind = i + 1;
                if(i<=midCount)
                {
                    cards[i].targetPosition = (ind - midCount) * (new Vector3(Fspace, 0)) + (midPos - new Vector3(Fspace/2, 0));
                }
                else
                {
                    cards[i].targetPosition = (i - midCount) * (new Vector3(Fspace, 0)) + (midPos + new Vector3(Fspace/2, 0));
                }
            }

        }
    }

    public void AllCardMove(float time = 0.3f,Card except = null)
    {
        Card[] cards = Hand.GetComponentsInChildren<Card>();

        for (int i = 0; i < cards.Length; i++)
        {
            if(cards[i]==except)
            {
                continue;
            }
            cards[i].CardMoveBack(time);
        }
        

    }

    public void FocusMove(Card card)
    {
        
        Card[] cards = GetComponentsInChildren<Card>();

        for (int i = 0; i < cards.Length; i++)
        {
            float ix = cards[i].targetPosition.x;
            
            if (cards[i].handIndex < card.handIndex)
            {
                
                cards[i].transform.DOMoveX(ix - 0.8f, 0.2f);
            }
            else if(cards[i].handIndex > card.handIndex)
            {
                
                cards[i].transform.DOMoveX(ix + 0.8f, 0.2f);

            }
            else
            {
                cards[i].transform.DOKill();
                transform.DOScale(1.1f, 0.1f);
                cards[i].CardMoveBack(0.15f);
            }
        }
    }

}

