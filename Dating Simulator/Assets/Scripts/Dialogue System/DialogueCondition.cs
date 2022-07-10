using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq.Expressions;

public interface DialogueCondition
{
    bool Execute();

    /* [TODO]: Add some Priority system to this */
}

public sealed class RelationshipCondition : DialogueCondition
{
    private Player _player;
    private NPC _npc;
    private int _relationshipValue;

    public RelationshipCondition(Player player, NPC npc, int relationshipValue)
    {
        _player = player;
        _npc = npc;
        _relationshipValue = relationshipValue;
    }

    public bool Execute()
    {
        return _player.GetPlayerRelationWithNPC(_npc) > _relationshipValue;
    }
}