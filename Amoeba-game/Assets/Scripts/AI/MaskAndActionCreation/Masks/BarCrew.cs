using NMoodyMaskSystem;

namespace NMoodyMaskSetup
{
    public class BarCrew
    {
        public static void CreateMask(MoodyMaskSystem MoodyMask)
        {
            MoodyMask.CreateNewMask("BarCrew", new float[] { 0.0f, 0.0f, 0.0f }, TypeMask.culture, new string[] { "Maid", "Owner", "Bartender" });

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