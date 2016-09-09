using System;
using UnityEngine;

public class VisitBankAndDepositGold : State<Bob>
{
    static readonly VisitBankAndDepositGold instance = new VisitBankAndDepositGold();

    public static VisitBankAndDepositGold Instance
    {
        get
        {
            return instance;
        }
    }

    public VisitBankAndDepositGold()
    {

    }

    public override void Enter(Bob agent)
    {
        //on entry the miner makes sure he is located at the bank
        if (agent.location != Locations.Bank)
        {
            Debug.Log(agent.ID + ": Goin' to the bank. Yes siree");

            agent.ChangeLocation(Locations.Bank);
        }
    }

    public override void Execute(Bob agent)
    {
        //deposit the gold
        agent.AddToWealth(agent.GoldCarried());

        agent.SetGoldCarried(0);

        Debug.Log(agent.ID + ": Depositing gold. Total savings now: " + agent.Wealth());

        //wealthy enough to have a well earned rest?
        if (agent.Wealth() >= Bob.ComfortLevel)
        {
            Debug.Log(agent.ID + ": WooHoo! Rich enough for now. Back home to mah li'lle lady");

            agent.ChangeState(GoHomeAndSleepTilRested.Instance);
        }

        //otherwise get more gold
        else
        {
            agent.ChangeState(EnterMineAndDigForNugget.Instance);
        }
    }

    public override void Exit(Bob agent)
    {
        Debug.Log(agent.ID + ": Leavin' the bank");
    }
}