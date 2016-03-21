using System;
using System.Collections.Generic;


namespace NMoodyMaskSystem
{
    public class Calculator
    {
        /// <summary>
        /// </summary>
        /// <param name="imp">impulsivity</param>
        /// <param name="abi">ability</param>
        /// <param name="curR">currentRule</param>
        /// <param name="rToTrig">rulesThatWillTrigger</param>
        /// <returns></returns>
        public static float CalculateEgo(float imp, float abi, Rule curR, List<Rule> rToTrig)
        {
            float tempEgo = 1.0f;

            if (rToTrig != null)
            {
                foreach (Rule rule in rToTrig)
                {
                    float visibility = rule.RuleInfoCont.VisCalc();

					tempEgo += rule.GetRuleStrength() * rule.ActionToTrigger.EstimationOfSuccess(abi) * visibility * CalculateGain(rule, false);

                    //probability is just r.strength for now. let's leave it like that for simplicity
					//right now it just check visibility for all people in world, not just the people involved in the action considered.
                }
            }

            tempEgo *= (1 - imp);

            float ego = imp * CalculateGain(curR, true) + tempEgo;

         //   debug.Write("Ego: " + ego);

            return ego;
        }

        /// <summary>
        /// </summary>
        /// <param name="r">rule</param>
        /// <param name="rs">rules</param>
        /// <param name="mInf">maskInfluence</param>
        /// <returns></returns>
        public static float CalculateSuperEgo(Rule r, List<Rule> rs, float mInf)
        {
            float superEgo = 0.0f;

                //own rules morality:
            superEgo += r.GetRuleStrength() * mInf;

                //consequent rules morality:
            if (rs != null)
            {
                foreach (Rule rule in rs)
                {
                    superEgo += rule.GetRuleStrength() * mInf;
                }
            }

			//debug.Write("SuperEgo: " + superEgo);

            return superEgo;
        }

        /// <summary>
        /// </summary>
        /// <param name="rat">rationality</param>
        /// <param name="mor">morality</param>
        /// <param name="imp">impulsivity</param>
        /// <param name="abi">ability</param>
        /// <param name="r">rule</param>
        /// <param name="rToTrig">rulesThatWillTrigger</param>
        /// <param name="mInf">maskInfluence</param>
        /// <returns></returns>
        public static float CalculateRule(Overlay absTraits, Rule r, List<Rule> rToTrig, float mInf)
        {
			return (CalculateEgo(absTraits._imp, absTraits._abi, r, rToTrig) * absTraits._rat) + (CalculateSuperEgo(r, rToTrig, mInf) * absTraits._mor);
        }

        /// <summary>
        /// </summary>
        /// <param name="r">rule</param>
        /// <param name="sG">selfGain</param>
        /// <returns></returns>
        public static float CalculateGain(Rule r, bool sG)
        {
            return r.ActionToTrigger.GetGain(sG);
        }

        /// <summary>
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="wF">weightingFactor</param>
        /// <returns></returns>
        public static float Blend(float x, float y, float wF)
        {
            float uWeightingFactor = 1 - ((1 - wF) / 2);
            float blend = y * uWeightingFactor + x * (1 - uWeightingFactor);

            return blend;
        }

        /// <summary>
        /// </summary>
        /// <param name="uBndNum">unboundedNumber</param>
        /// <param name="curVl">currentValue</param>
        /// <returns></returns>
        public static float UnboundAdd(float uBndNum, float curVl)
        {
            float dist;
            if (uBndNum > 0)
            {
                dist = Math.Abs((1) - curVl);
            }
            else
            {
                dist = Math.Abs((-1) - curVl);
            }
            return uBndNum * dist;
        }

        /// <summary>
        /// </summary>
        /// <param name="inp">inpact</param>
        /// <returns></returns>
		public static float NegPosTransform(float inp){
			return inp-(1-inp);
		}
    }
}