using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : Character
{
    public List<DialogueOption> DialogueOptions
    {
        get => _DialogueOptions;
    }
    public int RelationWithPlayer
    {
        get => _RelationWithPlayer;
        set
        {
            if (value <= _MaxRelationshipValue && value >= _MinRelationshipValue)
            {
                _RelationWithPlayer = value;
            }
        }
    }

    private List<DialogueOption> _DialogueOptions = new List<DialogueOption>();
    private int _RelationWithPlayer = 0;

    private void Start()
    {
        GameObject.FindGameObjectWithTag("MinimalGame").GetComponent<NPCManager>().GetNPCByName(Name);

        if (Name == null || Name.Length == 0)
        {
            Debug.LogError("NPC::Start() > NPC Name could not be found. Either MinimalGame or NPCManager are at fault.");
        }
    }

    public void AddDialogueOption(DialogueOption dialogueOption)
    {
        DialogueOptions.Add(dialogueOption);
    }

    /* [TODO]: Check if this function is ever used? */
    public override void OnDialogueOptionSelected(PlayerDialogueOption dialogueOption)
    {
        foreach (IDialogueResult result in dialogueOption.Results)
        {
            result.Execute();
        }
    }
}
