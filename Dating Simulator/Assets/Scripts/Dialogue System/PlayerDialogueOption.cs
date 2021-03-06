using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Json
{
    [System.Serializable]
    public class JsonPlayerDialogueOption
    {
        /* These variables MUST be case-sensitive */

        /* [TODO]: Change type to DialogueCondition */
        public object[] conditions;
        public int next;
        public int npcNodeIndex;
        public JsonDialogueResult[] results;
        public string text;

        public PlayerDialogueOption ToPlayerDialogueOption(NPCManager npcManager)
        {
            PlayerDialogueOption option = new PlayerDialogueOption(text, null, null);

            List<IDialogueResult> convResults = new List<IDialogueResult>();
            foreach (JsonDialogueResult result in results)
            {
                convResults.Add(result.ToDialogueResult(npcManager));
            }

            option.NextNode = next;
            option.Results = convResults;

            return option;
        }
    }
}

public class PlayerDialogueOption : DialogueOption
{
    public List<IDialogueResult> Results { get; set; }
    public NPC NPC { get; set; } /* The NPC this Dialogue Option is responding to */

    public PlayerDialogueOption(string message, NPC npc, List<IDialogueResult> results)
        : this(message, npc, null, results)
    { }
    public PlayerDialogueOption(string message, NPC npc, List<IDialogueCondition> dialogueConditions, List<IDialogueResult> results)
        : base(message, null, dialogueConditions)
    {
        Results = results;
        NPC = npc;
    }
}
