using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DestroyTilesScript : MonoBehaviour
{
    public string tilemapName;
    private Rigidbody2D rb;
    private Vector2 pastVelo;
    public bool doesBounce = false; //Set to true only if this object has the BounceOffWallsScript attached
    public bool destroyOnCollide = false; //Destroy projectile when it hits anything
    private bool hasHit = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        pastVelo = rb.velocity;
    }

    void FixedUpdate()
    {
        if (hasHit)
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        pastVelo = rb.velocity;
    }

    void OnCollisionEnter2D(Collision2D collision)

    {
        Debug.Log("Hit!");
        Tilemap map = collision.gameObject.GetComponent<Tilemap>();
        if (map != null && collision.gameObject.tag == tilemapName)

        {
            Debug.Log("Hit " + collision.gameObject.tag);

            int contactCount = collision.contactCount;
            ContactPoint2D[] contacts = new ContactPoint2D[contactCount];
            collision.GetContacts(contacts);

            Vector3 hitPosition = Vector3.zero;
            foreach (ContactPoint2D hit in contacts)
            {

                //hitPosition.x = contacts[i].point.x;
                //hitPosition.y = contacts[i].point.y;
                hitPosition.x = hit.point.x - 0.01f * hit.normal.x;
                hitPosition.y = hit.point.y - 0.01f * hit.normal.y;
                map.SetTile(map.WorldToCell(hitPosition), null);
            }
            if (!doesBounce)
            {
                rb.velocity = pastVelo;
            }
            hasHit = true;
        }
        else if (!doesBounce)
        {
            Destroy(gameObject);
        }
    }

    
    /*
    public GameObject tilemapGameObject;

    Tilemap tilemap;

    void Start()
    {
        var rb = GetComponent<Rigidbody2D>();
        rb.velocity = initialVelocity.x * UnityEngine.Random.Range(-1f, 1f) * Vector3.right + initialVelocity.y * Vector3.down;
        if (tilemapGameObject != null)
        {
            tilemap = tilemapGameObject.GetComponent<Tilemap>();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Vector3 hitPosition = Vector3.zero;
        if (tilemap != null && tilemapGameObject == collision.gameObject)
        {
            foreach (ContactPoint2D hit in collision.contacts)
            {
                hitPosition.x = hit.point.x - 0.01f * hit.normal.x;
                hitPosition.y = hit.point.y - 0.01f * hit.normal.y;
                tilemap.SetTile(tilemap.WorldToCell(hitPosition), null);
            }
        }
    }
    */
}
