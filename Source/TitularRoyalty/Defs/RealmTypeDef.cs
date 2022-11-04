//using System; 
using RimWorld;
using Verse;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TitularRoyalty
{
    public class RealmTypeDef : Def
    {
        public RealmTypeDef inheritsFrom;
        public bool HasParent
        {
            get
            {
                return inheritsFrom != null;
            }
        }

        [NoTranslate]
        public string SaveID;

        public override IEnumerable<string> ConfigErrors()
        {
            foreach (string item in base.ConfigErrors())
            {
                yield return item;
            }

            if (SaveID == null) { yield return $"def {defName} must have a SaveID"; }
        }
    }
}
