using UnityEngine;
using System.Collections;
using System;

public class WifesGlobalState : State<Elsa> {

	static readonly WifesGlobalState instance = new WifesGlobalState();

	public static WifesGlobalState Instance
	{
		get
		{
			return instance;
		}
	}

	public override void Enter(Elsa agent)
	{

	}

	public override void Execute(Elsa agent)
	{
		//1 in 10 chance of needing the bathroom
		if (UnityEngine.Random.value < 0.1)
		{
			agent.stateMachine.ChangeState (VisitBathroom.Instance);
		}
	}

	public override bool OnMessage(Elsa agent, Telegram msg)
	{
		switch(msg.Msg)
		{
		case MessageTypes.HiHoneyImHome:
			{
				Debug.Log(agent.ID + ": Message handled at " + DateTime.Now.ToShortTimeString());

				Debug.Log(agent.ID + ": Hi honey. Let me make you some of mah fine country stew");

				agent.stateMachine.ChangeState (CookStew.Instance);
			}

			return true;

		}//end switch

		return false;
	}

	public override void Exit(Elsa agent)
	{
		Debug.Log(agent.ID + ": Leaving the house");
	}
}
