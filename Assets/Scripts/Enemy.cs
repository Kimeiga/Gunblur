using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float health;
    public float maxHealth = 100;

    // so that their body parts can roll around when you kill them
    // eventually these will have to be destroyed
    public GameObject[] bodyParts;

    // delay until body parts and rest of game object is destroyed.
    public float destroyDelay = 5;

    public ParticleSystem hurtParticles;
    
    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void IncreaseHealth(float change)
    {
        health += change;

    }
    
    public void DecreaseHealth(float change, Vector3? hurtLocation, Quaternion? hurtNormalRotation)
    {
        if (health <= 0)
        {
            // already dead
            return;
        }
        
        health -= change;

        if (hurtNormalRotation != null && hurtLocation != null)
        {
            // instantiate hurt particles at point
            Instantiate(hurtParticles, hurtLocation.Value, hurtNormalRotation.Value);
        }
        
        
        if (health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        // in this case we don't wanna destroy the game object, just show an end screen

        foreach (GameObject bodyPart in bodyParts)
        {
            bodyPart.transform.parent = null;
            Rigidbody r = bodyPart.GetComponent<Rigidbody>();
            r.isKinematic = false;
            r.useGravity = true;

            Collider c = bodyPart.GetComponent<Collider>();
            c.isTrigger = false;
            
        }

        GameManager.instance.numKills++;

        // I think I add this in right?
        Destroy(gameObject);
    }
}
