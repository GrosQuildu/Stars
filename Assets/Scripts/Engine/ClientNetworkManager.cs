using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ClientNetworkManager : NetworkManager
{
    public InputField serverAddressInputField;
    public InputField serverPortInputField;
    public InputField playerInputField;

    public string playerName;

    public void SetupClient()
    {
        Debug.Log("Setup client with " + serverAddressInputField.text + ":" + serverPortInputField.text + " " + playerInputField.text);
        this.networkAddress = serverAddressInputField.text;
        this.networkPort = int.Parse(serverPortInputField.text);
        this.playerName = playerInputField.text;
        this.StartClient();
    }

    // Client callbacks

    public override void OnClientConnect(NetworkConnection conn)

    {
        base.OnClientConnect(conn);
        Debug.Log("Connected successfully to server");

    }

    public override void OnClientDisconnect(NetworkConnection conn)
    {

        StopClient();

        if (conn.lastError != NetworkError.Ok)

        {

            if (LogFilter.logError) { Debug.LogError("ClientDisconnected due to error: " + conn.lastError); }

        }

        Debug.Log("Client disconnected from server: " + conn);

    }

    public override void OnClientError(NetworkConnection conn, int errorCode)
    {

        Debug.Log("Client network error occurred: " + (NetworkError)errorCode);

    }

    public override void OnClientNotReady(NetworkConnection conn)
    {

        Debug.Log("Server has set client to be not-ready (stop getting state updates)");

    }

    public override void OnStartClient(NetworkClient client)
    {

        Debug.Log("Client has started");

    }

    public override void OnStopClient()
    {

        Debug.Log("Client has stopped");

    }

    public override void OnClientSceneChanged(NetworkConnection conn)
    {

        base.OnClientSceneChanged(conn);

        Debug.Log("Server triggered scene change and we've done the same, do any extra work here for the client...");

    }

}
