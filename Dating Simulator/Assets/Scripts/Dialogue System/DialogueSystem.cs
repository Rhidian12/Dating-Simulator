using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueSystem : MonoBehaviour
{
    /* Private Member Fields */
    private StoryNode _CurrentStoryNode;
    private StoryManager _StoryManager;
    private UIManager _UIManager;

    private void Start()
    {
        _StoryManager = GetComponent<StoryManager>();
        _UIManager = GetComponent<UIManager>();

        if (_StoryManager == null)
        {
            Debug.LogError("DialogueSystem::Start() > StoryManager could not be found!");
        }
        if (_UIManager == null)
        {
            Debug.LogError("DialogueSystem::Start() > UIManager could not be found!");
        }
    }

    private void Update()
    {
        /* This means an NPC has been clicked at some point and a story node has been started */
        /* [CRINGE]: Should this be in Update? I don't think so */
        // if (_CurrentStoryNode != null)
        // {
        //     RenderDialogueOptions();
        // }
    }

    /* Called from the UI Manager */
    public void OnDialogueOptionClick(DialogueOption selectedDialogueOption)
    {
        /* 'reset' all dialogue boxes */
        _UIManager.StopAllDialogueRendering();

        /* Change the Current Dialogue Options */
        _CurrentStoryNode.OnDialogueOptionSelected(selectedDialogueOption as PlayerDialogueOption);
    }

    /* Called from the UI Manager */
    public void OnNPCClick(NPC npc)
    {
        if (npc != null)
        {
            _CurrentStoryNode = _StoryManager.GetStoryNode(npc);

            RenderDialogueOptions();
        }
    }

    private void RenderDialogueOptions()
    {
        List<DialogueOption> possibleNPCdialogueOptions = new List<DialogueOption>();
        DialogueOption npcDialogue = null;

        for (int i = 0; i < _CurrentStoryNode.CurrentDialogueOptions.Count; ++i)
        {
            /* Check if we should render the current dialogue option */
            bool shouldOptionBeRendered = true;

            if (_CurrentStoryNode.CurrentDialogueOptions[i].Conditions != null && _CurrentStoryNode.CurrentDialogueOptions[i].Conditions.Count > 0)
            {
                foreach (IDialogueCondition condition in _CurrentStoryNode.CurrentDialogueOptions[i].Conditions)
                {
                    if (!condition.Execute())
                    {
                        shouldOptionBeRendered = false;
                        break;
                    }
                }
            }

            if (shouldOptionBeRendered)
            {
                possibleNPCdialogueOptions.Add(_CurrentStoryNode.CurrentDialogueOptions[i]);
            }
        }

        if (possibleNPCdialogueOptions.Count == 0)
        {
            Debug.LogError("DialogueSystem::RenderDialogueOptions() > No npc dialogue could be found");
        }
        else if (possibleNPCdialogueOptions.Count == 1)
        {
            npcDialogue = possibleNPCdialogueOptions[0];
        }
        else
        {
            /* Should this be random, ask others */
            /* Should one story node contain all possible routes? */
            npcDialogue = possibleNPCdialogueOptions[Random.Range(0, possibleNPCdialogueOptions.Count)];
        }

        List<DialogueOption> playerResponses = new List<DialogueOption>();

        if (npcDialogue.NextOptions == null && npcDialogue.NextNode == -1)
        {
            _UIManager.StopAllDialogueRendering();
        }
        else
        {
            foreach (DialogueOption dialogueOption in npcDialogue.NextOptions)
            {
                if (AreDialogueConditionsMet(dialogueOption))
                {
                    playerResponses.Add(dialogueOption);
                }
            }

            _UIManager.RenderDialogueOptions(npcDialogue, playerResponses);
        }
    }

    private bool AreDialogueConditionsMet(DialogueOption dialogueOption)
    {
        if (dialogueOption.Conditions != null && dialogueOption.Conditions.Count > 0)
        {
            foreach (IDialogueCondition condition in dialogueOption.Conditions)
            {
                if (!condition.Execute())
                {
                    return false;
                }
            }
        }

        return true;
    }
}
