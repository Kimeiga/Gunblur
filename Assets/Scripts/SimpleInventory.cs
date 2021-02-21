using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleInventory : MonoBehaviour
{
    /// <summary>
    /// "Simple" Inventory to avoid name conflict.
    /// Can only carry one weapon at a time
    /// (this makes the game harder and requires you to move around looking for guns while not requiring any UI)
    /// </summary>

    // the one current item that you have right now
    // don't need an "item" script since they are all guns right?
    public GameObject currentItem;

    private FPSWalker fpsWalker;
    private Player player;
    
    public float dropSpeedMod = 10;
    public float dropForce = 0.5f;

    // used by the grabbing functionality
    public LayerMask gunMask;
    public float grabDistance = 5;
    private RaycastHit hit;

    public Transform gunHolder;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Player>();
        fpsWalker = GetComponent<FPSWalker>();
    }

    // Update is called once per frame
    void Update()
    {
        // If you aim at a gun and it is in range and you press e, you drop your current gun and pick up that gun
        if (Physics.Raycast(player.head.position, player.head.forward, out hit, grabDistance, gunMask))
        {
            print("raycast");
            if (Input.GetButton("Grab"))
            {
                print("grab");
                if (currentItem)
                {
                    // if you already have a gun, drop it first
                    // DropItem();
                    Destroy(currentItem);
                }   
                
                // grab new gun
                GrabItem(hit.transform.gameObject);
                
            }    
        }
        
        // If you just wanna drop your gun (maybe get a speed boost by doing this
        if (currentItem && Input.GetButtonDown("Drop"))
        {
            DropItem();
        }
        
        
    }

    void GrabItem(GameObject item)
    {
        currentItem = item;
        
        // set working variables
        Gun currentItemGunScript = currentItem.GetComponent<Gun>();
        

        // child item to super hand 
        currentItem.transform.parent = gunHolder;
        
        // position it
        currentItem.transform.localPosition = currentItemGunScript.localHoldPosition;
        currentItem.transform.localRotation = Quaternion.Euler(currentItemGunScript.localHoldRotation);

        currentItemGunScript.fireTransform = player.head;

        // deactivate rigidbody physics
        Rigidbody rigid = currentItem.GetComponent<Rigidbody>();
        rigid.isKinematic = true;
        rigid.useGravity = false;

        // activate item
        currentItemGunScript.active = true;

        currentItemGunScript.swayScript = GameManager.instance.contestantPlayer.swayScript;

        // I think that's all you have to do...
    }
    
    // drop *current* item, so don't worry about it being *any* item
    void DropItem()
    {
        //set working variables
        Gun currentItemGunScript = currentItem.GetComponent<Gun>();
        

        // unchild item from super hand 
        currentItem.transform.parent = null;

        // activate rigidbody physics
        Rigidbody rigid = currentItem.GetComponent<Rigidbody>();
        rigid.isKinematic = false;
        rigid.useGravity = true;

        // unactivate item
        currentItemGunScript.active = false;


        //ammoPanel.SetActive(false);

        //slotTexts[inventoryIndex].text = (inventoryIndex + 1).ToString();


        //I want to compound the force with the player's own movement so it can fly farther when you run and jump and shit you know.
        //cuz that's how physics works anyways
        //so i'm going to make a new vector and add it in the addforce clause
        //but i'm going to need a class that does this for me because i'm going to reference player displacement in gun script too hahahahahaha

        Vector3 playerMovementMod = fpsWalker.velocity * dropSpeedMod;


        // throw the fucker
        rigid.AddForce((player.head.forward * dropForce) + playerMovementMod, ForceMode.Impulse);

        // hopefully I won't have to implement this....
        //make sure that its trajectory is not going to be influenced by the current player so long as they are intersecting
        // StartCoroutine(DontCollideWithDroppingItem(prevItemScript.col));

        currentItem = null;
    }
}
