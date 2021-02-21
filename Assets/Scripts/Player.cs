using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;
using PostProcessAttribute = UnityEngine.Rendering.PostProcessing.PostProcessAttribute;

public class Player : MonoBehaviour
{
    /// <summary>
    /// This just has to keep track of health really...
    /// </summary>
    
    public Transform head;

    public float health;
    public float maxHealth = 100;
    

    [Header("Scripts (Player Only)")]
    public MouseLook bodyMouseLook;
    public MouseLook headMouseLook;

    public PostProcessVolume postProcessVolume;
    private ChromaticAberration chromaticAberration;

    public Sway swayScript;

    private AudioSource audioSource;
    public AudioClip hurtSound;
    
    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;

        postProcessVolume.sharedProfile.TryGetSettings(out chromaticAberration);
        
        // this will represent our health without the need for a HUD element
        chromaticAberration.intensity.value = 0;

        audioSource = GetComponent<AudioSource>();
    }

    public void IncreaseHealth(float change)
    {
        health += change;

        chromaticAberration.intensity.value = Mathf.Lerp(1, 0, health / 100f);
    }
    
    public void DecreaseHealth(float change)
    {
        if (health <= 0)
        {
            // already dead
            return;
        }
        
        health -= change;

        chromaticAberration.intensity.value = Mathf.Lerp(1, 0, health / 100f);
        
        audioSource.PlayOneShot(hurtSound);
        
        if (health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        // in this case we don't wanna destroy the game object, just show an end screen
        GameManager.instance.ContestantDies();
        // Destroy(gameObject);
    }

    
}
