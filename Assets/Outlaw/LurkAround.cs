using UnityEngine;
using System.Collections;
using System;

public class LurkAround : State<Outlaw> {

	static readonly LurkAround instance = new LurkAround();

	public static LurkAround Instance
	{
		get
		{
			return instance;
		}
	}

	public override void Enter(Outlaw agent)
	{
		Debug.Log(agent.ID + ": Lurkin around");
	}

	public override void Execute(Outlaw agent)
	{
		if (agent.location == Locations.OutlawCamp)
			agent.ChangeLocation (Locations.Cemetary);
		else
			agent.ChangeLocation (Locations.OutlawCamp);
	}

	public override void Exit(Outlaw agent)
	{
		
	}
}
