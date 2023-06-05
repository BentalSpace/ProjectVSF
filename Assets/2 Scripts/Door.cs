using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public int roomNum;
    public bool isRight;
    public Vector3 targetPos;

    [SerializeField]
    GameObject door1;
    [SerializeField]
    GameObject door2;

    [SerializeField]
    GameObject colliderPrefab;

    public void Init(int _roomNum, bool _isRight)
    {
        roomNum = _roomNum;
        isRight = _isRight;
    }
    private void Start()
    {
        //float door1Dis = Mathf.Abs(door1.transform.position.y - transform.position.y);
        //float door2Dis = Mathf.Abs(door2.transform.position.y - transform.position.y);

        // 콜라이더 생성

        //GameObject obj = Instantiate(colliderPrefab);
        //obj.transform.position = transform.position + Vector3.up;
        //obj.transform.localScale = new Vector3(1, (door1Dis - 2) * -1, 1);

        //obj = Instantiate(colliderPrefab);
        //obj.transform.position = transform.position + Vector3.down;
        //obj.transform.localScale = new Vector3(1, door2Dis - 2, 1);
    }

    void InNextRoom()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            InNextRoom();
        }
    }
}
