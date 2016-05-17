using UnityEngine;
using System.Collections;

public class TutorialManager : MonoBehaviour {
    [SerializeField]
    private bool _startTutorialAvailable;
    [SerializeField]
    public RectTransform _startTutorialScreen;
    
    
	void Awake () 
	{
        if (GameManager.TutorialAccessible)
        {
            if (_startTutorialAvailable && _startTutorialScreen != null)
            {
                _startTutorialScreen.gameObject.SetActive(true);
            }
        }
    }
}