using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEnd : MonoBehaviour
{
    public void Back()
    {
        GameManager.Instance.LoadScene(1);
    }
}
