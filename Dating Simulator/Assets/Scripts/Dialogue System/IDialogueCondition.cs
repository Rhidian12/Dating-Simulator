using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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