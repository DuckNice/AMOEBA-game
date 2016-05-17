using System;
using System.Collections.Generic;
using System.Linq;


namespace NMoodyMaskSystem
{
    public class Mask
    {
		string _maskName;
        TypeMask _maskType;
        public Dictionary<string, Rule> Rules;
        public List<string> Roles;
        public Overlay MaskOverlay;

        /// <summary>
        /// </summary>
        /// <param name="mTyp">maskType</param>
        /// <param name="mOvly">maskOverlay</param>
        /// <param name="n">name</param>
        public Mask(TypeMask mTyp, Overlay mOvly, string n)
        {
			_maskName = n;
            this._maskType = mTyp;
            Roles = new List<string>();
            Rules = new Dictionary<string,Rule>();
            MaskOverlay = mOvly;
        }

        /// <summary>
        /// </summary>
        /// <param name="rRlN">ruleRoleName</param>
        /// <param name="r">rule</param>
        public void AddRule(string rRlN, Rule r)
        {
            rRlN = rRlN.ToLower();

            Rules.Add(rRlN, r.HalfDeepCopy());
        }

        /// <summary>
        /// </summary>
        /// <param name="rN">ruleName</param>
        /// <returns></returns>
        public bool RemoveRule(string rN)
        {
            rN = rN.ToLower();

            if(Rules.Remove(rN))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// </summary>
        /// <param name="n">name</param>
        public void AddRole(string n)
        {
            n = n.ToLower();

            if(!Roles.Contains(n))
            {
                Roles.Add(n);
            }
        }
        
        /// <summary>
        /// </summary>
        /// <param name="rlN">roleName</param>
        /// <returns></returns>
        public int FindRole(string rlN) 
        {
            rlN = rlN.ToLower();

            return Roles.FindIndex(x => x == rlN);
        }
        

        public RuleAndStr CalculateActionToUse(AListCont actions, Person self, Overlay absTraits, Dictionary<Person, Dictionary<string, float>> roleReferences, Dictionary<Rule, float> rulePreferenceModifiers)
        {
            RuleAndStr chosenRule = new RuleAndStr();
			chosenRule.ChosenRule = Rule.Empty;
            chosenRule.StrOfAct = -9999f;

            foreach(Rule rule in Rules.Values.ToList())
            {
                if (actions.NotPosActions != null && actions.NotPosActions.Contains(rule.ActionToTrigger))
                    continue;

                List<Person> posPeople = new List<Person>();

                bool reaction = false;
                
                if(actions.PosActions != null && actions.PosActions.Count > 0){
                    int index = actions.PosActions.FindIndex(x => x.Action == rule.ActionToTrigger);
                    reaction = true;

                    if (index >= 0)
                        foreach (Person person in actions.PosActions[index].ReactToPeople)
                            posPeople.Add(person);
                    else
                        continue;
                }

                if (roleReferences != null && roleReferences.Count > 0)
                {
                    if (reaction){
                        for (int i = posPeople.Count - 1; i >= 0; i--){
                            if (!roleReferences[Person.Empty].ContainsKey(rule.Role) && (!roleReferences.ContainsKey(posPeople[i]) || !roleReferences[posPeople[i]].ContainsKey(rule.Role))){
                                posPeople.RemoveAt(i);
                            }
                        }
                    }
                    else
                    {
                        if (!roleReferences[Person.Empty].ContainsKey(rule.Role)){
                            reaction = true;

                            foreach (Person person in roleReferences.Keys){
                                if (roleReferences[person].ContainsKey(rule.Role) && person != Person.Empty){
                                    posPeople.Add(person);
                                }
                            }
                        }
                    }
                }
                else
                {
                    System.Console.WriteLine("Warning: No roleRefs were passed to mask '" + _maskName + "'.");
                    
                    break;
                }

				//debug.Write("Checking condition "+rule.ruleName+"  "+self.name);
				
                if(rule.Condition(self, posPeople, reaction, ((rulePreferenceModifiers.ContainsKey(rule)) ? rulePreferenceModifiers[rule] : 0)))
				{
                    float maskCalculation = -99999999999f;

					if (    roleReferences != null && 
                            roleReferences.ContainsKey(rule.SelfOther[self].Person) && 
                            roleReferences[rule.SelfOther[self].Person].ContainsKey(rule.Role)){
						maskCalculation = Calculator.CalculateRule(absTraits, rule, rule.RulesThatMightHappen, 
                                                                    roleReferences[rule.SelfOther[self].Person][rule.Role]);
					} 
					else if (roleReferences != null && 
                            roleReferences.ContainsKey(Person.Empty) && 
                            roleReferences[Person.Empty].ContainsKey(rule.Role)){
						maskCalculation = Calculator.CalculateRule(absTraits, rule, rule.RulesThatMightHappen, 
                                                                    roleReferences[Person.Empty][rule.Role]);
					}
					else{
                        System.Console.WriteLine("Did not calculate "+rule.RuleName+". Maybe rolerefs did not contain person to check for: "+rule.SelfOther[self].Person.Name);
						continue;
					}


                    System.Console.WriteLine("Calculating "+rule.ActionToTrigger.Name+" to "+rule.SelfOther[self].Person.Name+" in "+_maskName);
					
                    float newActionStrength = maskCalculation + rule.SelfOther[self].Pref;

                    System.Console.WriteLine(maskCalculation+"  (+)  "+rule.SelfOther[self].Pref+"  =  "+newActionStrength);
					
                    if (newActionStrength > chosenRule.StrOfAct)
					{
						chosenRule.StrOfAct = newActionStrength;
						chosenRule.ChosenRule = rule;
					}
				}
			}

			//debug.Write ("RETURNING " + chosenAction.chosenRule.ruleName);
			return chosenRule;
		}
		
		public TypeMask GetMaskType(){ return _maskType; }
		public string GetMaskName(){ return _maskName; }
    }
}