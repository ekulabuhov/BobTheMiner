using UnityEngine;

public class EnterMineAndDigForNugget : State<Bob>
{
	static readonly EnterMineAndDigForNugget instance = new EnterMineAndDigForNugget();
	
	public static EnterMineAndDigForNugget Instance {
		get {
			return instance;
		}
	}

	#region implemented abstract members of State

	public override void Enter (Bob agent)
	{
		if (agent.location != Locations.Goldmine) {
			Debug.Log(agent.ID + ": Walkin' to the gold mine");

			agent.ChangeLocation(Locations.Goldmine);
		}
	}

	public override void Execute (Bob agent)
	{
		agent.AddToGoldCarried (1);
		agent.IncreaseFatigue ();
		Debug.Log (agent.ID + ": Pickin' up a nugget");

		if (agent.PocketsFull ()) {
			agent.ChangeState(VisitBankAndDepositGold.Instance);
		}

		if (agent.Thirsty ()) {
			agent.ChangeState(QuenchThirst.Instance);
		}
	}

	public override void Exit (Bob agent)
	{
        Debug.Log(agent.ID + ": Ah'm leavin' the gold mine with mah pockets full o' sweet gold");
    }

	#endregion
}