using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;
using HarmonyLib;
using Verse;

namespace TitularRoyalty.Extensions
{
    public static class ILUtils
    {
        /// <summary>
        /// Returns a CodeInstruction array that will call Log.Message with the provided text
        /// </summary>
        /// <param name="text">Text to log</param>
        /// <param name="useLogTR">Use LogTR instead of Verse.Log</param>
        /// <param name="isError">Use the log.error method instead</param>
        /// <returns>Ldstr and Call Instruction with the provided text, so it can call log</returns>
        public static CodeInstruction[] LogIL(string text, bool useLogTR = true, bool isError = false)
        {
            var instructions = new CodeInstruction[2];

            instructions[0] = new CodeInstruction(OpCodes.Ldstr, text);

            var logType = useLogTR ? typeof(LogTR) : typeof(Log);
            
            if (isError)
            {
                instructions[1] = new CodeInstruction(OpCodes.Call,
                    AccessTools.Method(logType, "Error", new[] { typeof(string) }));
            }
            else
            {
                instructions[1] = new CodeInstruction(OpCodes.Call,
                    AccessTools.Method(logType, "Message", new[] { typeof(string) }));
            }

            return instructions;
        }

        /// <summary>
        /// Adds a log.message instruction to the list.
        /// </summary>
        /// <param name="codeInstructions">Instructions to add to</param>
        /// <param name="text">Text to log</param>
        /// <param name="isError">Use the log.error method instead</param>
        public static void LogIL(this List<CodeInstruction> codeInstructions, string text, bool isError = false)
        {
            foreach (var instruction in LogIL(text, isError))
            {
                codeInstructions.Add(instruction);
            }
        }
        
        /// <summary>
        /// Converts the CodeInstruction instance to a debug-ready string format.
        /// </summary>
        /// <param name="instruction">This CodeInstruction instance.</param>
        /// <returns>A string in format of 'opcode : operand' or 'opcode : No Operand' if operand is null.</returns>
        public static string ToDebugString(this CodeInstruction instruction)
        {
            var sb = new StringBuilder();
            
            sb.Append(instruction.opcode.ToString());
            
            sb.Append(" : ");
            
            if (instruction.operand != null)
            {
                sb.Append(instruction.operand);
            }
            else
            {
                sb.Append("No Operand");
            }

            return sb.ToString();
        }
    }
}