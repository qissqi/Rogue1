using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SettingMenu : MonoBehaviour
{
    private enum L
    {
        简体中文,Chinese
    }

    private L language = L.简体中文;
    public Text languageText;
    public Text volume1, volume2;

    private void Start()
    {
        float v1 = SoundManager.Instance.CurBGMVolume,v2 = SoundManager.Instance.CurSEVolume;
        volume1.text = ((int)v1*100).ToString();
        volume1.transform.parent.GetComponent<Slider>().value = v1;
        volume2.text = ((int)v2*100).ToString();
        volume2.transform.parent.GetComponent<Slider>().value = v2;
    }

    private void OnEnable()
    {
        GameManager.Instance.ChangeScene(GameManager.GameScene.Setting);
        transform.DOScaleX(0, 0.25f).From().OnComplete(() =>
        {
            Time.timeScale = 0;
        });
    }

    public void Close()
    {
        Time.timeScale = 1;
        SaveManager.Instance.SaveBasic();
        GameManager.Instance.SceneBack();
        transform.DOScaleX(0, 0.25f).OnComplete(() =>
        {
            transform.localScale = new Vector3(1, 1);
            gameObject.SetActive(false);
        });
    }

    public void PlaySound2()
    {
        SoundManager.Instance.PlaySE("Click2");
    }
    public void PlaySound1()
    {
        SoundManager.Instance.PlaySE("Click1");
    }

    public void SetBGM(float v)
    {
        SoundManager.Instance.SetBGMVolume(v);
    }

    public void SetSE(float v)
    {
        SoundManager.Instance.SetSEVolume(v);
    }

    public void SetBGMVolumeText(float v)
    {
        volume1.text = ((int)(v * 100)).ToString();
    }
    public void SetSEVolumeText(float v)
    {
        volume2.text = ((int)(v * 100)).ToString();
    }

    public void ChangeLanguage()
    {
        switch (language)
        {
            case L.简体中文:
                language++;
                languageText.text = language.ToString();
                break;

            case L.Chinese:
                language = L.简体中文;
                languageText.text = L.简体中文.ToString();
                break;

            default:
                break;
        }
    }

    public void BackToTitle()
    {
        SaveManager.Instance.SaveBasic();
        GameManager.Instance.ChangeScene(GameManager.GameScene.Menu);
        Time.timeScale = 1;
        GameManager.AsyncLoadScene(1);
    }

    public void ExitGame()
    {
        SaveManager.Instance.SaveBasic();
        Application.Quit();
    }

}
