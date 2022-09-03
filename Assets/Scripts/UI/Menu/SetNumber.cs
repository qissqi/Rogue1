using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetNumber : MonoBehaviour
{
    public void SetVolumeNumber(float v)
    {
        GetComponent<Text>().text = ((int)(v * 100)).ToString();
    }
}
