using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using DG.Tweening;


//控制整个游戏的流程与各项Manager脚本的管理等
public class GameManager : Singleton<GameManager>
{
    public static int nextGameScene;
    
    public PlayerInfo playerInfo;
    public enum GameScene
    {
        Menu,Setting,Map,Inventory,Battle,Event
    }
    
    [Header("Prefabs")]
    public GameObject ExplainBoxPre;
    private GameObject explainBox;

    [Header("Info")]
    public GameScene lastScene;
    public GameScene currentscene;

    [Header("Basic")]
    public SO_SO allRelics;
    public SO_SO allEquipment;
    public GO_List allConsumables;

    [Header("OtherInfo")]
    public bool firstShop;
    public bool tutorial;
    public int level;


    [Header("Debug")]
    public bool inBattle;
    public GameObject CharacterPrefab;
    public CharacterControl currentCharacter;
    public Enemy_OnMap EoM;

    [Header("Test")]
    public bool debugMod;
    public SaveData testGOL;
    public GameObject testGO;

    protected override void Awake()
    {
        base.Awake();
        playerInfo = new PlayerInfo();
    }

    private void Start()
    {        if (debugMod)
            return;
        DontDestroyOnLoad(gameObject);
        Screen.SetResolution(1920, 1080, true);
        AsyncLoadScene(1);
    }

    #region 场景加载
    public void LoadScene(int index)
    {
        SceneManager.LoadScene(index);
    }

    public static void AsyncLoadScene(int index)
    {
        nextGameScene = index;
        SceneManager.LoadScene("Loading");
    }

    #endregion

    /// <summary>
    /// 仅在新游戏时
    /// </summary>
    public void PlayerinfoClear()
    {
        playerInfo.Relics_Get.Clear();
        playerInfo.Equipment_Get.Clear();

        playerInfo.Relics_Left[0].Clear();
        playerInfo.Relics_Left[1].Clear();
        playerInfo.Relics_Left[0].AddRange(allRelics.objects[0]?.gameObjects);
        playerInfo.Relics_Left[1].AddRange(allRelics.objects[1]?.gameObjects);

        playerInfo.Equipment_Left[0].Clear();
        playerInfo.Equipment_Left[1].Clear();
        playerInfo.Equipment_Left[0].AddRange(allEquipment.objects[0].gameObjects);
        playerInfo.Equipment_Left[1].AddRange(allEquipment.objects[1].gameObjects);
    }

    public void ChangeScene(GameScene scene)
    {
        lastScene = currentscene;
        currentscene = scene;
    }

    public void SceneBack()
    {
        currentscene = lastScene;
    }

    void Update()
    {
        Developer();
    }

    public void LoadNewMap()
    {
        AsyncLoadScene(2);
    }


    #region 注释框
    public void OpenExplainBox(string info,PointerEventData eventData,Transform transform__,GameObject sourceGO)
    {
        while (transform__.GetComponent<Canvas>()==null)
        {
            transform__ = transform__.parent;
        }
        explainBox = Instantiate(ExplainBoxPre,transform__);
        explainBox.GetComponent<ExplainBox>().SetPos();
        explainBox.GetComponent<ExplainBox>().SetInfo(info,sourceGO);
    }

    public void CloseExplainBox()
    {
        if(explainBox)
            Destroy(explainBox);
    }
    #endregion

    public void Developer()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log(Input.mousePosition);
            Debug.Log(Camera.main.pixelWidth + "," + Camera.main.pixelHeight);
        }
        if (Input.GetKeyDown(KeyCode.K)&& inBattle)
        {
            ActionManager.Instance.ActionEnd();
        }
        if(Input.GetKeyDown(KeyCode.O))
        {
            Instantiate(testGO).GetComponent<Item>().Add();
        }
        if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit(0);
            }
        }
    }

}
