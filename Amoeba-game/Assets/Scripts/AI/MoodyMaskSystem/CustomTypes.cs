using System;
using System.Collections.Generic;


namespace NMoodyMaskSystem
{
    public enum TypeMask
    {
        selfPerc,
        interPers,
        culture
    }

    //good = 1, bad = -1. e.g. hap = 1, sad = -1.
	public enum MoodTypes
	{
		hapSad,
		arousDisgus,
		angryFear,
		energTired
	}

    public struct RuleAndStr
    {
        public Rule ChosenRule;
        public float StrOfAct;
    };

    public struct PersonAndPreference
    {
        public Person Person;
        public float Pref;

        /// <summary>
        /// </summary>
        /// <param name="p">person</param>
        /// <param name="prf">preference</param>
        public PersonAndPreference(Person p, float prf)
        {
            Person = p;
            Pref = prf;
        }
    }

    //good = 1, bad = -1. e.g. nice = 1, nasty = -1.
    public enum TraitTypes
    {
        NiceNasty,
        CharitableGreedy,
        HonestFalse
    }

    [Serializable]
	public class Opinion
	{
        //TODO: Make an opinion contain all 3 opinions instead of 1.
		public TraitTypes Trait;
		public Person Pers;
		public float Value;

        /// <summary>
        /// </summary>
        /// <param name="trt">trait</param>
        /// <param name="p">person</param>
        /// <param name="vl">value</param>
		public Opinion(TraitTypes trt, Person p, float vl){
			Trait = trt;
			Pers = p;
			Value = vl;
		}
	};

    public struct MaskAdds
    {
        public string Role;
        public string Mask;
        public float LvOInf;

        /// <summary>
        /// </summary>
        /// <param name="rol">role</param>
        /// <param name="m">mask</param>
        /// <param name="lvOInf">levelOfInfluence</param>
        public MaskAdds(string rol, string m, float lvOInf)
        {
            Role = rol.ToLower();
            Mask = m.ToLower();
            LvOInf = lvOInf;
        }
    };

    public class PosAItem
    {
        public MAction Action;
        public List<Person> ReactToPeople;

        /// <summary>
        /// </summary>
        /// <param name="a">action</param>
        /// <param name="rTp">peopleToReactTo</param>
        public PosAItem(MAction a, Person rTp)
        {
            Action = a;
            ReactToPeople = new List<Person>();
            ReactToPeople.Add(rTp);
        }

        /// <summary>
        /// </summary>
        /// <param name="a">action</param>
        /// <param name="rTp">peopleToReactTo</param>
        public PosAItem(MAction a, List<Person> rTp)
        {
            Action = a;
            ReactToPeople = rTp;
        }
    }

    public struct MiscTraitCont
    {
        public float Rat;
        public float Mor;
        public float Imp;
        public float Abi;

        public MiscTraitCont(float rat, float mor, float imp, float abi)
        {
            Rat = rat;
            Mor = mor;
            Imp = imp;
            Abi = abi;
        }
    };

    public struct RuleInfoCont
    {
        public static RuleInfoCont Empty = new RuleInfoCont(null, null, null);

        public RuleConditioner RuleCondition;
        public RulePreference RulePreference;
        public VisibilityCalculator VisCalc;

        public RuleInfoCont(RuleConditioner rCond, RulePreference rPref, VisibilityCalculator visCalc)
        {
            RuleCondition = rCond;
            RulePreference = rPref;
            VisCalc = visCalc;
        }
    };

    public struct AListCont
    {
        public List<MAction> NotPosActions;
        public List<PosAItem> PosActions;

        public AListCont(List<MAction> notPosActions, List<PosAItem> posActions)
        {
            NotPosActions = notPosActions;
            PosActions = posActions;
        }
    };

    public delegate void ActionInvoker(UnityEngine.UI.Text text, Person subject, Person direct, Person[] indiPpl = null,  object[] misc = null);

	public delegate bool RuleConditioner(Person self, Person other, Person[] indiPpl = null);

    public delegate float RulePreference(Person self, Person other, float preferenceModifier);

    public delegate float VisibilityCalculator(object[] misc = null);
}