using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExplainBox : MonoBehaviour
{
    public GameObject son;
    public GameObject sourceGO;

    public void SetInfo(string info,GameObject _sourceGO)
    {
        var str = info.Split('\t');
        sourceGO = _sourceGO;
        son.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text = str[0];
        for (int i = 1; i < str.Length; i++)
        {
            var _b = Instantiate(son, transform).transform.GetChild(0);
            _b.GetComponent<TMPro.TMP_Text>().text = str[i];
        }
    }

    void Update()
    {
        SetPos();
        if(!sourceGO ||!sourceGO.activeSelf )
        {
            Destroy(gameObject);
        }
    }

    private void OnDisable()
    {
        Destroy(gameObject);
    }

    public void SetPos()
    {
        //屏幕坐标转画布坐标
        var _x = transform.parent.GetComponent<RectTransform>().rect.width / Camera.main.pixelWidth;
        var _y = transform.parent.GetComponent<RectTransform>().rect.height / Camera.main.pixelHeight;
        var Etr = GetComponent<RectTransform>();
        var _pos = Input.mousePosition;
        float x_fix = GetComponent<RectTransform>().rect.width/2, y_fix = 0;
        if (_pos.x > Camera.main.pixelWidth / 2)
        {
            x_fix = -GetComponent<RectTransform>().rect.width/2;
        }
        if (_pos.y > Camera.main.pixelHeight*3 / 4)
        {
            y_fix = -GetComponent<RectTransform>().rect.height;
        }
        Etr.anchoredPosition3D = new Vector3(_pos.x * _x + x_fix, _pos.y * _y + y_fix, 0);
    }

}
