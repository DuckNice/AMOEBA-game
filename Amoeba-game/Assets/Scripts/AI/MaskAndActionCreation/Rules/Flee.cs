using NMoodyMaskSystem;

public class Flee
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
        };
        //Rain should be rule-specific, not action specific.
        GameManager.MoodyMask.AddAction(new MAction("play", GameManager.MoodyMask, play, 5f));


        RuleConditioner playCondition = (self, other, indPpl) =>
        {
            if (self.Moods[MoodTypes.energTired] > -0.5f && self != other)
            {
                return true;
            }

            return false;
        };


        RulePreference playPreference = (self, other) => {
            float reff = Calculator.UnboundAdd(self.Moods[MoodTypes.energTired], 0);
            reff += Calculator.UnboundAdd(self.GetOpinionValue(TraitTypes.NiceNasty, self), reff);

            return reff;
        };


        return new ActionInfo(play, playCondition, playPreference);
    }
}