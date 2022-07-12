using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI����֮��Ӧ���ڴ����и�ֵ������
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

        ///��ɫ����
        ///1.��ɫ #FFFFFF
        ///2.��ɫ #FF0000
        ///3 ��ɫ #00FF00
        ///4 ��ɫ #0000FF
        ///5 ĵ���� #FF00FF
        ///6 ��ɫ #00FFFF
        ///7 ��ɫ #FRFF00
        ///8 ��ɫ #000000
        ///9 ���� #70DB93
        ///10 �ɿ���ɫ #5C3317
        
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

    #region ���
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
