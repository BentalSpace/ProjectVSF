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

    // 1번층에 방이 몇번들에 있는지, 
    // 2번층엔 몇번들에 있는지
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

        // 어떤 배치가 되어있는 방을 사용할 지
        if (isThere)
            mapInfo[mapNum].roomType[roomNum] = Random.Range(0, roomPrefab.Length);
    }
    private void Start()
    {
        // Map 초기화
        for(int i = 0; i < mapCount; i++)
            mapInfo[i] = new Map(roomsPerFloor);

        // 방 배열 초기화
        for (int i = 0; i < mapCount; i++)
        {
            if (i == 0)
            {
                for (int j = 0; j < roomsPerFloor; j++)
                {
                    // 첫번째 방이라면 다 생성
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
                    // ※ 현재 있는 방의 위치를 기준으로 앞에 있는 방을 생성하는거임
                    if (j == 0)
                    {
                        // 0, 1 둘 중에 방 생성
                        // 두개 다 생성할 지, 하나만 생성할 지(세개일때도 똑같음)
                        rand = Random.Range(0, 10f);

                        if (rand <= 1f)
                        {
                            // 두개 다 생성
                            SetRoom(mapPlus1, 0);
                            SetRoom(mapPlus1, 1);
                        }
                        else
                        {
                            // 둘 중 하나에만 생성
                            rand = Random.Range(0, 2);

                            if (rand == 0)
                                SetRoom(mapPlus1, 0);
                            else
                                SetRoom(mapPlus1, 1);
                        }
                    }
                    else if (j == roomsPerFloor - 1)
                    {
                        // roomsPerFloor -1, -2 둘 중에 방 생성
                        rand = Random.Range(0, 10f);

                        if (rand <= 1f)
                        {
                            // 두개 다 생성
                            SetRoom(mapPlus1, roomsPerFloor - 1);
                            SetRoom(mapPlus1, roomsPerFloor - 2);
                        }
                        else
                        {
                            // 둘 중 하나에만 생성
                            rand = Random.Range(0, 2);
                            if (rand == 0)
                                SetRoom(mapPlus1, roomsPerFloor - 1);
                            else
                                SetRoom(mapPlus1, roomsPerFloor - 2);
                        }
                    }
                    else
                    {
                        // 내가 있는 방 0, +1, -1 셋 중에 방 생성
                        rand = Random.Range(0, 10f);
                        if (rand <= 3f)
                        {
                            // 3곳 다 생성
                            SetRoom(mapPlus1, j - 1);
                            SetRoom(mapPlus1, j);
                            SetRoom(mapPlus1, j + 1);
                        }
                        else if (rand <= 4f)
                        {
                            // 2곳 생성 (1 2 | 1 3 | 2 3)
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
                            // 1곳 생성 60% (1 2 3)
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

        // 방 생성
        for (int i = 0; i < mapCount; i++)
        {
            for (int j = 0; j < roomsPerFloor; j++)
            {
                if (mapInfo[i].isRoom[j])
                {
                    GameObject obj = Instantiate(roomPrefab[mapInfo[i].roomType[j]].gameObject);
                    obj.transform.position = new Vector2(i * 26, 30 * (j - 1));

                    // 문 생성
                    // 다음 방과 이전방을 생각해서
                    // 만들어야 함

                    // 다음 방
                    if (i < mapCount - 1)
                    {
                        DoorCheck(i, j, obj.transform.position);
                    }
                    // 이전 방
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
        // 다음 방 확인
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

        // 방이 있는지 확인 후 그 방향의 문 생성
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
