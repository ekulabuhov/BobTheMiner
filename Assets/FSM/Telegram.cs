using UnityEngine;
using System.Collections;

public enum MessageTypes
{
	HiHoneyImHome,
	StewReady,
	Gunfight,
	OutlawDied,
	SheriffEncountered,
	UndertakerArrived,
	OutlawRespawn
};

public struct Telegram
{
	//the entity that sent this telegram
	public string Sender;

	//the entity that is to receive this telegram
	public string Receiver;

	//the message itself. These are all enumerated in the file
	//"MessageTypes.h"
	public MessageTypes Msg;

	//messages can be dispatched immediately or delayed for a specified amount
	//of time. If a delay is necessary this field is stamped with the time
	//the message should be dispatched.
	public double DispatchTime;

	//any additional information that may accompany the message
	//void*        ExtraInfo;
}