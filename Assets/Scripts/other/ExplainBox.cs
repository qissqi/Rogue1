using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplainBox : MonoBehaviour
{
    void Update()
    {
        SetPos();
    }

    private void OnDisable()
    {
        Destroy(gameObject);
    }

    public void SetPos()
    {
        var _x = transform.parent.GetComponent<RectTransform>().rect.width / Camera.main.pixelWidth;
        var _y = transform.parent.GetComponent<RectTransform>().rect.height / Camera.main.pixelHeight;
        var Etr = GetComponent<RectTransform>();
        var _pos = Input.mousePosition;
        Etr.anchoredPosition3D = new Vector3(_pos.x * _x, _pos.y * _y,0);
    }

}
