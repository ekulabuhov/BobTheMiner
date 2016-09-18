using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Timers;


public enum SenseType
{
	Sight,
	Hearing,
	Smell
};

public struct Sense
{
	public string Sender;
	public string Receiver;
	public SenseType senseType;

	public Sense (string s, string r, SenseType st)
	{
		Sender = s;
		Receiver = r;
		senseType = st;
	}
}

public static class SenseEvent
{
	public static AStar aStar;

	static float SENSE_RANGE = 3.0f;

	public static void UpdateSensors ()
	{
			
		// Agents pairwise check
		for (int i = 0; i < EntityManager.GetCount (); ++i) {
			Agent a1 = EntityManager.GetEntity (i);
			for (int j = 0; j < EntityManager.GetCount (); ++j) {
				if (i != j) {
					Agent a2 = EntityManager.GetEntity (j);

					// If close enough
					if (Vector2.Distance (a1.CurrentPosition, a2.CurrentPosition) < SENSE_RANGE) {
						// Propogate the sense
						var a1Pos = new Point() { x = (int)a1.CurrentPosition.x, y = (int)a1.CurrentPosition.y };
						var a2Pos = new Point() { x = (int)a2.CurrentPosition.x, y = (int)a2.CurrentPosition.y };
						if (aStar.calculatePath (a1Pos, a2Pos) != null) {
							// Sense the agent
							Sense sense = new Sense (a2.ID, a1.ID, SenseType.Sight);
							a1.HandleSenseEvent (sense);
						}
					}
				}
			}
		}
	}
}
