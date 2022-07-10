using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : Character
{
    public List<DialogueOption> DialogueOptions
    {
        get => _dialogueOptions;
    }
    public int RelationWithPlayer
    {
        get => _relationWithPlayer;
        set
        {
            if (value <= _maxRelationshipValue && value >= _minRelationshipValue)
                _relationWithPlayer = value;
        }
    }

    private List<DialogueOption> _dialogueOptions = new List<DialogueOption>();
    private int _relationWithPlayer = 0;

    public void AddDialogueOption(DialogueOption dialogueOption)
    {
        DialogueOptions.Add(dialogueOption);
    }
    public override void OnDialogueOptionSelected(PlayerDialogueOption dialogueOption)
    {
        foreach (DialogueResult result in dialogueOption.Results)
        {
            result.Execute();
        }
    }
    private void Start()
    {
        GameObject.FindGameObjectWithTag("MinimalGame").GetComponent<NPCManager>().GetNPCByName(Name);
    }
}
