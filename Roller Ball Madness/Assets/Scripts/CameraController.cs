using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Video 1 - https://www.youtube.com/watch?v=KPCV89buN4o&list=PLLH3mUGkfFCWCsGUfwLMnDWdkpQuqW3xa&index=9 ------ Continue from 13 mins in

public class CameraController : MonoBehaviour
{
	public Transform lookAt;

	private Vector3 offset;

	private float distance = 15f;
	private float yOffset = 3.5f;

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

		transform.position = lookAt.position + offset;
		transform.LookAt(lookAt);
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
