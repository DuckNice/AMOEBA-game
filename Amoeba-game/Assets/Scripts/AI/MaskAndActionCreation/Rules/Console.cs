using NMoodyMaskSystem;

public static class Console
{
    public static ActionInfo BuildActionInfo(MoodyMaskSystem MoodyMask)
    {
        //--------------------------------------------------Implementation
        ActionInvoker Console = (subject, direct, indPpl, misc) =>
        {
            direct.Moods[MoodTypes.energTired] += Calculator.UnboundAdd(-0.1f, direct.Moods[MoodTypes.energTired]);
            subject.Moods[MoodTypes.energTired] += Calculator.UnboundAdd(-0.1f, subject.Moods[MoodTypes.energTired]);
        };

        MoodyMask.AddAction(new MAction("console", 0.0f, 0.0f, MoodyMask, Console, 10f));

        //--------------------------------------------------Condition
        RuleConditioner ConsoleCondition = (self, other, indPpl) =>
        {
            if (self != other)
            { //LvlOfInfl0.2
                if (other.Moods[MoodTypes.energTired] > -0.6)
                {
                    return true;
                }
            }
            return false;
        };

        //--------------------------------------------------Preference
        RulePreference ConsolePreference = (self, other) => {
            return Calculator.UnboundAdd(self.GetOpinionValue(TraitTypes.NiceNasty, other), self.GetOpinionValue(TraitTypes.HonestFalse, other)); //LVLOFINFL
        };

        //--------------------------------------------------ActionInfo

        return new ActionInfo(Console, ConsoleCondition, ConsolePreference);
    }
}