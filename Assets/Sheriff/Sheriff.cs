using UnityEngine;
using System.Collections;

public class Sheriff : MovingAgent<Sheriff> {
	#region implemented abstract members of Agent
	public override string ID {
		get {
			return "Sheriff";
		}
	}

	public override void Update () {

	}
	#endregion

	public override bool UpdateStateMachine()
	{
		if (base.UpdateStateMachine ())
			return true;

		this.stateMachine.Update();
		MessageDispatcher.DispatchDelayedMessages("Sheriff");

		return true;
	}

	public void Awake()
	{
		this.stateMachine = new StateMachine<Sheriff>();
		this.stateMachine.Init(this, Patrol.Instance, SheriffsGlobalState.Instance);
	}

	//Protected, virtual functions can be overridden by inheriting classes.
	protected override void Start()
	{
		base.Start ();

		InvokeRepeating("UpdateStateMachine", 1, 1);
	}
}
