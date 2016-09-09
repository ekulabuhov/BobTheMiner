using System;
using UnityEngine;

public class EatStew : State<Bob>
{
	static readonly EatStew instance = new EatStew();

	public static EatStew Instance
	{
		get
		{
			return instance;
		}
	}

	public override void Enter(Bob agent)
	{
		Debug.Log(agent.ID + ": Smells Reaaal goood Elsa!");
	}

	public override void Execute(Bob agent)
	{
		Debug.Log(agent.ID + ": Tastes real good too!");

		agent.stateMachine.RevertToPreviousState ();
	}

	public override void Exit(Bob agent)
	{ 
		Debug.Log(agent.ID + ": Thankya li'lle lady. Ah better get back to whatever ah wuz doin'");
	}
}