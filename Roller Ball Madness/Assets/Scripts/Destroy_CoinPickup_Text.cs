using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy_CoinPickup_Text : MonoBehaviour
{
	void Start()
	{	// Destroy the text after .45 seconds, .45 seconds is a little less than the length of the animation witch is .5 seconds.
		Destroy(gameObject, 0.45f);
	}
}
