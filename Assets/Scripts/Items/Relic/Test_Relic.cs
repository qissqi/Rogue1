using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Relic : Item
{
    public override string GetIntro()
    {
        return explainInfo;
    }

    private void Start()
    {
        Lv = Random.Range(1, 6);
        type = (ItemType)Random.Range(0, 3);
        explainInfo = type.ToString();
    }
}