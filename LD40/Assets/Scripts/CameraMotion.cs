using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMotion : MonoBehaviour
{
    private float defaultYPosition;

	// Use this for initialization
	void Start ()
	{
	    defaultYPosition = transform.position.y;
	}
	
	// Update is called once per frame
	void Update ()
	{
	    Vector3 position = transform.position;
	    position.y = defaultYPosition;
	    transform.position = position;
	}
}
