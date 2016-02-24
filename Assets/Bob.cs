﻿using Completed;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public enum Locations
{
	Goldmine,
    Bank,
    Shack,
    Saloon
}

public class MyFileLogHandler : ILogHandler
{
    private ILogHandler m_DefaultLogHandler = Debug.logger.logHandler;
    public Text foodText;						//UI Text to display current player food total.

    public MyFileLogHandler(Text foodText)
    {
        // Replace the default debug log handler
        Debug.logger.logHandler = this;
        this.foodText = foodText;
    }

    public void LogFormat(LogType logType, UnityEngine.Object context, string format, params object[] args)
    {
        foodText.text = String.Format(format, args);
        m_DefaultLogHandler.LogFormat(logType, context, format, args);
    }

    public void LogException(Exception exception, UnityEngine.Object context)
    {
        m_DefaultLogHandler.LogException(exception, context);
    }
}

public class Bob : Agent {
	#region implemented abstract members of Agent
	public override string ID {
		get {
			return "Bob";
		}
	}

	public override void Update () {

	}
    #endregion

    public void UpdateStateMachine()
    {
        // before changing the state we need to walk our waypoints
        if (this.waypoints.Count > 0)
        {
            int xDir = this.waypoints[0].x - (int)rb2D.position.x;
            int yDir = this.waypoints[0].y - (int)rb2D.position.y;
            playerScript.MovePlayer(xDir, yDir);
            this.waypoints.RemoveAt(0);
            return;
        }

        m_iThirst += 1;

        this.stateMachine.Update();
    }

    public void Awake()
    {
        this.stateMachine = new StateMachine<Bob>();
        this.stateMachine.Init(this, GoHomeAndSleepTilRested.Instance);
    }

    //Protected, virtual functions can be overridden by inheriting classes.
    protected virtual void Start()
    {
        //Get a component reference to this object's Rigidbody2D
        rb2D = GetComponent<Rigidbody2D>();
        //Get a component reference to the attached BoardManager script
        boardScript = FindObjectOfType<BoardManager>();
        //Get a component reference to the attached Player script
        playerScript = FindObjectOfType<Player>();

        new MyFileLogHandler(foodText);

        InvokeRepeating("UpdateStateMachine", 1, 1);
    }

    private StateMachine<Bob> stateMachine;

	public static int WAIT_TIME = 5;
	public int waitedTime = 0;
	public int createdTime = 0;
	public Locations location;
    private Rigidbody2D rb2D;				// The Rigidbody2D component attached to this object.
    private BoardManager boardScript;		// Store a reference to our BoardManager which will set up the level.
    public Text foodText;					// UI Text to display current player food total.
    private AStar aStar = new AStar();
    private List<Point> waypoints = new List<Point>();             // A-Star path
    private Player playerScript;			// Store a reference to our Player which will move our player.
    
    //the amount of gold a miner must have before he feels comfortable
    public const int ComfortLevel = 5;
    //the amount of nuggets a miner can carry
    const int MaxNuggets = 3;

    //how many nuggets the miner has in his pockets
    int m_iGoldCarried;

    int m_iMoneyInBank;

    //the higher the value, the thirstier the miner
    int m_iThirst;

    //the higher the value, the more tired the miner
    int m_iFatigue;

    internal void BuyAndDrinkAWhiskey()
    {
        m_iThirst = 0; m_iMoneyInBank -= 2;
    }

    internal bool Fatigued()
    {
        return m_iFatigue > TirednessThreshold;
    }

    //above this value a miner is thirsty
    const int ThirstLevel = 5;
    //above this value a miner is sleepy
    const int TirednessThreshold = 5;

    internal int GoldCarried()
    {
        return m_iGoldCarried;
    }

    internal void DecreaseFatigue()
    {
        m_iFatigue -= 1;
    }

    public void IncreaseFatigue()
    {
        m_iFatigue += 1;
    }

    internal void SetGoldCarried(int val)
    {
        m_iGoldCarried = val;
    }

    internal void AddToWealth(int val)
    {
        m_iMoneyInBank += val;

        if (m_iMoneyInBank < 0) m_iMoneyInBank = 0;
    }    

	public void IncreaseWaitedTime (int amount) {
		this.waitedTime += amount;
	}

	public bool WaitedLongEnough () {
		return this.waitedTime >= WAIT_TIME;
	}

    internal int Wealth()
    {
        return m_iMoneyInBank;
    }

    public void CreateTime () {
		this.createdTime++;
		this.waitedTime = 0;
	}

    internal bool Thirsty()
    {
        if (m_iThirst >= ThirstLevel) { return true; }

        return false;
    }

    public void ChangeState (State<Bob> state) {
		this.stateMachine.ChangeState(state);
	}

	public void ChangeLocation (Locations location)
	{
        var pathStart = new Point { x = (int)rb2D.position.x, y = (int)rb2D.position.y };
        Vector3 newPos = new Vector3();

        switch (location)
        {
            case Locations.Goldmine:
                newPos = boardScript.exit.transform.position;
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
            default:
                break;
        }

        var pathEnd = new Point { x = (int)newPos.x, y = (int)newPos.y };
        this.waypoints = aStar.calculatePath(pathStart, pathEnd);

        foreach (Transform child in boardScript.boardHolder)
        {
            // remove previous color
            child.GetComponent<SpriteRenderer>().color = Color.white;

            // if child is on the path
            bool onPath = waypoints.Any((p) => { return p.x == child.position.x && p.y == child.position.y; });
            if (onPath)
                child.GetComponent<SpriteRenderer>().color = Color.red;
        }

        this.location = location;
	}

	public void AddToGoldCarried (int val)
	{
        m_iGoldCarried += val;

        if (m_iGoldCarried < 0) m_iGoldCarried = 0;
    }

	public bool PocketsFull ()
	{
        return m_iGoldCarried >= MaxNuggets;
    }
}
