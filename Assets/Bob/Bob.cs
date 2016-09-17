using Completed;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public enum Locations
{
	None,
	Goldmine,
    Bank,
    Shack,
    Saloon,
	Cemetary,
	OutlawCamp,
	UndertakersOffice,
	Outlaw
}

public class MyFileLogHandler : ILogHandler
{
    private ILogHandler m_DefaultLogHandler = Debug.logger.logHandler;
    public Text bobText;
	public Text elsaText;

	public MyFileLogHandler(Text bobText, Text elsaText)
    {
        // Replace the default debug log handler
        Debug.logger.logHandler = this;
		this.bobText = bobText;
		this.elsaText = elsaText;
    }

    public void LogFormat(LogType logType, UnityEngine.Object context, string format, params object[] args)
    {
		var message = String.Format (format, args);
		if (message.StartsWith ("Bob:")) {
			bobText.text = message;
		} else if (message.StartsWith ("Elsa:")) {
			elsaText.text = message;
		} else {
			m_DefaultLogHandler.LogFormat (logType, context, format, args);
		}
    }

    public void LogException(Exception exception, UnityEngine.Object context)
    {
        m_DefaultLogHandler.LogException(exception, context);
    }
}

public class Bob : MovingAgent<Bob> {
	#region implemented abstract members of Agent
	public override string ID {
		get {
			return "Bob";
		}
	}

	public override void Update () {

	}
    #endregion

    public override bool UpdateStateMachine()
    {
		if (base.UpdateStateMachine ())
			return true;

        m_iThirst += 1;

        this.stateMachine.Update();
		MessageDispatcher.DispatchDelayedMessages(ID);

		return true;
    }

    public void Awake()
    {
        
    }

    //Protected, virtual functions can be overridden by inheriting classes.
    protected override void Start()
    {
		base.Start ();

		new MyFileLogHandler(bobText, elsaText);

		this.stateMachine = new StateMachine<Bob>();
		this.stateMachine.Init(this, EnterMineAndDigForNugget.Instance);

        InvokeRepeating("UpdateStateMachine", 1, 1);
    }

	public static int WAIT_TIME = 5;
	public int waitedTime = 0;
	public int createdTime = 0;
    public Text bobText;					// UI Text to display Bobs thoughts.
	public Text elsaText;					// UI Text to display Elsas thoughts.
    
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
