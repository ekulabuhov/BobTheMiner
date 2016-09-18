using UnityEngine;
using System.Collections;
using System;

public class DeadOutlaw : State<Outlaw> {

	static readonly DeadOutlaw instance = new DeadOutlaw();

	public static DeadOutlaw Instance
	{
		get
		{
			return instance;
		}
	}

	public override void Enter(Outlaw agent)
	{
		agent.isDead = true;
		MessageDispatcher.DispatchMessage (0, "Undertaker", agent.ID, MessageTypes.OutlawDied);
	}

	public override void Execute(Outlaw agent)
	{
		
	}

	public override void Exit(Outlaw agent)
	{

	}

	public override bool OnMessage (Outlaw agent, Telegram telegram)
	{
		switch (telegram.Msg) {
		case MessageTypes.UndertakerArrived:
			// hide graveyard
			agent.HideBody ();
			return true;
		default:
			return false;
		}
	}
}
