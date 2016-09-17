using System;
using System.Collections.Generic;

public static class EntityManager
{
	static List<Agent> agents = new List<Agent>();

	public static void RegisterAgent(Agent agent) {
		agents.Add(agent);
	}

	public static int GetCount() {
		return agents.Count;
	}

	public static Agent GetEntity(string entityID) {
		return agents.Find((Agent agent) => agent.ID == entityID);
	}

	public static Agent GetEntity(int index) {
		return agents [index];
	}
}
