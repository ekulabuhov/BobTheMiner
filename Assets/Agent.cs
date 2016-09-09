using UnityEngine;

public abstract class Agent<T> : MonoBehaviour
{
	public StateMachine<T> stateMachine;

	public abstract string ID { get; }
	public abstract void Update();

	//all subclasses can communicate using messages.
	public virtual bool HandleMessage(Telegram msg) {
		return stateMachine.HandleMessage(msg);
	}
}