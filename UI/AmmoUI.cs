using UnityEngine;
using UnityEngine.UI; 


public class AmmoUI : MonoBehaviour 
{

	public Text ammoText;

	int PlayerAmmo;

	PlayerCharacterManager playerManagerReference;



	void Start () 
	{
		GameObject playerManagerObjectRef = GameObject.FindGameObjectWithTag("PlayerManager");
		playerManagerReference = playerManagerObjectRef.GetComponent<PlayerCharacterManager>();

		ammoText = gameObject.GetComponent<Text>();
	}



	void Update () 
	{
		PlayerAmmo = playerManagerReference.GetCurrentAmmo();

		ammoText.text = PlayerAmmo.ToString ();

	}
}
