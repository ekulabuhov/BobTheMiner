using UnityEngine;
using System.Collections;
using System;

public class HangoutInTheOffice : State<Undertaker> {

	static readonly HangoutInTheOffice instance = new HangoutInTheOffice();

	public static HangoutInTheOffice Instance
	{
		get
		{
			return instance;
		}
	}

	public override void Enter(Undertaker agent)
	{
		Debug.Log(agent.ID + ": Heading to the office");
		agent.ChangeLocation (Locations.UndertakersOffice);
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
