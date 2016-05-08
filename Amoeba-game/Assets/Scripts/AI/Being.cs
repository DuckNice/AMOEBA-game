using System.Collections.Generic;
using UnityEngine;
using System;

    //Namespaces
using NMoodyMaskSystem;


public class Being : MonoBehaviour
{
	public static List<Being> Beings { get; private set;}

    [Header("Info")]
	public string Name;
	public bool IsPlayer;
    public string[] UpdateLists;

    [Header("Attributes")]
    [Range(-1,1)]
    public float Impulsivity;
    [Range(-1, 1)]
    public float Rationality;
    [Range(-1, 1)]
    public float Morality;

    [Header("Traits")]
    [Range(-1, 1)]
    public float NiceNasty;
    [Range(-1, 1)]
    public float CharitableGreedy;
    [Range(-1, 1)]
    public float HonestFalse;

    [Header("Moods")]
    [Range(-1, 1)]
    public float HapSad;
    [Range(-1, 1)]
    public float ArousDisgus;
    [Range(-1, 1)]
    public float AngryFear;
    [Range(-1, 1)]
    public float EnergTired;
    
    [Header("Masks")]
    public Dictionary<string, float> PersonalRules;
    public MaskInfo[] CultureMasks;
    public InterPersonInfo[] InterPersons;
    public NMoodyMaskSystem.Opinion[] Opinion;

    [System.Serializable]
	public struct MaskInfo
	{
		public string MaskName;
		public string Role;
		public float Strength;
	};
    
	[System.Serializable]
	public struct InterPersonInfo
	{
        [Range(-1f,1f)]
		public float NiceNasty;
        [Range(-1f, 1f)]
        public float HonestFalse;
        [Range(-1f, 1f)]
        public float CharitableGreedy;
		[HideInInspector]
		public string PersonName;
		public string TargetName;
		public LinkInfo[] Links;
	};

	[System.Serializable]
	public struct LinkInfo
	{
		public string MaskName;
		public TypeMask MaskType;
		public LinkRefInfo[] LinkRefs;
	};

	[System.Serializable]
	public struct LinkRefInfo
	{
		public string Role;
		public float Strength;
	};

    
    public void Awake()
    {
        if(GetComponent<PlayerMotion>() != null)
        {
            IsPlayer = true;
        }

        if (Beings == null)
        {
            Beings = new List<Being>();
        }

        if (Name == "")
        {
            Debug.LogWarning("Warning: Being was initialized without a name! This might break the system.");
        }
        else if (Beings.Exists(x => (x.Name == Name)))
        {
            Debug.LogWarning("Warning: Being with name" + Name + " already exists! This might break the system.");
        }

        Name = Name.ToLower();

		for(int i = InterPersons.Length -1; i >= 0; i--)
		{
			InterPersons[i].PersonName = Name;
		}
    }

    
	public void Start()
	{
		if(IsPlayer)
		{
			GameManager.AIManager.MoodyMask.PlayerName.Add (Name);
		}

		if (Beings == null)
		{
			Beings = new List<Being>();
		}

		Beings.Add (this);

		foreach (string list in UpdateLists)
		{
			GameManager.AIManager.MoodyMask.AddUpdateList(list.ToLower());
            GameManager.AIManager.MoodyMask.AddListToActives(list.ToLower());
            GameManager.AIManager.MoodyMask.AddPersonToUpdateList (list.ToLower(), GameManager.AIManager.MoodyMask.GetPerson (Name));
		}
        
        NPCCreator.CreatePerson(GameManager.AIManager.MoodyMask, Name, CultureMasks, PersonalRules, Rationality, Morality, Impulsivity, 
            new []{ NiceNasty, CharitableGreedy, HonestFalse }, 
            new[] { HapSad, ArousDisgus, AngryFear, EnergTired });

        foreach (InterPersonInfo interPerson in InterPersons)
		{
			StartCoroutine(NPCCreator.SetupInterPerson(GameManager.AIManager.MoodyMask, interPerson));
		}
	}

    
    [HideInInspector]
    public Rule CurrentRule { get; protected set; }
    [HideInInspector]
    public float ActionStartTime { get; protected set; }
    public UnityEngine.UI.Text text;


    public void playerInput(string input)
    {
        input = input.ToLower();

        string[] seps = { " " };

        string[] sepInput = input.Split(seps, StringSplitOptions.RemoveEmptyEntries);

        if (sepInput != null && sepInput.Length > 0)
        {
            if (GameManager.MoodyMask.PosActions.ContainsKey(sepInput[0]))
            {
                if (GameManager.MoodyMask.UpdateLists["Main"].Exists( x => x.Name == "Kasper".ToLower().Trim()))
                {
                    MAction actionToDo = GameManager.MoodyMask.PosActions[sepInput[0]];

                    if (sepInput.Length > 1)
                    {
                        Person target = GameManager.MoodyMask.PplAndMasks.GetPerson(sepInput[1]);

                        if (target != null)
                        {
                            Person self = GameManager.MoodyMask.GetPerson("Kasper".ToLower().Trim());

                            actionToDo.DoAction(text, self, target, self.GetRule(actionToDo.Name));
                        }
                        else
                        {
                            text.text = "Error: didn't recognize '" + sepInput[1] + "'.";

                        }
                    }
                    else if (!actionToDo.NeedsDirect)
                    {
                        Person self = GameManager.MoodyMask.GetPerson("Kasper".ToLower().Trim());

                        actionToDo.DoAction(text, self, null, self.GetRule(actionToDo.Name));
                    }
                    else
                    {
                        text.text = "Action needs a target person.";
                    }
                }
                else
                {
                    text.text = "You are dead! You can't do anything!";
                }
            }
            else
            {
                text.text =  sepInput[0] + " not recognized.";
            }
        }
    }


    public void NPCAction(float time, bool forced)
	{
		if (!GameManager.AIManager.MoodyMask.PlayerName.Contains(Name.ToLower()) && !IsPlayer)
        {
            Person self = GameManager.AIManager.MoodyMask.PplAndMasks.GetPerson(Name);

            if (CurrentRule != null && ActionStartTime + CurrentRule.ActionToTrigger.Duration > time && !forced)
            {
                CurrentRule.SustainAction(text, self, CurrentRule.SelfOther[self].Person, CurrentRule);
            }
            else
            {
                Rule _rule = self.GetAction(time);

                if (_rule.ActionToTrigger.Name.ToLower() != "empty")
                {
                    CurrentRule = _rule;
                    ActionStartTime = time;

                    _rule.DoAction(text, self, _rule.SelfOther[self].Person, _rule);
                }
                else
                {
                    CurrentRule = null;
                }
            }
        }
    }
}