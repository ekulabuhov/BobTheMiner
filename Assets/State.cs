abstract public class State <T> {

	abstract public void Enter (T agent);
	abstract public void Execute (T agent);
	abstract public void Exit (T agent);
	//this executes if the agent receives a message from the
	//message dispatcher
	public virtual bool OnMessage(T agent, Telegram msg) {
		//send msg to global message handler
		return false;
	}
}