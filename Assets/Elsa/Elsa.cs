using Completed;
using UnityEngine;
using System.Collections;

public class Elsa : MovingAgent<Elsa> {
	public bool Cooking;

	#region implemented abstract members of Agent
	public override string ID {
		get {
			return "Elsa";
		}
	}

	public override void Update () {

	}
	#endregion

	public override bool UpdateStateMachine()
	{	
		this.stateMachine.Update();
		MessageDispatcher.DispatchDelayedMessages(ID);

		return true;
	}

	// Use this for initialization
	protected override void Start () {		
		base.Start ();

		this.rb2D.position = this.boardScript.wigwam.transform.position;
		InvokeRepeating("UpdateStateMachine", 1, 1);
	}

	public void Awake() {
		this.stateMachine = new StateMachine<Elsa>();
		this.stateMachine.Init(this, DoHouseWork.Instance, WifesGlobalState.Instance);
	}
}
