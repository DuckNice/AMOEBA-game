using NMoodyMaskSystem;

namespace NMoodyMaskSetup
{
    public class ForestPeople
    {
        public static void CreateMask(MoodyMaskSystem moodyMask)
        {
            moodyMask.CreateNewMask("ForestPeople", new float[] { 0,0,0}, TypeMask.culture, new string[] { "Forester", "Witch", "Son", "Daughter"});

            ActionInfo KillInfo = Kill.BuildActionInfo();

            moodyMask.CreateNewRule("Kill", "Kill", KillInfo.RConditioner, KillInfo.RPreference);

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