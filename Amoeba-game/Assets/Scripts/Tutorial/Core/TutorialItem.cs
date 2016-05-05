using UnityEngine;
using UnityEngine.Events;

public class TutorialItem : MonoBehaviour 
{
    public GameObject ItemToHighlight;
	public TutorialItem NextTutItem;
    public float HighLightSpeed = 1f;
    public float HighLightScale = 1.3f;
    protected Vector3 OriginalScale;
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

        if (ItemToHighlight != null)
        {
            OriginalScale = ItemToHighlight.transform.localScale;
        }
    }


    float _time = 0.0f;
    bool _up = true;
    protected virtual void Update()
    {
        if(ItemToHighlight != null)
        {
            _time += ((_up) ? 1:-1) * Time.deltaTime * HighLightSpeed;
            if (_up && _time > 1f)
                _up = false;
            else if (!_up && _time < 0f)
                _up = true;

            //  float additive = Vector3.Lerp(OriginalScale * (HighLightScale * -0.5f), OriginalScale * (HighLightScale * 0.5f), _time);
            //  additive = Mathf.Sin(Time.time * HighLightSpeed) * HighLightScale;

            ItemToHighlight.transform.localScale =
                Vector3.Lerp(OriginalScale, OriginalScale * HighLightScale, _time);
        }
    }

    protected void OnDisable()
    {
        if (ItemToHighlight != null)
        {
            ItemToHighlight.transform.localScale = OriginalScale;
        }
    }
}