using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Video 1 - https://www.youtube.com/watch?v=A4Hyh4UdslQ&list=PLLH3mUGkfFCWCsGUfwLMnDWdkpQuqW3xa&index=12 --- Comments have alternate solutions to try
// Video 2 - https://www.youtube.com/watch?v=Ta7v27yySKs - Restrict cameras up/down to 90 degrees
// Video 3 - https://www.youtube.com/watch?v=KcKo8QHOjlk&list=PLLH3mUGkfFCWCsGUfwLMnDWdkpQuqW3xa&index=30 -- Buffer time(Time till game starts after selecting a level)
// UnityDocs 1 - https://docs.unity3d.com/Manual/Layers.html -- Layer mask and bitshift info.
// UnityDocs 2 - https://docs.unity3d.com/ScriptReference/Physics.Raycast.html -- Im using Physics.Raycast but the Physics.Linecast example found in the Layers docs helped alot. 

public class FreeCamera : MonoBehaviour
{
	public VirtualJoystick cameraJoystick;

	private const float Y_ANGLE_MIN = 8f;
	private const float Y_ANGLE_MAX = 20f;
	// Cameras left and right min/max values.
	private const float X_ANGLE_MIN = -15f;
	private const float X_ANGLE_MAX = 15f;

	public Transform thePlayer; // what the camera looks at
	public Transform camTransform; 

	private Camera cam;

	[Range(0, 40)][Header("Distance Value")][Tooltip("The distance between the player and the camera.")]
	[SerializeField] private float idealCamOffset = 20f; // distance between player and camera
	private float camOffset;
	private float camOffsetOffset = -1;


	[Range(0, 5)][Header("Camera Sensitivity")][Tooltip("The camrea joysticks sensitivity.")]
	[SerializeField] private float sensitivityX = 3f;
	[Range(0, 5)]
	[SerializeField] private float sensitivityY = 1f;

	private float currentX = 0f;    // The X angle the camera starts at.
	private float currentY = 14f;	// The Y angle the camera starts at.

	private float startTime = 0f;
	private const float TIME_BEFORE_START = 2.5f;

	// Raycast
	public GameObject castFrom;
	public GameObject castTo;


	private void Start()
	{
		camTransform = transform;
		cam = Camera.main;
	}

	private void Update()
	{
		//	If we havent started playing yet(If the game timer hasnt started yet)
		if (Time.time - startTime < TIME_BEFORE_START)
		{
			return;	// Dont allow the player to move the camera.
		}

		currentX += cameraJoystick.InputDirection.x * sensitivityX;
		currentY += cameraJoystick.InputDirection.z * sensitivityY;

		//	Clamp the cameras Y-axis value to a min and max, so the player cant move the camera too high or low.
		currentY = Mathf.Clamp(currentY, Y_ANGLE_MIN, Y_ANGLE_MAX);
		// Clamp the cameras X-axis value to a min and max, so the player cant move the camera too far left and right.
		currentX = Mathf.Clamp(currentX, X_ANGLE_MIN, X_ANGLE_MAX);
	}

	private void LateUpdate()
	{
		//	If we havent started playing yet(If the game timer hasnt started yet)
		if (Time.time - startTime < TIME_BEFORE_START)
		{
			return;	// Dont allow the player to move the camera.
		}

		// Raycast Stuff.
		// Bit shift the index of the layer (9) to get a bit mask
		// This will cast rays only against colliders in layer 9. ------ Dont forget to set NonClippable objects to the NonClippable layer.
		int layerMask = 1 << 9;
		RaycastHit hit;
		// If the ray intersects any objects in layer 9...
		if (Physics.Raycast(castFrom.transform.position, castTo.transform.position, out hit, idealCamOffset, layerMask))
		{
			float clippedCamOffset = hit.distance + camOffsetOffset; // clippedCamOffset is set to the distance between the rays orign point and the rays hit point plus the offsets offset. 
			camOffset = clippedCamOffset;	// The cameras offset is set to the clippedCamOffset.
			//Debug.Log("Did Hit");
			//Debug.Log(hit.distance);
		}
		//	If the ray dosn't intersect any valid objects...
		else
		{
			camOffset = idealCamOffset;	// The cameras offset is set to the ideal offset.
			//Debug.Log("Did not Hit");
		}

		Vector3 dir = new Vector3(0, 0, -camOffset);						//
		Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);		//
		camTransform.position = thePlayer.position + rotation * dir;		//	
		camTransform.LookAt(thePlayer.position);							//	Make the camera look at the player.
	}

}
