using UnityEngine;
using UnityEngine.UI;


public class SwitchGunsUI : MonoBehaviour 
{
	//This will be the object that will store the image of the current weapon that the 
	//game character is currently holding.
	public Image weaponOnScreen;


	//Sprites that will be attached to the "weaponOnScreen" Image to always update 
	//the weapon that the game character is holding with the weapon that is on the HUD.
	public Sprite Pistol;
	public Sprite Shotgun;
	public Sprite Assault;
	public Sprite UltimateWeapon;


	//Referencing the Player Character Manager script
	PlayerCharacterManager playerManagerReference;


	void Start () 
	{
		GameObject playerManagerObjectRef = GameObject.FindGameObjectWithTag("PlayerManager");
		playerManagerReference = playerManagerObjectRef.GetComponent<PlayerCharacterManager>();

		weaponOnScreen = gameObject.GetComponent<Image>();

	}


	//the following code will update the HUD so that the current weapon the game character is holding 
	//will match the image of the same weapon on the HUD
	void Update () 
	{
		
		if (playerManagerReference.GetWep () == Weapons.HANDGUN) 
		{
			weaponOnScreen.sprite = Pistol;
		} 

		else if (playerManagerReference.GetWep() == Weapons.MACHINEGUN) 
		{
			weaponOnScreen.sprite = Assault;
		} 

		else if (playerManagerReference.GetWep () == Weapons.SHOTGUN) 
		{
			weaponOnScreen.sprite = Shotgun;
		} 

		else if (playerManagerReference.GetWep () == Weapons.RAILGUN) 
		{
			weaponOnScreen.sprite = UltimateWeapon;
		}
	}
}
