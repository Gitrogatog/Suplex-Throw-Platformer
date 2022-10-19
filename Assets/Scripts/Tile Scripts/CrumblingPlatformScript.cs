using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CrumblingPlatformScript : MonoBehaviour
{
    private Tilemap tilemap;
    public float crumbleTime;

    void Awake()
    {
        tilemap = GetComponent<Tilemap>();
    }

    void OnCollisionEnter2D(Collision2D collision)

    {
        Debug.Log("Hit!");
        if (collision.gameObject.tag == "Player")

        {

            Debug.Log("Hit Player");

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
                StartCoroutine(CrumbleBlock(hitPosition));
                
            }

        }
    }

    IEnumerator CrumbleBlock(Vector3 hitPos)
    {
        yield return new WaitForSeconds(crumbleTime);
        tilemap.SetTile(tilemap.WorldToCell(hitPos), null);
    }
}