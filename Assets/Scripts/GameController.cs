using UnityEngine;
using System.Collections;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using Assets.Scripts;
using System.Collections.Generic;
using UnityEngine.SceneManagement;


public class GameController : MonoBehaviour
{
    private static List<GameObject> players;
    public GameObject PlayerPrefab;
    private static int currentPlayerIndex;

    public GameObject PlanetPrefab;
    public GameObject StartPrefab;
    public GameObject ScoutPrefab;
    public GameObject ColonizerPrefab;
    public GameObject MinerPrefab;

    private static int year;

    private HexGrid grid;
    TurnScreen turnScreen;
    

    void Start()
    {
       
    }

    public void InitGame()
    {
        Debug.Log("ServerGameManager init game");


        grid = GameObject.Find("HexGrid").GetComponent<HexGrid>();
        turnScreen = GameObject.Find("Canvas").GetComponentInChildren<TurnScreen>();

        turnScreen.gameObject.SetActive(false);

        InitPlayers();
        InitMap();
        InitSpaceships();

        StartGame();
    }

    public void InitClient()
    {
        Debug.Log("init client");
    }

    public void WaitForTurn()
    {
        Debug.Log("WaitForTurn");
        SceneManager.LoadScene("WaitScene");
    }

    void InitPlayers()
    {
        // Create players from prefab.
        players = new List<GameObject>();
        players.Add(Instantiate(PlayerPrefab));
        players[0].GetComponent<Player>().Human = true;
        players[0].name = "Player";

        for (int i = 1; i < 1; i++)
        {
            players.Add(Instantiate(PlayerPrefab));
            players[i].GetComponent<Player>().Human = false;
            players[i].name = "AI-" + i;
        }

        currentPlayerIndex = 0;
    }

    void InitSpaceships()
    {
        GameObject spaceship;
        foreach (GameObject player in players)
        {
            Planet homePlanet = player.GetComponent<Player>().GetPlanets().Cast<Planet>().First();

            // 1x scout
            for (int i = 0; i < 1; i++)
            {
                spaceship = homePlanet.BuildSpaceship(ScoutPrefab);
                if (spaceship != null)
                {

                    spaceship.GetComponent<Spaceship>().Owned(player.GetComponent<Player>());
                }
            }

            // 0x miner
            for (int i = 0; i < 0; i++)
            {
                spaceship = homePlanet.BuildSpaceship(MinerPrefab);
                if (spaceship != null)
                {

                    spaceship.GetComponent<Spaceship>().Owned(player.GetComponent<Player>());
                }
            }

            // 0x colonizer
            for (int i = 0; i < 0; i++)
            {
                spaceship = homePlanet.BuildSpaceship(ColonizerPrefab);
                if (spaceship != null)
                {

                    spaceship.GetComponent<Spaceship>().Owned(player.GetComponent<Player>());
                }
            }
        }
    }

    void InitMap()
    {
        // Create map from file / random.
        // todo: in main menu we should decide if map is from file or random and set parameters
        // todo: move json deserialization to Planet's FromJson method
        // serializacje w unity ssie, trzeba bedzie doprawcowac (potrzebne bedzie do save/load i pewnie networkingu...)
        // todo: w jsonach nie moze byc utf8

        JObject o = JObject.Parse(Resources.Load("map1").ToString());
        InitPlanets((JArray)o["planets"]);
        InitStars((JArray)o["stars"]);
    }

    void InitPlanets(JArray jPlanetsCollection)
    {
        int playersWithHomePLanet = 0;

        foreach (JObject jPlanetSerialized in jPlanetsCollection)
        {
            GameObject planet = Instantiate(original: PlanetPrefab, position: new Vector3(
                (float)jPlanetSerialized["position"][0], (float)jPlanetSerialized["position"][1], (float)jPlanetSerialized["position"][2]), rotation: Quaternion.identity
            );
            JsonUtility.FromJsonOverwrite(jPlanetSerialized["planetMain"].ToString(), planet.GetComponent<Planet>());
            planet.name = jPlanetSerialized["name"].ToString();

            float radius = (float)jPlanetSerialized["radius"];
            //planet.GetComponent<SphereCollider>().radius = radius;
            planet.transform.localScale = new Vector3(radius, radius, radius);

            string materialString = (string)jPlanetSerialized["material"];
            if (materialString != null)
            {
                Material newMaterial = Resources.Load(materialString, typeof(Material)) as Material;
                if (materialString != null)
                    planet.GetComponentsInChildren<MeshRenderer>()[0].material = newMaterial;
            }

            if ((bool)jPlanetSerialized["mayBeHome"] == true && playersWithHomePLanet < players.Count())
            {
                planet.GetComponent<Planet>().Colonize(players[playersWithHomePLanet].GetComponent<Player>());
                playersWithHomePLanet++;
            }
        }

        if (playersWithHomePLanet < players.Count())
        {
            throw new Exception("Not enough planets for players");
        }
    }

    void InitStars(JArray jStarsCollection)
    {
        foreach (JObject jStarSerialized in jStarsCollection)
        {
            GameObject star = Instantiate(original: StartPrefab, position: new Vector3(
                (float)jStarSerialized["position"][0], (float)jStarSerialized["position"][1], (float)jStarSerialized["position"][2]), rotation: Quaternion.identity
            );
            star.name = jStarSerialized["name"].ToString();

            float radius = (float)jStarSerialized["radius"];
            star.GetComponent<SphereCollider>().radius = radius;
            star.transform.localScale = new Vector3(radius, radius, radius);

            string materialString = (string)jStarSerialized["material"];
            if (materialString != null)
            {
                Material newMaterial = Resources.Load(materialString, typeof(Material)) as Material;
                if (materialString != null)
                    star.GetComponentsInChildren<MeshRenderer>()[0].material = newMaterial;
            }
        }
    }

    void StartGame()
    {
        Debug.Log("Starting game");
        currentPlayerIndex = players.Count() - 1; // NextTurn will wrap index to zero at the beginning
        year = -1;  // NextTurn will increment Year at the beginning
        NextTurn();
    }

    public void NextTurn()
    {
        turnScreen.gameObject.SetActive(true);
        turnScreen.Play();
        currentPlayerIndex = (currentPlayerIndex + 1) % players.Count();
        if (currentPlayerIndex == 0)
        {
            year++;
            Debug.Log("New year: " + year);
        }

        foreach (Ownable owned in GetCurrentPlayer().GetOwned())
        {
            owned.SetupNewTurn();
        }

        EventManager.selectionManager.SelectedObject = null;
        grid.SetupNewTurn(GetCurrentPlayer());
        GameObject.Find("MiniMap").GetComponent<MiniMapController>().SetupNewTurn(GetCurrentPlayer());

        Debug.Log("Next turn, player: " + GetCurrentPlayer().name);
    }

    public static Player GetCurrentPlayer()
    {
        return players[currentPlayerIndex].GetComponent<Player>();
    }

    public static int GetYear()
    {
        return year;
    }

    public void Colonize()
    {
        var colonizer = EventManager.selectionManager.SelectedObject.GetComponent<Colonizer>();
        if (colonizer != null)
        {
            if (colonizer.ColonizePlanet())
            {
                grid.FromCoordinates(colonizer.Coordinates).ClearObject();
                GetCurrentPlayer().Lose(colonizer);

                Destroy(colonizer.gameObject);

            }
        }

    }
    public void BuildSpaceship(GameObject spaceshipPrefab)
    {
        var planet = EventManager.selectionManager.SelectedObject.GetComponent<Planet>();
        if (planet != null)
        {
            if (planet.IsPossibleBuildSpaceship())
            {
                GameObject spaceship = planet.BuildSpaceship(spaceshipPrefab);
                spaceship.GetComponent<Spaceship>().Owned(GetCurrentPlayer());

                EventManager.selectionManager.SelectedObject = null;
                grid.SetupNewTurn(GetCurrentPlayer());
                GameObject.Find("MiniMap").GetComponent<MiniMapController>().SetupNewTurn(GetCurrentPlayer());
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

}
