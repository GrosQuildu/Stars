using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class ClientStartController : MonoBehaviour
{
    

    ClientNetworkManager clientNetworkManager;

    private void Start()
    {
        clientNetworkManager = GameObject.Find("ClientNetworkManager").GetComponent<ClientNetworkManager>();
    }

    
}