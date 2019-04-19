using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	private const float TIME_BEFORE_START = 3.0f;

	public float moveSpeed = 5.0f;
	public float drag = .5f;
	public float terminalRotationSpeed = 25f;

	private Rigidbody controller;
	private Transform camTransform;

	private void Start()
	{
		controller = GetComponent<Rigidbody>();
		controller.maxAngularVelocity = terminalRotationSpeed;
		controller.drag = drag;

		camTransform = Camera.main.transform;
	}

	private void Update()
	{
		Vector3 dir = Vector3.zero;

		dir.x = Input.GetAxis("Horizontal");
		dir.z = Input.GetAxis("Vertical");

		if (dir.magnitude > 1)
		{
			dir.Normalize();
		}

		//Rotate our direction with the camera
		Vector3 rotatedDir = camTransform.TransformDirection(dir);
		rotatedDir = new Vector3(rotatedDir.x, 0, rotatedDir.z);
		rotatedDir = rotatedDir.normalized * dir.magnitude;

		controller.AddForce(rotatedDir * moveSpeed);
	}
}
