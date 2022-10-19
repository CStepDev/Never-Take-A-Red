using UnityEngine;
using UnityEngine.UI;


public class CollectibleCardsUI : MonoBehaviour 
{

	public GameObject CollectibleCard1;
	public GameObject CollectibleCard2;
	public GameObject CollectibleCard3;


	//Referencing the Player Character Manager script
	PlayerCharacterManager playerManagerReference;


	void Start () 
	{
		GameObject playerManagerObjectRef = GameObject.FindGameObjectWithTag("PlayerManager");
		playerManagerReference = playerManagerObjectRef.GetComponent<PlayerCharacterManager>();


		CollectibleCard1.gameObject.SetActive(false);
		CollectibleCard2.gameObject.SetActive(false);
		CollectibleCard3.gameObject.SetActive(false);
	}



	void Update () 
	{
		if (playerManagerReference.GetKeyCount () == 1) 
		{
			CollectibleCard1.gameObject.SetActive(true);
		}

		else if (playerManagerReference.GetKeyCount () == 2) 
		{
			CollectibleCard2.gameObject.SetActive(true);

		}

		else if (playerManagerReference.GetKeyCount () == 3) 
		{
			CollectibleCard3.gameObject.SetActive(true);

		}
	}
}
