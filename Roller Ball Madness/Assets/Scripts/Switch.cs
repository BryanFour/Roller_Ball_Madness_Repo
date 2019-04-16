using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
	public Transform target;

	private void OnCollisionEnter(Collision col)
	{
		if (target != null) // If the target exits
		{
			Destroy(target.gameObject);
		}
	}
}
