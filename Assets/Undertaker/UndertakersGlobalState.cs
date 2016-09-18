using UnityEngine;
using System.Collections;
using System;

public class UndertakersGlobalState : State<Undertaker> {

	static readonly UndertakersGlobalState instance = new UndertakersGlobalState();

	public static UndertakersGlobalState Instance
	{
		get
		{
			return instance;
		}
	}

	public override void Enter(Undertaker agent)
	{
		
	}

	public override void Execute(Undertaker agent)
	{
		
	}

	public override void Exit(Undertaker agent)
	{
		
	}

	public override bool OnMessage(Undertaker agent, Telegram telegram) 
	{
		switch (telegram.Msg) {
		case MessageTypes.OutlawDied:
			Debug.Log (agent.ID + ": Let's bring that scum to the cemetary!");
			agent.ChangeLocation (Locations.Outlaw, () => {
				MessageDispatcher.DispatchMessage(0, "Outlaw", agent.ID, MessageTypes.UndertakerArrived);
				agent.stateMachine.ChangeState (DragOffTheBody.Instance);
			});
			return true;
		default:
			return false;
		}
	}
}
