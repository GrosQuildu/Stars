using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public abstract class Ownable : NetworkBehaviour
{
    protected Player owner;
    public float RadarRange;

    void Start()
    {
    
    }

    void Update()
    {

    }

    public void Owned(Player newOwner)
    {
        if (this.owner != null)
            this.owner.Lose(this);
        newOwner.Own(this);
        this.owner = newOwner;
    }

    public string GetOwnerName()
    {
        if (this.owner == null)
            return "";
        return this.owner.name;
    }

    public Player GetOwner()
    {
        return this.owner;
    }

    abstract public void SetupNewTurn();
}
