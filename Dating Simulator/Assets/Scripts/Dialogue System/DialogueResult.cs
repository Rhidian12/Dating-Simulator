using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Json
{
    [System.Serializable]
    public class JsonDialogueResult
    {
        public string change;
        public string npc;
        public int relationshipValue;

        public DialogueResult ToDialogueResult(NPCManager npcManager)
        {
            /* This errors: error CS0165: Use of unassigned local variable 'result' */
            /* C# is cringe */
            // DialogueResult result;

            // if (change != null)
            // {
            //    result = new RelationshipDialogueResult(change == "+", npcManager.GetNPCByName(npc), relationshipValue);
            // }

            // return result;

            if (change != null)
            {
                return new RelationshipDialogueResult(change == "+", npcManager.GetNPCByName(npc), relationshipValue);
            }

            return null;
        }
    }
}

public interface DialogueResult
{
    public void Execute();
}

public class RelationshipDialogueResult : DialogueResult
{
    public bool Positive = false;
    public NPC NPC = null;
    public int RelationshipValue = 0;

    public RelationshipDialogueResult(bool isPositive, NPC npc, int relationshipValue)
    {
        Positive = isPositive;
        NPC = npc;
        RelationshipValue = relationshipValue;
    }

    public void Execute()
    {
        if (Positive)
        {
            NPC.RelationWithPlayer += RelationshipValue;
        }
        else
        {
            NPC.RelationWithPlayer -= RelationshipValue;
        }
    }
}
