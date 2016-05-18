using System;
using System.Collections.Generic;
using System.Linq;


namespace NMoodyMaskSystem
{
    public partial class MoodyMaskSystem
    {
        public PersCont PplAndMasks = new PersCont();
		//public HistoryContainer historyFunctions = new HistoryContainer(0.5f, 0.002f);
        public Dictionary<string, MAction> PosActions = new Dictionary<string, MAction>();
        public Dictionary<string, List<Person>> UpdateLists = new Dictionary<string, List<Person>>();
        public Dictionary<string, List<Person>> ActiveLists = new Dictionary<string, List<Person>>();
        public List<HistoryItem> HistoryBook = new List<HistoryItem>();
        public List<string> PlayerName = new List<string>();


        public MoodyMaskSystem()
        {
            MAction.MoodyMask = this;
        }


        public bool HistoryBookContains(MAction action) 
        {
            return HistoryBook.Exists(x => x.GetAction() == action);
        }


        public List<Person> CreateActiveListsList()
        {
            List<Person> list = new List<Person>();

            foreach(List<Person> people in ActiveLists.Values)
            {
                foreach (Person person in people)
                {
                    if (!list.Contains(person))
                    {
                        list.Add(person);
                    }
                }
            }

            return list;
        }


        public List<Person> GetUpdateList(string name)
        {
            name = name.ToLower().Trim();
            if (UpdateLists.ContainsKey(name))
            {
                return UpdateLists[name];
            }
            else
            {
                return null;
            }
        }


        public List<string> CreateActiveListsListNames()
        {
            List<string> list = new List<string>();

            foreach (List<Person> people in ActiveLists.Values)
            {
                foreach (Person person in people)
                {
                    if (!list.Contains(person.Name))
                    {
                        list.Add(person.Name);
                    }
                }
            }

            return list;
        }


        public void CreateNewMask(string nameOfMask, float[] traits = null, TypeMask maskType = TypeMask.interPers, string[] roles = null) 
        {
            nameOfMask = nameOfMask.ToLower().Trim();

            List<Trait> newTraits = new List<Trait>();
            
            for(int i = 0; i < Enum.GetNames(typeof(TraitTypes)).Length; i++)
            {
                float insertTrait = 0.0f;

                if(i < traits.Length && traits[i] >= -1.0f && traits[i] <= 1.0f)
                    insertTrait = traits[i];

                newTraits.Add(new Trait((TraitTypes)i, insertTrait));
            }

            PplAndMasks.CreateNewMask(nameOfMask, maskType, new Overlay(newTraits, 0, 0, 0, 0));

            if(roles != null)
            {
                foreach(string role in roles)
                {
                    if(role != "")
                    {
                        PplAndMasks.AddRoleToMask(nameOfMask, role);
                    }
                }
            }
        }


		public Mask GetMask(string name)
		{
			return PplAndMasks.GetMask (name);
		}


        public void CreateNewRule(string ruleName, string actName, float selfGain, float againstGain, RuleConditioner ruleCondition = null, RulePreference rulePreference = null, VisibilityCalculator visCalc = null)
        {
           ruleName = ruleName.ToLower().Trim();
           actName = actName.ToLower().Trim();
           
           if(PplAndMasks.FindRule(ruleName) == null){
               PplAndMasks.CreateNewRule(ruleName, PosActions[actName], selfGain, againstGain, new RuleInfoCont(ruleCondition, rulePreference, visCalc));
           }
           else
           {
                System.Console.WriteLine("Warning: Rule with name '" + ruleName + "' Already exists. Not adding rule.");
           }
        }


		public void CreateNewPerson(MaskAdds selfMask, List<MaskAdds> cults, List<MaskAdds> intPpl, float rational, float moral, float impulse, float[] traits = null, float[] moods = null)
		{
			List<Trait> newTraits = new List<Trait>();
			
			for(int i = 0; i < Enum.GetNames(typeof(TraitTypes)).Length; i++)
			{
				float insertTrait = 0.0f;
				
				if(i < traits.Length && traits[i] >= -1.0f && traits[i] <= 1.0f)
					insertTrait = traits[i];
				
				newTraits.Add(new Trait((TraitTypes)i, insertTrait));
			}


			Dictionary<MoodTypes, float> newMoods = new Dictionary<MoodTypes, float>();
			
			for(int i = 0; i < Enum.GetNames(typeof(MoodTypes)).Length; i++)
			{
				float insertMood = 0.0f;

				if(moods != null && i < moods.Length && moods[i] >= -1.0f && moods[i] <= 1.0f)
					insertMood = moods[i];
				
				newMoods.Add((MoodTypes)i, insertMood);
			}


            Link selfPersMask = new Link(selfMask.Role, PplAndMasks.GetMask(selfMask.Mask), selfMask.LvOInf);

            List<Link> newCults = new List<Link>();

            foreach(MaskAdds cult in cults)
            {
				newCults.Add(new Link(cult.Role, PplAndMasks.GetMask(cult.Mask), cult.LvOInf));
            }

            List<Link> newIntPpl = new List<Link>();

            foreach(MaskAdds intPers in intPpl)
            {
                newIntPpl.Add(new Link(intPers.Role, PplAndMasks.GetMask(intPers.Mask), intPers.LvOInf));
            }

            Person person = new Person(selfMask.Mask, selfPersMask, newIntPpl, newCults, this);
			Overlay persOverlay = new Overlay (newTraits, rational, moral, impulse, 1);
			person.AbsTraits = persOverlay;
			person.Moods = newMoods;

            PplAndMasks.CreateNewPerson(selfMask.Mask, person);
        }

        //TODO: No gameStateManager here.
        public void DidAction(MAction action, Person subject, Person direct, Rule rule)
        {
			HistoryBook.Add(new HistoryItem(action, subject, direct, GameManager.Time, rule));
		}

        
        public Person GetPerson(string name)
        {
            return PplAndMasks.GetPerson(name.ToLower().Trim());
        }

        public List<Person> GetAllPeople()
        {
            return PplAndMasks.People.Values.ToList();
        }
    }
}