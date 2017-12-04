using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Insect : MonoBehaviour
{
    private static readonly float bonusHp = 0.2f;
    private Camera left;
    private Camera right;
    private float clipTimeout;

    private void OnTriggerEnter(Collider other)
    {
        GameObject otherGameObject = other.gameObject;
        Bird bird = otherGameObject.GetComponent<Bird>();
        if (bird != null)
        {
            AudioSource.PlayClipAtPoint(Resources.Load<AudioClip>("Music/Crunch"), Camera.main.transform.position);
            bird.ChangeHealth(bonusHp);
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        left = GameObject.Find("CameraLeft").GetComponent<Camera>();
        right = GameObject.Find("CameraRight").GetComponent<Camera>();
    }

    private void Update()
    {
        transform.position -= Time.deltaTime * Vector3.left;
        if (clipTimeout <= 0 && (IsVisibleBy(left) || IsVisibleBy(right)))
        {
            //AudioSource.PlayClipAtPoint(Resources.Load<AudioClip>("Music/Fly"), Camera.main.transform.position);
            clipTimeout = 5;
        }
        clipTimeout -= Time.deltaTime;
    }

    private bool IsVisibleBy(Camera camera)
    {
        Vector3 worldToScreenPoint = camera.WorldToScreenPoint(transform.position);
        if (camera.pixelRect.Contains(worldToScreenPoint))
            return true;
        return false;
    }
}
