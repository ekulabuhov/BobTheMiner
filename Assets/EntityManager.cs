using System;
using System.Collections.Generic;

public static class EntityManager<T>
{
	static List<Agent<T>> agents = new List<Agent<T>>();

	public static void RegisterAgent(Agent<T> agent) {
		agents.Add(agent);
	}

	public static Agent<T> GetEntity() {
		return agents [0];
	}
}

