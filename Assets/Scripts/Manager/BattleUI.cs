using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

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
    public Button item_Left, item_Right;
    public RectTransform relicRect;
    public RectTransform Inventory_Scene;
    public SpriteList EnemyIntentSprites;
    public GameObject EffectBox;
    public GameObject NumberBox;
    public SpriteList Effects;
    

    private Queue<GameObject> NumPool = new Queue<GameObject>();
    private Transform pool;

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
        int _Cmax = BattleInfo.Instance.player.maxCost;
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

    #region 特效
    public void PlayDefendEffect(Transform _transform)
    {
        var ef = Instantiate(EffectBox, transform);
        ef.GetComponent<Image>().sprite = Effects.sprites[0];
        ef.transform.position = _transform.position;
        ef.transform.localScale = new Vector3(1.5f, 1.5f);
        ef.transform.DOMoveY(_transform.position.y + 5, 0.5f).From().OnComplete(() =>
        {
            Destroy(ef);
        });
    }

    public void ShowHit(Transform _targetTransform,string _info,Color _color,float _scale = 1)
    {

        var ib = GetInfoPool(_targetTransform);

        ib.GetComponent<Text>().text = _info;
        ib.GetComponent<Text>().color = _color;

        ib.SetActive(true);
        ib.transform.position = _targetTransform.position + new Vector3(0,1);
        ib.transform.localScale = new Vector3(_scale, _scale);

        ib.transform.DOMoveY(_targetTransform.position.y + 3, 0.8f).OnComplete(() =>
        {
            NumPool.Enqueue(ib);
            ib.transform.SetParent(pool);
            ib.SetActive(false);
        });

    }

    public void ShowTipInfo(string info,float _scale = 1)
    {
        var ib = GetInfoPool(transform);
        ib.GetComponent<Text>().text = info;
        ib.GetComponent<Text>().color = Color.white;
        ib.SetActive(true);
        ib.transform.position =transform.position- new Vector3(0, 1);
        ib.transform.localScale = new Vector3(_scale, _scale);

        ib.transform.DOMoveY(ib.transform.position.y + 3, 0.8f).OnComplete(() =>
        {
            NumPool.Enqueue(ib);
            ib.transform.SetParent(pool);
            ib.SetActive(false);
        });

    }

    public GameObject GetInfoPool(Transform _targetTransform)
    {
        if (pool == null)
        {
            pool = new GameObject("TipPool").transform;
            pool.transform.SetParent(transform);
        }

        GameObject ib;
        if (NumPool.Count == 0)
        {
            ib = Instantiate(NumberBox, _targetTransform);
        }
        else
        {
            ib = NumPool.Dequeue();
            ib.transform.SetParent(_targetTransform);
        }
        return ib;
    }


    #endregion


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

    #region 物品格
    public void Item_Reset()
    {
        relicRect.anchoredPosition = new Vector2(0, 0);
        if(relicRect.rect.xMax>=0)
        {
            item_Left.gameObject.SetActive(false);
            item_Right.gameObject.SetActive(false);
        }
        else
        {
            item_Left.gameObject.SetActive(true);
            item_Right.gameObject.SetActive(true);
        }
    }

    public void Item_Left()
    {
        //if(relicRect.rect.xMin<0)
        //{
        //    relicRect.anchoredPosition += new Vector2(100, 0);
        //}
        if(relicRect.anchoredPosition.x<0)
            relicRect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, relicRect.anchoredPosition.x+125, 50);
    }

    public void Item_Right()
    {
        if(relicRect.rect.width+relicRect.anchoredPosition.x-(Inventory_Scene.rect.width-920)>0)
            relicRect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, relicRect.anchoredPosition.x-125, 50);
    }

    #endregion

}
