using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    float speed = 3f;
    [SerializeField]
    float hp;

    Rigidbody2D rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }
    void FixedUpdate()
    {
        Vector2 dir = Player.instance.transform.position - transform.position;
        dir = dir.normalized * speed * Time.deltaTime;

        rigid.MovePosition(rigid.position + dir);
    }

    public void Hit(float dmg)
    {
        hp -= dmg;

        if(hp <= 0)
        {
            Die();
        }
    }
    void Die()
    {
        Destroy(gameObject);
    }
}
