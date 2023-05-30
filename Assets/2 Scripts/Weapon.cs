using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField]
    GameObject bullet;

    [SerializeField]
    float delayTime;
    float delayCurTime;

    [SerializeField]
    float atkRadius;
    [SerializeField]
    float LookRadius;

    [SerializeField]
    Transform targetTr;

    private void Update()
    {
        delayCurTime += Time.deltaTime;

        if (delayCurTime >= delayTime)
        {
            if (targetTr)
            {
                if(Vector2.Distance(transform.position, targetTr.transform.position) >= LookRadius)
                {
                    targetTr = null;
                }
            }
            if (!targetTr)
            {
                RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, atkRadius, Vector3.up, 5, 1 << 6);

                float minDis = atkRadius + 1;
                for (int i = 0; i < hits.Length; i++)
                {
                    float dis = Vector2.Distance(transform.position, hits[i].transform.position);
                    if (minDis > dis)
                    {
                        minDis = dis;
                        targetTr = hits[i].transform;
                    }
                }
            }
            delayCurTime = 0;
            Shot();
        }
    }

    void Shot()
    {
        GameObject _bullet = Instantiate(bullet);
        _bullet.transform.position = Player.instance.transform.position;

        Vector2 dir = targetTr.position - _bullet.transform.position;
        dir = dir.normalized;

        _bullet.transform.rotation = Quaternion.FromToRotation(Vector3.up, dir);
        //_bullet.transform.rotation = Quaternion.identity;
    }
}
