//using System; 
using RimWorld;
using Verse;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

namespace TitularRoyalty
{
    public class PlayerTitlePermitDef : RoyalTitlePermitDef
    {
        public int usersPerPopulation = 5;
        public int requiredPopulation = 10;
        public int maxUsers = 100;

        public static int currentUserCount;
        
        public bool MeetsPopulationRequirements(int population)
        {
            return (population / requiredPopulation) * maxUsers < maxUsers;
        }
    }

}
