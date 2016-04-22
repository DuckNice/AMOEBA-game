using UnityEngine;
using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

	//Namespaces
using NMoodyMaskSystem;

public struct ActionInfo
{
    public ActionInvoker Invoker;
    public RuleConditioner RConditioner;
    public RulePreference RPreference;

    public ActionInfo(ActionInvoker invoker, RuleConditioner rConditioner, RulePreference rPreference)
    {
        Invoker = invoker;
        RConditioner = rConditioner;
        RPreference = rPreference;
    }
}

public class NPCCreator 
{
	public static string statsString;    

    public static void CreateMask(MoodyMaskSystem MoodyMask, string maskName)
    {
        try {
            Type mask = Type.GetType(maskName);
            MethodInfo method = mask.GetMethod("CreateMask");

            if (method != null)
            {
                method.Invoke(null, new object[] { MoodyMask });
            }
            else
            {
                Debug.LogError("Error: No mask with name: " + maskName + " was found.");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Warning: creating the mask of type: '" + maskName + "' threw an exception: \n" + ex.ToString());
        }
    }


    public static void CreatePerson(MoodyMaskSystem MoodyMask, string personName, Being.MaskInfo[] masks, Dictionary<string, float> personalRules, float rationality, float morality, float impulsivity, float[] traits, float[] moods)
	{
		List<MaskAdds>  culture = new List<MaskAdds>();

		foreach(Being.MaskInfo mask in masks)
		{
			if(MoodyMask.GetMask(mask.MaskName) == null)
			{
				CreateMask (MoodyMask, mask.MaskName);
				culture.Add (new MaskAdds(mask.Role, mask.MaskName, mask.Strength));
			}
		}

		MoodyMask.CreateNewMask(personName, new float[] { 0.0f, 0.0f, 0.0f }, TypeMask.selfPerc, new string[] { "self" });
		MaskAdds selfPersMask = new MaskAdds("self", personName, 0.0f);
		
		MoodyMask.CreateNewPerson(selfPersMask, culture, new List<MaskAdds>(), rationality, morality, impulsivity, traits, moods);

        CreatePersonalRules(MoodyMask, personName, personalRules);
    }


    public static void CreatePersonalRules(MoodyMaskSystem MoodyMask, string personName, Dictionary<string, float> personalRules)
    {
        if (personalRules != null)
        {
            foreach (string RuleName in personalRules.Keys)
            {
                Type actionFetcherType = default(Type);

                actionFetcherType = Type.GetType(RuleName);

                if (!actionFetcherType.Equals(null))
                {
                    ActionInfo actionInfo = default(ActionInfo);

                    actionInfo = (ActionInfo)actionFetcherType.GetMethod("BuildActionInfo").Invoke(null, null);


                    MoodyMask.CreateNewRule(RuleName, RuleName, actionInfo.RConditioner, actionInfo.RPreference);

                    //TODO: Get List of rules to trigger from somewhere
                    List<Rule> chatRulesToTrigger = new List<Rule>();
                    chatRulesToTrigger.Add(MoodyMask.GetRule(RuleName));
                    MoodyMask.AddPossibleRulesToRule(RuleName, chatRulesToTrigger);
                }

                MoodyMask.AddRuleToMask(personName, "Self", RuleName, personalRules[RuleName]);
            }
        }
    }

	public static IEnumerator SetupInterPerson(MoodyMaskSystem MoodyMask, Being.InterPersonInfo info)
	{
		while (true) 
		{
            Person target = MoodyMask.GetPerson(info.TargetName);

            if (target != null)
			{
                Person me = MoodyMask.GetPerson(info.PersonName);

                me.CreateOpinion(target, info.NiceNasty, info.CharitableGreedy, info.HonestFalse);

#if ALL_DEBUG_MODE || NPCCREATOR_DEBUG_MODE
                Debug.Log("Setup person '" + info.PersonName + ", " + info.TargetName + " executed.");
#endif

                yield break;
			}

            Debug.LogWarning("Setting up inter-person '" + info.PersonName + " towards '" + ((string.IsNullOrEmpty(info.TargetName) ) ? "<<No Name>>" : info.TargetName) + "' has no target. Waiting.");

			yield return new WaitForSeconds(0.5f);
		}
	}

    public static void CreateFirstPeople(MoodyMaskSystem MoodyMask)
	{
		// ----------------------------------------------------------------------------------------------- ADDING RULES TO MASKS

        #region addingRulesToMask
        
        // INTERPERSONAL
        MoodyMask.AddRuleToMask("Rivalry", "Enemy", "chat", 0.2f);
        
        MoodyMask.AddRuleToMask("Rivalry", "Enemy", "greet", -0.4f);

		#endregion addingRulesToMask
        
//  ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
		#region LINKS
		  //  MoodyMask.AddLinkToPerson("Bill", TypeMask.interPers, "", "Rivalry", 0);
		 //   MoodyMask.AddRefToLinkInPerson("Bill", TypeMask.interPers, "Enemy", "Rivalry", MoodyMask.PlayerName[0], 0.5f);
		 //   MoodyMask.AddRefToLinkInPerson("Bill",TypeMask.culture, "bunce","Bungary", MoodyMask.PlayerName[0], 0.3f);
		#endregion LINKS 
        /*
        #region Opinions
	        MoodyMask.GetPerson("bill").SetOpinionValue(TraitTypes.NiceNasty, MoodyMask.GetPerson(MoodyMask.PlayerName[0]), -0.2f);
	        MoodyMask.GetPerson("bill").SetOpinionValue(TraitTypes.HonestFalse, MoodyMask.GetPerson(MoodyMask.PlayerName[0]), -0.1f);
	        MoodyMask.GetPerson("bill").SetOpinionValue(TraitTypes.CharitableGreedy, MoodyMask.GetPerson(MoodyMask.PlayerName[0]), 0.1f);
        #endregion Opinions*/
	}
}