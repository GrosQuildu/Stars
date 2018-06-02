﻿using UnityEngine;
using System.Collections;

public class JoinGameSceneInit : MonoBehaviour
{

    private GameApp gameApp;
    private LevelLoader levelLoader;

    void Start()
    {
        Debug.Log("JoinGameSceneInit");

        gameApp = GameObject.Find("GameApp").GetComponent<GameApp>();
        levelLoader = GameObject.Find("LevelLoader").GetComponent<LevelLoader>();
    }


    public void Back()
    {
        levelLoader.LoadLevel("mainMenuScene");
    }

    public void Join()
    {
        gameApp.PersistAllParameters("JoinGameScene");

        ClientNetworkManager clientNetworkManager= GameObject.Find("ClientNetworkManager").GetComponent<ClientNetworkManager>();
        clientNetworkManager.SetupClient();
    }

}
