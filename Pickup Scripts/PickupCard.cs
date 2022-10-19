// Author  : Curtis Stephenson
// Date    : 11/05/2017
// Purpose : The script responsible for the keycard pickups found within the environments of the level, and
//           are used by the player the 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupCard : MonoBehaviour
{
    PlayerCharacterManager playerManagerReference;
    Vector3 positionHolder;

	void Start ()
    {
        GameObject playerManagerObjectRef = GameObject.FindGameObjectWithTag("PlayerManager");
        playerManagerReference = playerManagerObjectRef.GetComponent<PlayerCharacterManager>();

        // We're going to store the starting position of the card so we can actually access the Y value easily
        positionHolder = transform.position;
	}

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            playerManagerReference.AddKey();

            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<MeshCollider>().enabled = false;
            GetComponent<BoxCollider>().enabled = false;
        }
    } // OnTriggerStay(Collider other)

    void Update ()
    {
        transform.Rotate(new Vector3(0, 0, 45) * Time.deltaTime);

        positionHolder.y += Mathf.Sin(Time.fixedTime * Mathf.PI * 1.0f) * 0.075f;

        transform.position = positionHolder;
    }
}
