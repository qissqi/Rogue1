using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Room : MonoBehaviour
{
    public bool EnterLock;
    public bool hasEvent;
    public enum RoomType
    {
        Normal, Shop, Chest, Boss
    }
    public GameObject Up_, Down_, Left_, Right_;
    public GameObject Fog;
    public GameObject Mark;
    public LayerMask VisiableLayer;
    public LayerMask MarkLayer;
    //[System.Flags]
    public enum OpenPath
    {
        Up=1,Down=2,Left=4,Right=8
    }

    
    public RoomType roomType;
    public float Xoffset, Yoffset;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player_Map") && Fog!=null)
        {
            MapManager.Instance.MapCamera.transform.position = transform.position + new Vector3(0, 0, -10);
            MapManager.Instance.currentRoom = this;
            Fog.SetActive(false);
            if(EnterLock)
            {
                SetDoor(false);
            }

            MarkVisiable(true);
            for (int i = 1; i < 5; i++)
            {
                GetNearRoom(i)?.MarkVisiable(false);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player_Map"))
        {
            Mark.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    /// <param name="_mark">1-普通敌人，2-精英敌人，3-Boss敌人，4-随机事件，5-商店，6-宝箱</param>
    public void SetMark(int _mark,bool open)
    {
        GameObject mk = null;
        switch (_mark)
        {
            case 1:
                mk = Mark.transform.Find("Enemy").gameObject;
                break;

            case 2:
                mk = Mark.transform.Find("Elite").gameObject;
                break;

            case 3:
                mk = Mark.transform.Find("Boss").gameObject;
                break;

            case 4:
                mk = Mark.transform.Find("RandomEvent").gameObject;
                break;

            case 5:
                mk = Mark.transform.Find("Shop").gameObject;
                break;

            case 6:
                mk = Mark.transform.Find("Chest").gameObject;
                break;

            default:
                mk = null;
                break;
        }
        mk?.SetActive(open);
    }

    public void MarkVisiable(bool currentRoom)
    {
        Fog.layer = 7;
        for (int i = 0; i < Fog.transform.childCount; i++)
        {
            Fog.transform.GetChild(i).gameObject.layer = 7;
        }

        Mark.gameObject.SetActive(true);
        if(currentRoom)
            Mark.GetComponent<SpriteRenderer>().color =Color.yellow;

        Mark.layer = 8;
        for (int i = 0; i < Mark.transform.childCount; i++)
        {
            Mark.transform.GetChild(i).gameObject.layer = 8;
        }
    }

    /// <summary>
    /// 设置房间边界门或墙等（仅有上、右有门）
    /// </summary>
    /// <param name="direction">1-上，2-下，3-左，4-右</param>
    /// <param name="open">-1全关闭，0-墙，1-门关，2-门开</param>
    public void SetEdge(int direction, int open)
    {
        switch (direction)
        {
            case 1:
                for (int i = 0; i < 3; i++)
                {
                    Up_.transform.GetChild(i).gameObject.SetActive(false);
                }
                if(open!=-1)
                    Up_.transform.GetChild(open).gameObject.SetActive(true);
                break;

            case 2:
                for (int i = 0; i < 3; i++)
                {
                    Down_.transform.GetChild(i).gameObject.SetActive(false);
                }
                if (open != -1)
                    Down_.transform.GetChild(open).gameObject.SetActive(true);
                break;

            case 3:
                for (int i = 0; i < 3; i++)
                {
                    Left_.transform.GetChild(i).gameObject.SetActive(false);
                }
                if (open != -1)
                    Left_.transform.GetChild(open).gameObject.SetActive(true);
                break;

            case 4:
                for (int i = 0; i < 3; i++)
                {
                    Right_.transform.GetChild(i).gameObject.SetActive(false);
                }
                if (open != -1)
                    Right_.transform.GetChild(open).gameObject.SetActive(true);
                break;

            default:
                break;


        }
    }

    /// <param name="dir">1-上，2-下，3-左，4-右</param>
    /// <returns></returns>
    public Room GetNearRoom(int dir)
    {
        Room aim = null;
        RaycastHit2D ray;
        Vector3 aimpos;
        LayerMask RoomLayer = LayerMask.GetMask("Room");
        switch (dir)
        {
            case 1:
                aimpos = transform.position + new Vector3(0, 2 * Yoffset);
                ray = Physics2D.Raycast(aimpos, Vector2.zero, 0.1f, RoomLayer);
                if (ray.collider!=null)
                {
                    aim = ray.collider.gameObject.GetComponent<Room>();
                }
                break;

            case 2:
                aimpos = transform.position + new Vector3(0, -2 * Yoffset);
                ray = Physics2D.Raycast(aimpos, Vector2.zero, 0.1f, RoomLayer);
                if (ray.collider != null)
                {
                    aim = ray.collider.gameObject.GetComponent<Room>();
                }
                break;

            case 3:
                aimpos = transform.position + new Vector3(-2*Xoffset,0);
                ray = Physics2D.Raycast(aimpos, Vector2.zero, 0.1f, RoomLayer);
                if (ray.collider != null)
                {
                    aim = ray.collider.gameObject.GetComponent<Room>();
                }
                break;

            case 4:
                aimpos = transform.position + new Vector3(2 * Xoffset, 0);
                ray = Physics2D.Raycast(aimpos, Vector2.zero, 0.1f, RoomLayer);
                if (ray.collider != null)
                {
                    aim = ray.collider.gameObject.GetComponent<Room>();
                }
                break;

            default:
                break;
        }
        return aim;
    }

    public void SetDoor(bool open)
    {
        int i = 2;
        if(!open)
        {
            i = 1;
        }
        if (GetNearRoom(1) != null)
            SetEdge(1, i);

        if(GetNearRoom(4)!=null)
            SetEdge(4, i);

        GetNearRoom(2)?.SetEdge(1, i);
        GetNearRoom(3)?.SetEdge(4, i);
    }


}
