using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuAnimation : MonoBehaviour
{
	
	// Update is called once per frame
	void Update ()
	{
	    transform.position += 3 * Time.deltaTime * Vector3.right;
	    if (transform.position.x > 25)
	    {
	        transform.position = new Vector3(-10, 8 * (Random.value - 0.5f), 0);
            
	    }
	}
}
