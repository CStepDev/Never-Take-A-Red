// Author  : Curtis Stephenson
// Date    : 09/02/2017
// Purpose : This is the class used for pickup point management. This will be separate from other scripts which dictate the contents of
//           such points directly, such as keycards and the like, these will purely be for weapons and pickups despite what the awkward
//           class name that I might change is. 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupPoint : MonoBehaviour 
{
    public GameObject[] everyWeapon;
    public GameObject[] everyPickup;

	// This will hold a reference to the player, or whatever ends up holding the script component we need to access, that being the object
	// with the PlayerCharacterManager, so we can access it and use the SetWeapon or SetPickup functions to give the player a certain item
	//GameObject playerReference;
    PlayerCharacterManager playerManagerReference;

    GameObject currentPickup;

    // We're storing the transform of the parent so we can alter it because Unity is being annoying and I don't want to spend all night doing
    // something which should be really simple
    Vector3 parentTransform;

	// These two functions handle how long should be waited between new pickups being generated once the platform is no longer occupied
	// by anything, meaning that the platform will not just refresh for infinite pickups while the player stands on it
	const float pickupRefreshTimeMax = 10.0f;
	float pickupRefreshTimeCurrent = 0.0f;

	// GeneratePickup is used to generate a pickup for the PickupPoint, using random ranges of numbers to determine what to place on
	// them, making the points random in terms of their contents in multiple playthroughs. The current way this works is there's a 
	// 7/10 chance for a pickup point to hold a weapon, and a 3/10 chance for it to contain a pickup, with a generic roll between
	// the items in those categories so there's no additional chance weighting
	void GeneratePickup()
	{
		if (!currentPickup)
		{
			int rollWepOrPick = Random.Range (1, 10);

			if (rollWepOrPick <= 7)
			{
				int rollWeapon = Random.Range (0, 3);

                currentPickup = Instantiate(everyWeapon[rollWeapon], parentTransform + new Vector3(0, 3, 0), transform.rotation);
            }
			else
			{
				int rollPick = Random.Range (0, 4);

                currentPickup = Instantiate(everyPickup[rollPick], parentTransform + new Vector3(0, 3, 0), transform.rotation);
            }
		}
	} // GeneratePickup()

	// Use this for initialization
	void Start () 
	{
        parentTransform = transform.position;
        //GameObject playerManagerObjectRef = GameObject.FindGameObjectWithTag("PlayerManager");
        //playerManagerReference = playerManagerObjectRef.GetComponent<PlayerCharacterManager>();
		GeneratePickup ();
	} // Start()

	// Update is called once per frame
	void Update () 
	{
        if (!currentPickup)
        {
            pickupRefreshTimeCurrent -= (1.0f * Time.deltaTime);
        }
        else
        {
            pickupRefreshTimeCurrent = pickupRefreshTimeMax;
        }

		if ((pickupRefreshTimeCurrent <= 0.0f) && (!currentPickup))
		{
			GeneratePickup ();
		}
	} // Update()
}
