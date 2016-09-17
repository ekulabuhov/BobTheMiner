using UnityEngine;

public abstract class Agent : MonoBehaviour
{
	public abstract string ID { get; }
	public abstract void Update();

	public abstract bool HandleMessage(Telegram msg);
	public abstract bool HandleSenseEvent(Sense sense);
	protected Rigidbody2D rb2D;				// The Rigidbody2D component attached to this object.

	public Vector2 CurrentPosition {
		get {
			return rb2D.transform.position;
		}
	}
}