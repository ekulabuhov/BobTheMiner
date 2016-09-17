using System;
using System.Collections.Generic;
using UnityEngine;
using Completed;
using System.Linq;

public abstract class MovingAgent<T> : Agent
{
	public StateMachine<T> stateMachine;
	private AStar aStar = new AStar();
	protected List<Point> waypoints = new List<Point>();             // A-Star path
	private Action onChangeComplete;
	protected BoardManager boardScript;		// Store a reference to our BoardManager which will set up the level.
	public Locations location, movingTowards;    
	private Player playerScript;			// Store a reference to our Player which will move our player.

	//all subclasses can communicate using messages.
	public override bool HandleMessage(Telegram msg) {
		return stateMachine.HandleMessage(msg);
	}

	public override bool HandleSenseEvent(Sense sense) {
		return stateMachine.HandleSenseEvent (sense);
	}

	public virtual bool UpdateStateMachine()
	{
		// before changing the state we need to walk our waypoints
		if (this.waypoints.Count > 0)
		{
			int xDir = this.waypoints[0].x - (int)rb2D.position.x;
			int yDir = this.waypoints[0].y - (int)rb2D.position.y;
			playerScript.MovePlayer(xDir, yDir);
			this.waypoints.RemoveAt(0);
			if (this.waypoints.Count == 0) {
				this.location = movingTowards;
				if (this.onChangeComplete != null) {
					this.onChangeComplete ();
					this.onChangeComplete = null;
				}
					
			}
			return true;
		}

		return false;
	}

	//Protected, virtual functions can be overridden by inheriting classes.
	protected virtual void Start()
	{
		//Get a component reference to this object's Rigidbody2D
		rb2D = GetComponent<Rigidbody2D>();
		//Get a component reference to the attached BoardManager script
		boardScript = FindObjectOfType<BoardManager>();
		//Get a component reference to the attached Player script
		playerScript = GetComponent<Player>();

		EntityManager.RegisterAgent (this);
	}

	public void ChangeLocation (Locations location, Action onChangeComplete = null)
	{
		var pathStart = new Point { x = (int)rb2D.position.x, y = (int)rb2D.position.y };
		Vector3 newPos = new Vector3();

		this.onChangeComplete = onChangeComplete;

		switch (location)
		{
		case Locations.Goldmine:
			newPos = boardScript.mine.transform.position;
			break;
		case Locations.Bank:
			newPos = boardScript.bank.transform.position;
			break;
		case Locations.Shack:
			newPos = boardScript.wigwam.transform.position;
			break;
		case Locations.Saloon:
			newPos = boardScript.barrels.transform.position;
			break;
		case Locations.Cemetary:
			newPos = boardScript.cemetary.transform.position;
			break;
		case Locations.OutlawCamp:
			newPos = boardScript.outlawCamp.transform.position;
			break;
		case Locations.UndertakersOffice:
			newPos = boardScript.undertakersOffice.transform.position;
			break;
		case Locations.Outlaw:
			newPos = EntityManager.GetEntity ("Outlaw").CurrentPosition;
			break;
		default:
			break;
		}

		var pathEnd = new Point { x = (int)newPos.x, y = (int)newPos.y };
		var forrest = boardScript.forrest.Select((go) => { 
			return new Point() { x = (int)go.transform.position.x, y = (int)go.transform.position.y };
		}).ToArray(); 
		this.waypoints = aStar.calculatePath(pathStart, pathEnd);

		foreach (Transform child in boardScript.boardHolder)
		{
			var spriteRenderer = child.GetComponent<SpriteRenderer> ();
			// remove previous color
			if (spriteRenderer.color == Color.magenta) {
				spriteRenderer.color = Color.cyan;
			} else if (spriteRenderer.color == Color.red) {
				spriteRenderer.color = Color.white;
			}

			// if child is on the path
			bool onPath = waypoints.Any((p) => { return p.x == child.position.x && p.y == child.position.y; });
			if (onPath) {
				if (spriteRenderer.color == Color.cyan) {
					spriteRenderer.color = Color.magenta;
				} else {
					spriteRenderer.color = Color.red;
				}
			}
		}
		this.movingTowards = location;
	}
}

