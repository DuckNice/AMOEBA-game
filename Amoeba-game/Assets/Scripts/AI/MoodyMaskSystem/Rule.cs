﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


namespace NMoodyMaskSystem
{
    public class Rule
    {
        public static Rule Empty = new Rule("Empty", MAction.Empty, RuleInfoCont.Empty);

        public string RuleName;
        //	public Dictionary<string, MAction> actionsByRoles; 
        public MAction ActionToTrigger;
        public List<Rule> RulesThatMightHappen = new List<Rule>();
        float _strength = 0.0f;
        public string Role = "none";
        public Dictionary<Person, PersonAndPreference> SelfOther = new Dictionary<Person, PersonAndPreference>();
        public RuleInfoCont RuleInfoCont;        

        public Rule HalfDeepCopy()
        {
            Rule other = (Rule)this.MemberwiseClone();
            other.RuleName = String.Copy(RuleName);
            other.Role = String.Copy(Role);
            other.SelfOther = new Dictionary<Person, PersonAndPreference>();

            return other;
        }
        

        public Rule(string ruleName, MAction act, RuleInfoCont ruleInfoCont)
        {
            RuleName = ruleName;
            ActionToTrigger = act;
            RuleInfoCont = ruleInfoCont;

            if (RuleInfoCont.VisCalc == null)
            {
                RuleInfoCont.VisCalc = new VisibilityCalculator(x => { return 1.0f; });
            }
        }


        public bool Condition(Person self, List<Person> reacters = null, bool reaction = false)
        {
            if (SelfOther.ContainsKey(self)) 
                SelfOther.Remove(self);
            
            List<Person> people = MAction.MoodyMask.PplAndMasks.People.Values.ToList();

            if(people.Contains(self))
                people.Remove(self);

            if (reacters != null && reacters.Count > 0)
            {
                for (int i = 0; i < people.Count; i++)
                {
                    if (!reacters.Contains(people[i]))
                    {
                        people.RemoveAt(i);
                        i--;
                    }
                }
            }
            else if(reaction)
            {
                return false;
            }

            float strength = -10.0f;
            Person personToAdd = self;

            foreach(Person other in people)
            {
                try
                {
                    if (RuleInfoCont.RuleCondition == null || RuleInfoCont.RuleCondition(self, other))
                    {
                        if(RuleInfoCont.RulePreference != null)
                        {
                            float _strength = RuleInfoCont.RulePreference(self, other);
                        
                            if(_strength > strength)
                            {
                                strength = _strength;

                                personToAdd = other;
                            }
                        }
                        else
                        {
                            SelfOther.Add(self, new PersonAndPreference(other, 0.0f));
                            return true;
                        }
                    }
                }
                catch(Exception e)
                {
                    System.Console.WriteLine("Warning: ruleCondition for " + other.Name + " in " + RuleName + " returned and error with code: '" + e + "'. Skipping condition.");
                }
            }

            if (personToAdd != self)
            {
                SelfOther.Add(self, new PersonAndPreference(personToAdd, strength));
                return true;
            }
            return false;
        }


		public void DoAction(Person subject, Person dirObject, Rule rule, Person[] indPpl = null, object[] misc = null){
			ActionToTrigger.DoAction (subject, dirObject,rule, indPpl, misc);
		}

		public void SustainAction(Person subject, Person dirObject, Rule rule, Person[] indPpl = null, object[] misc = null){
			ActionToTrigger.DoSustainAction (subject, dirObject,rule, indPpl, misc);
		}

        public float GetRuleStrength() { return _strength; }
		public void SetRuleStrength(float inp){ _strength = inp;  }
		public void AddToRuleStrength(float inp){ _strength += Calculator.UnboundAdd(inp,_strength);  }
    }
}