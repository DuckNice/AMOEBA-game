using NMoodyMaskSystem;

namespace NMoodyMaskSetup
{
    public class ForestPeople
    {
        public static void CreateMask(MoodyMaskSystem moodyMask)
        {
            moodyMask.CreateNewMask("ForestPeople", new float[] { 0,0,0}, TypeMask.culture, new string[] { "Forester", "Commoner", "WiseOne", "Child"});
            
            ActionInfo WorkInfo = Work.BuildActionInfo();
            moodyMask.CreateNewRule("Work", "Work", 0.1f, 0f, WorkInfo.RConditioner, WorkInfo.RPreference);

            ActionInfo PlayInfo = Play.BuildActionInfo();
            moodyMask.CreateNewRule("Play", "Play", 0.2f, 0f, PlayInfo.RConditioner, PlayInfo.RPreference);

            ActionInfo ChaseInfo = Chase.BuildActionInfo();
            moodyMask.CreateNewRule("Chase", "Chase", 0.2f, 0f, ChaseInfo.RConditioner, ChaseInfo.RPreference);

            ActionInfo KillInfo = Kill.BuildActionInfo();
            moodyMask.CreateNewRule("Kill", "Kill", -0.5f, -0.7f, KillInfo.RConditioner, KillInfo.RPreference);

            ActionInfo SleepInfo = Sleep.BuildActionInfo();
            moodyMask.CreateNewRule("Sleep", "Sleep", 0.2f, 0f, SleepInfo.RConditioner, SleepInfo.RPreference);

            //Child
            moodyMask.AddRuleToMask("ForestPeople", "Child", "Play", 0.6f);
            moodyMask.AddRuleToMask("ForestPeople", "Child", "Chase", 0.1f);
            moodyMask.AddRuleToMask("ForestPeople", "Child", "Work", 0.3f);

            //Forester
            moodyMask.AddRuleToMask("ForestPeople", "Commoner", "Work", 0.7f);
            moodyMask.AddRuleToMask("ForestPeople", "Forester", "Chase", 0.4f);
            moodyMask.AddRuleToMask("ForestPeople", "Forester", "Kill", 0.1f);

            //Commoner
            moodyMask.AddRuleToMask("ForestPeople", "Commoner", "Work", 0.5f);
            moodyMask.AddRuleToMask("ForestPeople", "Commoner", "Sleep", 0.3f);

            //WiseOne


            //  ActionInfo GreetInfo = Greet.BuildActionInfo();

            //  MoodyMask.CreateNewRule("greetfbunce", "greet", GreetInfo.RConditioner, GreetInfo.RPreference);
            //  MoodyMask.CreateNewRule("greetfcess", "greet", GreetInfo.RConditioner, GreetInfo.RPreference);
            //  MoodyMask.CreateNewRule("greetfbunsant", "greet", GreetInfo.RConditioner, GreetInfo.RPreference);

            //   List<Rule> greetRulesToTrigger = new List<Rule>(); 
            //   greetRulesToTrigger.Add(MoodyMask.GetRule("greet")); 

            //   MoodyMask.AddPossibleRulesToRule("greetfcess", greetRulesToTrigger);
            //   MoodyMask.AddPossibleRulesToRule("greetfbunce", greetRulesToTrigger);
            //   MoodyMask.AddPossibleRulesToRule("greetfbunsant", greetRulesToTrigger);
        }
    }
}