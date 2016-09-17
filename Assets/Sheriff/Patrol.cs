using UnityEngine;
using System.Collections;
using System;

public class Patrol : State<Sheriff> {

	static readonly Patrol instance = new Patrol();

	public static Patrol Instance
	{
		get
		{
			return instance;
		}
	}

	public override void Enter(Sheriff agent)
	{
		Debug.Log(agent.ID + ": On patrol!");
	}

	public override void Execute(Sheriff agent)
	{
		var values = Enum.GetValues(typeof(Locations));
		var nextLocation = (Locations)(((int)agent.location + 1) % values.Length);

		Debug.Log(agent.ID + ": Going to " + nextLocation);
		agent.ChangeLocation (nextLocation);
	}

	public override void Exit(Sheriff agent)
	{
		// shouldn't really happen
	}
}
