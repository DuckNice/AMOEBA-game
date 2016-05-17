using UnityEngine;
using System.Collections;

using NMoodyMaskSystem;

public class Kill
{
    public static ActionInfo BuildActionInfo()
    {
        ActionInvoker kill = (text, subject, direct, indPpl, misc) =>
        {
            if (subject.Name == "Kasper".ToLower().Trim())
                text.text = (subject.Name + " Kill " + direct.Name + "!!");
            else
                text.text = (subject.Name + " Kills " + direct.Name + "!!");

            subject.Moods[MoodTypes.arousDisgus] += Calculator.UnboundAdd(-0.5f, subject.Moods[MoodTypes.arousDisgus]);
            subject.Moods[MoodTypes.hapSad] += Calculator.UnboundAdd(-0.5f, subject.Moods[MoodTypes.hapSad]);
            subject.Moods[MoodTypes.energTired] += Calculator.UnboundAdd(-0.5f, subject.Moods[MoodTypes.hapSad]);
            AMOEBAManager.KillCharacter(direct.Name);
        };
        GameManager.MoodyMask.AddAction(new MAction("kill", GameManager.MoodyMask, kill, 7f));

        RuleConditioner killCondition = (self, other, indPpl) =>
        {
            if (GameManager.MoodyMask.HistoryBook.Exists(x => x.GetAction() == GameManager.MoodyMask.PosActions["steal"] && (x.GetSubject() == other)) ||
                   GameManager.MoodyMask.HistoryBook.Exists(x => x.GetAction() == GameManager.MoodyMask.PosActions["fight"] && (x.GetSubject() == other)) ||
                   GameManager.MoodyMask.HistoryBook.Exists(x => x.GetRule().GetRuleStrength() < -0.4f && GameManager.MoodyMask.GetUpdateList("Main").Contains(other) && self != other))
            { return true; }

            if (self != other)
            {
                if (self.GetOpinionValue(TraitTypes.NiceNasty, other) < -0.6 && self.CalculateTraitType(TraitTypes.NiceNasty) < -0.1)
                {
                    return true;
                }
            }

            return false;
        };

        RulePreference killPreference = (self, other, preferenceModifier) => {
            float pref = Calculator.UnboundAdd(-self.GetOpinionValue(TraitTypes.NiceNasty, other), -self.CalculateTraitType(TraitTypes.NiceNasty));
            pref += Calculator.UnboundAdd(-self.Moods[MoodTypes.angryFear], pref);
            pref += Calculator.UnboundAdd(-self.Moods[MoodTypes.arousDisgus], pref);
            pref += Calculator.UnboundAdd(-self.Moods[MoodTypes.hapSad], pref);
            pref += Calculator.UnboundAdd(preferenceModifier, pref);
            return pref;
        };

        return new ActionInfo(kill, killCondition, killPreference);
    }
}