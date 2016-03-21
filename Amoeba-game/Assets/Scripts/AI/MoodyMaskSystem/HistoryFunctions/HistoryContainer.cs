using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class HistoryContainer : MonoBehaviour
{
	struct EventList
	{
		public List<HistoryEvent> List ;
		public float LastDeg;

		public EventList (HistoryEvent firstEvent, float lastDeg)
		{
			List = new List<HistoryEvent>();
			if(firstEvent != null)
			{
				List.Add(firstEvent);
			}

			LastDeg = lastDeg;
		}
	};

	List<EventList> _historyItems = new List<EventList> ();
#pragma warning disable 0414
	ShortTermMemory stm = new ShortTermMemory ();
#pragma warning restore 0414
	float _delay;
	float _degEachS;
	float _histBatchMaxSize;


	public HistoryContainer (float delay, float degEachS, float histBatchMaxSize = 100)
	{
		_historyItems.Add (new EventList (null, GameManager.Time));
		_delay = delay;
		_degEachS = degEachS;
		_histBatchMaxSize = histBatchMaxSize;
		StartCoroutine ("DegenerateFunc");
	}


	/// <summary>
	/// Adds an event to a not-full event list or a new one if all full. 
	/// Potential problems could arise if event is put in a list just before it is updated, 
	/// taking a too big chunk of the degeneration.
	/// </summary>
	private void AddHistEvent (HistoryEvent newEvent)
	{
		int index = -1;
		for (int i = 0; i < _historyItems.Count; i++) {
			if (_historyItems [i].List.Count < _histBatchMaxSize) {
				index = i;
				break;
			}
		}

		if (index != -1)
		{
			_historyItems [index].List.Add (newEvent);
		} 
		else
		{
			_historyItems.Add (new EventList(newEvent, GameManager.Time));
		}
	}


	/// <summary>
	/// Degenerates the next list of history events based on how long since last degeneration of that list.
	/// </summary>
	private IEnumerator DegenerateFunc ()
	{
		int index = 0;

		while (true) 
		{
			//Do we need a more complex degeneration?

			float curTime = GameManager.Time;
			EventList curList = _historyItems[index];

			float deg = _degEachS * (curTime - _historyItems[index].LastDeg);

			curList.LastDeg = curTime;

			List<HistoryEvent> list = curList.List;

			for(int i = list.Count; i <= 0; i--)
			{
				list[i].EventStrength -= deg;

				if(list[i].EventStrength < 0f)
				{
					list.RemoveAt(i);
				}
				else
				{
					List<HistoryEvent> assotiations = list[i].Assosiations.Keys.ToList();

					foreach(HistoryEvent assotiation in assotiations)
					{
						list[i].Assosiations[assotiation] -= deg;

						if(list[i].Assosiations[assotiation] < 0f)
						{
							list[i].Assosiations.Remove(assotiation);
						}
					}
				}
			}

			_historyItems[index] = curList;

			index = ++index % _historyItems.Count;

			yield return new WaitForSeconds (_delay);
		}
	}
}