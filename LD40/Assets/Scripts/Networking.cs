using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.Networking;

public class Networking : MonoBehaviour
{
    private static Networking instance;
    private TcpClient client;
    private StreamWriter writer;

    public static Networking Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new Networking();
            }
            return instance;
        }
    }

    private Networking()
    {
        
    }
	// Use this for initialization
	void Start ()
    {
		SetupClient();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // Create a client and connect to the server port
    public void SetupClient()
    {
        client = new TcpClient ("127.0.0.1", 4444);
        writer = new StreamWriter(client.GetStream());
    }

    // client function
    public void OnConnected(NetworkMessage netMsg)
    {
        Debug.Log("Connected to server");
    }
}
