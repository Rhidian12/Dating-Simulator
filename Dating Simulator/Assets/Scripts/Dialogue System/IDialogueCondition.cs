using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Json
{
    [System.Serializable]
    public class JsonDialogueCondition
    {
        /* These variables MUST be case-sensitive */
        public string character;
        public string comparison;
        public int relationshipValue;

        public IDialogueCondition ToDialogueCondition(NPCManager npcManager)
        {
            if (comparison != null && comparison != "")
            {
                RelationshipCondition cond = new RelationshipCondition(
                    GameObject.FindGameObjectWithTag("MinimalGame").GetComponent<Player>(),
                    npcManager.GetNPCByName(character),
                    relationshipValue
                    );

                return cond;
            }

            Debug.LogError("JsonDialogueCondition::ToDialogueCondition() > No Dialogue Condition could be made");

            return null;
        }
    }
}

public interface IDialogueCondition
{
    bool Execute();

    /* [TODO]: Add some Priority system to this */
}

public sealed class RelationshipCondition : IDialogueCondition
{
    private Player _Player;
    private NPC _NPC;
    private int _RelationshipValue;

    public RelationshipCondition(Player player, NPC npc, int relationshipValue)
    {
        _Player = player;
        _NPC = npc;
        _RelationshipValue = relationshipValue;
    }

    public bool Execute()
    {
        return _Player.GetPlayerRelationWithNPC(_NPC) > _RelationshipValue;
    }
}