using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    public Dictionary<NPC, List<DialogueOption>> DialogueOptions
    {
        get => _dialogueOptions;
    }
    public Dictionary<NPC, int> RelationsWithNPCs
    {
        get => _NPCs;
    }

    private Dictionary<NPC, List<DialogueOption>> _dialogueOptions = new Dictionary<NPC, List<DialogueOption>>();
    private Dictionary<NPC, int> _NPCs = new Dictionary<NPC, int>();

    public void AddNPC(NPC npc)
    {
        _NPCs.Add(npc, _MinRelationshipValue);
    }
    public void RemoveNPC(NPC npc)
    {
        _NPCs.Remove(npc);
    }
    public void AddDialogueOption(NPC npc, DialogueOption dialogue)
    {
        if (_dialogueOptions.ContainsKey(npc))
        {
            _dialogueOptions[npc].Add(dialogue);
        }
        else
        {
            _dialogueOptions.Add(npc, new List<DialogueOption>() { dialogue });
        }
    }
    public int GetPlayerRelationWithNPC(NPC npc)
    {
        if (_NPCs.ContainsKey(npc))
            return _NPCs[npc];
        else
            return -1;
    }
    public List<DialogueOption> GetDialogueOptions(NPC npc)
    {
        if (_dialogueOptions.ContainsKey(npc))
        {
            return _dialogueOptions[npc];
        }
        else
        {
            return null;
        }
    }
    public override void OnDialogueOptionSelected(PlayerDialogueOption dialogueOption)
    {
        foreach (IDialogueResult result in dialogueOption.Results)
        {
            result.Execute();
        }
    }
}
