using NMoodyMaskSystem;

namespace NMoodyMaskSetup
{
    public class Animal
    {
        public static void CreateMask(MoodyMaskSystem moodyMask)
        {
            moodyMask.CreateNewMask("Animal", new float[] { 0, 0, 0 }, TypeMask.culture, new string[] { "Forester", "Witch", "Son", "Daughter" });


        }
    }
}