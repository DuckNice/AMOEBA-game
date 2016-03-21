using System;
using System.Collections.Generic;

namespace NMoodyMaskSystem
{
    public class Overlay
    {
        public Dictionary<TraitTypes, Trait> Traits = new Dictionary<TraitTypes, Trait>();

        public float _rat;
        public float _mor;
        public float _imp;
        public float _abi;
        
        public Overlay(List<Trait> traits, float rat, float mor, float imp, float abi)
        {
            foreach(Trait trait in traits)
            {
                Traits.Add(trait.Name, trait);
            }

            _rat = rat;
            _mor = mor;
            _imp = imp;
            _abi = abi;
        }
    }
}