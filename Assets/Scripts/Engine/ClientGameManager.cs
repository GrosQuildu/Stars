using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ClientGameManager : MonoBehaviour
{

    public GameObject PlanetPrefab;
    public GameObject StartPrefab;
    public GameObject ScoutPrefab;
    public GameObject ColonizerPrefab;
    public GameObject MinerPrefab;

    private static int year;

    private HexGrid grid;
    TurnScreen turnScreen;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        Debug.Log("GameController init game");


        grid = GameObject.Find("HexGrid").GetComponent<HexGrid>();
        turnScreen = GameObject.Find("Canvas").GetComponentInChildren<TurnScreen>();

        turnScreen.gameObject.SetActive(false);
    }

    void Update()
    {

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
}
