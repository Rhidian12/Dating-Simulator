using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Json;

public class StoryManager : MonoBehaviour
{
    public Dictionary<NPC, List<StoryNode>> StoryNodes { get; private set; } = new Dictionary<NPC, List<StoryNode>>();

    private StoryNode _CurrentStoryNode;

    private void Start()
    {
        NPCManager npcManager = GetComponent<NPCManager>();

        if (npcManager != null)
        {
            foreach (NPC npc in npcManager.NPCs)
            {
                StoryNodes.Add(npc, new List<StoryNode>());
            }
        }
        else
        {
            Debug.LogError("StoryManager::Start() > Failed to get NPCManager!");
        }
    }

    public StoryNode GetStoryNode(NPC npc)
    {
        /* Do we already have this story node? */
        if (StoryNodes.ContainsKey(npc))
        {
            if (StoryNodes[npc].Count > 0)
            {
                /* [TODO]: Get Location */
                /* [TODO]: Get day */
                /* [TODO]: Get time of day */

                // List<StoryNode> nodes = StoryNodes[npc].FindAll(/*x => x.Location && x.Time*/);

                /* [TODO]: Don't make this random, make it prioritize unchosen options */
                /* If there are no unchosen options check if there is one we can read in */
                _CurrentStoryNode = StoryNodes[npc][Random.Range(0, StoryNodes[npc].Count - 1)];
                return _CurrentStoryNode;
            }
        }

        /* Read the story node from the file */
        /* [TODO]: Don't hardcode this path */
        /* This will require a Serialized field for the path to the folder, but then we need to check which Node we need to check (0, 1, 2 or end) */
        Json.JsonStoryNode node = Json.JsonReader.ReadFile<Json.JsonStoryNode>("Assets/_Dev/Output.txt");
        StoryNode storyNode = node.ToStoryNode();

        StoryNodes[npc].Add(storyNode);

        return storyNode;
    }
}
