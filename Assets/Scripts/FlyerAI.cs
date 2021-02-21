using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;


public class FlyerAI : MonoBehaviour
{
    private Transform player;
    private Player contestantPlayer;

    private Vector3 up;
    public float speed = 1;

    public float damage = 10;

    public Transform spinChild;

    public Renderer capsuleRenderer;
    public Renderer cubeRenderer;

    public Material[] bodyMaterials;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameManager.instance.contestant.transform;
        contestantPlayer = GameManager.instance.contestantPlayer;
        up = transform.up;

        // randomize initial variables
        speed = Random.Range(0, 0.04f);
        damage = Random.Range(0, 10);

        // randomize the material
        
        int r = Random.Range(0, bodyMaterials.Length);
        capsuleRenderer.material = bodyMaterials[r];
        
        
        r = Random.Range(0, bodyMaterials.Length);
        cubeRenderer.material = bodyMaterials[r];
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(player.position + new Vector3(0,1,0), up);

        
    }

    private void FixedUpdate()
    {
        
        transform.Translate(new Vector3(0, 0, speed) , Space.Self);
        transform.Translate(spinChild.up * 0.1f, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        print(other.transform.name);
        
        if (other.transform.CompareTag("Contestant"))
        {
            contestantPlayer.DecreaseHealth(damage);
        }

    }
}
