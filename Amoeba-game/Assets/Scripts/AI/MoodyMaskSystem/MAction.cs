namespace NMoodyMaskSystem
{
    public class MAction
    {
        public static MAction Empty = new MAction("Empty");
        public string Name = "";
        ActionInvoker _actionInvoker;
        ActionInvoker _sustainActionInvoker;
        public static MoodyMaskSystem MoodyMask;

		public bool NeedsIndirect;
		public bool NeedsDirect;
        public float Duration = 2.0f;

        /// <summary>
        /// </summary>
        /// <param name="name">name</param>
        /// <param name="selfGain">selfGain</param>
        /// <param name="againstGain">The gain of the other character</param>
        /// <param name="moodyMask">moodyMask</param>
        /// <param name="actionInvoker">The action to be executed when the action fires</param>
        /// <param name="duration">duration</param>
        /// <param name="sustainActionInvoker">The action to execute while we wait for the action to end.</param>
        /// <param name="needsDirect">needs a direct target</param>
        /// <param name="needsIndirect">needs and indirect target</param>
		public MAction(string name, MoodyMaskSystem moodyMask, ActionInvoker actionInvoker = null, float duration = 2.0f, ActionInvoker sustainActionInvoker = null, bool needsDirect = true, bool needsIndirect = false)
        {
            Name = name;
            MoodyMask = moodyMask;
            _actionInvoker = actionInvoker;
            _sustainActionInvoker = sustainActionInvoker;
			Duration = duration;
            NeedsDirect = needsDirect;
            NeedsIndirect = needsIndirect;
        }

        /// <summary>
        /// </summary>
        /// <param name="n">name</param>
        /// <param name="sG">selfGain</param>
        /// <param name="agG">againstGain</param>
        public MAction(string n)
        {
            Name = n;
        }

        /// <summary>
        /// </summary>
        /// <param name="sub"></param>
        /// <param name="dr"></param>
        /// <param name="r"></param>
        /// <param name="inPpl"></param>
        /// <param name="misc"></param>
        public void DoAction(UnityEngine.UI.Text text, Person sub, Person dr, Rule r, Person[] inPpl = null, object[] misc = null)
        { //SUBJECT, VERB, OBB, DIROBJ    Setup

			if(_actionInvoker != null)
            {
                _actionInvoker(text, sub, dr, inPpl, misc);
			}
			else
            {
                System.Console.WriteLine("Warning: No action to do in action '" + Name + "'.");
            }

            MoodyMask.DidAction(this, sub, dr, r);
        }

        /// <summary>
        /// </summary>
        /// <param name="sub">subject</param>
        /// <param name="dr">direct</param>
        /// <param name="r">rule</param>
        /// <param name="inPpl">indirectPeople</param>
        /// <param name="misc">miscelaneous</param>
        public void DoSustainAction(UnityEngine.UI.Text text, Person sub, Person dr, Rule r, Person[] inPpl = null, object[] misc = null)
        { //SUBJECT, VERB, OBB, DIROBJ    Setup
            if (_sustainActionInvoker != null)
            {
                _sustainActionInvoker(text, sub, dr, inPpl, misc);
            }
            else
            {
                //debug.Write("Warning: No action to do in action '" + name + "'.");
            }
        }


        public float EstimationOfSuccess(float abi)
        {
            return abi; //RIGHT now, just ability
        }
    }
}
