using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using HarmonyLib;
using RimWorld;
using TitularRoyalty.Extensions;
using TitularRoyalty.UI;
using Verse;

namespace TitularRoyalty.Patches
{
    // Patch to the title to the top stack of the character card, along with the factions and ideos
    
    [HarmonyPatch(typeof(CharacterCardUtility), nameof(CharacterCardUtility.DoTopStack))]
    public static class CharacterCardUtility_DoTopStack_Patch
    {
        public static void AddTitlePlate(Pawn pawn)
        {
            if (!pawn.PlayerRoyalty().HasAnyTitle)
            {
                return;
            }

            
            var title = pawn.PlayerRoyalty().title;

            var tmpStackElements = 
                HarmonyFields.CharacterCardUtility_TmpStackElements.GetValue(null) as List<GenUI.AnonymousStackElement>;

            if (tmpStackElements == null)
            {
                Log.Error("Couldn't get tmpStackElements from the CharacterCardUtility");
                return;
            }
            
            tmpStackElements.Add(new GenUI.AnonymousStackElement
            {
                drawer = TitleUIUtility.CreateTitlePlateDrawer(pawn, title),
                width = Text.CalcSize(title.GetLabelForHolder()).x + 22f + 14f
            });
        }
        
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var codeInstructions = instructions.ToList();
            
            var insertionIndex = -1;
            
            // C#: if (pawn.royalty != null && pawn.royalty.AllTitlesForReading.Count > 0)
            // IL_04d4: ldloc.0 <-- Insert After this, due to some jump stuff
            // IL_04d5: ldfld class Verse.Pawn RimWorld.CharacterCardUtility/'<>c__DisplayClass41_0'::pawn
            // IL_04da: ldfld class RimWorld.Pawn_RoyaltyTracker Verse.Pawn::royalty <-- Looking for this

            for (int i = 0; i < codeInstructions.Count; i++)
            {
                if (codeInstructions[i].opcode == OpCodes.Ldfld &&
                    codeInstructions[i].LoadsField(AccessTools.Field(typeof(Pawn), "royalty")) &&
                    codeInstructions[i - 2].opcode == OpCodes.Ldloc_0)
                {
                    insertionIndex = i - 1; // after ldloc.0
                    break;
                }
            }

            if (insertionIndex == -1)
            {
                Log.Error("Couldn't find a proper IL location to insert the code in CharacterCardUtility_DoTopStack_Patch");
                return codeInstructions; // original    
            }
            
            var instructionsToInsert = new List<CodeInstruction>();
            
            // Pop ldloc.0
            instructionsToInsert.Add(new CodeInstruction(OpCodes.Pop)); 
            
            // load the pawn onto the stack
            instructionsToInsert.Add(new CodeInstruction(OpCodes.Ldarg_0)); 
            // load the tmpStackElements Field onto the stack
            //instructionsToInsert.Add(CodeInstruction.LoadField(typeof(CharacterCardUtility), "tmpStackElements"));

            // Call the AddTitlePlate Method
            var addPlateMethod = AccessTools.Method(typeof(CharacterCardUtility_DoTopStack_Patch), nameof(AddTitlePlate));
            instructionsToInsert.Add(new CodeInstruction(OpCodes.Call, addPlateMethod));
            
            // Reload ldloc.0 we replaced
            instructionsToInsert.Add(new CodeInstruction(OpCodes.Ldloc_0));
            
            // Insert the new instructions
            codeInstructions.InsertRange(insertionIndex, instructionsToInsert);
            return codeInstructions.AsEnumerable();
        }
    }
}