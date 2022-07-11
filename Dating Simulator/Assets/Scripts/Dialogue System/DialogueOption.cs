using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace Json
{
    [System.Serializable]
    public class JsonDialogueOption
    {
        /* These variables MUST be case-sensitive */
        public string character;
        /* [TODO]: Change type to DialogueCondition */
        public object[] conditions;
        public int index;
        public int next;
        public JsonPlayerDialogueOption[] options; /* Player responses */
        public string text;

        public DialogueOption ToDialogueOption(NPCManager npcManager)
        {
            if (npcManager == null)
            {
                return null;
            }

            DialogueOption dialogueOption;
            if (npcManager.GetNPCByName(character) == null)
            {
                /* [TODO]: Uncomment part of constructor */
                dialogueOption = new PlayerDialogueOption(text, null/*, conditions*/, null);
            }
            else
            {
                /* [TODO]: Uncomment part of constructor */
                dialogueOption = new NPCDialogueOption(text, npcManager.GetNPCByName(character)/*, conditions*/);

                dialogueOption.Index = index;

                foreach (JsonPlayerDialogueOption jsonPlayerDialogueOption in options)
                {
                    PlayerDialogueOption playerDialogueOption = jsonPlayerDialogueOption.ToPlayerDialogueOption(npcManager);
                    playerDialogueOption.NPC = npcManager.GetNPCByName(character);

                    dialogueOption.NextOptions.Add(playerDialogueOption);
                }
            }

            return dialogueOption;
        }
    }
}

public abstract class DialogueOption
{
    /* Public Properties */
    public string Text { get; set; }
    public int Index { get; set; }
    public int NextNode { get; set; } /* Index of the next node */
    public List<IDialogueCondition> Conditions /* The conditions before this node can be accessed */
    {
        get { return _Conditions; }
    }
    public Character Character { get; set; } /* The Character that the DialogueOption is attached to */
    public List<DialogueOption> NextOptions { get; set; } = new List<DialogueOption>(); /* The player responses */

    private List<IDialogueCondition> _Conditions = new List<IDialogueCondition>();

    public DialogueOption(string message, Character character)
        : this(message, character, null)
    { }
    public DialogueOption(string message, Character character, List<IDialogueCondition> dialogueConditions)
    {
        Text = message;
        _Conditions = dialogueConditions;
        Character = character;
    }

    public void AddCondition(IDialogueCondition condition)
    {
        _Conditions.Add(condition);
    }
}
