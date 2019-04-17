using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyMe : MonoBehaviour
{
	public float duration = 1.25f;

	private float statTime;

    void Start()
    {
		statTime = Time.time;    
    }

    void Update()
    {
		if(Time.time - statTime > duration)
		{
			Destroy(this.gameObject);
		}        
    }
}
