using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ServerStartController : MonoBehaviour
{

    public InputField player1;
    public InputField player2;
    public InputField player3;

    private ServerNetworkManager serverNetworkManager;

    void Start()
    {
        serverNetworkManager = GameObject.Find("ServerNetworkManager").GetComponent<ServerNetworkManager>();
    }

    

}
