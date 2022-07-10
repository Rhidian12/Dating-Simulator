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
    public List<DialogueCondition> Conditions /* The conditions before this node can be accessed */
    {
        get { return _conditions; }
    }
    public Character Character { get; set; } /* The Character that the DialogueOption is attached to */
    public List<DialogueOption> NextOptions { get; set; } = new List<DialogueOption>(); /* The player responses */

    /* Private Member Variables */
    private List<DialogueCondition> _conditions = new List<DialogueCondition>();
    // private TextMesh _textComponent;

    public DialogueOption(string message, Character character)
        : this(message, character, null)
    { }
    public DialogueOption(string message, Character character, List<DialogueCondition> dialogueConditions)
    {
        Text = message;
        _conditions = dialogueConditions;
        Character = character;

        //_textComponent = GetComponentInChildren<TextMesh>();
        //_textComponent.text = Text;
    }

    public void AddCondition(DialogueCondition condition)
    {
        _conditions.Add(condition);
    }
}
