using UnityEngine;
using System.Collections;

using NMoodyMaskSystem;

public class Kill : MonoBehaviour {
    public static ActionInfo BuildActionInfo()
    {
        ActionInvoker kill = (text, subject, direct, indPpl, misc) =>
        {
            if (subject.Name == "Kasper".ToLower().Trim())
                text.text = (subject.Name + " EAT " + direct.Name + "!!");
            else
                text.text = (subject.Name + " EATS " + direct.Name + "!!");

            subject.Moods[MoodTypes.arousDisgus] += Calculator.UnboundAdd(-0.5f, subject.Moods[MoodTypes.arousDisgus]);
            subject.Moods[MoodTypes.hapSad] += Calculator.UnboundAdd(-0.5f, subject.Moods[MoodTypes.hapSad]);
            subject.Moods[MoodTypes.energTired] += Calculator.UnboundAdd(-0.5f, subject.Moods[MoodTypes.hapSad]);
            GameManager.MoodyMask.AddPersonToUpdateList("Dead", direct);
            GameManager.MoodyMask.RemovePersonFromUpdateList("Main", direct);
        };
        GameManager.MoodyMask.AddAction(new MAction("kill", -0.9f, -0.8f, GameManager.MoodyMask, kill, 7f));

        return default(ActionInfo);
    }
}