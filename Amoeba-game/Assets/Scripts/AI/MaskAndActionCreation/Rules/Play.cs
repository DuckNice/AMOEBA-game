using NMoodyMaskSystem;

public class Play
{
    public static ActionInfo BuildActionInfo()
    {
        ActionInvoker play = (text, subject, direct, indPpl, misc) =>
        {
            if (subject.Name == "Kasper".ToLower().Trim())
                text.text = ("You play with " + direct.Name + ".");
            else
                text.text = (subject.Name + " plays with " + direct.Name + ".");

            subject.Moods[MoodTypes.arousDisgus] += Calculator.AddTowards0(0.1f, subject.Moods[MoodTypes.arousDisgus]);
            subject.Moods[MoodTypes.hapSad] += Calculator.UnboundAdd(0.1f, subject.Moods[MoodTypes.hapSad]);
            subject.Moods[MoodTypes.energTired] += Calculator.UnboundAdd(-0.1f, subject.Moods[MoodTypes.energTired]);
            // subject.Moods[MoodTypes.angryFear] += Calculator.UnboundAdd(0.1f, subject.Moods[MoodTypes.angryFear]);
        };

        GameManager.MoodyMask.AddAction(new MAction("play", GameManager.MoodyMask, play, 5f));


        RuleConditioner playCondition = (self, other, indPpl) =>
        {
            if (self.Moods[MoodTypes.energTired] > -0.5f && self != other)
            {
                return true;
            }

            return false;
        };


        RulePreference playPreference = (self, other, preferenceModifier) => {
            float pref = Calculator.UnboundAdd(self.Moods[MoodTypes.energTired], 0);
            pref += Calculator.UnboundAdd(self.GetOpinionValue(TraitTypes.NiceNasty, self), pref);
            pref += Calculator.UnboundAdd(preferenceModifier, pref);
            return pref;
        };


        return new ActionInfo(play, playCondition, playPreference);
    }
}