// Author  : Curtis Stephenson
// Date    : 01/03/2018
// Purpose : PlayerWeapon manages which weapon should be shot when pressing the right trigger. It also stores information on each of the
//           guns, such as their fire speeds and ranges.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour 
{
	// These variables are related to the various weapons that can be used by the player, and only
	// leaves the damage per weapon type exposed to be modified outside of the script. The time it
	// takes between shots has been left as a const to keep it from being modified in any way.
	// -- START OF WEAPON VARIABLES --
	public int handgunDamagePerShot = 20;
	const float handgunTimeBetweenBullets = 0.50f;
	const float handgunRange = 25f;
	const float handgunEffectTime = 0.05f;

	public int machinegunDamagePerShot = 10;
	const float machinegunTimeBetweenBullets = 0.15f;
	const float machinegunRange = 25f;
	const float machinegunEffectTime = 0.025f;

	public int shotgunDamagePerShot = 50;
	const float shotgunTimeBetweenBullets = 0.75f;
	const float shotgunRange = 10f;
	const float shotgunEffectTime = 0.05f;

	public int railgunDamagePerShot = 100;
	const float railgunTimeBetweenBullets = 1.0f;
	const float railgunRange = 100f;
	const float railgunEffectTime = 0.05f;

	public int modifier = 1; // This is used specifically for the damage boost power up, I couldn't think of a better place for it
	// -- END OF WEAPON VARIABLES --

	PlayerCharacterManager playerManagerReference; // A variable used to hold a reference to the player manager
	Weapons currentPlayerWeapon; // A variable used to store the current weapon the player is holding in PlayerCharacterManager

	float timer; // This variable keeps track over whether the weapon can be fired or not
	Ray shootRay; // Reference to the raycasts used
	RaycastHit shootHit; // The end position of the raycast
	int shootableMask; // The mask reference, which covers all "shootable" layer objects
	LineRenderer gunLine; // The visible line in the scene when shooting, merely a reference
	Light gunLight; // The visible light in the scene when shooting, again a reference
	float effectsDisplayTime = 0.05f; // Determines how long the "effect" sticks around on screen
	RaycastHit[] hits; // Stores the hits detected per shot of a piercing weapon (Shotgun, Railgun)


	// This is where the majority of the magic happens. This function will essentially determine what
	// to fire based on which weapon is currently being held, along with determining what is hit by the
	// raycasts
	void Shoot()
	{
		playerManagerReference.UseBullet ();
		timer = 0f; // Reset the timer used to clear effects

		// A massive Switch statement to handle the shooting possibilities of each gun the player can
		// hold. There may be a line of code here or there that isn't required for the specific weapon
		// case it falls under, but I was too lazy to tidy it up considering it doesn't really affect
		// much in the long run so long as the gun is shooting correctly
		switch (currentPlayerWeapon)
		{
		case Weapons.HANDGUN:
			AkSoundEngine.PostEvent ("HandGun", gameObject);

			gunLight.enabled = true;

			gunLine.endWidth = 0.5f;
			gunLine.enabled = true;
			gunLine.SetPosition (0, transform.position);

			shootRay.origin = transform.position;
			shootRay.direction = -(transform.forward);

			if (Physics.Raycast(shootRay, out shootHit, handgunRange, shootableMask))
			{
				EnemyHealth enemyHealth = shootHit.collider.GetComponent<EnemyHealth> ();

				if (enemyHealth != null)
				{
					enemyHealth.TakeDamage (handgunDamagePerShot * modifier);
				}

				gunLine.SetPosition (1, shootHit.point);
			}
			else
			{
				gunLine.SetPosition (1, shootRay.origin + shootRay.direction * handgunRange);
			}
			break; // End of HANDGUN case

		case Weapons.MACHINEGUN:

			AkSoundEngine.PostEvent ("MachineGun", gameObject);

			gunLight.enabled = true;

			gunLine.endWidth = 0.5f;
			gunLine.enabled = true;
			gunLine.SetPosition (0, transform.position);

			shootRay.origin = transform.position;
			shootRay.direction = -(transform.forward);

			if (Physics.Raycast(shootRay, out shootHit, machinegunRange, shootableMask))
			{
				EnemyHealth enemyHealth = shootHit.collider.GetComponent<EnemyHealth> ();

				if (enemyHealth != null)
				{
					enemyHealth.TakeDamage (machinegunDamagePerShot * modifier);
				}

				gunLine.SetPosition (1, shootHit.point);
			}
			else
			{
				gunLine.SetPosition (1, shootRay.origin + shootRay.direction * machinegunRange);
			}
			break; // End of MACHINEGUN case

		case Weapons.SHOTGUN:
			AkSoundEngine.PostEvent ("ShotGun", gameObject);

			gunLight.enabled = true;

			gunLine.endWidth = 5.0f;
			gunLine.enabled = true;
			gunLine.SetPosition (0, transform.position);

			shootRay.origin = transform.position;
			shootRay.direction = -(transform.forward);

			gunLine.SetPosition (1, shootRay.origin + shootRay.direction * shotgunRange);

			hits = Physics.RaycastAll(shootRay.origin, shootRay.direction, shotgunRange, shootableMask);

			for (int i = 0; i < hits.Length; i++)
			{
				EnemyHealth enemyHealth = hits[i].collider.GetComponent<EnemyHealth> ();

				if (enemyHealth != null)
				{
					enemyHealth.TakeDamage (shotgunDamagePerShot * modifier);
				}
			}
			break; // End of SHOTGUN case

		case Weapons.RAILGUN:
			AkSoundEngine.PostEvent ("RailGun", gameObject);

			gunLight.enabled = true;

			gunLine.endWidth = 0.5f;
			gunLine.enabled = true;
			gunLine.SetPosition (0, transform.position);

			shootRay.origin = transform.position;
			shootRay.direction = -(transform.forward);

			gunLine.SetPosition (1, shootRay.origin + shootRay.direction * railgunRange);

			hits = Physics.RaycastAll(shootRay.origin, shootRay.direction, railgunRange, shootableMask);

			for (int i = 0; i < hits.Length; i++)
			{
				EnemyHealth enemyHealth = hits[i].collider.GetComponent<EnemyHealth> ();

				if (enemyHealth != null)
				{
					enemyHealth.TakeDamage (railgunDamagePerShot * modifier);
				}
			}
			break; // End of RAILGUN case
		}
	}


	// Here's where we disable the 'effects', which essentially involves just turning off the line renderer
	// which is drawn once a gun is fired and turns off the muzzle flash
	public void DisableEffects()
	{
		gunLine.enabled = false;
		gunLight.enabled = false;

		//gunLine.enabled = false;
	}

	// This grabs references once the object is 'Awake'. It's basically Start, but it's not. I can't remember
	// the fine details, but I just did it because I felt like I wanted to do something different. Start() would
	// have probably worked, but eh
	void Awake()
	{
		GameObject playerManagerObjectRef = GameObject.FindGameObjectWithTag("PlayerManager");
		playerManagerReference = playerManagerObjectRef.GetComponent<PlayerCharacterManager>();

		shootableMask = LayerMask.GetMask ("Shootable");
		gunLine = GetComponent<LineRenderer>();
		gunLight = GetComponent<Light>();
	}

	// Update() is where the PlayerWeapon 'listens' for a RightTrigger hit, then calls the Shoot() function if it
	// was and the player is allowed to fire again. It also disables the effects of the weapon depending on the 
	// timer being greater than the time between the currently held weapon's bullets. 
	void Update()
	{
		timer += Time.deltaTime; // Add a specific amount to the timer per frame. Do not mess with this, the world hinges on it.

		// The check for the weapon must be made each and every frame, this is to ensure the PlayerWeapon script is
		// always adhering to the rules of the weapon that is currently equipped.
		currentPlayerWeapon = playerManagerReference.GetWep();

		// This is the Switch statement which deals with all of the shooting related stuff. It helps reduce the
		// clutter of a bunch of if else statements, making it look a little more tidy in the process. This kind
		// of statement can be used to check for a number of things, you just need to know what each individual case 
		// it could hit should be before relying on it. If there's a chance you won't know the case, use if else.
		// We do however know it in this circumstance, as we can only ever have one of four weapons.
		switch (currentPlayerWeapon)
		{
			case Weapons.HANDGUN:
				if ((Input.GetAxis("RightTrigger") > 0.0f) && (timer >= handgunTimeBetweenBullets) && (Time.timeScale != 0))
				{
					Shoot ();
				}
				else if ((Input.GetAxis("Fire1") > 0.0f) && (timer >= handgunTimeBetweenBullets) && (Time.timeScale != 0))
				{
					Shoot ();
				}

				if (timer >= (handgunTimeBetweenBullets * effectsDisplayTime))
				{
					DisableEffects ();
				}
				break; // End of HANDGUN

			case Weapons.MACHINEGUN:
				if ((Input.GetAxis("RightTrigger") > 0.0f) && (timer >= machinegunTimeBetweenBullets) && (Time.timeScale != 0))
				{
					Shoot ();
				}
				else if ((Input.GetAxis("Fire1") > 0.0f) && (timer >= machinegunTimeBetweenBullets) && (Time.timeScale != 0))
				{
					Shoot ();
				}

				if (timer >= (machinegunTimeBetweenBullets * effectsDisplayTime))
				{
					DisableEffects ();
				}
				break; // End of MACHINEGUN

			case Weapons.SHOTGUN:
				if ((Input.GetAxis("RightTrigger") > 0.0f) && (timer >= shotgunTimeBetweenBullets) && (Time.timeScale != 0))
				{
					Shoot ();
				}
				else if ((Input.GetAxis("Fire1") > 0.0f) && (timer >= shotgunTimeBetweenBullets) && (Time.timeScale != 0))
				{
					Shoot ();
				}

				if (timer >= (shotgunTimeBetweenBullets * effectsDisplayTime))
				{
					DisableEffects ();
				}
				break; // End of SHOTGUN

			case Weapons.RAILGUN:
				if ((Input.GetAxis("RightTrigger") > 0.0f) && (timer >= railgunTimeBetweenBullets) && (Time.timeScale != 0))
				{
					Shoot ();
				}
				else if ((Input.GetAxis("Fire1") > 0.0f) && (timer >= railgunTimeBetweenBullets) && (Time.timeScale != 0))
				{
					Shoot ();
				}

				if (timer >= (railgunTimeBetweenBullets * effectsDisplayTime))
				{
					DisableEffects ();
				}
				break; // End of RAILGUN
		}
	}
}
