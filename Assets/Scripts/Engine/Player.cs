using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Player : NetworkBehaviour
{
    public bool Human;
    private ArrayList spaceships;
    private ArrayList planets;
    public bool IsConnected;

    // Use this for initialization
    void Awake()
    {
        spaceships = new ArrayList();
        planets = new ArrayList();
        IsConnected = false;
        Human = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AssignConnection(NetworkConnection conn)
    {
        this.GetComponent<NetworkIdentity>().AssignClientAuthority(conn);
        foreach (Ownable ownable in GetOwned())
        {
            ownable.GetComponent<NetworkIdentity>().AssignClientAuthority(conn);
        }
    }

    public void RemoveConnection(NetworkConnection conn)
    {
        this.GetComponent<NetworkIdentity>().RemoveClientAuthority(conn);
        foreach (Ownable ownable in GetOwned())
        {
            ownable.GetComponent<NetworkIdentity>().RemoveClientAuthority(conn);
        }
    }

    public IEnumerable GetOwned()
    {
        foreach (Planet planet in planets)
        {
            yield return planet;
        }
        foreach (Spaceship spaceship in spaceships)
        {
            yield return spaceship;
        }
    }

    public IEnumerable GetSpaceships()
    {
        foreach (Spaceship spaceship in spaceships)
        {
            yield return spaceship;
        }
    }

    public IEnumerable GetPlanets()
    {
        foreach (Planet planet in planets)
        {
            yield return planet;
        }
    }

    public void Own(Ownable thing)
    {
        if (thing is Planet)
            planets.Add(thing);
        if (thing is Spaceship)
            spaceships.Add(thing);
    }

    public void Lose(Ownable thing)
    {
        if (thing is Planet)
            planets.Remove(thing);
        if (thing is Spaceship)
            spaceships.Remove(thing);
    }
}
