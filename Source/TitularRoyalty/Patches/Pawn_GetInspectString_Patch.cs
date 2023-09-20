using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using HarmonyLib;
using TitularRoyalty.Extensions;
using Verse;

namespace TitularRoyalty.Patches
{
    [HarmonyPatch(typeof(Pawn), nameof(Pawn.GetInspectString))]
    public static class Pawn_GetInspectString_Patch
    {
        
        public static void AddTitleInspectString(StringBuilder sb, Pawn pawn)
        {
            if (pawn.NonHumanlikeOrWildMan())
                return;
            
            var pawnRoyalty = pawn.PlayerRoyalty();
            
            if (pawnRoyalty.HasAnyTitle)
            {
                sb.AppendLine(pawnRoyalty.title.GetInspectString());
            }
        }
        
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            // todo: skip the original title label if there's going to be a player one 
            var insertionIndex = -1;

            var codeInstructions = instructions.ToList();

            // run after: stringBuilder.AppendLine(MainDesc(writeFaction: true));
            for (int i = 0; i < codeInstructions.Count; i++)
            {
                if (codeInstructions[i].opcode == OpCodes.Pop)
                {
                    insertionIndex = i + 1;
                    break;
                }
            }

            var instructionsToInsert = new List<CodeInstruction>();
            
            instructionsToInsert.Add(new CodeInstruction(OpCodes.Ldloc_0)); // Load stringbuilder
            instructionsToInsert.Add(new CodeInstruction(OpCodes.Ldarg_0)); // Load pawn

            instructionsToInsert.Add(new CodeInstruction(OpCodes.Call,
                AccessTools.Method(typeof(Pawn_GetInspectString_Patch), nameof(AddTitleInspectString))));
            


            // Add the new instructions
            codeInstructions.InsertRange(insertionIndex, instructionsToInsert);
            return codeInstructions.AsEnumerable();
        }
    }
}