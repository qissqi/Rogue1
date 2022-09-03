using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChange : MonoBehaviour
{
    public bool timeStop;
    public GameManager.GameScene scene;


    private void OnEnable()
    {
        Change();
        if(timeStop)
        {
            Time.timeScale = 0;
        }
    }

    private void OnDisable()
    {
        GameManager.Instance.SceneBack();
        Time.timeScale = 1;
    }


    public void Change()
    {
        GameManager.Instance.ChangeScene(scene);
    }



}
