using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantBulletSpawner : MonoBehaviour
{
    public float bulletAngle = 0;
    public float timeBtwBulletSpawn;
    private float bulletSpawnTimer;
    public float bulletSpawnOffset;
    private bool isOffsetting = true;
    public Transform bulletSource;
    public GameObject spawner;
    // Start is called before the first frame update
    void Start()
    {
        bulletSpawnTimer = bulletSpawnOffset;
    }

    // Update is called once per frame
    void Update()
    {
        bulletSpawnTimer -= Time.deltaTime;
        if (bulletSpawnTimer <= 0)
        {
            if (isOffsetting)
            {
                isOffsetting = false;
            }
            else
            {
                SpawnBulletPattern();
            }
            bulletSpawnTimer = timeBtwBulletSpawn;
        }
    }

    public void SpawnBulletPattern()
    {
        GameObject proj = Instantiate(spawner, bulletSource.position, transform.rotation);
        proj.BroadcastMessage("SpawnBullet", bulletAngle);
        Destroy(proj, .2f);
    }
}
