using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Json
{
    [System.Serializable]
    public class JsonStoryNode
    {
        /* These variables MUST be case-sensitive */
        public JsonDialogueOption[] dialogueoptions;

        public StoryNode ToStoryNode()
        {
            List<NPCDialogueOption> npcOptions = new List<NPCDialogueOption>();
            List<PlayerDialogueOption> playerOptions = new List<PlayerDialogueOption>();
            NPCManager npcManager = GameObject.FindGameObjectWithTag("MinimalGame").GetComponent<NPCManager>();

            if (npcManager == null)
            {
                Debug.LogError("JsonStoryNode::ToStoryNode() > Could not find MinimalGame or NPCManager in MinimalGame!");
            }

            foreach (JsonDialogueOption option in dialogueoptions)
            {
                DialogueOption dialogueOption = option.ToDialogueOption(npcManager);

                if (dialogueOption.Character != null)
                {
                    npcOptions.Add(dialogueOption as NPCDialogueOption);
                }
                else
                {
                    playerOptions.Add(dialogueOption as PlayerDialogueOption);
                }
            }

            StoryNode storyNode = new StoryNode(npcOptions, playerOptions);

            return storyNode;
        }
    }
}

public class StoryNode
{
    public List<NPCDialogueOption> NPCDialogueOptions { get; set; }
    public List<PlayerDialogueOption> PlayerDialogueOptions { get; set; }
    public bool WasAlreadySelected { get; set; } = false;
    public List<DialogueOption> CurrentDialogueOptions { get; private set; } = new List<DialogueOption>();
    /* [CRINGE]: Parser starts index at 1 */
    public int CurrentDialogueOptionIndex { get; private set; } = 1;

    /* [TODO]: Add Conditionals to this */

    /* [TODO]: Add Location to this */

    /* [TODO]: Add time to this */

    public StoryNode(List<NPCDialogueOption> npcOptions, List<PlayerDialogueOption> playerOptions)
    {
        NPCDialogueOptions = npcOptions;
        PlayerDialogueOptions = playerOptions;

        foreach (NPCDialogueOption option in NPCDialogueOptions)
        {
            if (option.Index == CurrentDialogueOptionIndex)
            {
                CurrentDialogueOptions.Add(option);
            }
        }
    }
    public void OnDialogueOptionSelected(PlayerDialogueOption dialogueOption)
    {
        CurrentDialogueOptionIndex = dialogueOption.NextNode;

        CurrentDialogueOptions.Clear();
        foreach (NPCDialogueOption option in NPCDialogueOptions)
        {
            if (option.Index == CurrentDialogueOptionIndex)
            {
                CurrentDialogueOptions.Add(option);
            }
        }
    }
}
