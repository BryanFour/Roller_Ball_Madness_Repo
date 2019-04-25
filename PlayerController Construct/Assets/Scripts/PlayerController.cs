using System.Collections;
using System.Collections.Generic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

	public float speed = 10.0f;
	public float rotation = 360f;
	public float gravity;
	public float jump = 7f;

	private CharacterController player;
	private float groundDistance;

	float dr = 0f;

	// Use this for initialization
	void Start()
	{
		player = GetComponent<CharacterController>();
		groundDistance = player.bounds.extents.y;
	}

	// Update is called once per frame
	void Update()
	{

		// get the movement
		float moveFB = Input.GetAxis("Vertical") * speed;
		float moveLR = Input.GetAxis("Horizontal") * speed;


		if (moveLR != 0) dr = 90 * moveLR;
		if (moveFB < 0) dr = 180;
		if (moveFB > 0) dr = 0;

		transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, dr, 0), rotation * Time.deltaTime);

		// not normalized, yet.
		Vector3 movement = new Vector3(moveLR, gravity, moveFB) * Time.deltaTime * 2f;
		player.Move(movement);

		// handle jumping
		if (isGrounded())
		{
			if (Input.GetButtonDown("Jump"))
			{
				gravity += jump;
			}
		}

		// apply gravity effect
		if (!isGrounded())
		{
			gravity += (Physics.gravity.y * 3) * Time.deltaTime;
		}
		else
		{
			gravity = 0f;
		}

	}

	bool isGrounded()
	{
		return Physics.Raycast(transform.position, -Vector3.up, groundDistance + 0.1f);
	}
}