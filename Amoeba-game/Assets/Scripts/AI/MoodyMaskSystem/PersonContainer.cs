using System.Collections.Generic;


namespace NMoodyMaskSystem
{
    public class PersCont:MaskCont
    {
        public Dictionary<string, Person> People = new Dictionary<string, Person>();
        public List<string> PeopleNames = new List<string>();

        public void CreateNewPerson(string personName, Person person)
        {
            if ((person.GetLinks(TypeMask.selfPerc))[0] != null)
            {
                personName = personName.ToLower().Trim();
                People.Add(personName, person);
                PeopleNames.Add(personName);
            }
            else
            {
                System.Console.WriteLine("Error: person '" + personName + "' to be inserted has no selfPersonality Link. Aborting insert.");
                
				return;
            }
        }

        public Person GetPerson(string name)
        {
            name = name.ToLower().Trim();

            if (People.ContainsKey(name))
            {
                return People[name];
            }
            else
            {
                return null;
            }
        }
    }
}