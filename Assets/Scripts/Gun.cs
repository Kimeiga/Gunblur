using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;

public class Gun : MonoBehaviour
{
    public bool active = false;

    private AudioSource fireAudioSource;
    public Transform fireTransform;

    public Sway swayScript;
    
    //gun stats
    public int maxAmmo = 30;
    public int ammo;
    
    public bool automatic;
    private bool fireCommand;

    public float fireRate = 0.02f;
    private float nextFire;

    public float range = 1000;

    public float damage = 20;


    //firing variables
    private RaycastHit[] hits;
    public LayerMask fireMask;


    //base local pos/rot for holding
    public Vector3 localHoldPosition;
    public Vector3 localHoldRotation;

    // for animating shooting
    public float kickback = 0.4f;
    public float totalKickback = 0;
    
    private Tweener kickbackTweener;
    public float kickTweenDuration = 1;
    
    // Use this for initialization
    void Start()
    {
        //initialize references
        fireAudioSource = GetComponent<AudioSource>();


        //initialize numbers
        nextFire = 0;
        ammo = maxAmmo;
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            //cue fire command depending on the the automatic fire bool
            if (automatic)
            {
                fireCommand = Input.GetButton("Fire1");
            }

            if (!automatic)
            {
                fireCommand = Input.GetButtonDown("Fire1");
            }


            if (fireCommand)
            {
                if (ammo > 0)
                {
                    if (Time.time > nextFire)
                    {
                        //fire
                        hits = Physics.RaycastAll(fireTransform.position, fireTransform.forward, range,
                            fireMask);

                        foreach (RaycastHit hit in hits)
                        {
                            Enemy enemy = hit.transform.GetComponent<Enemy>();
                            enemy.DecreaseHealth(damage, hit.point, Quaternion.Euler(hit.normal));
                        }
                        
                        nextFire = Time.time + fireRate;

                        fireAudioSource.PlayOneShot(fireAudioSource.clip);

                        ammo--;

                        
                        // animation
                        
                        DOTween.Kill(kickbackTweener);
                        
                        totalKickback -= kickback;
                    }
                }
                else
                {
                    //gun is empty
                    //play empty sound
                }
            }


            if (Input.GetButtonDown("Reload"))
            {
                ammo = maxAmmo;
            }

            swayScript.zOffset = totalKickback;
            
            // kickbackTweener = DOTween.To(() => totalKickback, x => totalKickback = x, 0, kickTweenDuration);
            totalKickback = Mathf.Lerp(totalKickback, 0, Time.deltaTime * 5);
        }
    }
}