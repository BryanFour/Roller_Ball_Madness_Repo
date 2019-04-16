using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Video 1 - https://www.youtube.com/watch?v=N7_dQz-WGYw&list=PLLH3mUGkfFCWCsGUfwLMnDWdkpQuqW3xa&index=17

public class DestroyableObject : MonoBehaviour
{
	public float forceRequired = 15f;

	private void OnCollisionEnter(Collision col)
	{
		if (col.impulse.magnitude > forceRequired)
		{
			Destroy(gameObject); // Destroy the gameObject that this script is attached to.
		}
	}
}
