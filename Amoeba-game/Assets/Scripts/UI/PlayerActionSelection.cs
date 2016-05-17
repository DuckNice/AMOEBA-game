using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;

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
        //TODO: find a system instead of all these hard-coded playerNames.
        if (ActionsPanel != null && GameManager.Instance.ActionSelectionAccessible && characterName != "Kasper".ToLower().Trim())
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
    

    public bool IsWithinActionCanvas(Vector2 point)
    {
        Vector2 size = Vector2.Scale(ActionsCanvas.rect.size, ActionsCanvas.lossyScale);
        Rect rect = new Rect(ActionsCanvas.position.x, ActionsCanvas.position.y - size.y, size.x, size.y);
        Vector2 screenPos = Camera.main.ScreenToWorldPoint(point);

        return rect.Contains(screenPos);
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
            buttonRect.localPosition = new Vector3(0, 0, 0);
            buttonRect.anchoredPosition = new Vector2(0, i * 25);
            
            buttonRect.localScale = new Vector3(1, 1, 1);
            buttonObj.name = action;
            
            Text butText = buttonObj.GetComponentInChildren<Text>();
            butText.text = action.ToUpper();

            buttonObj.GetComponent<Button>().onClick.AddListener(
                () =>
                    {
                        ToggleActionSelection("", false);
                        GameManager.MoodyMask.PosActions[action].DoAction(
                        playerText,
                        GameManager.MoodyMask.GetPerson("Kasper"),
                        GameManager.MoodyMask.GetPerson(characterName), new NMoodyMaskSystem.Rule(action, GameManager.MoodyMask.PosActions[action]));
                    }
            );
            
            i++;
        }

        ActionsCanvas.pivot = new Vector2(0, 1);
        ActionsCanvas.sizeDelta = new Vector2(ActionsCanvas.rect.width, i * 25);
        ActionsCanvas.position = new Vector3(PlayerMotion.MouseClicked.x, PlayerMotion.MouseClicked.y, -8);
    }
}