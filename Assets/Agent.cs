using UnityEngine;

public abstract class Agent : MonoBehaviour
{
	public abstract string ID { get; }
	public abstract void Update();
}