using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;


//控制整个游戏的流程与各项Manager脚本的管理等
public class GameManager : Singleton<GameManager>
{
    public class PlayerInfo
    {
        public PlayerInfo()
        {
            Init_SO(GameManager.Instance.allRelics, GameManager.Instance.allEquipment);
        }

        private bool loaded = false;
        public int MaxHP;
        public int HP;
        public int Golds;

        public void SetProperty(int maxHP,int hp,int startGolds)
        {
            if(!loaded)
            {
                loaded = true;
                MaxHP = maxHP;
                HP = hp;
                Golds = startGolds;
            }
        }

        //public SO_SO SO_Relics = null;
        //public SO_SO SO_Equipment = null;
        public List<List<GameObject>> Relics = new List<List<GameObject>>();
        public List<List<GameObject>> Equipment = new List<List<GameObject>>();

        public void Init_SO(SO_SO relics, SO_SO equipment)
        {

            if (relics.objects.Count > 0)
            {
                Relics.Add(new List<GameObject>());
                Relics.Add(new List<GameObject>());
                Relics[0].AddRange(relics.objects[0].gameObjects);
                Relics[1].AddRange(relics.objects[1].gameObjects);
            }
            if (equipment.objects.Count > 0)
            {
                Equipment.Add(new List<GameObject>());
                Equipment.Add(new List<GameObject>());
                Equipment[0].AddRange(equipment.objects[0].gameObjects);
                Equipment[1].AddRange(equipment.objects[1].gameObjects);
            }
        }
    }
    public PlayerInfo playerInfo;
    public enum GameScene
    {
        Menu,Setting,Map,Inventory,Battle,Event
    }
    
    [Header("Prefabs")]
    public GameObject ExplainBoxPre;
    private GameObject explainBox;
    //public GameObject battleCanvasPre;
    //public GameObject CardPackPre;
    //public GameObject battleCanvas;
    //public Item TestItem;

    [Header("Info")]
    public GameScene currentscene;

    [Header("Basic")]
    public SO_SO allRelics;
    public SO_SO allEquipment;
    public GO_List allConsumables;

    [Header("Debug")]
    public bool inBattle;
    public GameObject CharacterPrefab;
    public GameObject currentCharacter;
    public Enemy_OnMap EoM;
    [Header("Test")]
    public bool debug;
    public GO_List testGOL;
    public GameObject testGO;

    protected override void Awake()
    {
        base.Awake();
        playerInfo = new PlayerInfo();
    }

    private void Start()
    {
        
        if (debug)
            return;
        DontDestroyOnLoad(gameObject);
        SceneManager.LoadScene(1);
    }

    void Update()
    {
        Developer();
    }

    public void LoadNewMap()
    {
        SceneManager.LoadScene(2);
    }


    #region 注释框
    public void OpenExplainBox(string info,PointerEventData eventData,Transform transform__)
    {
        while (transform__.GetComponent<Canvas>()==null)
        {
            transform__ = transform__.parent;
        }
        explainBox = Instantiate(ExplainBoxPre,transform__);
        explainBox.GetComponent<ExplainBox>().SetPos();
        explainBox.GetComponentInChildren<Text>().text = info;
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
    }

}
