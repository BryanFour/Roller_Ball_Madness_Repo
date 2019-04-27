using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
	public GameObject pickupText;	// The +1 text prefab.
	public float offsetY = 1;	// The instanciate positions Y offset.
	public float offsetX = 1;   // The instanciate positions X offset.

	private void OnTriggerEnter(Collider col)
	{	//	If the coin is hit by the player.
		if (col.gameObject.tag == "Player")
		{	// Add 1 to the players currency -- The currency is saved when we run the Gamemanagers Victory() method. (We we comple the level.)
			GameManager.Instance.currency += 1;
			//	The position we want to instanciate the coin. -- The coins position plus the offsets
			Vector3 instanciatePos = new Vector3(transform.position.x + offsetX, transform.position.y + offsetY, transform.position.z);
			//	Instanciate the +1 at the instanciate position we created above.
			Instantiate(pickupText, instanciatePos, Quaternion.identity);
			//	Destroy the +1.
			Destroy(gameObject);
		}
	}
}
