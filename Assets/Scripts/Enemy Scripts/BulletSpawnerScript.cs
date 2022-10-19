using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawnerScript : MonoBehaviour
{
    public float angleOffset = 0;
    public float bulletSpeed = 5;
    public bool invertDir = true;
    public Rigidbody2D projectile;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnBullet(float angle)
    {
        float totalAngle = angle + Mathf.PI * angleOffset / 180; //totalAngle and angle are in degrees
        float invertMult = 1;
        if (invertDir)
        {
            invertMult = -1;
        }
        Rigidbody2D proj = Instantiate(projectile, transform.position, transform.rotation);
        float xVelo = Mathf.Cos(totalAngle) * bulletSpeed * invertMult;
        float yVelo = Mathf.Sin(totalAngle) * bulletSpeed * invertMult;
        proj.velocity = new Vector2(xVelo, yVelo);
        //BulletScript pScript = proj.GetComponent<BulletScript>();
    }
}
