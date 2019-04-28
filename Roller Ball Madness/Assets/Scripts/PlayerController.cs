using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Video 1 - https://www.youtube.com/watch?v=KPCV89buN4o&list=PLLH3mUGkfFCWCsGUfwLMnDWdkpQuqW3xa&index=9
// Video 2 - https://www.youtube.com/watch?v=KcKo8QHOjlk&list=PLLH3mUGkfFCWCsGUfwLMnDWdkpQuqW3xa&index=30 Start time buffer

public class PlayerController : MonoBehaviour
{
	private const float TIME_BEFORE_START = 4.0f; // Any changes to this value also have to be changed in the LevelManager script.

	public float moveSpeed = 5.0f;
	public float drag = .5f;
	public float terminalRotationSpeed = 25f;
	public VirtualJoystick moveJoystick;

	//Boost Stuff
	private float boostSpeed = 20f;
	public float boostCooldown = 2f;
	private float lastBoost;
	//Jump Stuff
	private float jumpForce = 10f;
	private float jumpCooldown = 2f;
	private float lastJump;
	private bool canJump = true;
	// Moving Obsticle Collision Stuff
	public float colForce = 5;

	private Rigidbody rigidB;
	private Transform camTransform;

	private float startTime;

	public Material[] playerMatArray;
	public GameObject playerPrefab;

	private void Start()
	{
		int skinIndex = GameManager.Instance.currentSkinIndex;

		// Change the players skin to the selected material.										--------- Try moving all this to the GameManager script, Dont forget this duplicate code in the MainMenu script
		Renderer playerRenderer = playerPrefab.GetComponent<Renderer>();    // Get access to the Renderer Component on the player.
		playerRenderer.sharedMaterial = playerMatArray[skinIndex];      // Set the players skin to the material in the material array at the GameManagers currentSkinIndex number.

		//lastBoost = Time.time - boostCooldown; // Enables the boost as soon as the game starts, otherwise the game would start with the boost cooldown in effect. ---- Dosnt seem to be needed.
		startTime = Time.time;

		rigidB = GetComponent<Rigidbody>();
		rigidB.maxAngularVelocity = terminalRotationSpeed;
		rigidB.drag = drag;

		camTransform = Camera.main.transform;
	}

	private void Update()
	{
		// Debuging Controlls
		if (Input.GetKeyDown(KeyCode.Space))
		{
			Jump();
		}
		if (Input.GetKeyDown(KeyCode.LeftShift))
		{
			Boost();
		}
		// Debuging Controlls end.
	}

	private void FixedUpdate()
	{
		if (Time.time - startTime < TIME_BEFORE_START)
		{
			return;
		}

		Vector3 dir = Vector3.zero;

		dir.x = Input.GetAxis("Horizontal");
		dir.z = Input.GetAxis("Vertical");

		if(dir.magnitude > 1)
		{
			dir.Normalize();
		}

		if (moveJoystick.InputDirection != Vector3.zero)
		{
			dir = moveJoystick.InputDirection;
		}

		//Rotate our direction with the camera
		Vector3 rotatedDir = camTransform.TransformDirection(dir);
		rotatedDir = new Vector3(rotatedDir.x, 0, rotatedDir.z);
		rotatedDir = rotatedDir.normalized * dir.magnitude;

		rigidB.AddForce(rotatedDir * moveSpeed);
	}

	public void Boost()
	{
		if (Time.time - startTime < TIME_BEFORE_START)
		{
			return;
		}

		if (Time.time - lastBoost > boostCooldown)
		{
			lastBoost = Time.time;
			rigidB.AddForce(moveJoystick.InputDirection * boostSpeed, ForceMode.VelocityChange);        // Adds force in the direction of the move joystick.
			//rigidB.AddForce(camTransform.forward * boostSpeed, ForceMode.VelocityChange);				// Add force in the direction the camera is facing.
			//rigidB.AddForce(controller.velocity.normalized * boostSpeed, ForceMode.VelocityChange);	// Adds force in the direction that the ball is moving.
		}
	}

	public void Jump()
	{
		if (Time.time - startTime < TIME_BEFORE_START)
		{
			return;
		}

		if (Time.time - lastJump > jumpCooldown || (canJump == true))	// If current point in time minus the time it was when we last jumped is greater than the boost cooldown value
		{																// or if the canJump bool is set to true...
			lastJump = Time.time;	// Set the value of last jump to the current point in time
			rigidB.AddForce(camTransform.up * jumpForce, ForceMode.VelocityChange);   // Add force in the up direction relative to the camera.	
			canJump = false;	// Set the canJump bool to false.
		}
	}

	private void OnCollisionEnter(Collision col)
	{
		// Ground Collisions
		if (col.gameObject.tag == "Ground") // If we collide with the ground --Dont forget to tag all ground objects with the "Ground" tag.
		{
			canJump = true; // Set the canjump bool to true.
		}
		// Ground Collisions end.

		// Moving Obsticle Collisions.
		// force is how forcefully we will push the player away from the enemy.
		// If the object we hit is the enemy
		if (col.gameObject.tag == "Moving Obsticle")
		{
			// Calculate Angle Between the collision point and the player
			Vector3 dir = col.contacts[0].point - transform.position;
			// We then get the opposite (-Vector3) and normalize it
			dir = -dir.normalized;
			// And finally we add force in the direction of dir and multiply it by force. 
			// This will push back the player
			rigidB.AddForce(dir * colForce, ForceMode.VelocityChange);
			Debug.Log("Player pushed back");
			// Moving Obsticle Collisions end.
		}
	}
}
