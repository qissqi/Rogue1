using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBuilder : Singleton<MapBuilder>
{
    [Header("ScriptObj")]
    public SO_SO EnemiesByLevel;
    public SO_SO BossByLevel;
    public SO_SO EliteByLevel;

    public GO_List GameEvents;
    public GameObject Shop;

    public GameObject room;
    public GameObject bossRoom;

    private Room nextRoom;
    private Vector3 InitPos = new Vector3(0, 0, 0);
    private Transform MapPa;


    [Header("Debug")]
    public Room.OpenPath StartDirection;
    public int count;

    protected override void Awake()
    {
        base.Awake();
        MapPa = GameObject.Find("Map").transform;
        //StartDirection = (Room.OpenPath)Mathf.Pow(2, Random.Range(0, 4));
        StartDirection = (Room.OpenPath)(Random.Range(1, 3) * 7 - 6);

        nextRoom = room.GetComponent<Room>();
    }

    public void BuildMap()
    {
        //生成人物
        GameManager.Instance.currentCharacter = Instantiate(GameManager.Instance.CharacterPrefab);
        //生成总地图
        for (int i = 0; i < count; i++)
        {
            //指定方式生成随机房间
            ////Debug.Log(InitPos);
            CreatRoom(room,InitPos);
            if(Random.value<0.7f && i<count-1&&i!=0)
            {
                i++;
                Vector3 _v = SideRoomPos(room.GetComponent<Room>());
                ////Debug.Log("Side:\n" + _v);
                CreatRoom(room, _v);
                if(Random.value<0.7f)
                {
                    InitPos = _v;
                    ////Debug.Log("New Direction");
                }
            }
            //最后一间房坐标给Boss房
            if(i==count-1)
            {
                nextRoom = bossRoom.GetComponent<Room>();
            }
            SetNextRoomPos(room.GetComponent<Room>());
        }
        CreatRoom(bossRoom, InitPos);

        //补充门等
        foreach (var _r in MapManager.Instance.allRooms)
        {
            SetRoomDoor(_r);
        }


        List<Room> tmpRooms = new List<Room>();
        tmpRooms.AddRange( MapManager.Instance.allRooms);
        tmpRooms.RemoveAt(tmpRooms.Count - 1);
        tmpRooms.RemoveAt(0);

        //TODO: 塞塞塞塞塞塞塞塞塞塞塞塞塞塞塞塞塞塞塞塞塞塞塞塞塞塞塞塞塞塞塞塞
        //塞小怪
        ///弃用的方法
        ///var enemies = EnemiesByLevel.objects[MapManager.Instance.level-1];
        ///int num = enemies.gameObjects.Count;
        ///for(int i = 1;i<MapManager.Instance.allRooms.Count-1;i++)
        ///{
        ///    if (Random.value < 0.3)
        ///        continue;
        ///    var _room = MapManager.Instance.allRooms[i];
        ///    int r = Random.Range(0, num);
        ///    var _e = Instantiate(enemies.gameObjects[r], _room.transform);
        ///    int r1 = Random.Range(-3, 3), r2 = Random.Range(-3, 3);
        ///    _e.transform.localPosition = new Vector3(0+r1, 0+r2);
        ///   _room.EnterLock = true;
        ///    _room.hasEvent = true;
        ///    _room.Mark.transform.Find("Enemy").gameObject.SetActive(true);
        ///}
        
        int enemyMin = tmpRooms.Count / 2;
        int enemyMax = tmpRooms.Count * 2 / 3;
        int Enum = Random.Range(enemyMin, enemyMax);

        int _En = 0;
        while (_En<=Enum)
        {
            int ri = Random.Range(0, tmpRooms.Count);
            var _room = tmpRooms[ri];

            var randE = Random.Range(0, EnemiesByLevel.objects[MapManager.Instance.level - 1].gameObjects.Count);
            var _e = Instantiate(EnemiesByLevel.objects[MapManager.Instance.level - 1].gameObjects[randE],_room.transform);
            _e.transform.localPosition = new Vector3(0, 0);
            _room.EnterLock = true;
            _room.hasEvent = true;
            _room.SetMark(1, true);

            tmpRooms.Remove(_room);
            _En++;
        }

        //精英怪
        //暂略

        //商店
        if (tmpRooms.Count > 0)
        {
            int ri = Random.Range(0, tmpRooms.Count);
            var _room = tmpRooms[ri];
            var _s = Instantiate(Shop, _room.transform);
            _s.transform.localPosition = new Vector3(0, 0);
            _s.GetComponent<ShopEvent>().InitShop();
            _room.hasEvent = true;
            _room.SetMark(5, true);
            tmpRooms.Remove(_room);
        }

        //事件
        ///for (int i = 1; i < MapManager.Instance.allRooms.Count - 1; i++)
        ///{
        ///    Room _room = MapManager.Instance.allRooms[i];
        ///    if (_room.hasEvent)
        ///        continue;
        ///    int _rand = Random.Range(0, GameEvents.gameObjects.Count);
        ///    var _e = Instantiate(GameEvents.gameObjects[_rand], _room.transform);
        ///    _e.transform.localPosition = new Vector3(0, 0);
        ///    _room.hasEvent = true;
        ///    _room.Mark.transform.Find("RandomEvent").gameObject.SetActive(true);
        ///}

        foreach (var _room in tmpRooms)
        {
            int ri = Random.Range(0, GameEvents.gameObjects.Count);
            var _e = Instantiate(GameEvents.gameObjects[ri], _room.transform);
            _e.transform.localPosition = new Vector3(0, 0);
            _room.hasEvent = true;
            _room.SetMark(4, true);
        }

        //Boss  就是最后一个生成的房间
        var _bossRoom = MapManager.Instance.allRooms[MapManager.Instance.allRooms.Count - 1];
        _bossRoom.SetMark(3, true);
        var allBoss = BossByLevel.objects[MapManager.Instance.level - 1];
        int _randboss = Random.Range(0, allBoss.gameObjects.Count);
        var boss = Instantiate(allBoss.gameObjects[_randboss], _bossRoom.transform);
        boss.transform.localPosition = new Vector3(0, 0);


    }




    public void CreatRoom(GameObject _Room,Vector3 pos)
    {
        var _r= Instantiate(_Room, pos, Quaternion.identity, MapPa);
        MapManager.Instance.allRooms.Add(_r.GetComponent<Room>());
    }

    public void SetNextRoomPos(Room _room)
    {
        switch (StartDirection)
        {
            case Room.OpenPath.Up:
                InitPos += new Vector3(0, _room.Yoffset + 1 + nextRoom.Yoffset);
                break;
            case Room.OpenPath.Down:
                InitPos += new Vector3(0, -(_room.Yoffset + 1 + nextRoom.Yoffset));
                break;
            case Room.OpenPath.Left:
                InitPos += new Vector3(-(_room.Xoffset+1+nextRoom.Xoffset),0);
                break;
            case Room.OpenPath.Right:
                InitPos += new Vector3(_room.Xoffset + 1 + nextRoom.Xoffset, 0);
                break;

            default:
                break;
        }
    }

    public Vector3 SideRoomPos(Room _room)
    {
        Vector3 _v;
        switch (StartDirection)
        {
            case Room.OpenPath.Up:
            case Room.OpenPath.Down:
                if(Random.value<0.5f)
                {
                    _v = InitPos+ new Vector3(-(_room.Xoffset + 1 + nextRoom.Xoffset), 0);
                }
                else
                {
                    _v = InitPos + new Vector3((_room.Xoffset + 1 + nextRoom.Xoffset), 0);
                }

                break;
            case Room.OpenPath.Left:
            case Room.OpenPath.Right:
                if (Random.value < 0.5f)
                {
                    _v = InitPos + new Vector3(0,-(_room.Yoffset + 1 + nextRoom.Yoffset));
                }
                else
                {
                    _v = InitPos + new Vector3(0,(_room.Yoffset + 1 + nextRoom.Yoffset));
                }

                break;

            default:
                _v = InitPos;
                break;
        }

        return _v;
    }

    public void SetRoomDoor(Room _room)
    {
        //小修改：只有上、右方向设置门，其余方向设置空

        //Vector3 aimpos;

        //aimpos = _room.transform.position + new Vector3(0,2 * _room.Yoffset+1);
        //if(Physics2D.OverlapCircle(aimpos,1,LayerMask.GetMask("Room")))
        if(_room.GetNearRoom(1)!=null)  //上
        {
            _room.SetEdge(1, 2);
        }
        else
        {
            _room.SetEdge(1, 0);
        }

        //aimpos = _room.transform.position + new Vector3(0,-2 * _room.Yoffset-1 );
        //if (Physics2D.OverlapCircle(aimpos, 1, LayerMask.GetMask("Room")))
        if (_room.GetNearRoom(2) != null)  //下
        {
            _room.SetEdge(2, -1);
        }
        else
        {
            _room.SetEdge(2, 0);
        }

        //aimpos = _room.transform.position + new Vector3(-2*_room.Xoffset-1,0);
        //if (Physics2D.OverlapCircle(aimpos, 1, LayerMask.GetMask("Room")))
        if(_room.GetNearRoom(3) != null)  //左
        {
            _room.SetEdge(3, -1);
        }
        else
        {
            _room.SetEdge(3, 0);
        }

        //aimpos = _room.transform.position + new Vector3(2 * _room.Xoffset+1, 0);
        //if (Physics2D.OverlapCircle(aimpos, 1, LayerMask.GetMask("Room")))
        if(_room.GetNearRoom(4) != null)  //右
        {
            _room.SetEdge(4, 2);
        }
        else
        {
            _room.SetEdge(4, 0);
        }

    }





}


