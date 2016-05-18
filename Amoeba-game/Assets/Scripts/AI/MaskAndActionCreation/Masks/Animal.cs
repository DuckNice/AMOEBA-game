using NMoodyMaskSystem;

namespace NMoodyMaskSetup
{
    public class Animal
    {
        public static void CreateMask(MoodyMaskSystem moodyMask)
        {
            moodyMask.CreateNewMask("Animal", new float[] { 0, 0, 0 }, TypeMask.culture, new string[] { "Base", "Predator", "HumanFriend" });

            ActionInfo EatInfo = Eat.BuildActionInfo();
            moodyMask.CreateNewRule("Eat", "Eat", 0f, -0.7f, EatInfo.RConditioner, EatInfo.RPreference);

            ActionInfo SleepInfo = Sleep.BuildActionInfo();
            moodyMask.CreateNewRule("Sleep", "Sleep", 0.2f, 0f, SleepInfo.RConditioner, SleepInfo.RPreference);

            ActionInfo PlayInfo = Play.BuildActionInfo();
            moodyMask.CreateNewRule("Play", "Play", 0.2f, 0f, PlayInfo.RConditioner, PlayInfo.RPreference);

            ActionInfo ChaseInfo = Chase.BuildActionInfo();
            moodyMask.CreateNewRule("Chase", "Chase", 0.2f, 0f, ChaseInfo.RConditioner, ChaseInfo.RPreference);

            ActionInfo FleeInfo = Flee.BuildActionInfo();
            moodyMask.CreateNewRule("Flee", "Flee", 0.0f, -0.3f, FleeInfo.RConditioner, FleeInfo.RPreference);

            //Base
            moodyMask.AddRuleToMask("Animal", "Base", "Flee", 0.6f);
            moodyMask.AddRuleToMask("Animal", "Base", "Sleep", 0.8f);


            //Predator
            moodyMask.AddRuleToMask("Animal", "Predator", "Eat", 0.5f);


            //HumanFriend
            moodyMask.AddRuleToMask("Animal", "HumanFriend", "Play", 0.6f);
            moodyMask.AddRuleToMask("Animal", "HumanFriend", "Chase", 0.2f);


        }
    }
}