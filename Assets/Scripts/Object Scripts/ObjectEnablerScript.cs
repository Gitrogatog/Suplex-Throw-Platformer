using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectEnablerScript : MonoBehaviour
{
    private Vector3 startPos;
    private Collider2D[] cols;
    private SpriteRenderer sprite;
    private bool enabled = true;

    public float reenableTime = 0;
    private float timer = 0;

    void Awake()
    {
        cols = GetComponentsInChildren<Collider2D>();
        sprite = GetComponent<SpriteRenderer>();
        startPos = transform.position;
    }

    void Update()
    {
        if(!enabled && reenableTime > 0)
        {
            timer -= Time.deltaTime;
            if(timer <= 0)
            {
                EnableObject();
            }
        }
    }

    public void DisableObject()
    {
        for (int i = 0; i < cols.Length; i++)
        {
            cols[i].enabled = false;
        }
        sprite.enabled = false;
        enabled = false;
        timer = reenableTime;
    }

    public void EnableObject()
    {
        for (int i = 0; i < cols.Length; i++)
        {
            cols[i].enabled = true;
        }
        sprite.enabled = true;
        transform.position = startPos;
        enabled = true;
    }

    public bool IsEnabled()
    {
        return enabled;
    }
}
