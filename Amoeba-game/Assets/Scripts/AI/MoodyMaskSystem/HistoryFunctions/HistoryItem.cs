using UnityEngine;
using System.Collections.Generic;

using NMoodyMaskSystem;

public class HistoryItem {
	private MAction _action;
	private Person _subject;
	private Person _direct;
	private float _time;
	private Rule _rule;
	private List<Person> _peopleReacted;
	
	/// <summary>
	/// </summary>
	/// <param name="a">action</param>
	/// <param name="sub">person doing the action</param>
	/// <param name="dr">direct target</param>
	/// <param name="t">time</param>
	/// <param name="r">rule</param>
	public HistoryItem(MAction a, Person sub, Person dr, float t, Rule r)
	{
		_action = a;
		_subject = sub;
		_direct = dr;
		_time = t;
		_rule = r;
		_peopleReacted = new List<Person>();
	}
	
	public MAction GetAction() { return _action; }
	public Person GetSubject() { return _subject; }
	public Person GetDirect() { return _direct; }
	public float GetTime() { return _time; }
	public Rule GetRule() { return _rule; }
	/// <summary>
	/// </summary>
	/// <param name="p">person</param>
	/// <returns></returns>
	public bool HasReacted( Person p ) { if(_peopleReacted.Contains( p )){ return true; } return false; }
	/// <summary>
	/// </summary>
	/// <param name="p">person</param>
	public void SetReacted( Person p ) { _peopleReacted.Add( p ); }
}
