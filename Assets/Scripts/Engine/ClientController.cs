using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ClientController : MonoBehaviour
{

    private NetworkClient myClient;
    public InputField serverAddress;
    public InputField serverPort;
    public InputField playerName;

    // Use this for initialization
    void Start()
    {
        
    }

    // Create a client and connect to the server port
    public void SetupClient()
    {
        Debug.Log("Setup client with " + serverAddress.text + ":" + serverPort.text + " " + playerName.text);
        myClient = new NetworkClient();
        myClient.RegisterHandler(MsgType.Connect, OnConnected);
        myClient.Connect(serverAddress.text, int.Parse(serverPort.text));
    }

    // client function
    public void OnConnected(NetworkMessage netMsg)
    {
        Debug.Log("Connected to server");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
