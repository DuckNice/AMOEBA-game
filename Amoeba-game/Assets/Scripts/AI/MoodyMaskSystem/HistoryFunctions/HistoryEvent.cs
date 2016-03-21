using UnityEngine;
using System.Collections.Generic;

public class HistoryEvent : ScriptableObject {
	#pragma warning disable 0414
	private List<HistoryItem> _items = new List<HistoryItem>();
	//Strength and assosiation
	public Dictionary <HistoryEvent, float> Assosiations = new Dictionary<HistoryEvent, float>(); 
	public float EventStrength;
	#pragma warning restore 0414
}