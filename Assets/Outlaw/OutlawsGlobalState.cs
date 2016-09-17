using UnityEngine;
using System.Collections;
using System;

public class OutlawsGlobalState : State<Outlaw> {

	static readonly OutlawsGlobalState instance = new OutlawsGlobalState();

	public static OutlawsGlobalState Instance
	{
		get
		{
			return instance;
		}
	}

	public override void Enter(Outlaw agent)
	{

	}

	public override void Execute(Outlaw agent)
	{
		if (agent.isDead)
			return;
		
		//1 in 10 chance of needing the bathroom
		if (UnityEngine.Random.value < 0.1)
		{
			agent.stateMachine.ChangeState (RobBank.Instance);
		}
	}
		
	public override void Exit(Outlaw agent)
	{
		
	}

	public override bool OnMessage (Outlaw agent, Telegram telegram)
	{
		switch (telegram.Msg) {
		case MessageTypes.SheriffEncountered:
			Debug.Log (agent.ID + ": Shot by the sheriff :(");
			agent.stateMachine.ChangeState (DeadOutlaw.Instance);
			return true;
		default:
			return false;
		}
	}
}
