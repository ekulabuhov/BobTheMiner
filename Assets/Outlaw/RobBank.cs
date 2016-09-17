using UnityEngine;
using System.Collections;
using System;

public class RobBank : State<Outlaw> {

	static readonly RobBank instance = new RobBank();

	public static RobBank Instance
	{
		get
		{
			return instance;
		}
	}

	public override void Enter(Outlaw agent)
	{
		if (agent.location != Locations.Bank) {
			Debug.Log(agent.ID + ": Goin' to rob that bank");

			agent.ChangeLocation(Locations.Bank);
		}
	}

	public override void Execute(Outlaw agent)
	{
		if (agent.location == Locations.Bank) {
			Debug.Log (agent.ID + ": Ah'm leavin' the bank with mah pockets full o' sweet gold");

			agent.stateMachine.RevertToPreviousState ();
		}
	}

	public override void Exit(Outlaw agent)
	{
		
	}
}
