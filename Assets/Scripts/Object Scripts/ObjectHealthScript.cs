using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHealthScript : MonoBehaviour
{
    public int maxHealth = 1;
    private int currentHealth;
    private ObjectEnablerScript objectEnabler;
    // Start is called before the first frame update
    void Awake()
    {
        currentHealth = maxHealth;
        objectEnabler = GetComponent<ObjectEnablerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateHealth(int damageTaken)
    {
        currentHealth -= damageTaken;
        if (currentHealth <= 0)
        {
            objectEnabler.DisableObject();
        }
    }
}
