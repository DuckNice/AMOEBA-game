using System.Collections.Generic;
using System.Linq;


namespace NMoodyMaskSystem
{
    public partial class MoodyMaskSystem
    {
        public Rule GetRule(string name)
        {
            return PplAndMasks.GetRule(name);
        }
        

        public void AddListToActives(string name)
        {
            if(!ActiveLists.ContainsKey(name) && UpdateLists.ContainsKey(name))
            {
                ActiveLists.Add(name, UpdateLists[name]);
            }
        }


        public void RemoveListFromActives(string name)
        {
            if(ActiveLists.ContainsKey(name))
            {
                ActiveLists.Remove(name);
            }
        }


        public void AddUpdateList(string name)
        {
            if (!UpdateLists.ContainsKey(name))
            {
                UpdateLists.Add(name, new List<Person>());
            }
        }


        public void RemoveUpdateList(string name)
        {
            if (UpdateLists.ContainsKey(name))
            {
                UpdateLists.Remove(name);
            }
        }


        public void AddPersonToUpdateList(string name, Person person)
        {
            if(UpdateLists.ContainsKey(name) && person != null)
            {
                if(!UpdateLists[name].Contains(person))
                {
                    UpdateLists[name].Add(person);
                }
            }
        }


        public bool RemovePersonFromUpdateList(string name, Person person)
        {
            if (UpdateLists.ContainsKey(name) && person != null)
            {
                if (UpdateLists[name].Contains(person))
                {
                    UpdateLists[name].Remove(person);

                    return true;
                }
            }

            return false;
        }


        public void AddRolesToMask(string maskName, string[] roles = null)
        {
            maskName = maskName.ToLower();

            if (roles != null)
            {
                foreach (string role in roles)
                {
                    if (role != "")
                    {
                        PplAndMasks.AddRoleToMask(maskName, role.ToLower());
                    }
                }
            }
        }


        public void AddRuleToMask(string maskName, string roleName, string ruleName, float str, List<Rule> possibleRules = null)
        {
            maskName = maskName.ToLower();
            roleName = roleName.ToLower();
            ruleName = ruleName.ToLower();

			PplAndMasks.AddRuleToMask(maskName, ruleName, roleName, str, possibleRules);
        }


        public void AddPossibleRulesToRule(string ruleName, List<Rule> possibleRules)
        {
            PplAndMasks.AddPossibleRulesToRule(ruleName.ToLower(), possibleRules);
        }


        public void AddLinkToPerson(string persName, TypeMask maskType, string role, string mask, float str)
        {
            persName = persName.ToLower();
            role = role.ToLower();
            mask = mask.ToLower();

            PplAndMasks.GetPerson(persName).AddLink(maskType, new Link(role, PplAndMasks.GetMask(mask), str));
        }


        public void AddRefToLinkInPerson(string persName, TypeMask maskType, string role, string mask, string linkRel, float str)
        {
            persName = persName.ToLower();
            role = role.ToLower();
			mask = mask.ToLower ();
			linkRel = linkRel.ToLower ();

            Person personRelated = PplAndMasks.GetPerson(linkRel.ToLower());

            PplAndMasks.GetPerson(persName).AddRoleRefToLink(maskType, PplAndMasks.GetMask(mask), role, personRelated, str);
        }


        public void AddAction(MAction action)
        {
            if(!PosActions.ContainsKey(action.Name.ToLower()))
            {
                PosActions.Add(action.Name.ToLower(), action);
            }
            else
            {
                System.Console.WriteLine("Warning: Action with name: '" + action.Name + "' already exists. Please note that action names are not case sensitive.");
            }
        }


        public void UpdateLvlOfInfl(Person self, float changeValue)
        {
            List<Person> personRoom = UpdateLists.First(x => x.Value.Contains(self)).Value;

            List<Mask> cultureMasks = new List<Mask>();

            foreach (Person person in personRoom)
            {
                foreach (Link link in person.Culture)
                {
                    if (!cultureMasks.Contains(link.RoleMask))
                    {
                        cultureMasks.Add(link.RoleMask);
                    }
                }
            }

            foreach (Link link in new List<Link>(self.Culture))
            {
                foreach (Person person in new List<Person>(link.GetRoleRefPpl()))
                {
                    foreach (string role in new List<string>(link.RoleRefs[person].Keys))
                    {
                        if (cultureMasks.Exists(x => x == link.RoleMask))
                            link.AddToLvlOfInfl(changeValue, role, person);
                        else
                            link.AddToLvlOfInfl(-changeValue, role, person);
                    }
                }
            }

            foreach (Link link in new List<Link>(self.InterPersonal))
            {
                foreach (Person person in new List<Person>(link.GetRoleRefPpl()))
                {
                    foreach (string role in new List<string>(link.RoleRefs[person].Keys))
                    {
                        if (personRoom.Contains(person))
                            link.AddToLvlOfInfl(changeValue, role, person);
                        else
                            link.AddToLvlOfInfl(-changeValue, role, person);
                    }
                }
            }
        }
    }
}
