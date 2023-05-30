using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCreator : MonoBehaviour
{
    [SerializeField]
    GameObject buttonPrefab;
    [SerializeField]
    GameObject UIPanel;

    [SerializeField]
    Room[] roomPrefab;
    [SerializeField]
    GameObject doorPrefab;

    int mapCount = 50;
    int roomsPerFloor = 5;

    // 1������ ���� ����鿡 �ִ���, 
    // 2������ ����鿡 �ִ���
    public class Map
    {
        public Map(int roomCnt)
        {
            isRoom = new bool[roomCnt];
            roomType = new int[roomCnt];
        }
        public bool[] isRoom;
        public int[] roomType;
    }
    Dictionary<int, Map> mapInfo = new Dictionary<int, Map>();

    int nowMap = 0;
    

    private void Start()
    {
        // �� �迭 �ʱ�ȭ
        for(int i = 0; i < mapCount; i++)
        {
            mapInfo[i] = new Map(roomsPerFloor);
            int useRoomNum = Random.Range(0, roomsPerFloor);
            mapInfo[i].isRoom[useRoomNum] = true;

            for(int j = 0; j < roomsPerFloor; j++)
            {
                if (j == useRoomNum)
                    continue;
                float num = Random.Range(0, 10f);
                if(num <= 2)
                {
                    mapInfo[i].isRoom[j] = true;
                    mapInfo[i].roomType[j] = Random.Range(0, roomPrefab.Length);
                }
            }
        }

        // �� ����
        for(int i = 0; i < mapCount; i++)
        {
            for(int j = 0; j < roomsPerFloor; j++)
            {
                if (mapInfo[i].isRoom[j])
                {
                    GameObject obj = Instantiate(roomPrefab[mapInfo[i].roomType[j]].gameObject);
                    obj.transform.position = new Vector2(i * 26, 30 * (j - 1));

                    // �� ����
                    // ���� ��� �������� �����ؼ�
                    // ������ ��

                    // ���� ��
                    if(i < mapCount - 1)
                    {
                        DoorCheck(i, j, obj.transform.position);
                    }
                    // ���� ��
                    if (i != 0)
                    {
                        DoorCheck(i, j, obj.transform.position, false);
                    }
                }
            }
        }
    }

    void DoorCheck(int mapNum, int now, Vector3 pos, bool isRight = true)
    {
        // ���� �� Ȯ��
        if (isRight)
            mapNum++;
        else
            mapNum--;

        Vector3 lastDoor = Vector3.zero;

        if (now == 0)
            for (int i = 0; i < 2; i++)
                Create(i);
        else if (now == roomsPerFloor - 1)
            for (int i = roomsPerFloor - 2; i < roomsPerFloor; i++)
                Create(i);
        else
            for (int i = now - 1; i < now + 2; i++)
                Create(i);

        //switch (now)
        //{
        //    case 0:
        //        for(int i = 0; i < 2; i++)
        //            Create(i);
        //        break;

        //    case 1:
        //        for(int i = 0; i < 3; i++)
        //            Create(i);
        //        break;

        //    case 2:
        //        for(int i = 1; i < 4; i++)
        //            Create(i);
        //        break;

        //    case 3:
        //        for (int i = 2; i < 5; i++)
        //            Create(i);
        //        break;

        //    case 4:
        //        for (int i = 3; i < 5; i++)
        //            Create(i);

        //        break;
        //}

        // ���� �ִ��� Ȯ�� �� �� ������ �� ����
        void Create(int _i)
        {
            if (mapInfo[mapNum].isRoom[_i])
            {
                GameObject door = Instantiate(doorPrefab);
                SetDoorPosition(pos, door.transform, _i - now, isRight);
            }
        }
    }
    void SetDoorPosition(Vector3 roomPos, Transform doorTr, int y, bool isRight)
    {
        if(isRight)
            doorTr.position = roomPos + (Vector3.right * 11.25f) + Vector3.up * y * 5;
        else
            doorTr.position = roomPos + (Vector3.left * 11.25f) + Vector3.up * y * 5;
    }
    
    public void RoomSPawn()
    {
        GameObject obj = Instantiate(roomPrefab[mapInfo[nowMap].roomType[0]].gameObject);

        obj.transform.position = Vector3.zero;
    }
}
