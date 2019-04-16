using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinScript : MonoBehaviour
{
	private void OnTriggerEnter(Collider col)
	{
		if(col.tag == "Player")
		{
			LevelManager.Instance.Victory();
		}
	}
}
