using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Video 1 - https://www.youtube.com/watch?v=KPCV89buN4o&list=PLLH3mUGkfFCWCsGUfwLMnDWdkpQuqW3xa&index=9
// Video 2 - https://www.youtube.com/watch?v=KcKo8QHOjlk&list=PLLH3mUGkfFCWCsGUfwLMnDWdkpQuqW3xa&index=30 Start time buffer

public class PlayerController : MonoBehaviour
{
	private const float TIME_BEFORE_START = 3.0f;

	public float moveSpeed = 5.0f;
	public float drag = .5f;
	public float terminalRotationSpeed = 25f;
	public VirtualJoystick moveJoystick;

	//Boost Stuff
	private float boostSpeed = 20f;
	public float boostCooldown = 2f;
	private float lastBoost;

	private Rigidbody controller;
	private Transform camTransform;

	private float startTime;

	public Material[] playerMatArray;
	public GameObject playerPrefab;

	private void Start()
	{
		int skinIndex = GameManager.Instance.currentSkinIndex;

		// Change the players skin to the selected material. --------- Try moving all this to the GameManager script.
		Renderer playerRenderer = playerPrefab.GetComponent<Renderer>();    // Get access to the Renderer Component on the player.
		playerRenderer.sharedMaterial = playerMatArray[skinIndex];      // Set the players skin to the material in the material array at the GameManagers currentSkinIndex number.

		lastBoost = Time.time - boostCooldown; // Enables the boost as soon as the game starts, otherwise the game would start with the boost cooldown in effect.
		startTime = Time.time;

		controller = GetComponent<Rigidbody>();
		controller.maxAngularVelocity = terminalRotationSpeed;
		controller.drag = drag;

		camTransform = Camera.main.transform;
	}

	private void Update()
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

		controller.AddForce(rotatedDir * moveSpeed);
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
			controller.AddForce(controller.velocity.normalized * boostSpeed, ForceMode.VelocityChange);
		}
	}
}
