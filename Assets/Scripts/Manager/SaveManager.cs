using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveManager : Singleton<SaveManager>
{
    public bool HasSave;
    public SaveData2 saveData;


    public GO_List GO;
    public GameObject test;


    private void Start()
    {
        LoadSave();
    }


    /// <summary>
    /// 不完善，只保存一些基本设定
    /// </summary>
    public void SaveBasic()
    {
        saveData.firstShop = GameManager.Instance.firstShop;
        saveData.Tutorial = GameManager.Instance.tutorial;
        var json = JsonUtility.ToJson(saveData,true);
        var path = Path.Combine(Application.persistentDataPath, "save.all");
        File.WriteAllText(path, json);
    }

    public void LoadSave()
    {

        var path = Path.Combine(Application.persistentDataPath, "save.all");
        if (!File.Exists(path))
            return;
        var json = File.ReadAllText(path);
        saveData = JsonUtility.FromJson<SaveData2>(json);
        GameManager.Instance.firstShop = saveData.firstShop;
        GameManager.Instance.tutorial = saveData.Tutorial;

        SoundManager.Instance.SetBGMVolume(saveData.BGMvolume);
        SoundManager.Instance.SetSEVolume(saveData.SEvolume);
    }

}
