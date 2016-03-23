using NMoodyMaskSystem;

public static class Chat
{
    public static ActionInfo BuildActionInfo(MoodyMaskSystem MoodyMask)
    {		
        //--------------------------------------------------Implementation
        ActionInvoker Chat = (subject, direct, indPpl, misc) =>
        {
            direct.Moods[MoodTypes.energTired] += Calculator.UnboundAdd(-0.1f, direct.Moods[MoodTypes.energTired]);
            subject.Moods[MoodTypes.energTired] += Calculator.UnboundAdd(-0.1f, subject.Moods[MoodTypes.energTired]);
        };

        MoodyMask.AddAction(new MAction("chat", 0.0f, 0.0f, MoodyMask, Chat, 10f));

        //--------------------------------------------------Condition
        RuleConditioner ChatCondition = (self, other, indPpl) =>
        {
            if (self != other)
            { //LvlOfInfl0.2
                if (self.Moods[MoodTypes.energTired] > -0.6)
                {
                    return true;
                }
            }
            return false;
        };

        //--------------------------------------------------Preference
        RulePreference ChatPreference = (self, other) => {
            return Calculator.UnboundAdd(self.GetOpinionValue(TraitTypes.NiceNasty, other), self.GetOpinionValue(TraitTypes.HonestFalse, other)); //LVLOFINFL
        };

        //--------------------------------------------------ActionInfo

        return new ActionInfo(Chat, ChatCondition, ChatPreference);
    }
}