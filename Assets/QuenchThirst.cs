using System;
using UnityEngine;

public class QuenchThirst : State<Bob>
{
    static readonly QuenchThirst instance = new QuenchThirst();

    public static QuenchThirst Instance
    {
        get
        {
            return instance;
        }
    }

    public override void Enter(Bob agent)
    {
        if (agent.location != Locations.Saloon)
        {
            agent.ChangeLocation(Locations.Saloon);

            Debug.Log(agent.ID + ": Boy, ah sure is thusty! Walking to the saloon");
        }
    }

    public override void Execute(Bob agent)
    {
        if (agent.Thirsty())
        {
            agent.BuyAndDrinkAWhiskey();

            Debug.Log(agent.ID + ": That's mighty fine sippin liquer");

            agent.ChangeState(EnterMineAndDigForNugget.Instance);
        }

        else
        {
            Debug.Log("\nERROR!\nERROR!\nERROR!");
        }
    }

    public override void Exit(Bob agent)
    {
        Debug.Log(agent.ID + ": Leaving the saloon, feelin' good");
    }
}