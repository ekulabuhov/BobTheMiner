using Completed;
using UnityEngine;
using System.Collections;

public class Elsa : Agent<Elsa> {
	public Player playerScript;			// Store a reference to our Player which will move our player.
	private BoardManager boardScript;		// Store a reference to our BoardManager which will set up the level.
	private Rigidbody2D rb2D;				// The Rigidbody2D component attached to this object.
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

	public void UpdateStateMachine()
	{	
		this.stateMachine.Update();
		MessageDispatcher<Elsa>.DispatchDelayedMessages();
	}

	// Use this for initialization
	void Start () {		
		//Get a component reference to this object's Rigidbody2D
		rb2D = GetComponent<Rigidbody2D>();
		//Get a component reference to the attached BoardManager script
		boardScript = FindObjectOfType<BoardManager>();
		//Get a component reference to the attached Player script
		playerScript = GetComponent<Player>();

		rb2D.position =	boardScript.wigwam.transform.position;
		InvokeRepeating("UpdateStateMachine", 1, 1);
		EntityManager<Elsa>.RegisterAgent (this);
	}

	public void Awake() {
		this.stateMachine = new StateMachine<Elsa>();
		this.stateMachine.Init(this, DoHouseWork.Instance, WifesGlobalState.Instance);
	}
}
