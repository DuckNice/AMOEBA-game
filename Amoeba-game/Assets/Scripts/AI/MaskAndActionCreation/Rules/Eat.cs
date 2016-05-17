using UnityEngine;
using System.Collections;

using NMoodyMaskSystem;

public class Eat {
    public static ActionInfo BuildActionInfo()
    {
        ActionInvoker eat = (text, subject, direct, indPpl, misc) =>
        {
            if (subject.Name == "Kasper".ToLower().Trim())
                text.text = (subject.Name + " EAT " + direct.Name + "!!");
            else
                text.text = (subject.Name + " EATS " + direct.Name + "!!");

            subject.Moods[MoodTypes.arousDisgus] += Calculator.UnboundAdd(-0.5f, subject.Moods[MoodTypes.arousDisgus]);
            subject.Moods[MoodTypes.hapSad] += Calculator.UnboundAdd(-0.5f, subject.Moods[MoodTypes.hapSad]);
            subject.Moods[MoodTypes.energTired] += Calculator.UnboundAdd(-0.5f, subject.Moods[MoodTypes.hapSad]);
            AMOEBAManager.KillCharacter(direct.Name);
        };
        GameManager.MoodyMask.AddAction(new MAction("eat", GameManager.MoodyMask, eat, 7f));

        RuleConditioner eatCondition = (self, other, indPpl) =>
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

        RulePreference eatPreference = (self, other, preferenceModifier) => {
            float ret = Calculator.UnboundAdd(-self.GetOpinionValue(TraitTypes.NiceNasty, other), -self.CalculateTraitType(TraitTypes.NiceNasty));
            ret += Calculator.UnboundAdd(-self.Moods[MoodTypes.angryFear], ret);
            ret += Calculator.UnboundAdd(-self.Moods[MoodTypes.arousDisgus], ret);
            ret += Calculator.UnboundAdd(-self.Moods[MoodTypes.hapSad], ret);
            ret += Calculator.UnboundAdd(preferenceModifier, ret);
            return ret;
        };

        return new ActionInfo(eat, eatCondition, eatPreference);
    }
}