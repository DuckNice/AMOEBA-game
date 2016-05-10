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


    public void ToggleActionSelection(string name, bool active)
    {
        ActionsPanel.gameObject.SetActive(active);

        if(active)
            CreateActionButtons(name);
    }

    public void Awake()
    {
        ActionsPanel.gameObject.SetActive(false);
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
            GameObject buttonObj = new GameObject();
            buttonObj.AddComponent<RectTransform>();
            buttons.Add(buttonObj.AddComponent<Button>());
            buttonObj.GetComponent<RectTransform>().anchorMax = new Vector2(0,0);
            buttonObj.GetComponent<RectTransform>().pivot = new Vector2(0, 0);
            buttonObj.GetComponent<RectTransform>().anchorMin = new Vector2(0,0);
            buttonObj.transform.parent = ActionsPanel;
            buttonObj.transform.position = new Vector3(0, i * 50, 0);
            buttonObj.name = action;

            buttonObj.AddComponent<Text>();
            buttonObj.GetComponentInChildren<Text>().text = action;
            i++;
        }

        ActionsCanvas.sizeDelta = new Vector2(ActionsCanvas.rect.width, i * 50);
        ActionsCanvas.position = new Vector3(PlayerMotion.MouseClicked.x, PlayerMotion.MouseClicked.y, 0);
    }
}