using UnityEngine;
using UnityEngine.UI;


public class PowerupUI : MonoBehaviour 
{
	
	public Image PowerupOnScreen;


	public Sprite HealthSmall;
	public Sprite HealthLarge;
	public Sprite SpeedBoost;
	public Sprite DamageBoost;


	//Referencing the Player Character Manager script
	PlayerCharacterManager playerManagerReference; 


	void Start () 
	{
		GameObject playerManagerObjectRef = GameObject.FindGameObjectWithTag("PlayerManager");
		playerManagerReference = playerManagerObjectRef.GetComponent<PlayerCharacterManager>(); 

		PowerupOnScreen = gameObject.GetComponent<Image>();

		PowerupOnScreen.enabled = false;
	}


	void Update () 
	{
		if (playerManagerReference.GetPick () == Pickups.SPEED) 
		{
			//AkSoundEngine.PostEvent ("SpeedBoost", gameObject);

			PowerupOnScreen.enabled = true;
			PowerupOnScreen.sprite = SpeedBoost;

		} 

		else if (playerManagerReference.GetPick () == Pickups.DAMAGEBOOST) 
		{
			//AkSoundEngine.PostEvent ("DamageBoost", gameObject);

			PowerupOnScreen.enabled = true;
			PowerupOnScreen.sprite = DamageBoost;

		} 

		else if (playerManagerReference.GetPick () == Pickups.HEALTHSMALL) 
		{
			//AkSoundEngine.PostEvent ("HealthSmall", gameObject);

			PowerupOnScreen.enabled = true;
			PowerupOnScreen.sprite = HealthSmall;

		} 

		else if (playerManagerReference.GetPick () == Pickups.HEALTHLARGE) 
		{
			//AkSoundEngine.PostEvent ("HealthLarge", gameObject);

			PowerupOnScreen.enabled = true;
			PowerupOnScreen.sprite = HealthLarge;
		}
	}

}
