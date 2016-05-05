using UnityEngine;
using System.Collections;

public class TutItemHoldingButton : TutorialItem {
	public KeyCode KeyToHold;
	public bool EnteringHold;


	protected new void OnEnable()
	{
		GameManager.ToggleGameOn (false);

		if(!EnteringHold)
		{
			gameObject.GetComponent<UnityEngine.UI.Button>().onClick.AddListener( 
         	()=>{
				gameObject.SetActive (false);
				GameManager.Instance.KeyHoldManager.ReleaseOpenClose(KeyToHold);
				
				if(NextTutItem != null)
				{
					NextTutItem.gameObject.SetActive(true);
				}
				else
				{
					GameManager.ToggleGameOn (true);
				}
			});
		}
	}


	protected override void Update()
	{
        base.Update();

		if (EnteringHold) {
			if (Input.GetKeyDown (KeyToHold)) {
				gameObject.SetActive (false);
				GameManager.Instance.KeyHoldManager.ForceOpen (KeyToHold);
				if (NextTutItem != null) {
					NextTutItem.gameObject.SetActive (true);
				} else {
					GameManager.ToggleGameOn (true);
				}
			}
		}
	}
}