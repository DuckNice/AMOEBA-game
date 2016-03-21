using UnityEngine;
using System.Collections.Generic;

public class ShortTermMemory : MonoBehaviour {
	#pragma warning disable 0414
	List<HistoryEvent> slots = new List<HistoryEvent>();
	#pragma warning restore 0414

	void UpdateSlots()
	{
		//When moving along an assosiation: Strenghten assosiation
	}
}