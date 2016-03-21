

namespace NMoodyMaskSystem
{
    public class Trait
    {
        public TraitTypes Name;
        float value;

        public Trait(TraitTypes name, float value)
        {
            Name = name;
            this.value = value;
        }

		public float GetTraitValue(){ return value; }
		public void SetTraitValue(float inp){ value = inp; }
		public void AddToTraitValue(float inp){ value += inp; }
    }
}