// Author  : Curtis Stephenson
// Date    : 21/10/2017
// Purpose : The CharacterController script is used to house all of the player's controller inputs during gameplay. (Not menu controls, these will be a different script)
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterController : MonoBehaviour 
{
	// These floats hold the values corresponding to the stick states, and are modified down in Update.
	// (See the use of deltaTime with the leftStick values for the usefulness of doing it this way)
	public float leftStickX;
	public float leftStickY;
	public float rightStickX;
	public float rightStickY;


	// This was the missing component from getting controller inputs, which I was reminded of thanks to Dan. These dead zone values are here
	// so we know when NOT to poll the sticks for data to store in the floats above. These dead zones represent the states of the stick near
	// their resting positions in the center. I stress NEAR center because the sticks on the controller are never completely in the center no 
	// matter how you let go of them, leaving them returning small values. The Constant (const) keyword that comes before them means they cannot
	// be modified after they're initialized here. This is handy just in case something were to try and change the dead zones for whatever reason
	const float lowerDeadZone = -0.25f;
	const float upperDeadZone = 0.25f;


	// This will hold a reference to the Rigidbody component, and is used to add velocity and rotations to the character. 
	public Rigidbody rb;


	// This is a reference to the animator component, and is used to change animations and such according to variables
	public Animator anim;


	// Oooh, this is a naughty public variable. Shh, don't tell anyone!
	// This float will control the character's speed, and can be modified from outside of this class due to it being public. This can be done in
	// the editor too, which is why you'll see it there.
	public float characterSpeed = 750;


	// This one is borrowed from Dan's PS4 tutorials. This bool will enable or disable, depending on the current state, error messages from the stick.
	public bool verbose;


	// This variable is used to hold a LayerMask of the floor, needed for mouse based aiming
	private int floorMask;

	// This variable is used to cast from the camera to the floor mask
	private float camRayLength = 100.0f;

	// Two magic functions I'd forgot we'd need originally. These clear the values inside of the stick storages we're using.
	// I'd forgot the values within wouldn't be cleared on their own, which lead to me almost tearing my hair out over why it wouldn't 
	//do what I wanted. It's been a long day, I'd appreciate a tiny bit of slack for my disregard for coding logic.
	void ClearLeftStick()
	{
		leftStickX = 0.0f;
		leftStickY = 0.0f;
	} // End of ClearLeftStick


	void ClearAllSticks()
	{
		leftStickX = 0.0f;
		leftStickY = 0.0f;
		rightStickX = 0.0f;
		rightStickY = 0.0f;
	} // End of ClearAlLSticks


	// We're using the start function for two things, resetting the states of the joysticks to zero/neutral until they're moved again, and
	// ensuring we start off with freshly cleared stick storage floats. You may think it's safe to assume they'd be empty after 
	// being declared further up, but you'd be wrong. Safety comes first, and this is ensuring we're being safe. (Search about junk data
	// in initialized variables for a much longer explanation)
	void Start () 
	{
		Input.ResetInputAxes ();
		ClearAllSticks ();
		rb = GetComponent<Rigidbody> ();
		anim = GetComponent<Animator> ();
		floorMask = LayerMask.GetMask ("Floor");
	} // End of Start


	// The second input method programmed for NTAR. It should've likely been the first, but in the
	// design document we'd settled on a controller due to the type of game we wanted it to be and
	// that's what we went with. I would've added keyboard and mouse before the deadline if things
	// didn't get in the way, but now I'm finally making good on the extension task of implementing
	// the control method. It's based on the controller method with some variables and names swapped
	// or omitted, it could've been implemented more efficiently than this overall but oh well.
	void MoveKeyboardMouse()
	{
		// Start of LeftStickX
		if ((Input.GetAxisRaw("Horizontal") < lowerDeadZone) || (Input.GetAxisRaw("Horizontal") > upperDeadZone))
		{
			// DEBUG
			if (verbose)
			{
				Debug.Log("Bypassed DeadZone on Horizontal!");
			} // DEBUG


			if (Input.GetAxisRaw("Horizontal") > 0)
			{
				leftStickX = ((characterSpeed + (characterSpeed / 5)) * Time.deltaTime);
			}

			if (Input.GetAxisRaw("Horizontal") <= 0)
			{
				leftStickX = ((characterSpeed + (characterSpeed / 5)) * Time.deltaTime) * -1;
			}

			// DEBUG
			if (verbose)
			{
				Debug.Log("Horizontal = " + leftStickX);
			} // DEBUG
		} // End of LeftStickX

		// Start of LeftStickY
		if ((Input.GetAxisRaw("Vertical") < lowerDeadZone) || (Input.GetAxisRaw("Vertical") > upperDeadZone))
		{
			// DEBUG
			if (verbose)
			{
				Debug.Log("Bypassed DeadZone on Vertical");
			} // DEBUG

			if (Input.GetAxisRaw("Vertical") > 0.0f)
			{
				leftStickY = (characterSpeed * Time.deltaTime);
			}

			if (Input.GetAxisRaw("Vertical") <= 0.0f)
			{
				leftStickY = (characterSpeed * Time.deltaTime) * -1;
			}

			// DEBUG
			if (verbose)
			{
				Debug.Log("Vertical = " + leftStickY);
			} // DEBUG
		} // End of LeftStickY

		// Rotational data for mouse hitting the floor of the level
		Ray camRay = Camera.main.ScreenPointToRay (Input.mousePosition);

		RaycastHit floorHit;

		if (Physics.Raycast(camRay, out floorHit, camRayLength, floorMask))
		{
			Vector3 playerToMouse = floorHit.point - transform.position;

			playerToMouse.y = 0.0f;

			// Quaternion to use on rotation
			Quaternion newRotation = Quaternion.LookRotation(playerToMouse);

			rb.MoveRotation (newRotation);
		}


		// I'm adding LeftStickY to the Z compontent of a Vector3 because of 3D space. Adding LeftStickY to the Y component would make
		// the character rise and fall with each stick input due to how 3D space works. 
		rb.velocity = new Vector3 (leftStickX, 0, leftStickY);

		// This ensures the rotation of the character will always be towards the rotation of the stick. Please do not ask me how exactly this works,
		// because I can't wrap my head around quaternions entirely no matter how much I read about them.
		//rb.rotation = Quaternion.Euler(rb.rotation.x, Mathf.Atan2(-rightStickX, -rightStickY) * Mathf.Rad2Deg, rb.rotation.z);

		// The final thing we need to do before cleaning data is to check if animation variables need updating based on what stick activity we stored during
		// this cycle. The first thing we check is if the left stick has any value greater than zero, and if it does we play the moving animation
		if ((leftStickX != 0) || (leftStickY != 0))
		{
			anim.SetBool ("Moving", true);
		}
		else
		{
			anim.SetBool ("Moving", false);
		}

		//This is the part of the script where I accidently ruin Curtis's script. more animation variables which are controlled by the movement of the right analog stick.
		if ((rightStickX <= -0.26))
		{
			anim.SetBool ("lookingEast", true);
			anim.SetBool ("lookingSouth", false);
			anim.SetBool ("lookingWest", false);

		}

		if ((rightStickX >= 0.26))
		{
			anim.SetBool ("lookingSouth", false);
			anim.SetBool ("lookingWest", true);
			anim.SetBool ("lookingEast", false);
		}

		if ((rightStickY <= -0.26))
		{
			anim.SetBool("lookingSouth", false);
			anim.SetBool("lookingWest", false);
			anim.SetBool("lookingEast", false);
		}

		if ((rightStickY >= 0.26))
		{
			anim.SetBool ("lookingSouth", true);
			anim.SetBool ("lookingWest", false);
			anim.SetBool ("lookingEast", false);
		}
			
		ClearLeftStick();
	} // End of MoveKeyboardMouse


	// This is ALL of the specific code for using the controller with the game. It likely isn't the
	// most efficient way of doing it, but it's a way which works with the game. This was the only
	// input originally defined for NTAR, so other input methods like keyboard and mouse were built
	// using the code.
	void MoveController()
	{
		// Start of LeftStickX
		if ((Input.GetAxisRaw("ControllerLeftStickX") < lowerDeadZone) || (Input.GetAxisRaw("ControllerLeftStickX") > upperDeadZone))
		{
			// DEBUG
			if (verbose)
			{
				Debug.Log("Bypassed DeadZone on ControllerLeftStickX");
			} // DEBUG


			if (Input.GetAxisRaw("ControllerLeftStickX") > 0)
			{
				leftStickX = ((characterSpeed + (characterSpeed / 5)) * Time.deltaTime);
			}

			if (Input.GetAxisRaw("ControllerLeftStickX") <= 0)
			{
				leftStickX = ((characterSpeed + (characterSpeed / 5)) * Time.deltaTime) * -1;
			}

			// DEBUG
			if (verbose)
			{
				Debug.Log("LeftStick X = " + leftStickX);
			} // DEBUG
		} // End of LeftStickX

		// Start of LeftStickY
		if ((Input.GetAxisRaw("ControllerLeftStickY") < lowerDeadZone) || (Input.GetAxisRaw("ControllerLeftStickY") > upperDeadZone))
		{
			// DEBUG
			if (verbose)
			{
				Debug.Log("Bypassed DeadZone on ControllerLeftStickY");
			} // DEBUG

			if (Input.GetAxisRaw("ControllerLeftStickY") > 0.0f)
			{
				leftStickY = (characterSpeed * Time.deltaTime);
			}

			if (Input.GetAxisRaw("ControllerLeftStickY") <= 0.0f)
			{
				leftStickY = (characterSpeed * Time.deltaTime) * -1;
			}

			// DEBUG
			if (verbose)
			{
				Debug.Log("LeftStick Y = " + leftStickY);
			} // DEBUG
		} // End of LeftStickY

		// Start of RightStickX
		if ((Input.GetAxisRaw("ControllerRightStickX") < lowerDeadZone) || (Input.GetAxisRaw("ControllerRightStickX") > upperDeadZone))
		{
			// DEBUG
			if (verbose)
			{
				Debug.Log("Bypassed DeadZone on ControllerRightStickX");
			} // DEBUG

			// We need to store BOTH axes if it breaks over the deadzone, because otherwise we won't be able to hit north, south, east or west
			// due to conditions never being fulfilled for the other stick axis. 
			rightStickX = Input.GetAxisRaw ("ControllerRightStickX"); 
			rightStickY = Input.GetAxisRaw ("ControllerRightStickY");

			// DEBUG
			if (verbose)
			{
				Debug.Log ("rightStickX = " + rightStickX);
				Debug.Log ("rightStickY = " + rightStickY);
			} // DEBUG
		} // End of RightStickX

		// Start of RightStickY
		if ((Input.GetAxisRaw("ControllerRightStickY") < lowerDeadZone / 3) || (Input.GetAxisRaw("ControllerRightStickY") > upperDeadZone / 3))
		{
			// DEBUG
			if (verbose)
			{
				Debug.Log("Bypassed DeadZone on ControllerRightStickY");
			} // DEBUG

			// We need to store BOTH axes if it breaks over the deadzone, because otherwise we won't be able to hit north, south, east or west
			// due to conditions never being fulfilled for the other stick axis.
			rightStickX = Input.GetAxisRaw ("ControllerRightStickX");
			rightStickY = Input.GetAxisRaw ("ControllerRightStickY");

			// DEBUG
			if (verbose)
			{
				Debug.Log ("rightStickX = " + rightStickX);
				Debug.Log ("rightStickY = " + rightStickY);
			} // DEBUG
		} // End of RightStickY


		// I'm adding LeftStickY to the Z compontent of a Vector3 because of 3D space. Adding LeftStickY to the Y component would make
		// the character rise and fall with each stick input due to how 3D space works. 
		rb.velocity = new Vector3 (leftStickX, 0, leftStickY);

		// This ensures the rotation of the character will always be towards the rotation of the stick. Please do not ask me how exactly this works,
		// because I can't wrap my head around quaternions entirely no matter how much I read about them.
		rb.rotation = Quaternion.Euler(rb.rotation.x, Mathf.Atan2(-rightStickX, -rightStickY) * Mathf.Rad2Deg, rb.rotation.z);

		// The final thing we need to do before cleaning data is to check if animation variables need updating based on what stick activity we stored during
		// this cycle. The first thing we check is if the left stick has any value greater than zero, and if it does we play the moving animation
		if ((leftStickX != 0) || (leftStickY != 0))
		{
			anim.SetBool ("Moving", true);
		}
		else
		{
			anim.SetBool ("Moving", false);
		}

		//This is the part of the script where I accidently ruin Curtis's script. more animation variables which are controlled by the movement of the right analog stick.
		if ((rightStickX <= -0.26))
		{
			anim.SetBool ("lookingEast", true);
			anim.SetBool ("lookingSouth", false);
			anim.SetBool ("lookingWest", false);

		}

		if ((rightStickX >= 0.26))
		{
			anim.SetBool ("lookingSouth", false);
			anim.SetBool ("lookingWest", true);
			anim.SetBool ("lookingEast", false);
		}

		if ((rightStickY <= -0.26))
		{
			anim.SetBool("lookingSouth", false);
			anim.SetBool("lookingWest", false);
			anim.SetBool("lookingEast", false);
		}

		if ((rightStickY >= 0.26))
		{
			anim.SetBool ("lookingSouth", true);
			anim.SetBool ("lookingWest", false);
			anim.SetBool ("lookingEast", false);
		}


		// We call this at the end of the PlayerCharacterController update so the left stick floats are empty for the next check of the controller. We do not
		// clear both sticks of their data because then the character would stop facing the last pushed direction of the right stick, because we
		// would've emptied the float telling it to do so.
		ClearLeftStick();
	} // End of MoveController


	// The fucntion which controls the function used for moving the player character around the level,
	// which essentially checks which movement type should be used depending on which option for input
	// is being used
	void Move()
	{
		MoveKeyboardMouse ();
	}


	// The update functions handles polling both of the sticks on the controller each frame and responds appropriately. There isn't much to
	// say up above because I feel it's easier to explain step by step in comments below.
	void Update () 
	{
		Move ();
	} // End of Update
} // End of PlayerCharacterController
