using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField]
    GameObject door1;
    [SerializeField]
    GameObject door2;

    [SerializeField]
    GameObject colliderPrefab;

    private void Start()
    {
        float door1Dis = Mathf.Abs(door1.transform.position.y - transform.position.y);
        float door2Dis = Mathf.Abs(door2.transform.position.y - transform.position.y);

        GameObject obj = Instantiate(colliderPrefab);
        obj.transform.position = transform.position + Vector3.up;
        obj.transform.localScale = new Vector3(1, (door1Dis - 2) * -1, 1);

        obj = Instantiate(colliderPrefab);
        obj.transform.position = transform.position + Vector3.down;
        obj.transform.localScale = new Vector3(1, door2Dis - 2, 1);
    }
}
