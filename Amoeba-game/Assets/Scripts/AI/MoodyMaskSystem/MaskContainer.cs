using System;
using System.Collections.Generic;
using System.Linq;


namespace NMoodyMaskSystem
{
    public class MaskCont
    {
        public Dictionary<string, Mask> InstMasks = new Dictionary<string, Mask>();
        public Dictionary<string, Rule> InstRules = new Dictionary<string, Rule>();

        /// <summary>
        /// </summary>
        /// <param name="n">name</param>
        /// <param name="mTyp">maskType</param>
        /// <param name="mOvly">maskOverlay</param>
        public void CreateNewMask(string n, TypeMask mTyp, Overlay mOvly) 
        {
            n = n.ToLower();

            Mask newMask = new Mask(mTyp, mOvly, n);

            if(newMask != null && !(InstMasks.ContainsKey(n)))
            {
                InstMasks.Add(n, newMask);
            }
            else if (newMask != null)
            {
                System.Console.WriteLine("Error: Mask with name '" + n + "' Already Exists.");
            }
            else
            {
                System.Console.WriteLine("Error: Mask not created with CreateNewMask.");
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="mN">maskName</param>
        /// <returns></returns>
        public Mask GetMask(string mN) 
        {
            mN.ToLower();

			if(InstMasks.ContainsKey(mN))
			{
				return InstMasks[mN];
            }
            
            return null;
        }

        /// <summary>
        /// </summary>
        /// <param name="rN">ruleName</param>
        /// <param name="posA">possibleAction</param>
        /// <param name="rCon">ruleConditioner</param>
        /// <param name="rPrf">rulePreference</param>
        /// <param name="visClc">visibilityCalculator</param>
        public void CreateNewRule(string rN, MAction posA, RuleInfoCont rInfoCont)
        {
            rN = rN.ToLower();

            if (!InstRules.Keys.Contains(rN))
            {
                InstRules.Add(rN, new Rule(rN.ToLower(), posA, rInfoCont));
            }
            else
            {
                System.Console.WriteLine("Warning: Rule with name '" + rN + "' Already exists. Not adding rule.");
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="rN">ruleName</param>
        /// <returns></returns>
        public Rule FindRule(string rN)
        {
            rN = rN.ToLower();

            if (InstRules.Keys.Contains(rN))
            {
                return InstRules[rN];
            }

            return null;
        }

        /// <summary>
        /// </summary>
        /// <param name="rN">ruleName</param>
        /// <param name="posR">possibleRules</param>
        public void AddPossibleRulesToRule(string rN, List<Rule> posR)
        {
            rN = rN.ToLower();

            InstRules[rN].RulesThatMightHappen.AddRange(posR);
        }

        /// <summary>
        /// </summary>
        /// <param name="mN">maskName</param>
        /// <param name="rN">ruleName</param>
        /// <param name="rlN">roleName</param>
        /// <param name="str">strength</param>
        /// <param name="posR">possibleRules</param>
        public void AddRuleToMask(string mN, string rN, string rlN, float str, List<Rule> posR = null)
        {
            mN = mN.ToLower();
            rN = rN.ToLower();
            rlN = rlN.ToLower();

            int roleIndex = GetMaskRoleIndex(mN, rlN);

			if(roleIndex > -1 && InstMasks[mN].Roles.Count > roleIndex)
            {
                Rule rule = FindRule(rN);

                if(rule != null)
                {
                    rule.Role = rlN;
                    rule.SetRuleStrength(str);

                    if(posR != null)
                    {
                        foreach(Rule possibleRule in posR)
                        {
                            rule.RulesThatMightHappen.Add(possibleRule);
                        }
                    }

                    InstMasks[mN].AddRule(rN, rule);
                }
            }
            else
            {
                System.Console.WriteLine("Error: role did not exist, choose other role.");
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="mN">maskName</param>
        /// <param name="rN">ruleName</param>
        public bool RemoveRuleFromMask(string mN, string rN)
        {
            mN = mN.ToLower();

			if (InstMasks.ContainsKey (mN) && InstMasks [mN].RemoveRule (rN.ToLower())) {
				return true;
			}

			return false;
        }

        /// <summary>
        /// </summary>
        /// <param name="mN">maskName</param>
        /// <param name="newRlN">newRoleName</param>
        public void AddRoleToMask(string mN, string newRlN)
        {
            mN = mN.ToLower();
            newRlN = newRlN.ToLower();

            InstMasks[mN].AddRole(newRlN);
        }

        /// <summary>
        /// </summary>
        /// <param name="mN">maskName</param>
        /// <param name="i"></param>
        /// <returns></returns>
        public string GetMaskRole(string mN, int i)
        {
            mN = mN.ToLower();

            if(i < InstMasks[mN].Roles.Count)
            {
                return InstMasks[mN].Roles[i];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="mN">maskName</param>
        /// <param name="rlN">roleName</param>
        /// <returns></returns>
        public int GetMaskRoleIndex(string mN, string rlN)
        {
            mN = mN.ToLower();
            rlN = rlN.ToLower();

			if(InstMasks.ContainsKey(mN))
			{
				return InstMasks[mN].FindRole(rlN);
			}

			return -1;
        }

        /// <summary>
        /// </summary>
        /// <param name="rN">ruleName</param>
        /// <returns></returns>
		public Rule GetRule(string rN){
			rN = rN.ToLower ();
			foreach (Rule r in InstRules.Values) {
				if(r.RuleName == rN){
					return r;
				}
			}
            System.Console.WriteLine("Rule " + rN + " doesn't exist. Spelling error?");
			return null;
		}

    }
}