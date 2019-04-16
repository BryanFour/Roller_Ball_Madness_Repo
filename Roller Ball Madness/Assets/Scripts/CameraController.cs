using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Video 1 - https://www.youtube.com/watch?v=KPCV89buN4o&list=PLLH3mUGkfFCWCsGUfwLMnDWdkpQuqW3xa&index=9 
// Video 2 - https://www.youtube.com/watch?v=0ee_Mjzb3Jk&list=PLLH3mUGkfFCWCsGUfwLMnDWdkpQuqW3xa&index=11 --- Swipe Functions

	/// <summary>
	/// /////////////////////////////// This script is only used for the swipe camera
	/// </summary>

public class CameraController : MonoBehaviour
{
	public Transform lookAt;

	private Vector3 offset;
	private Vector3 desiredPosition;

	private Vector2 touchPosition;
	private float swipeResistance = 200f;

	private float smoothSpeed = 7.5f;
	private float distance =20f;
	private float yOffset = 6.5f;

	private void Start()
	{
		offset = new Vector3(0, yOffset, -1f * distance);
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			SlideCamera(true);
		}
		else if (Input.GetKeyDown(KeyCode.RightArrow))
		{
			SlideCamera(false);
		}
		// Mobile touch input stuff
		if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
		{
			touchPosition = Input.mousePosition;
		}

		if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
		{
			float swipeForce = touchPosition.x - Input.mousePosition.x;
			if (Mathf.Abs(swipeForce) > swipeResistance)
			{
				if (swipeForce < 0) // If we swipe left
				{
					SlideCamera(true);
				}
				else // Slide camera right
				{
					SlideCamera(false);
				}
			}
		}
		// Mobile touch input stuff end.
	}

	private void FixedUpdate()
	{
		desiredPosition = lookAt.position + offset;
		transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
		transform.LookAt(lookAt.position + Vector3.up);
	}

	public void SlideCamera(bool left)
	{
		if (left)
		{
			offset = Quaternion.Euler(0, 90, 0) * offset;
		}
		else
		{
			offset = Quaternion.Euler(0, -90, 0) * offset;
		}
	}
}
