using System;
using UnityEngine;

internal class GoHomeAndSleepTilRested : State<Bob>
{
    static readonly GoHomeAndSleepTilRested instance = new GoHomeAndSleepTilRested();

    public static GoHomeAndSleepTilRested Instance
    {
        get
        {
            return instance;
        }
    }

    public override void Enter(Bob agent)
    {
        if (agent.location != Locations.Shack)
        {
            Debug.Log(agent.ID + ": Walkin' home");

            agent.ChangeLocation(Locations.Shack);
        }
    }

    public override void Execute(Bob agent)
    {
        //if miner is not fatigued start to dig for nuggets again.
        if (!agent.Fatigued())
        {
            Debug.Log(agent.ID + ": What a God darn fantastic nap! Time to find more gold");

            agent.ChangeState(EnterMineAndDigForNugget.Instance);
        }

        else
        {
            //sleep
            agent.DecreaseFatigue();

            Debug.Log(agent.ID + ": ZZZZ... ");
        }
    }

    public override void Exit(Bob agent)
    {
        Debug.Log(agent.ID + ": Leaving the house");
    }
}