using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempEffect : MonoBehaviour
{
    private void OnDisable()
    {
        Destroy(gameObject);
    }
}
