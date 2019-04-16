using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Video 1 - https://www.youtube.com/watch?v=KPCV89buN4o&list=PLLH3mUGkfFCWCsGUfwLMnDWdkpQuqW3xa&index=9

public class PlayerController : MonoBehaviour
{
	public float moveSpeed = 5.0f;
	public float drag = .5f;
	public float terminalRotationSpeed = 25f;

	private Rigidbody controller;

	private void Start()
	{
		controller = GetComponent<Rigidbody>();
		controller.maxAngularVelocity = terminalRotationSpeed;
		controller.drag = drag;
	}

	private void Update()
	{
		Vector3 dir = Vector3.zero;

		dir.x = Input.GetAxis("Horizontal");
		dir.z = Input.GetAxis("Vertical");

		if(dir.magnitude > 1)
		{
			dir.Normalize();
		}

		controller.AddForce(dir * moveSpeed);
	}
}
