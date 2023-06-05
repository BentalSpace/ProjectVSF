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


    void SetRoom(int mapNum, int roomNum, bool isThere = true)
    {
        mapInfo[mapNum].isRoom[roomNum] = isThere;

        // � ��ġ�� �Ǿ��ִ� ���� ����� ��
        if (isThere)
            mapInfo[mapNum].roomType[roomNum] = Random.Range(0, roomPrefab.Length);
    }
    private void Start()
    {
        // Map �ʱ�ȭ
        for(int i = 0; i < mapCount; i++)
            mapInfo[i] = new Map(roomsPerFloor);

        // �� �迭 �ʱ�ȭ
        for (int i = 0; i < mapCount; i++)
        {
            if (i == 0)
            {
                for (int j = 0; j < roomsPerFloor; j++)
                {
                    // ù��° ���̶�� �� ����
                    SetRoom(i, j);
                }
            }

            if (i != mapCount - 1)
            {
                int mapPlus1 = i + 1;
                float rand;
                for (int j = 0; j < roomsPerFloor; j++)
                {
                    if (!mapInfo[i].isRoom[j])
                        continue;
                    // �� ���� �ִ� ���� ��ġ�� �������� �տ� �ִ� ���� �����ϴ°���
                    if (j == 0)
                    {
                        // 0, 1 �� �߿� �� ����
                        // �ΰ� �� ������ ��, �ϳ��� ������ ��(�����϶��� �Ȱ���)
                        rand = Random.Range(0, 10f);

                        if (rand <= 1f)
                        {
                            // �ΰ� �� ����
                            SetRoom(mapPlus1, 0);
                            SetRoom(mapPlus1, 1);
                        }
                        else
                        {
                            // �� �� �ϳ����� ����
                            rand = Random.Range(0, 2);

                            if (rand == 0)
                                SetRoom(mapPlus1, 0);
                            else
                                SetRoom(mapPlus1, 1);
                        }
                    }
                    else if (j == roomsPerFloor - 1)
                    {
                        // roomsPerFloor -1, -2 �� �߿� �� ����
                        rand = Random.Range(0, 10f);

                        if (rand <= 1f)
                        {
                            // �ΰ� �� ����
                            SetRoom(mapPlus1, roomsPerFloor - 1);
                            SetRoom(mapPlus1, roomsPerFloor - 2);
                        }
                        else
                        {
                            // �� �� �ϳ����� ����
                            rand = Random.Range(0, 2);
                            if (rand == 0)
                                SetRoom(mapPlus1, roomsPerFloor - 1);
                            else
                                SetRoom(mapPlus1, roomsPerFloor - 2);
                        }
                    }
                    else
                    {
                        // ���� �ִ� �� 0, +1, -1 �� �߿� �� ����
                        rand = Random.Range(0, 10f);
                        if (rand <= 3f)
                        {
                            // 3�� �� ����
                            SetRoom(mapPlus1, j - 1);
                            SetRoom(mapPlus1, j);
                            SetRoom(mapPlus1, j + 1);
                        }
                        else if (rand <= 4f)
                        {
                            // 2�� ���� (1 2 | 1 3 | 2 3)
                            rand = Random.Range(0, 3);

                            if (rand == 0)
                            {
                                SetRoom(mapPlus1, j - 1);
                                SetRoom(mapPlus1, j);
                            }
                            else if(rand == 1)
                            {
                                SetRoom(mapPlus1, j);
                                SetRoom(mapPlus1, j + 1);
                            }
                            else
                            {
                                SetRoom(mapPlus1, j - 1);
                                SetRoom(mapPlus1, j + 1);
                            }
                        }
                        else
                        {
                            // 1�� ���� 60% (1 2 3)
                            rand = Random.Range(0, 3);

                            if (rand == 0)
                                SetRoom(mapPlus1, j - 1);
                            else if (rand == 1)
                                SetRoom(mapPlus1, j);
                            else
                                SetRoom(mapPlus1, j + 1);
                        }
                    }
                }
            }
        }

        // �� ����
        for (int i = 0; i < mapCount; i++)
        {
            for (int j = 0; j < roomsPerFloor; j++)
            {
                if (mapInfo[i].isRoom[j])
                {
                    GameObject obj = Instantiate(roomPrefab[mapInfo[i].roomType[j]].gameObject);
                    obj.transform.position = new Vector2(i * 26, 30 * (j - 1));

                    // �� ����
                    // ���� ��� �������� �����ؼ�
                    // ������ ��

                    // ���� ��
                    if (i < mapCount - 1)
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
                GameObject obj = Instantiate(doorPrefab);
                SetDoorPosition(pos, obj.transform, _i - now, isRight);
                Door door = obj.GetComponent<Door>();
                door.Init(_i, isRight);
            }
        }
    }
    void SetDoorPosition(Vector3 roomPos, Transform doorTr, int y, bool isRight)
    {
        if (isRight)
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
