using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class TutorialItem : MonoBehaviour 
{
    public List<GameObject> ItemsToActivate = new List<GameObject>();
    public List<GameObject> ItemsToHighlight = new List<GameObject>();
	public TutorialItem NextTutItem;
    public float HighLightSpeed = 1f;
    public float HighLightScale = 1.3f;
    protected List<Vector3> OriginalScales = new List<Vector3>();
    protected UnityAction BaseContinueTrigger;


	protected virtual void OnEnable()
	{
		GameManager.ToggleGameOn (false);
        
        gameObject.GetComponent<UnityEngine.UI.Button>().onClick.AddListener( 
		()=>{
            BaseContinueTrigger();
        });
    }


    protected virtual void Awake()
    {
        BaseContinueTrigger = () =>
        {
            if (NextTutItem != null)
            {
                NextTutItem.gameObject.SetActive(true);
            }
            else
            {
                GameManager.ToggleGameOn(true);
            }
            gameObject.SetActive(false);
        };

        if (ItemsToHighlight != null)
        {
            foreach (GameObject ItemToHighlight in ItemsToHighlight)
            {
                OriginalScales.Add(ItemToHighlight.transform.localScale);
            }
        }

        if (ItemsToActivate != null)
        {
            foreach (GameObject item in ItemsToActivate)
            {
                item.SetActive(true);
            }
        }
    }


    float _time = 0.0f;
    bool _up = true;
    protected virtual void Update()
    {
        if(ItemsToHighlight != null)
        {
            _time += ((_up) ? 1:-1) * Time.deltaTime * HighLightSpeed;
            if (_up && _time > 1f)
                _up = false;
            else if (!_up && _time < 0f)
                _up = true;

            //  float additive = Vector3.Lerp(OriginalScale * (HighLightScale * -0.5f), OriginalScale * (HighLightScale * 0.5f), _time);
            //  additive = Mathf.Sin(Time.time * HighLightSpeed) * HighLightScale;

            for (int i = 0; i < ItemsToHighlight.Count; i++)
            {
                ItemsToHighlight[i].transform.localScale =
                Vector3.Lerp(OriginalScales[i], OriginalScales[i] * HighLightScale, _time);
            }
        }
    }

    protected void OnDisable()
    {
        if (ItemsToHighlight != null)
        {
            for(int i = 0; i < ItemsToHighlight.Count; i++)
            {
                ItemsToHighlight[i].transform.localScale = OriginalScales[i];
            }
        }
        if (ItemsToActivate != null)
        {
            foreach(GameObject item in ItemsToActivate)
            {
                item.SetActive(false);
            }
        }
    }
}