using UnityEngine;
using System;
using System.Collections.Generic;

public class MessageDispatcher
{
	public static void Discharge(Agent receiver, Telegram telegram)
	{
		if (!receiver.HandleMessage(telegram))
		{
			//telegram could not be handled
			Console.WriteLine ("Message not handled");
		}
	}

	public static void DispatchMessage(double delay, string receiverID, string senderID, MessageTypes msg) 
	{
		var receiver = EntityManager.GetEntity (receiverID);

		var telegram = new Telegram() { Sender = senderID, Receiver = receiver.ID, Msg = msg };

		double CurrentTime = DateTime.Now.Subtract (DateTime.Today).TotalSeconds;

		//if there is no delay, route telegram immediately                       
		if (delay <= 0.0f)                                                        
		{
			Debug.Log("Instant telegram dispatched at time: " + CurrentTime + 
				" by " + senderID + " for " + receiver.ID + ". Msg is " + telegram.Msg);

			//send the telegram to the recipient
			Discharge(receiver, telegram);
		}
		//else calculate the time when the telegram should be dispatched
		else
		{
			telegram.DispatchTime = CurrentTime + delay;

			//and put it in the queue
			priorityQ.Add(telegram);

			Debug.Log("Delayed telegram recorded at time: " + CurrentTime + 
				" by " + senderID + " for " + receiver.ID + ". Msg is " + telegram.Msg);
		}
	}

	static List<Telegram> priorityQ = new List<Telegram>();

	//---------------------- DispatchDelayedMessages -------------------------
	//
	//  This function dispatches any telegrams with a timestamp that has
	//  expired. Any dispatched telegrams are removed from the queue
	//------------------------------------------------------------------------
	public static void DispatchDelayedMessages(string receiverID)
	{
		//get current time
		double CurrentTime = DateTime.Now.Subtract(DateTime.Today).TotalSeconds;

		//now peek at the queue to see if any telegrams need dispatching.
		//remove all telegrams from the front of the queue that have gone
		//past their sell by date
		var telegrams = priorityQ.FindAll ((telegram) => {
			return telegram.DispatchTime < CurrentTime;
		});

		//read the telegram from the front of the queue
		telegrams.ForEach ((telegram) => {
			//find the recipient
			var receiver = EntityManager.GetEntity (receiverID);
			Debug.Log("Queued telegram ready for dispatch: Sent to " + receiver.ID + ". Msg is " + telegram.Msg);
			//send the telegram to the recipient
			Discharge(receiver, telegram);
			//remove it from the queue
			priorityQ.Remove(telegram);
		});
	}
}