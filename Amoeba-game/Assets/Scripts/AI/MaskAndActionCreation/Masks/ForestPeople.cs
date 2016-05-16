using NMoodyMaskSystem;

namespace NMoodyMaskSetup
{
    public class ForestPeople
    {
        public static void CreateMask(MoodyMaskSystem moodyMask)
        {
            moodyMask.CreateNewMask("ForestPeople", new float[] { 0,0,0}, TypeMask.culture, new string[] { "Forester", "Commoner", "WiseOne", "Child"});

            ActionInfo EatInfo = Eat.BuildActionInfo();

            moodyMask.CreateNewRule("Eat", "Eat", -0.4f, -0.9f, EatInfo.RConditioner, EatInfo.RPreference);

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