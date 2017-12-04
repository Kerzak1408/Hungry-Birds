using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlineGame : MonoBehaviour
{
    private Networking networking;

    // Use this for initialization
	void Start ()
	{
	    networking = Networking.Instance;
        networking.SetupClient();
	}
}
