using System.Collections.Generic;
using UnityEngine;

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
		public float NiceNasty;
		public float HonestFalse;
		public float CheritableGreedy;
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
	public Rule CurrentRule;
    [HideInInspector]
	public float ActionStartTime;

    
	public void NPCAction(float time)
	{
		if (!GameManager.AIManager.MoodyMask.PlayerName.Contains(Name.ToLower()))
        {
            Person self = GameManager.AIManager.MoodyMask.PplAndMasks.GetPerson(Name);

            if (CurrentRule != null && ActionStartTime + CurrentRule.ActionToTrigger.Duration > time)
            {
                CurrentRule.SustainAction(self, CurrentRule.SelfOther[self].Person, CurrentRule);
            }
            else
            {
                Rule _rule = self.GetAction(time);

                if (_rule.ActionToTrigger.Name.ToLower() != "empty")
                {
                    CurrentRule = _rule;
                    ActionStartTime = time;

                    _rule.DoAction(self, _rule.SelfOther[self].Person, _rule);
                }
                else
                {
                    CurrentRule = null;
                }
            }
        }
    }
}