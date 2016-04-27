using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EmotionStudyController : MonoBehaviour {
    public int _motionNo = 0;

    public MotionManager item1;
    Vector3 item1pos;
    public MotionManager item2;
    Vector3 item2pos;

    public Button butt1;
    public Button butt2;
    public Text motionTracker;

    public void Awake()
    {
        Screen.orientation = ScreenOrientation.Landscape;
        item1pos = item1.transform.position;
        item1.SetVelocity(Vector3.zero, 0);
        item2pos = item2.transform.position;
        item2.SetVelocity(Vector3.zero, 0);
    }


    public void Reset()
    {
        item1.transform.position = item1pos;
        item1.SetVelocity(Vector3.zero, 0);
        item2.transform.position = item2pos;
        item2.SetVelocity(Vector3.zero, 0);


        butt1.interactable = false;
        butt2.interactable = false;

        StartCoroutine(FindMotion());
    }


    public void NextMotion()
    {

        _motionNo++;

        motionTracker.text = "Motion no: " + _motionNo;

        Reset();
    }

    IEnumerator FindMotion()
    {
        switch(_motionNo)
        {
            case 0:
                item1pos = new Vector3(-6.7f, 0.9f, 0);
                item1.transform.position = item1pos;

                item2pos = new Vector3(5.1f, 0.7f, 0);
                item2.transform.position = item2pos;

                yield return StartCoroutine(MotionCollection.Hit(MotionCollection.BehaviourTypes.aristocrat, item1, item2));
                break;
            case 1:
                item1pos = new Vector3(-3.155905f, 0.8399305f, 0);
                item1.transform.position = item1pos;

                item2pos = new Vector3(7.851135f, 0.6533706f, 0);
                item2.transform.position = item2pos;

                StartCoroutine(MotionCollection.Walk(MotionCollection.BehaviourTypes.aristocrat, item2, new Vector2(8.5f, -1.1f)));
                yield return StartCoroutine(MotionCollection.Walk(MotionCollection.BehaviourTypes.aristocrat, item1, new Vector2(-7.6f, 8.7f)));

                break;
            case 2:
                item1pos = new Vector3(-7.6f, 8.7f, 0);
                item1.transform.position = item1pos;

                item2pos = new Vector3(8.5f, -1.1f, 0);
                item2.transform.position = item2pos;

                StartCoroutine(MotionCollection.Run(MotionCollection.BehaviourTypes.aristocrat, item1, new Vector2(5.1f, 0.7f)));
                yield return StartCoroutine(MotionCollection.Run(MotionCollection.BehaviourTypes.aristocrat, item2, new Vector2(-6.7f, 0.9f)));

                break;
            case 3:
                item1pos = new Vector3(5.1f, 0.7f, 0);
                item1.transform.position = item1pos;

                item2pos = new Vector3(-6.7f, 0.9f, 0);
                item2.transform.position = item2pos;

                yield return StartCoroutine(MotionCollection.Threaten(MotionCollection.BehaviourTypes.aristocrat, item2, item1));
                break;

            case 4:
                item1pos = new Vector3(10.19361f, 0.6f, 0);
                item1.transform.position = item1pos;

                item2pos = new Vector3(-0.6631324f, 0.7976f, 0);
                item2.transform.position = item2pos;


                yield return StartCoroutine(MotionCollection.Console(MotionCollection.BehaviourTypes.aristocrat, item2, item1));
                break;
            case 5:
                item1pos = new Vector3(10.19361f, 0.6f, 0);
                item1.transform.position = item1pos;

                item2pos = new Vector3(1.714227f, 0.7976f, 0);
                item2.transform.position = item2pos;


                yield return StartCoroutine(MotionCollection.Kiss(MotionCollection.BehaviourTypes.aristocrat, item2, item1));
                break;
            default:
                Application.Quit();
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#endif
                break;
        }


        butt1.interactable = true;
        butt2.interactable = true;
    }
}