using NMoodyMaskSystem;

namespace NMoodyMaskSetup
{
    public class Witch
    {
        public static void CreateMask(MoodyMaskSystem moodyMask)
        {
            moodyMask.CreateNewMask("Witch", new float[] { 0, 0, 0 }, TypeMask.culture, new string[] { "Common"});

            ActionInfo EatInfo = Eat.BuildActionInfo();
            moodyMask.CreateNewRule("Eat", "Eat", 0f, -0.7f, EatInfo.RConditioner, EatInfo.RPreference);

            ActionInfo WorkInfo = Work.BuildActionInfo();
            moodyMask.CreateNewRule("Work", "Work", 0.1f, 0f, WorkInfo.RConditioner, WorkInfo.RPreference);

            ActionInfo FleeInfo = Flee.BuildActionInfo();
            moodyMask.CreateNewRule("Flee", "Flee", 0.0f, -0.3f, FleeInfo.RConditioner, FleeInfo.RPreference);

            //Common
            moodyMask.AddRuleToMask("Witch", "Common", "Eat", 0.4f);
            moodyMask.AddRuleToMask("Witch", "Common", "Work", 0.6f);
            moodyMask.AddRuleToMask("Witch", "Common", "Flee", 0.4f);
        }
    }
}