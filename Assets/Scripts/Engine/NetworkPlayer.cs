using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class NetworkPlayer : NetworkBehaviour
{


    // Use this for initialization
    void Start()
    {
        Debug.Log("NetworkPlayer start");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void OnStartLocalPlayer()
    {
        Debug.Log("OnStartLocalPlayer");
        
    }
}
