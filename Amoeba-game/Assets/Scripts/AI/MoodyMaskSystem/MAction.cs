namespace NMoodyMaskSystem
{
    public class MAction
    {
        public static MAction Empty = new MAction("Empty", 0.0f, 0.0f);
        float _selfGain;
        float _againstGain;
        public string Name = "";
        ActionInvoker _actionInvoker;
        ActionInvoker _sustainActionInvoker;
        public static MoodyMaskSystem MoodyMask;

		public bool NeedsIndirect;
		public bool NeedsDirect;
        public float Duration = 2.0f;

        /// <summary>
        /// </summary>
        /// <param name="n">name</param>
        /// <param name="sG">selfGain</param>
        /// <param name="agG">againstGain</param>
        /// <param name="mM">moodyMask</param>
        /// <param name="aInv">actionInvoker</param>
        /// <param name="dur">duration</param>
        /// <param name="susAInv">sustainActionInvoker</param>
        /// <param name="ndDr">needsDirect</param>
        /// <param name="ndIndr">needsIndirect</param>
		public MAction(string n, float sG, float agG, MoodyMaskSystem mM, ActionInvoker aInv = null, float dur = 2.0f, ActionInvoker susAInv = null, bool ndDr = true, bool ndIndr = false)
        {
            _selfGain = sG;
            _againstGain = agG;
            Name = n;
            MoodyMask = mM;
            _actionInvoker = aInv;
            _sustainActionInvoker = susAInv;
			Duration = dur;
            NeedsDirect = ndDr;
            NeedsIndirect = ndIndr;
        }

        /// <summary>
        /// </summary>
        /// <param name="n">name</param>
        /// <param name="sG">selfGain</param>
        /// <param name="agG">againstGain</param>
        public MAction(string n, float sG, float agG)
        {
            _selfGain = sG;
            _againstGain = agG;
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


		public float GetGain(bool sG){ if(sG){return _selfGain;} return _againstGain; }
        public void SetGain(float inp, bool sG) { if (sG) { _selfGain = inp; } else { _againstGain = inp; } }
        public void AddToGain(float inp, bool sG) { if (sG) { _selfGain += inp; } else { _againstGain += inp; } }
    }
}
