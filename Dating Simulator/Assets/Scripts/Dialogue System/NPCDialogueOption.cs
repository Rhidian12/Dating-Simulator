using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialogueOption : DialogueOption
{
    public NPCDialogueOption(string message, Character character)
        : this(message, character, null)
    { }
    public NPCDialogueOption(string message, Character character, List<IDialogueCondition> dialogueConditions)
        : base(message, character, dialogueConditions)
    { }
}
