using UnityEngine;
using System.Collections;
using System;

public class SheriffsGlobalState : State<Sheriff> {

	static readonly SheriffsGlobalState instance = new SheriffsGlobalState();

	public static SheriffsGlobalState Instance
	{
		get
		{
			return instance;
		}
	}

	public override void Enter(Sheriff agent)
	{
		
	}

	public override void Execute(Sheriff agent)
	{
		
	}

	public override void Exit(Sheriff agent)
	{
		// shouldn't really happen
	}

	public override bool OnSenseEvent (Sheriff agent, Sense sense)
	{
		if (sense.Sender == "Outlaw") {
			MessageDispatcher.DispatchMessage (0, "Outlaw", agent.ID, MessageTypes.SheriffEncountered);
			return true;
		}

		return false;
	}
}
