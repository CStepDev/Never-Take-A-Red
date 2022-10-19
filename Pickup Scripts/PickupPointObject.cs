// Author  : Curtis Stephenson
// Date    : 11/05/2018
// Purpose : This script is used by the rotational object spawned by a pickup point, since I've given up on
//           fighting Unity to just change a texture and instead conceded to making a new object every time
//           I want to do it. 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupPointObject : MonoBehaviour
{
    // This will hold a reference to the player, or whatever ends up holding the script component we need to access, that being the object
    // with the PlayerCharacterManager, so we can access it and use the SetWeapon or SetPickup functions to give the player a certain item
    //GameObject playerReference;
    PlayerCharacterManager playerManagerReference;

    // We'll use these variables for dictating two things, the first is the item on the PickupPoint, which will draw the appropriate model
    // IF the check for isOccupied equates to true in update and draw nothing if false. We'll also use them to check if we even need to 
    // draw a model at all, because if a Weapons.HANDGUN or Pickups.NONE are on the spot, there's no point
    public Weapons pickupWeapon = Weapons.HANDGUN;
    public Pickups pickupPickup = Pickups.NONE;
    public int ammoValue;

    // This is when an object touches the pickup point's spinning area. It won't pay attention to enemies or anything random hitting it, 
    // but it will care when the player does, which is what we want
    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
			if (pickupWeapon != Weapons.HANDGUN)
			{
				playerManagerReference.SetWep(pickupWeapon, ammoValue);
			}
            
			if (pickupPickup != Pickups.NONE)
			{
				playerManagerReference.SetPick(pickupPickup);
			}
            
            // This disables both the renderer for the spinning area and the collider, effectively turning the pickup point off until refreshed
            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<MeshCollider>().enabled = false;
            GetComponent<BoxCollider>().enabled = false;

            DestroyObject(this.gameObject);
        }
    } // OnTriggerStay(Collider other)

    // Use this for initialization
    void Start ()
    {
        transform.eulerAngles = new Vector3(90, 0, 0);
        GameObject playerManagerObjectRef = GameObject.FindGameObjectWithTag("PlayerManager");
        playerManagerReference = playerManagerObjectRef.GetComponent<PlayerCharacterManager>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        transform.Rotate(new Vector3(0, 0, 45) * Time.deltaTime);
    }
}
