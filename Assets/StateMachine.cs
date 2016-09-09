public class StateMachine <T> {
	
	private T agent;
	private State<T> state, globalState, previousState;

	public void Awake () {
		this.state = null;
	}

	public void Init (T agent, State<T> startState, State<T> globalState = null) {
		this.agent = agent;
		this.state = startState;
		this.globalState = globalState;
	}

	public void Update () {
		//if a global state exists, call its execute method, else do nothing
		if (this.globalState != null)  this.globalState.Execute(this.agent);

		//same for the current state
		if (this.state != null) this.state.Execute(this.agent);
	}

	public bool HandleMessage(Telegram msg) {
		//first see if the current state is valid and that it can handle
		//the message
		if (state != null && state.OnMessage(agent, msg))
		{
			return true;
		}

		//if not, and if a global state has been implemented, send 
		//the message to the global state
		if (globalState != null && globalState.OnMessage(agent, msg))
		{
			return true;
		}

		return false;
	}
	
	public void ChangeState (State<T> nextState) {
		//keep a record of the previous state
		this.previousState = this.state;

		if (this.state != null) this.state.Exit(this.agent);
		this.state = nextState;
		if (this.state != null) this.state.Enter(this.agent);
	}

	//change state back to the previous state
	public void RevertToPreviousState()
	{
		ChangeState(this.previousState);
	}
}