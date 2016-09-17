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

			agent.ChangeLocation(Locations.Shack, () => {
				//let the wife know I'm home
				MessageDispatcher.DispatchMessage(0, //time delay
					"Elsa",
					agent.ID,        //ID of sender
					MessageTypes.HiHoneyImHome);   //the message
			});
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

	public override bool OnMessage(Bob agent, Telegram msg)
	{
		switch(msg.Msg)
		{
		case MessageTypes.StewReady:
			
			Debug.Log (agent.ID + ": Message handled at " + DateTime.Now.ToShortTimeString ());

			Debug.Log (agent.ID + ": Okay Hun, ahm a comin'!");

			agent.stateMachine.ChangeState (EatStew.Instance);

			return true;

		}//end switch

		return false; //send message to global message handler
	}
}