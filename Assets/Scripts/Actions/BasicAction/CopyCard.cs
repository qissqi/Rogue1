using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyCard : Actions
{
    Card card;
    System.Action<GameObject> Callback;
    public CopyCard(Card _card,System.Action<GameObject> _Callback)
    {
        card = _card;
        Callback = _Callback;
    }

    public override void execute()
    {
        GameObject g = Object.Instantiate(card.gameObject);
        g.SetActive(true);
        Callback(g);
        EndCall();
    }
}
