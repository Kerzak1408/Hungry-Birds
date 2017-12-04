using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleAnimation : MonoBehaviour
{
    private bool growing;
    private float scaleFactor = 1.0f;
    private float maxX;
    private float minX;
    private Vector3 originalScale;

    // Use this for initialization
	void Start ()
	{
	    originalScale = transform.localScale;
	    growing = true;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (growing)
        {
            scaleFactor += Time.deltaTime/3;
            if (scaleFactor > 1.05f)
            {
                growing = false;
            }
        }
        else
        {
            scaleFactor -= Time.deltaTime/3;
            if (scaleFactor < 0.85f)
            {
                growing = true;
            }
        }
        transform.localScale = originalScale * scaleFactor;
    }
}
