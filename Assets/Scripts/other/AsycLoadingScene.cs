using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using TMPro;

public class AsycLoadingScene : MonoBehaviour
{
	[SerializeField] private bool debugMod;
	public TMP_Text text_Loading,text_DAO;
    void Start()
    {
		if(!debugMod)
        {

			SoundManager.Instance.StopBGM();
			StartCoroutine(AsyncLoadScene());
        }
		text_DAO.DOColor(new Color(1,1,1,0f), 0.6f).SetLoops(-1, LoopType.Yoyo);
		DOTween.To(() =>"", x => text_Loading.text ="Loading"+ x, "......", 3f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart);
		
    }

    private IEnumerator AsyncLoadScene()
    {
		AsyncOperation operation = SceneManager.LoadSceneAsync(GameManager.nextGameScene); 
		operation.allowSceneActivation = false;

		yield return new WaitForSeconds(1);
		while (!operation.isDone)   //加载未完成，改变进度条
		{
			if (operation.progress >= 0.9f)
			{
				operation.allowSceneActivation = true;
			}
			yield return null;
		}
	}

}
