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


    public void ToggleActionSelection(string name, bool active)
    {
        ActionsPanel.gameObject.SetActive(active);
        IsActive = active;

        if (active)
            CreateActionButtons(name);
    }

    public void Awake()
    {
        ActionsPanel.gameObject.SetActive(false);
        IsActive = false;
    }


    public void CreateActionButtons(string name)
    {
        foreach(Button button in buttons)
        {
            Destroy(button);
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
            buttonObj.transform.parent = ActionsPanel;
            buttonRect.anchoredPosition = new Vector2(0, i * 25);
            
            buttonRect.localScale = new Vector3(1, 1, 1);
            buttonObj.name = action;
            
            Text butText = buttonObj.GetComponentInChildren<Text>();
            butText.text = action.ToUpper();
            i++;
        }

        ActionsCanvas.pivot = new Vector2(0, 1);
        ActionsCanvas.sizeDelta = new Vector2(ActionsCanvas.rect.width, i * 25);
        ActionsCanvas.position = new Vector3(PlayerMotion.MouseClicked.x, PlayerMotion.MouseClicked.y, -8);
    }
}