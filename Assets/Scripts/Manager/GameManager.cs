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
            loaded = false;
        }

        private bool loaded = false;
        public int maxHP;
        private int hp;
        public int HP 
        {
            set { hp = value > maxHP ? maxHP : value; }
            get => hp;
        }
        public int Golds;
        public int maxCost;
        public void SetProperty(int _maxHP,int _hp,int startGolds,int _maxCost)
        {
            if(!loaded)
            {
                loaded = true;
                maxHP = _maxHP;
                HP = _hp;
                Golds = startGolds;
                maxCost = _maxCost;
            }
            Debug.Log(JsonUtility.ToJson(GameManager.Instance.playerInfo, true));
        }

        public List<List<GameObject>> Relics_Left = new List<List<GameObject>>();
        public List<List<GameObject>> Equipment_Left = new List<List<GameObject>>();
        public List<GameObject> Relics_Get = new List<GameObject>();
        public List<GameObject> Equipment_Get = new List<GameObject>();

        public void Init_SO(SO_SO relics, SO_SO equipment)
        {

            if (relics.objects.Count > 0)
            {
                Relics_Left.Add(new List<GameObject>());
                Relics_Left.Add(new List<GameObject>());
                Relics_Left[0].AddRange(relics.objects[0].gameObjects);
                Relics_Left[1].AddRange(relics.objects[1].gameObjects);
            }
            if (equipment.objects.Count > 0)
            {
                Equipment_Left.Add(new List<GameObject>());
                Equipment_Left.Add(new List<GameObject>());
                Equipment_Left[0].AddRange(equipment.objects[0].gameObjects);
                Equipment_Left[1].AddRange(equipment.objects[1].gameObjects);
            }
        }

        public GameObject GetItem(Item.ItemType type,int a1,int a2)
        {
            GameObject item = null;
            switch (type)
            {
                case Item.ItemType.Relic:
                    item = Relics_Left[a1][a2];
                    Relics_Left[a1].RemoveAt(a2);
                    Relics_Get.Add(item);
                    break;

                case Item.ItemType.Equipment:
                    item = Equipment_Left[a1][a2];
                    Equipment_Left[a1].RemoveAt(a2);
                    Equipment_Get.Add(item);
                    break;

                default:
                    break;
            }
            return item;
        }

        public void NewGetItem(Item item)
        {
            switch (item.type)
            {
                case Item.ItemType.Relic:
                    foreach (var item1 in Relics_Left)
                    {
                        foreach (var item2 in item1)
                        {
                            if(item2.GetComponent<Item>().itemName == item.itemName)
                            {
                                item1.Remove(item2);
                                Relics_Get.Add(item2);
                                break;
                            }
                        }
                    }
                    break;

                case Item.ItemType.Equipment:
                    foreach (var item1 in Equipment_Left)
                    {
                        foreach (var item2 in item1)
                        {
                            if (item2.GetComponent<Item>().itemName == item.itemName)
                            {
                                item1.Remove(item2);
                                Equipment_Get.Add(item2);
                                break;
                            }
                        }
                    }
                    break;

                default:
                    break;
            }
        }

        public void ClearAll()
        {
            loaded = false;
            Relics_Left.Clear();
            Relics_Get.Clear();
            Equipment_Left.Clear();
            Equipment_Left.Clear();

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
    public SaveData saveData;

    [Header("Info")]
    public GameScene currentscene;

    [Header("Basic")]
    public SO_SO allRelics;
    public SO_SO allEquipment;
    public GO_List allConsumables;

    [Header("OtherInfo")]
    public bool firstShop = false;

    [Header("Debug")]
    public bool inBattle;
    public GameObject CharacterPrefab;
    public CharacterControl currentCharacter;
    public Enemy_OnMap EoM;

    [Header("Test")]
    public bool debug;
    public SaveData testGOL;
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

    /// <summary>
    /// 仅在新游戏时
    /// </summary>
    public void dataClear()
    {
        saveData.Relics_Get.Clear();
        saveData.Equipments_Get.Clear();
        saveData.ItemInfo.Clear();
        saveData.playerInfo = null;

        saveData.Relics_Left.Clear();
        saveData.Relics_Left = new List<List<GameObject>>(2);
        saveData.Relics_Left[0].AddRange(allRelics.objects[0].gameObjects);
        saveData.Relics_Left[1].AddRange(allRelics.objects[1].gameObjects);
        
        saveData.Equipments_Left.Clear();
        saveData.Equipments_Left = new List<List<GameObject>>(2);
        saveData.Equipments_Left[0].AddRange(allEquipment.objects[0].gameObjects);
        saveData.Equipments_Left[1].AddRange(allEquipment.objects[1].gameObjects);
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
        explainBox.GetComponent<ExplainBox>().SetInfo(info);
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
    }

}
