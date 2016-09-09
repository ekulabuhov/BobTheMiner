using UnityEngine;
using System.Collections;

public class DoHouseWork : State<Elsa> {

	static readonly DoHouseWork instance = new DoHouseWork();

	public static DoHouseWork Instance
	{
		get
		{
			return instance;
		}
	}

	public override void Enter(Elsa agent)
	{

	}

	public override void Execute(Elsa agent)
	{
		switch(Random.Range(0, 3))
		{
		case 0:
			Debug.Log(agent.ID + ": Moppin' the floor");
			break;
		case 1:
			Debug.Log(agent.ID + ": Washin' the dishes");
			break;
		case 2:
			Debug.Log(agent.ID + ": Makin' the bed");
			break;
		}
	}

	public override void Exit(Elsa agent)
	{
		
	}
}
