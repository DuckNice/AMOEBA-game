using System;
using System.Collections.Generic;
using System.Linq;

namespace NMoodyMaskSystem
{
    public class Link
    {
        public Dictionary<Person, Dictionary<string, float>> RoleRefs = new Dictionary<Person, Dictionary<string, float>>();
        public Mask RoleMask;

        /// <summary>
        /// </summary>
        /// <param name="gRlNm">generalRoleName</param>
        /// <param name="rlM">roleMask</param>
        /// <param name="lvOInf">levelOfInfluence</param>
        public Link(string gRlNm, Mask rlM, float lvOInf) 
        {
            
            RoleRefs.Add(Person.Empty, new Dictionary<string, float>());
            RoleRefs[Person.Empty].Add(gRlNm, lvOInf);
            RoleMask = rlM;
        }

        /// <summary>
        /// </summary>
        /// <param name="rlRfs">roleReferences</param>
        public void AddRoleRef(Dictionary<Person, Dictionary<string, float>> rlRfs)
        {
            foreach(KeyValuePair<Person, Dictionary<string, float>> roleref in rlRfs)
            {
                foreach (KeyValuePair<string, float> influence in roleref.Value)
                    AddRoleRef(influence.Key, influence.Value, roleref.Key);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="rlNm">roleName</param>
        /// <param name="lvOInf">levelOfInfluence</param>
        /// <param name="rlRf">roleReference</param>
        public void AddRoleRef(string rlNm, float lvOInf, Person rlRf = null)
        {
            if(rlRf == null)
                rlRf = Person.Empty;

            if (!RoleRefs.Keys.ToList().Exists(x => x.Name == rlRf.Name))
            {
                RoleRefs.Add(rlRf, new Dictionary<string, float>());
                RoleRefs[rlRf].Add(rlNm, lvOInf);
            }
            else
            {
                if (!RoleRefs[rlRf].ContainsKey(rlNm))
                    RoleRefs[rlRf].Add(rlNm, lvOInf);
                
                else
                    System.Console.WriteLine("Warning: roleName already associated with this character in link. Not adding role reference.");
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="rlNm">roleName</param>
        /// <param name="rlRf">roleReference</param>
        public void RemoveRoleRef(string rlNm, Person rlRf = null)
        {
            if (rlRf != null)
            {
                if (RoleRefs.Keys.ToList().Exists(x => x.Name == rlRf.Name))
                {
                    if (!string.IsNullOrEmpty(rlNm))
                        RoleRefs[rlRf].Remove(rlNm);
                    else
                        RoleRefs.Remove(rlRf);
                }
                else
                {
                    System.Console.WriteLine("Warning: roleName not associated with this character in link. Not removing role reference.");
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(rlNm))
                    RoleRefs[Person.Empty].Remove(rlNm);
                else
                    System.Console.WriteLine("Warning: roleName not associated with general in link. Not removing role reference.");
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="nPosA">notPossibleActions</param>
        /// <param name="posA">possibleActions</param>
        /// <param name="slf">self</param>
        /// <param name="absTraits">Traits</param>
        /// <returns></returns>
        public RuleAndStr ActionForLink(AListCont actions, Person slf, Overlay absTraits, Dictionary<Rule, float> rulePreferenceModifiers = null) 
        {
            RuleAndStr actionToSend;

            try
            {
				actionToSend = RoleMask.CalculateActionToUse(actions, slf, absTraits, RoleRefs, rulePreferenceModifiers);
					//debug.Write ("Trying from link "+self.name+" Maskname: "+ roleMask.GetMaskName() +" Rolename: "+roleName);
            }
            catch(Exception e)
            {
                System.Console.WriteLine("Catching actionForLink error with code: '" + e + "'.");
                actionToSend = new RuleAndStr();
                actionToSend.ChosenRule = Rule.Empty;
                actionToSend.StrOfAct = 0.0f;
            }

			//Console.WriteLine ("From LINK from "+roleName+" ::: " + actionToSend.chosenRule.ruleName);
            return actionToSend;
        }


        public List<Person> GetRoleRefPpl()
        {
            List<Person> roleRefs = RoleRefs.Keys.ToList();
            roleRefs.Remove(Person.Empty);

            return roleRefs;
        }

        /// <summary>
        /// </summary>
        /// <param name="nm">name</param>
        /// <returns></returns>
        public bool RoleRefPersExists(string nm)
        {
            return GetRoleRefPpl().Exists(y => y.Name == nm);
        }

        /// <summary>
        /// </summary>
        /// <param name="rlNm">roleName</param>
        /// <param name="rlRf">roleReference</param>
        /// <returns></returns>
		public float GetlvlOfInfl(string rlNm, Person rlRf = null)
        { 
            if(rlRf == null){
                rlRf = Person.Empty;    
            }
            
            if(RoleRefs.ContainsKey(rlRf) && RoleRefs[rlRf].ContainsKey(rlNm))
            {
                return RoleRefs[rlRf][rlNm]; 
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="inp">input</param>
        /// <param name="rlNm">roleName</param>
        /// <param name="rlRf">roleReference</param>
        /// <returns></returns>
		public bool SetlvlOfInfl(float inp, string rlNm, Person rlRf = null)
        {
            if(rlRf == null){
                rlRf = Person.Empty;
            }
            else if(RoleRefs.ContainsKey(rlRf) && RoleRefs[rlRf].ContainsKey(rlNm))
            {
                RoleRefs[rlRf][rlNm] = inp;
            }
            else
            {
                System.Console.WriteLine("Warning: Person " + rlRf.Name + " does not exist in link. Not setting lvlOfInfl.");
                return false;
            }

            return true;
        }

        /// <summary>
        /// </summary>
        /// <param name="inp">input</param>
        /// <param name="rlNm">roleName</param>
        /// <param name="rlRf">roleReference</param>
        /// <returns></returns>
		public bool AddToLvlOfInfl(float inp, string rlNm, Person rlRf = null)
        {
            if(RoleRefs[rlRf].ContainsKey(rlNm))
            {
                if (rlRf == null)
                {
                    rlRf = Person.Empty;
                }
                else if (!RoleRefs.ContainsKey(rlRf))
                {
                    System.Console.WriteLine("Warning: Person " + rlRf.Name + " does not exist in link. Not setting lvlOfInfl.");
                    return false;
                }

                RoleRefs[rlRf][rlNm] += Calculator.UnboundAdd(inp, RoleRefs[rlRf][rlNm]);

            }

            return true;
        }
    }
}