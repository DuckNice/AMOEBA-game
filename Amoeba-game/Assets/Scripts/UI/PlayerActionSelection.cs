using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class PlayerActionSelection : MonoBehaviour {
    [SerializeField]
    RectTransform ActionsCanvas;
    [SerializeField]
    RectTransform ActionsPanel;
    List<Button> buttons = new List<Button>();
    [SerializeField]
    GameObject buttonFap;
    public static bool IsActive { get; protected set; }
    [SerializeField]
    Text playerText;


    public void ToggleActionSelection(string characterName, bool active)
    {
        if (ActionsPanel != null && !GameManager.Instance.selectingFromActionPanel)
        {
            ActionsPanel.gameObject.SetActive(active);
            IsActive = active;

            if (active)
                CreateActionButtons(characterName);
        }
    }
    

    public void Awake()
    {
        if (ActionsPanel != null)
        { 
            ActionsPanel.gameObject.SetActive(false);
            IsActive = false;
        }
    }


    public void CreateActionButtons(string characterName)
    {
        foreach(Button button in buttons)
        {
            Destroy(button.gameObject);
        }
        buttons.Clear();

        List<string> actions =  GameManager.MoodyMask.PosActions.Keys.ToList();

        int i = 0;

        foreach(string action in actions)
        {
            GameObject buttonObj = Instantiate(buttonFap);
            RectTransform buttonRect = buttonObj.GetComponent<RectTransform>();
            buttons.Add(buttonObj.GetComponent<Button>());
            
            buttonRect.anchorMax = new Vector2(0,1);
            buttonRect.pivot = new Vector2(0, 1);
            buttonRect.anchorMin = new Vector2(0,1);
            buttonObj.transform.SetParent(ActionsPanel);
            buttonRect.anchoredPosition = new Vector2(0, i * 25);
            
            buttonRect.localScale = new Vector3(1, 1, 1);
            buttonObj.name = action;
            
            Text butText = buttonObj.GetComponentInChildren<Text>();
            butText.text = action.ToUpper();

            buttonObj.GetComponent<Button>().onClick.AddListener(
                    () =>
                    {
                        GameManager.Instance.ToggleSelectingFromActionPanel(false);
                        ToggleActionSelection("", false);
                        Debug.Log("Clicked");
                        GameManager.MoodyMask.GetRule(action).DoAction(
                        playerText,
                        GameManager.MoodyMask.GetPerson("Kasper"),
                        GameManager.MoodyMask.GetPerson(characterName));
                    }
                );

            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerDown;
            entry.callback = new EventTrigger.TriggerEvent();
            UnityEngine.Events.UnityAction<BaseEventData> call = new UnityEngine.Events.UnityAction<BaseEventData>(
                    (x) =>
                    {
                        Debug.Log("Clicked");
                        GameManager.MoodyMask.GetRule(action).DoAction(
                        playerText,
                        GameManager.MoodyMask.GetPerson("Kasper"),
                        GameManager.MoodyMask.GetPerson(characterName));
                    }
                );

            EventTrigger trigger = buttonObj.AddComponent<EventTrigger>();
            trigger.triggers.Add(entry);

            i++;
        }

        ActionsCanvas.pivot = new Vector2(0, 1);
        ActionsCanvas.sizeDelta = new Vector2(ActionsCanvas.rect.width, i * 25);
        ActionsCanvas.position = new Vector3(PlayerMotion.MouseClicked.x, PlayerMotion.MouseClicked.y, -8);
    }
}