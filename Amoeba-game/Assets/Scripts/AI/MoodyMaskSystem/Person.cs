using System.Collections.Generic;


namespace NMoodyMaskSystem
{
    public class Person
    {
        public Link SelfPerception;
        public List<Link> InterPersonal;
        public List<Link> Culture;
        public Overlay AbsTraits;
		public Dictionary<MoodTypes, float> Moods = new Dictionary<MoodTypes, float> ();
		public List<Opinion> Opinions = new List<Opinion> ();
        public MoodyMaskSystem MoodyMask;
#pragma warning disable 0649
        List<MAction> _notPosActions;
#pragma warning restore 0649
        public static Person Empty = new Person("none");
        public string Name;
        
        public Person(string name)
        {
            Name = name;
        }


        public Person(string name, Link selfPer, List<Link> interpers, List<Link> culture, MoodyMaskSystem relationSystem)
        {
            Name = name;
            SelfPerception = selfPer;
            InterPersonal = interpers;
            Culture = culture;
            MoodyMask = relationSystem;
        }


		public float CalculateRelation(Person person)
		{
			return 0;
		}


        public List<Link> GetLinks(TypeMask type)
        {
            if(type == TypeMask.culture)
            {
                return Culture;
            }
            else if(type == TypeMask.interPers)
            {
                return InterPersonal;
            }
            else
            {
                List<Link> go = new List<Link>();
                go.Add(SelfPerception);

                return go;
            }
        }


        //TODO: make this work.
        public void AddRoleRefToLink(TypeMask type, Mask maskRef, string role, Person refe, float lvlOfInfl)
        {
            if (type == TypeMask.selfPerc)
            {
                System.Console.WriteLine("Error: selfPersonMask does not contain roleRefs. Not adding RoleRef.");
            }
            else if (type == TypeMask.interPers)
            {
                int index = InterPersonal.FindIndex(x => x.RoleMask == maskRef);

                if (index < 0)
                    System.Console.WriteLine("Warning: link doesn't exist. Not Adding roleref.");
                else
                    InterPersonal[index].AddRoleRef(role, lvlOfInfl, refe);
            }
            else
            {
                int index = Culture.FindIndex(x => x.RoleMask == maskRef);

                if (index < 0)
                    System.Console.WriteLine("Warning: link doesn't exist. Not Adding roleref.");
                else
                    Culture[index].AddRoleRef(role, lvlOfInfl, refe);
            }
        }


        public void AddLink(TypeMask type, Link newLink) 
        {
            if(type == TypeMask.selfPerc && SelfPerception != null)
            {
                System.Console.WriteLine("Error: selfPersonMask already exists. Not adding Mask.");
            }
            else if(type == TypeMask.interPers)
            {
                int index = InterPersonal.FindIndex(x => x.RoleMask == newLink.RoleMask);

                if (index < 0)
                    InterPersonal.Add(newLink);
                else
                    InterPersonal[index].AddRoleRef(newLink.RoleRefs);
            }
            else
            {
                int index = Culture.FindIndex(x => x.RoleMask == newLink.RoleMask);

                if (index < 0)
                    Culture.Add(newLink);
                else
                    Culture[index].AddRoleRef(newLink.RoleRefs);
            }
        }


        //TODO: make this function take in mask and remove people and/or roles associated.
        public void RemoveRoleRef(TypeMask type, Mask maskRef, Person _ref = null, string role = "")
        {
            if (type == TypeMask.selfPerc)
            {
                System.Console.WriteLine("Error: selfPersonMask does not contain roleRefs. Not removing RoleRef.");
            }
            else if (type == TypeMask.interPers)
            {
                int index = InterPersonal.FindIndex(x => x.RoleMask == maskRef);

                if (index < 0)
                    InterPersonal[index].RemoveRoleRef(role, _ref);
                else
                    System.Console.WriteLine("Warning: link doesn't exist. Not Adding roleref.");
            }
            else
            {
                int index = Culture.FindIndex(x => x.RoleMask == maskRef);

                if (index < 0)
                    System.Console.WriteLine("Warning: link doesn't exist. Not Adding roleref.");
                else
                    Culture[index].RemoveRoleRef(role, _ref);
            }
        }


        public void RemoveLink(TypeMask type, Link oldLink)
        {
            if (type == TypeMask.selfPerc)
            {
                System.Console.WriteLine("Error: Cannot remove selfPersonMask.");
            }
            else if (type == TypeMask.interPers)
            {
                InterPersonal.Remove(oldLink);
            }
            else
            {
                Culture.Remove(oldLink);
            }
        }


        public float ReactMemory = 10f;


        public Rule GetAction(float time, Dictionary<Rule, float> rulePreferenceModifiers) 
        {
            List<PosAItem> posAction = new List<PosAItem>();

            for (int i = MoodyMask.HistoryBook.Count - 1; i >= 0; i--)
            {
                HistoryItem item = MoodyMask.HistoryBook[i];

                if (item.GetTime() < time - ReactMemory)
                {
                    break;
                }

                if (item.HasReacted(this) || item.GetDirect() != this)
                {
                    continue;
                }

                Person subject = item.GetSubject();

                if (subject.Name != Name)
                {
                    foreach (Rule rule in item.GetRule().RulesThatMightHappen)
                    {
                        int index = posAction.FindIndex(x => x.Action == rule.ActionToTrigger);

                        if (index < 0)
                        {
                            posAction.Add(new PosAItem(rule.ActionToTrigger, subject));
                        }
                        else if (!posAction[index].ReactToPeople.Contains(subject))
                        {
                            posAction[index].ReactToPeople.Add(subject);
                        }
                    }
                }
            }

            if (posAction.Count < 1)
            {
                posAction = null;
            }

            AListCont aCont = new AListCont(_notPosActions, posAction);

            
            RuleAndStr chosenAction = SelfPerception.ActionForLink(aCont, this, AbsTraits, rulePreferenceModifiers);

            if (InterPersonal != null)
            {
                foreach (Link curLink in InterPersonal)
                {
                    RuleAndStr curAction = curLink.ActionForLink(aCont, this, AbsTraits, rulePreferenceModifiers);

                    if (curAction.StrOfAct > chosenAction.StrOfAct)
                    {
                        chosenAction = curAction;
                    }
                }
            }

            if (Culture != null)
            {
                foreach (Link curLink in Culture)
                {
                    RuleAndStr curAction = curLink.ActionForLink(aCont, this, AbsTraits, rulePreferenceModifiers);

                    if (curAction.StrOfAct > chosenAction.StrOfAct)
                    {
                        chosenAction = curAction;
                    }
                }
            }

			return chosenAction.ChosenRule;
        }
        

        public Rule GetRule(string actionName) 
		{ 
			foreach(Rule r in SelfPerception.RoleMask.Rules.Values){
				if(r.ActionToTrigger.Name.ToLower() == actionName){
					return r;
				}
			}

			foreach(Link curLink in InterPersonal)
			{
				foreach(Rule r in curLink.RoleMask.Rules.Values){
					if(r.ActionToTrigger.Name.ToLower() == actionName){
						return r;
					}
				}
			}
			
			foreach (Link curLink in Culture)
			{
				foreach(Rule r in curLink.RoleMask.Rules.Values){
					if(r.ActionToTrigger.Name.ToLower() == actionName){
						return r;
					}
				}
			}

            System.Console.WriteLine("Error in GetRule from Person. Rule not found. Check spelling. Returning Empty.");
			
			return Rule.Empty;
		}


        public float CalculateTraitType(TraitTypes traitType)
        {
            float baseVal = AbsTraits.Traits[traitType].GetTraitValue();

            List<Person> activePeople = MoodyMask.CreateActiveListsList();

            foreach(Link link in InterPersonal)
            {
                foreach (Person person in link.GetRoleRefPpl())
                {
                    if (activePeople.Contains(person))
                    {
                        float go = link.RoleMask.MaskOverlay.Traits[traitType].GetTraitValue() * GetLvlOfInflToPerson(person);
                        baseVal += Calculator.UnboundAdd(go, baseVal);
                        break;
                    }
                }
            }

            foreach(Link link in Culture)
            {
				float go = link.RoleMask.MaskOverlay.Traits[traitType].GetTraitValue() * GetLvlOfInflToPerson();
                baseVal += Calculator.UnboundAdd(go, baseVal);
            }

            return baseVal;
        }


		public float GetOpinionValue(TraitTypes traitType, Person pers){
			foreach(Opinion o in Opinions ){
				if(o.Pers == pers && o.Trait == traitType){
					return o.Value;
				}
			}
            System.Console.WriteLine("Error in GetOpinionValue. Did not find person "+pers.Name+" or trait "+traitType+". Check spelling. Returning 0.0");
			return 0.0f;
		}


		public void SetOpinionValue(TraitTypes traitType, Person pers, float valToAdd){
			if (Opinions.Exists(x => x.Trait == traitType && x.Pers == pers)){
                foreach (Opinion o in Opinions){
                    if (o.Pers == pers && o.Trait == traitType){
                        o.Value = valToAdd;
                        return;
                    }
                }
            }
            else{
                Opinions.Add(new Opinion(traitType, pers, valToAdd));
            }
		}


		public void AddToOpinionValue(TraitTypes traitType, Person pers, float valToAdd){
            if (Opinions.Exists(x => x.Trait == traitType)){
                foreach (Opinion o in Opinions){
                    if (o.Pers == pers && o.Trait == traitType){
                        o.Value += Calculator.UnboundAdd(valToAdd, o.Value);
                        return;
                    }
                }
            }
            else{
                Opinions.Add(new Opinion(traitType, pers, valToAdd));
            }
		}
        

        public void CreateOpinion(Person pers, float NiceNasty, float CharitableGreedy, float HonestFalse, bool shouldOverride = false)
        {
            List<Opinion> curOpininon = Opinions.FindAll(x => x.Pers == pers);

            if(curOpininon.Count == 0)
            {
                Opinions.Add(new Opinion(TraitTypes.CharitableGreedy, pers, CharitableGreedy));
                Opinions.Add(new Opinion(TraitTypes.NiceNasty, pers, NiceNasty));
                Opinions.Add(new Opinion(TraitTypes.HonestFalse, pers, HonestFalse));
            }
            else if(shouldOverride)
            {
                foreach(Opinion opinion in Opinions)
                {
                    opinion.Value = (opinion.Trait == TraitTypes.CharitableGreedy) ? CharitableGreedy : ((opinion.Trait == TraitTypes.HonestFalse) ? HonestFalse : NiceNasty);
                }
            }
            else
            {
                System.Console.WriteLine("Error in CreateOpinion for " + Name + ". Opininon towards " + pers.Name + " already exists, but function not told to override. Not updating opinion");
            }

        }


		public bool CheckRoleName(string role, Person pers = null){

			if(pers == null){
				foreach (Link l in InterPersonal) {
					if (pers == null) {
						pers = Empty;
					}
					if(l.RoleRefs.ContainsKey(pers)){
						foreach(Person curPers in l.RoleRefs.Keys){
							if(l.RoleRefs[curPers].ContainsKey(role)){
								return true;
							}
						}
					}
				}
					foreach (Link l in Culture) {
						if (pers == null) {
							pers = Empty;
						}
						if(l.RoleRefs.ContainsKey(pers)){
							foreach(Person curPers in l.RoleRefs.Keys){
								if(l.RoleRefs[curPers].ContainsKey(role)){
									return true;
								}
							}
						}
					}
				}

				foreach (Link l in InterPersonal) {
					if (pers == null) {
						pers = Empty;
					}
					if(l.RoleRefs.ContainsKey(pers)){
						if(l.RoleRefs[pers].ContainsKey(role)){
							return true;
						}
					}
				}
				foreach (Link l in Culture) {
					if (pers == null) {
						pers = Empty;
					}
					if(l.RoleRefs.ContainsKey(pers)){
						if(l.RoleRefs[pers].ContainsKey(role)){
							return true;
						}
					}
				}
				return false;
		}


		public float GetLvlOfInflToPerson(Person pers = null){
			foreach (Link l in InterPersonal) {
				if (pers == null) {
					pers = Empty;
				}
				if(l.RoleRefs.ContainsKey(pers)){
					foreach(string s in l.RoleRefs[pers].Keys){
						return l.RoleRefs[pers][s];
					}
				}
			}
            System.Console.WriteLine("ERROR. Did not find person to getlvlofInfl from. In Person.cs");
			return 0.0f;
		}
    }
}