using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance;

    //move
    float h, v;
    [SerializeField]
    float speed = 3.5f;

    //stat
    float curHp;
    [SerializeField]
    float maxHp;

    Rigidbody2D rigid;

    void Awake()
    {
        instance = this;

        rigid = GetComponent<Rigidbody2D>();

        curHp = maxHp;
    }
    private void Update()
    {
        if (Input.GetButton("Horizontal"))
        {
            h = Input.GetAxis("Horizontal");
        }
        else if (Input.GetButtonUp("Horizontal"))
        {
            h = 0;
        }

        if (Input.GetButton("Vertical"))
        {
            v = Input.GetAxis("Vertical");
        }
        else if (Input.GetButtonUp("Vertical"))
        {
            v = 0;
        }
    }
    void FixedUpdate()
    {
        Vector2 nextVec = new Vector2(h, v).normalized * speed * Time.deltaTime;

        rigid.MovePosition(rigid.position + nextVec);
    }
    public void HpCalc(float _value)
    {
        curHp += _value;

        if(curHp <= 0)
        {
            // die
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("RoomIn"))
        {

        }
    }
}
