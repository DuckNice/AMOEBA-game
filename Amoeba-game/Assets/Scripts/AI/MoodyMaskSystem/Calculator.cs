using System;
using System.Collections.Generic;


namespace NMoodyMaskSystem
{
    public class Calculator
    {
        public static float CalculateEgo(float implusivity, float ability, Rule rule, List<Rule> rulesToTrigger)
        {
            float tempEgo = 1.0f;

            if (rulesToTrigger != null)
            {
                foreach (Rule curRule in rulesToTrigger)
                {
                    float visibility = curRule.RuleInfoCont.VisCalc();

					tempEgo += curRule.GetRuleStrength() * curRule.ActionToTrigger.EstimationOfSuccess(ability) * visibility * CalculateGain(curRule, false);

                    //probability is just r.strength for now. let's leave it like that for simplicity
					//right now it just check visibility for all people in world, not just the people involved in the action considered.
                }
            }

            tempEgo *= (1 - implusivity);

            float ego = implusivity * CalculateGain(rule, true) + tempEgo;

            return ego;
        }

        
        public static float CalculateSuperEgo(Rule rule, List<Rule> rules, float maskInfluence)
        {
            float superEgo = 0.0f;

                //own rules morality:
            superEgo += rule.GetRuleStrength() * maskInfluence;

                //consequent rules morality:
            if (rules != null)
            {
                foreach (Rule curRule in rules)
                {
                    superEgo += curRule.GetRuleStrength() * maskInfluence;
                }
            }

            return superEgo;
        }

        
        public static float CalculateRule(Overlay absTraits, Rule rule, List<Rule> rulesToTrigger, float maskInfluence)
        {
			return (CalculateEgo(absTraits._imp, absTraits._abi, rule, rulesToTrigger) * absTraits._rat) + (CalculateSuperEgo(rule, rulesToTrigger, maskInfluence) * absTraits._mor);
        }

        
        public static float CalculateGain(Rule rule, bool selfGain)
        {
            return rule.GetGain(selfGain);
        }

        
        public static float Blend(float x, float y, float weightingFactor)
        {
            float uWeightingFactor = 1 - ((1 - weightingFactor) / 2);
            float blend = y * uWeightingFactor + x * (1 - uWeightingFactor);

            return blend;
        }
        

        public static float UnboundAdd(float unboundNumber, float currentValue)
        {
            float temp = Math.Max(unboundNumber, -1);
            temp = Math.Min(temp, 1);

            float dist= Math.Abs(((unboundNumber > 0) ? 1: -1) - currentValue);

            return unboundNumber * dist;
        }


        public static float AddTowards0(float unboundNumber, float currentValue)
        {
            float temp = Math.Max(unboundNumber, 0);
            temp = Math.Min(temp, 1);

            float dist = Math.Abs((0) - currentValue);
            
            return ((currentValue > 0) ? -1: 1) * temp * dist;
        }

        
		public static float NegPosTransform(float impact){
			return impact-(1-impact);
		}
    }
}