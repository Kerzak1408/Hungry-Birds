using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    private OfflineGame Game;

    public void Start()
    {
        Game = Camera.main.GetComponent<OfflineGame>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Bird>() != null)
		    Game.GoalReached(other.name);
	}
}
