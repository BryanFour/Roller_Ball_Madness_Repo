using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Video 1 - https://www.youtube.com/watch?v=A4Hyh4UdslQ&list=PLLH3mUGkfFCWCsGUfwLMnDWdkpQuqW3xa&index=12 --- Comments have alternate solutions to try
// Video 2 - https://www.youtube.com/watch?v=Ta7v27yySKs - Restrict cameras up/down to 90 degrees
// Video 3 - https://www.youtube.com/watch?v=KcKo8QHOjlk&list=PLLH3mUGkfFCWCsGUfwLMnDWdkpQuqW3xa&index=30 -- Buffer time(Time till game starts after selecting a level)

public class FreeCamera : MonoBehaviour
{
	public VirtualJoystick cameraJoystick;

	private const float Y_ANGLE_MIN = 8f;
	private const float Y_ANGLE_MAX = 25f;

	public Transform lookAt; // what the camera looks at
	public Transform camTransform; 

	private Camera cam;

	[Range(0, 40)][Header("Distance Value")][Tooltip("The distance between the player and the camera.")]
	[SerializeField] private float distance = 20f; // distance between player and camera

	[Range(0, 5)][Header("Camera Sensitivity")][Tooltip("The camrea joysticks sensitivity.")]
	[SerializeField] private float sensitivityX = 3f;
	[Range(0, 5)]
	[SerializeField] private float sensitivityY = 1f;

	private float currentX = 0f;
	private float currentY = 0f;

	private float startTime = 0f;
	private const float TIME_BEFORE_START = 2.5f;

	private void Start()
	{
		camTransform = transform;
		cam = Camera.main;
	}

	private void Update()
	{
		if (Time.time - startTime < TIME_BEFORE_START)
		{
			return;
		}

		currentX += cameraJoystick.InputDirection.x * sensitivityX;
		currentY += cameraJoystick.InputDirection.z * sensitivityY;

		currentY = Mathf.Clamp(currentY, Y_ANGLE_MIN, Y_ANGLE_MAX);
	}

	private void LateUpdate()
	{
		if (Time.time - startTime < TIME_BEFORE_START)
		{
			return;
		}

		Vector3 dir = new Vector3(0, 0, -distance);
		Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
		camTransform.position = lookAt.position + rotation * dir;
		camTransform.LookAt(lookAt.position);
		//transform.position = lookAt.position + rotation * dir; // Commented out due to changes in Video 2
		//transform.LookAt(lookAt); // Commented out due to changes in Video 2
	}

}
