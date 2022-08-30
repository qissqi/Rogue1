using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Relic : Item, IBattleActive
{
    public int num=0;
    public bool useNum;

    public virtual void Reset()
    {
        type = ItemType.Relic;
    }

    public override void OnBattleLoad()
    {
        num = 0;
        if (useNum)
        {
            var n = new GameObject("Counter");
            var _t = GameObject.Instantiate(n, gameObject.transform);
            Destroy(n);
            _t.transform.localScale = new Vector3(1f, 1f);
            var text = _t.AddComponent<Text>();
            text.alignment = TextAnchor.LowerRight;
            text.font = BattleUI.Instance.Font;
            text.color = Color.white;
            text.alignByGeometry = true;
            text.fontSize = 40;
            text.text = num.ToString();
        }
    }

    public override void OnBattleEnd()
    {
        if (useNum)
        {
            Destroy(transform.GetChild(0)?.gameObject);
        }
    }

    public virtual void RefreshNumber()
    {
        if (!useNum || !GameManager.Instance.inBattle)
            return;
        transform.GetChild(0).GetComponent<Text>().text = num.ToString();
    }

    public virtual void AfterCardUse(Card card)
    {
        
    }

    public virtual float AtDamageGive(DamageInfo info)
    {
        return info.commonDamage;
    }

    public float AtDamageReceive(DamageInfo info)
    {
        return info.commonDamage;
    }

    public virtual bool Prior()
    {
        return false;
    }

    public float OnDefendGiven(float defend, bool caculate = false)
    {
        return defend;
    }

    public void OnDie()
    {
        
    }

    
}
