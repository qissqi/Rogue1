using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI变量之后应该在代码中赋值！！！
/// </summary>
public class BattleUI : Singleton<BattleUI>
{
    public Texture2D CursorNormal, CursorAttack;
    public GridLayoutGroup HandGroup;
    public Button endTurn;
    public Text EnergyText;
    public GameObject FloatingTips;
    public SpriteList BuffSprites;
    public Font Font;

    public void SetInfluencedUI(bool on)
    {
        HandGroup.enabled = on;
        endTurn.interactable = on;
    }

    public void RefreshEnergy(int energyWillChange=0)
    {

        ///颜色代码
        ///1.白色 #FFFFFF
        ///2.红色 #FF0000
        ///3 绿色 #00FF00
        ///4 蓝色 #0000FF
        ///5 牡丹红 #FF00FF
        ///6 青色 #00FFFF
        ///7 黄色 #FRFF00
        ///8 黑色 #000000
        ///9 海蓝 #70DB93
        ///10 巧克力色 #5C3317
        
        int _Cnow = BattleInfo.Instance.player.cost;
        int _Cmax = BattleInfo.Instance.player.costMax;
        if(energyWillChange !=0)
        {

            EnergyText.text = "<color=#00FF00>" + (_Cnow-energyWillChange) + "</color>/" + _Cmax;
        }
        else
        {
            EnergyText.text =  _Cnow + "/" + _Cmax;
        }

    }

    public IEnumerator StartFloatingTips(string info)
    {
        var _g = Instantiate(FloatingTips,transform);
        Text textBox = _g.GetComponent<Text>();
        textBox.text = info;
        _g.SetActive(true);
        for (float i = 0.75f; i > 0; i -= Time.deltaTime)
        {
            _g.transform.position += new Vector3(0, 0.0125f);
            yield return null;
        }
        Destroy(_g);
    }

    #region 光标
    public void SetCursorAttack()
    {
        Cursor.SetCursor(CursorAttack, new Vector2(16, 16), CursorMode.Auto);
    }

    public void SetCursorNormal()
    {
        Cursor.SetCursor(CursorNormal, new Vector2(0, 0), CursorMode.Auto);
    }

    #endregion

}
