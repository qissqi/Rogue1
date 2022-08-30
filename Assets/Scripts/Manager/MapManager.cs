using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MapManager : Singleton<MapManager>
{
    public Room currentRoom;
    public GameObject MapCamera;
    public GameObject MiniMap;
    //public CharacterControl currentPlayerControl;
    //public PlayerInfo currentPlayerInfo;
    public List<Room> allRooms = new List<Room>();
    public Slider bloodBar;
    public TMP_Text bloodBarText;
    public TMP_Text GoldsText;

    public int level=1;

    private void Start()
    {
        StartSet(1);

    }

    public void StartSet(int _level)
    {
        level = _level;
        GameManager.Instance.currentscene = GameManager.GameScene.Map;
        MapBuilder.Instance.BuildMap(level);
        //currentPlayerControl = GameObject.FindGameObjectWithTag("Player_Map").GetComponent<CharacterControl>();
        //currentPlayerControl = GameManager.Instance.currentCharacter.GetComponent<CharacterControl>();
        MapCamera = GameObject.FindGameObjectWithTag("MapCamera");
        RefreshUI();
    }

    public void Update()
    {
        Developer();
        QuickControl();
        RefreshUI();
    }

    public void RefreshUI()
    {
        float m = GameManager.Instance.playerInfo.maxHP;
        float hp = GameManager.Instance.playerInfo.HP;
        int g = GameManager.Instance.playerInfo.Golds;
        bloodBar.value = hp / m;
        bloodBarText.text = hp + " / " + m;
        GoldsText.text = g.ToString();

    }

    public void QuickControl()
    {
        if (GameManager.Instance.currentscene != GameManager.GameScene.Map
            && GameManager.Instance.currentscene != GameManager.GameScene.Inventory)
            return;

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            InventoryManager.Instance.gameObject.SetActive(!InventoryManager.Instance.gameObject.activeSelf);
        }

        if(Input.GetKeyDown(KeyCode.M) && GameManager.Instance.currentscene == GameManager.GameScene.Map)
        {
            var mmap = MiniMap.GetComponent<RectTransform>();
            Debug.Log(mmap.anchoredPosition+"\n"+mmap.sizeDelta);
            if (mmap.anchoredPosition.x > 1)
            {
                mmap.sizeDelta = new Vector2(0, 0);
                mmap.anchoredPosition = Vector2.zero;
            }
            else
            {
                mmap.sizeDelta = new Vector2(-560, -314);
                mmap.anchoredPosition = new Vector2(-mmap.sizeDelta.x / 2, -mmap.sizeDelta.y / 2);
            }
        }
    }

    public void Developer()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            currentRoom?.SetDoor(true);
        }
        if(Input.GetKeyDown(KeyCode.P))
        {
            currentRoom?.MarkVisiable(true);
        }
    }


}