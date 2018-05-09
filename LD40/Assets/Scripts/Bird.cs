using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    private Camera birdCamera;
    private int relatedFinger;
    private Plane[] planes;
    private float speed;
    private static readonly float maxSpeed = 1000;
    private float health;
    private static readonly float decreaseStep = 0.05f;
    private OfflineGame game;
    private bool paused;
    private Animator beakAnimator;
    private Animator flapAnimator;
    private AudioSource audioSource;
    private const float differenceInPosition = 0.04F;
    private bool isAIPlayer = false;

    public bool IsAIPlayer
    {
        get
        {
            return isAIPlayer;
        }
        set
        {
            isAIPlayer = value;
        }
    }

    // Use this for initialization
    private void Start ()
    {
        health = 1;
        speed = 5;
	    relatedFinger = int.MinValue;
	    birdCamera = GetComponentInChildren<Camera>();
        planes = GeometryUtility.CalculateFrustumPlanes(birdCamera);
        if (!isAIPlayer && MainMenu.IsAIPlayerActive)
        {
            Rect r = new Rect(0.1f, 0, 1, 1);
            birdCamera.rect = r;
#if UNITY_ANDROID && !UNITY_EDITOR
            birdCamera.transform.Rotate(0, 0, 90);
#endif
            Vector3 newPosition = birdCamera.WorldToScreenPoint(birdCamera.transform.position);
            newPosition.x += (0.9f * Screen.width) / 4;
            birdCamera.transform.position = birdCamera.ScreenToWorldPoint(newPosition);
        }
        else if (MainMenu.IsAIPlayerActive)
        {
            birdCamera.enabled = false;
        }
        game = Camera.main.GetComponent<OfflineGame>();
        GameObject beak = transform.Find("BirdModel/Beak").gameObject;
        beakAnimator = beak.GetComponent<Animator>();
        //beakAnimator.enabled = false;
        beakAnimator.speed *= 2;
        audioSource = transform.GetComponentInChildren<AudioSource>();
        audioSource.pitch *= 0.2f;
        flapAnimator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (paused) return;
        float currentDecreaseStep = decreaseStep;
        if (health < 1)
        {
            currentDecreaseStep = decreaseStep * (1 / Mathf.Pow(health, 1f));
        }
        float diff = -Time.deltaTime * currentDecreaseStep;
        ChangeHealth(diff);
        if (health <= 0)
        {
            game.BirdDied(this);
        }
        transform.position += speed * Time.deltaTime * Vector3.right;
        if (name == "Player 1")
        {
            game.BirdSliders[0].value = transform.position.x;
        }
        else
        {
            game.BirdSliders[1].value = transform.position.x;
        }
        if (isAIPlayer)
        {
            UpdateAIPlayer();
        }
        else
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            UpdateAndroid();
#endif
#if UNITY_STANDALONE || UNITY_EDITOR || UNITY_WEBGL
            UpdateStandalone();
#endif
        }
	}

    private void UpdateAIPlayer()
    {
        if (transform.position.x >= game.AllInsects[0].transform.position.x)
        {
            game.AllInsects.RemoveAt(0);
        }

        Vector3 potentialPosition = transform.position;
        if (health < 0.8)
        {
            if (!(transform.position.y - differenceInPosition < game.AllInsects[0].transform.position.y && game.AllInsects[0].transform.position.y < transform.position.y + differenceInPosition))
            {
                if (transform.position.y < game.AllInsects[0].transform.position.y)
                {
                    potentialPosition += 5 * Time.deltaTime * Vector3.up;
                }
                if (transform.position.y > game.AllInsects[0].transform.position.y)
                {
                    potentialPosition += 5 * Time.deltaTime * Vector3.down;
                }
            }
        }
        else
        {
            if (transform.position.y - 2 * transform.lossyScale.y < game.AllInsects[0].transform.position.y && game.AllInsects[0].transform.position.y < transform.position.y + 2 * transform.lossyScale.y)
            {
                if (transform.position.y > game.AllInsects[0].transform.position.y)
                {
                    potentialPosition += 5 * Time.deltaTime * Vector3.up;
                }
                else
                {
                    potentialPosition += 5 * Time.deltaTime * Vector3.down;
                }
            }
        }

        Vector3 worldToViewportPoint = birdCamera.WorldToViewportPoint(potentialPosition);
        worldToViewportPoint.y = Mathf.Clamp01(worldToViewportPoint.y);
        transform.position = birdCamera.ViewportToWorldPoint(worldToViewportPoint);
    }

    private void UpdateAndroid()
    {
        foreach (Touch touch in Input.touches)
        {
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    if (birdCamera.pixelRect.Contains(touch.position))
                    {
                        relatedFinger = touch.fingerId;
                    }
                    break;
                case TouchPhase.Moved:
                    if (relatedFinger == touch.fingerId)
                    {
                        Vector3 touchPosition = birdCamera.ScreenToWorldPoint(touch.position);
                        Vector3 previousTouchPosition =
                            birdCamera.ScreenToWorldPoint(touch.position - touch.deltaPosition);
                        float worldDeltaY = touchPosition.y - previousTouchPosition.y;
                        Vector3 originalPosition = transform.position;
                        Vector3 worldPosition = new Vector3(originalPosition.x, originalPosition.y + worldDeltaY,
                            originalPosition.z);

                        Vector3 worldToViewportPoint = birdCamera.WorldToViewportPoint(worldPosition);
                        worldToViewportPoint.x = Mathf.Clamp01(worldToViewportPoint.x);
                        worldToViewportPoint.y = Mathf.Clamp01(worldToViewportPoint.y);
                        transform.position = birdCamera.ViewportToWorldPoint(worldToViewportPoint);
                    }
                    break;
                case TouchPhase.Ended:
                    if (relatedFinger == touch.fingerId)
                    {
                        relatedFinger = int.MinValue;
                    }
                    break;
            }
        }
    }

    private void UpdateStandalone()
    {
        KeyCode left;
        KeyCode right;
        if (name == "Player 1")
        {
            left = KeyCode.W;
            right = KeyCode.S;
        }
        else
        {
            left = KeyCode.UpArrow;
            right = KeyCode.DownArrow;
        }

        Vector3 potentialPosition = transform.position;
        if (Input.GetKey(left))
        {
            potentialPosition += 5 * Time.deltaTime * Vector3.up;
        }
        if (Input.GetKey(right))
        {
            potentialPosition += 5 * Time.deltaTime * Vector3.down;
        }

        Vector3 worldToViewportPoint = birdCamera.WorldToViewportPoint(potentialPosition);
        worldToViewportPoint.y = Mathf.Clamp01(worldToViewportPoint.y);
        transform.position = birdCamera.ViewportToWorldPoint(worldToViewportPoint);
    }

    public void ChangeHealth(float diff)
    {
        if (diff > 0)
        {
            beakAnimator.SetTrigger("play");
        }
        health += diff;
        speed -= diff * 2;
        audioSource.pitch -= diff * 0.5f;
        flapAnimator.speed -= diff * 1;
        flapAnimator.speed = Mathf.Max(0.1f, flapAnimator.speed);
        if (speed > maxSpeed)
        {
            speed = maxSpeed;
        }
        transform.GetChild(0).localScale = Vector3.one * health;
        transform.GetComponent<SphereCollider>().radius = 0.6f * health;
    }

    public void Pause()
    {
        paused = true;
    }
}
