using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueSystem : MonoBehaviour
{
    /* Private Member Fields */
    private StoryNode _currentStoryNode;
    private StoryManager _storyManager;
    private UIManager _uiManager;

    /* Called from the UI Manager */
    public void OnDialogueOptionClick(DialogueOption selectedDialogueOption)
    {
        /* 'reset' all dialogue boxes */
        _uiManager.StopAllDialogueRendering();

        /* Change the Current Dialogue Options */
        _currentStoryNode.OnDialogueOptionSelected(selectedDialogueOption as PlayerDialogueOption);
    }

    /* Called from the UI Manager */
    public void OnNPCClick(NPC npc)
    {
        if (npc != null)
        {
            _currentStoryNode = _storyManager.GetStoryNode(npc);
        }
    }

    private void Start()
    {
        _storyManager = GetComponent<StoryManager>();
        _uiManager = GetComponent<UIManager>();
    }

    private void Update()
    {
        /* This means an NPC has been clicked at some point and a story node has been started */
        if (_currentStoryNode != null)
        {
            RenderDialogueOptions();
        }
    }

    private void RenderDialogueOptions()
    {
        List<DialogueOption> possibleNPCdialogueOptions = new List<DialogueOption>();
        DialogueOption npcDialogue = null;

        for (int i = 0; i < _currentStoryNode.CurrentDialogueOptions.Count; ++i)
        {
            /* Check if we should render the current dialogue option */
            bool shouldOptionBeRendered = true;

            if (_currentStoryNode.CurrentDialogueOptions[i].Conditions != null && _currentStoryNode.CurrentDialogueOptions[i].Conditions.Count > 0)
            {
                foreach (IDialogueCondition condition in _currentStoryNode.CurrentDialogueOptions[i].Conditions)
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
                possibleNPCdialogueOptions.Add(_currentStoryNode.CurrentDialogueOptions[i]);
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
            _uiManager.StopAllDialogueRendering();
        }
        else
        {
            foreach (DialogueOption dialogueOption in npcDialogue.NextOptions)
            {
                bool shouldOptionBeRendered = true;

                if (dialogueOption.Conditions != null && dialogueOption.Conditions.Count > 0)
                {
                    foreach (IDialogueCondition condition in dialogueOption.Conditions)
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
                    playerResponses.Add(dialogueOption);
                }
            }

            _uiManager.RenderDialogueOptions(npcDialogue, playerResponses);
        }
    }
}
