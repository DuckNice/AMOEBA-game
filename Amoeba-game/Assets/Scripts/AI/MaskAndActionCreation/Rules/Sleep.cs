using NMoodyMaskSystem;

public class Sleep
{
    public static ActionInfo BuildActionInfo()
    {
        ActionInvoker sleep = (text, subject, direct, indPpl, misc) =>
        {
            if (subject.Name == "Kasper".ToLower().Trim())
                text.text = ("You sleep.");
            else
                text.text = (subject.Name + " sleeps " + direct.Name + ".");

            subject.Moods[MoodTypes.arousDisgus] += Calculator.AddTowards0(0.1f, subject.Moods[MoodTypes.arousDisgus]);
            subject.Moods[MoodTypes.hapSad] += Calculator.UnboundAdd(0.05f, subject.Moods[MoodTypes.hapSad]);
            subject.Moods[MoodTypes.energTired] += Calculator.UnboundAdd(0.1f, subject.Moods[MoodTypes.energTired]); 
           // subject.Moods[MoodTypes.angryFear] += Calculator.UnboundAdd(0.1f, subject.Moods[MoodTypes.angryFear]);
        };
        //Rain should be rule-specific, not action specific.
        GameManager.MoodyMask.AddAction(new MAction("sleep", GameManager.MoodyMask, sleep, 5f));


        RuleConditioner sleepCondition = (self, other, indPpl) =>
        {
            if (self.Moods[MoodTypes.energTired] < 0.5f)
            {
                return true;
            }
            
            return false;
        };


        RulePreference sleepPreference = (self, other) => {
            return Calculator.UnboundAdd(self.Moods[MoodTypes.energTired], 0);
        };


        return new ActionInfo(sleep, sleepCondition, sleepPreference);
    }
}