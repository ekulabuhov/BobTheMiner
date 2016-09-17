using UnityEngine;
using System.Collections;
using System;

public class DragOffTheBody : State<Undertaker> {

	static readonly DragOffTheBody instance = new DragOffTheBody();

	public static DragOffTheBody Instance
	{
		get
		{
			return instance;
		}
	}

	public override void Enter(Undertaker agent)
	{
		Debug.Log(agent.ID + ": Heading to the cemetary");
		agent.ChangeLocation (Locations.Cemetary);
	}

	public override void Execute(Undertaker agent)
	{
		Debug.Log(agent.ID + ": In the office");
	}

	public override void Exit(Undertaker agent)
	{
		Debug.Log(agent.ID + ": Leaving the office");
	}
}
