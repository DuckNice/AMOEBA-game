using NMoodyMaskSystem;

public class Flee
{
    public static ActionInfo BuildActionInfo()
    {
        ActionInvoker flee = (text, subject, direct, indPpl, misc) =>
        {
            if (subject.Name == "Kasper".ToLower().Trim())
                text.text = ("You flee from the others");
            else
                text.text = (subject.Name + " flee from the others");

            subject.Moods[MoodTypes.arousDisgus] += Calculator.AddTowards0(0.1f, subject.Moods[MoodTypes.arousDisgus]);
            subject.Moods[MoodTypes.hapSad] += Calculator.UnboundAdd(0.1f, subject.Moods[MoodTypes.hapSad]);
            subject.Moods[MoodTypes.energTired] += Calculator.UnboundAdd(-0.1f, subject.Moods[MoodTypes.energTired]);
            // subject.Moods[MoodTypes.angryFear] += Calculator.UnboundAdd(0.1f, subject.Moods[MoodTypes.angryFear]);
        };


        //Rain should be rule-specific, not action specific.
        GameManager.MoodyMask.AddAction(new MAction("flee", GameManager.MoodyMask, flee, 5f));


        RuleConditioner fleeCondition = (self, other, indPpl) =>
        {
            if (self.Moods[MoodTypes.energTired] > -0.5f && self != other)
            {
                return true;
            }

            return false;
        };


        RulePreference fleePreference = (self, other) => {
            float reff = Calculator.UnboundAdd(self.Moods[MoodTypes.energTired], 0);
            reff += Calculator.UnboundAdd(self.GetOpinionValue(TraitTypes.NiceNasty, self), reff);

            return reff;
        };


        return new ActionInfo(flee, fleeCondition, fleePreference);
    }
}