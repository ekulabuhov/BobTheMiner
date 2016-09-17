using UnityEngine;
using System.Collections;

public class Outlaw : MovingAgent<Outlaw> {
	#region implemented abstract members of Agent
	public override string ID {
		get {
			return "Outlaw";
		}
	}

	public override void Update () {

	}
	#endregion

	bool _isDead = false;

	public bool isDead {
		set {
			this.GetComponent<Animator> ().SetBool ("isDead", value);
			_isDead = value;
		}
		get {
			return _isDead;
		}
	}

	public void Hide() {
		this.rb2D.gameObject.GetComponent<Renderer> ().enabled = false;
	}

	public override bool UpdateStateMachine()
	{
		if (!_isDead && base.UpdateStateMachine ())
			return true;

		this.stateMachine.Update();
		MessageDispatcher.DispatchDelayedMessages("Outlaw");

		return true;
	}

	public void Awake()
	{
		
	}

	//Protected, virtual functions can be overridden by inheriting classes.
	protected override void Start()
	{
		base.Start ();

		this.stateMachine = new StateMachine<Outlaw>();
		this.stateMachine.Init(this, LurkAround.Instance, OutlawsGlobalState.Instance);

		InvokeRepeating("UpdateStateMachine", 1, 1);
	}
}
