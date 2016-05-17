using UnityEngine;
using System.Collections.Generic;

public class KeyHoldEnable : MonoBehaviour {

	[System.Serializable]
	public class CustKeyValuePair : System.Object
	{
		[SerializeField]
		public KeyCode Key;
		[SerializeField]
		public RectTransform Value;
		[HideInInspector]
		public bool IsForcedOpen;
		[HideInInspector]
		public bool IsForcedClosed;
	}

	[SerializeField]
	List<CustKeyValuePair> KeyAndItemToActivate = new List<CustKeyValuePair>();


	public void ForceOpen(KeyCode key)
	{
		CustKeyValuePair val = KeyAndItemToActivate.Find(x => x.Key == key);

		if(val != null)
		{
			val.IsForcedOpen = true;
			val.IsForcedClosed = false;
		}
	}


	public void ForceClose(KeyCode key)
	{
		CustKeyValuePair val = KeyAndItemToActivate.Find(x => x.Key == key);
		
		if(val != null)
		{
			val.IsForcedOpen = false;
			val.IsForcedClosed = true;
		}
	}


	public void ReleaseOpenClose(KeyCode key)
	{
		CustKeyValuePair val = KeyAndItemToActivate.Find(x => x.Key == key);
		
		if(val != null)
		{
			val.IsForcedOpen = false;
			val.IsForcedClosed = false;
		}
	}


	public void OnEnable()
	{
		for (int i = KeyAndItemToActivate.Count -1; i >=0; i--) 
		{
			CustKeyValuePair pair = KeyAndItemToActivate[i];
			
			if(pair != null)
			{
				if(pair.Value == null)
				{
					KeyAndItemToActivate.RemoveAt(i);
				}
			}
			else
			{
				KeyAndItemToActivate.RemoveAt(i);
			}
		}
	}


	public void Update()
	{
        if(GameManager.TutorialAccessible)
        { 
		    for (int i = KeyAndItemToActivate.Count -1; i >=0; i--) 
		    {
			    CustKeyValuePair pair = KeyAndItemToActivate[i];

                if ((Input.GetKey(pair.Key) || pair.IsForcedOpen) && !pair.IsForcedClosed)
                {
                    pair.Value.gameObject.SetActive(true);
                }
                else
                {
                    pair.Value.gameObject.SetActive(false);
                }
            }
		}
	}
}