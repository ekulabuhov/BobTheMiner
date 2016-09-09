using UnityEngine;
using System.Collections;

public class VisitBathroom : State<Elsa> {

	static readonly VisitBathroom instance = new VisitBathroom();

	public static VisitBathroom Instance
	{
		get
		{
			return instance;
		}
	}

	public override void Enter(Elsa agent)
	{
		Debug.Log(agent.ID + ": Walkin' to the can. Need to powda mah pretty li'lle nose");
	}

	public override void Execute(Elsa agent)
	{
		Debug.Log (agent.ID + ": Ahhhhhh! Sweet relief!");

		agent.stateMachine.RevertToPreviousState ();
	}

	public override void Exit(Elsa agent)
	{
		Debug.Log(agent.ID + ": Leavin' the Jon");
	}
}
