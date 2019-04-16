using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Video 1 - https://www.youtube.com/watch?v=A4Hyh4UdslQ&list=PLLH3mUGkfFCWCsGUfwLMnDWdkpQuqW3xa&index=12 --- Comments have alternate solutions to try
// Video 2 - https://www.youtube.com/watch?v=Ta7v27yySKs - Restrict cameras up/down to 90 degrees

public class FreeCamera : MonoBehaviour
{
	public VirtualJoystick cameraJoystick;

	private const float Y_ANGLE_MIN = 0f;
	private const float Y_ANGLE_MAX = 50f;

	public Transform lookAt; // what the camera looks at
	public Transform camTransform; 

	private Camera cam;

	private float distance = 20f; // distance between player and camera
	private float currentX = 0f;
	private float currentY = 0f;
	private float sensitivityX = 3f;
	private float sensitivityY = 1f;

	private void Start()
	{
		camTransform = transform;
		cam = Camera.main;
	}

	private void Update()
	{
		currentX += cameraJoystick.InputDirection.x * sensitivityX;
		currentY += cameraJoystick.InputDirection.z * sensitivityY;

		currentY = Mathf.Clamp(currentY, Y_ANGLE_MIN, Y_ANGLE_MAX);
	}

	private void LateUpdate()
	{
		Vector3 dir = new Vector3(0, 0, -distance);
		Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
		camTransform.position = lookAt.position + rotation * dir;
		camTransform.LookAt(lookAt.position);
		//transform.position = lookAt.position + rotation * dir; // Commented out due to changes in Video 2
		//transform.LookAt(lookAt); // Commented out due to changes in Video 2
	}

}
