using UnityEngine;
using System;

public class CookStew : State<Elsa> {

	static readonly CookStew instance = new CookStew();

	public static CookStew Instance
	{
		get
		{
			return instance;
		}
	}

	public override void Enter(Elsa agent)
	{
		//if not already cooking put the stew in the oven
		if (!agent.Cooking)
		{
			Debug.Log(agent.ID + ": Putting the stew in the oven");

			//send a delayed message myself so that I know when to take the stew
			//out of the oven
			MessageDispatcher.DispatchMessage(1.5, //time delay
				"Elsa",
				agent.ID,           //sender ID
				MessageTypes.StewReady);         //msg


			agent.Cooking = true;
		}
	}


	public override void Execute(Elsa agent)
	{
		Debug.Log(agent.ID + ": Fussin' over food");
	}

	public override void Exit(Elsa agent)
	{
		Debug.Log(agent.ID + ": Puttin' the stew on the table");
	}

	public override bool OnMessage(Elsa agent, Telegram msg)
	{
		switch(msg.Msg)
		{
		case MessageTypes.StewReady:
			{
				Debug.Log(agent.ID + ": Message received at " + DateTime.Now.ToShortTimeString());

				Debug.Log(agent.ID + ": StewReady! Lets eat");

				//let hubby know the stew is ready
				MessageDispatcher.DispatchMessage(0, //time delay
					"Bob",
					agent.ID,           //sender ID
					MessageTypes.StewReady);         //msg

				agent.Cooking = false;

				agent.stateMachine.ChangeState (DoHouseWork.Instance);               
			}

			return true;

		}//end switch

		return false;
	}
}
