using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManager : MonoBehaviour
{
    /* Field is filled through serialisation */
    public List<string> NPCNames;
    public List<NPC> CurrentNPCs { get; set; } = new List<NPC>();
    public static List<NPC> AllNPCs { get; private set; } = new List<NPC>();

    /* ============== UNITY MESSAGES ============== */
    private void Awake()
    {
        foreach (string name in NPCNames)
        {
            if (!AllNPCs.Find(x => x.Name.Equals(name)))
            {
                GameObject go = new GameObject(name);
                NPC npc = go.AddComponent<NPC>();

                npc.Name = name;

                AllNPCs.Add(npc);
            }
        }

        /* [CRINGE]: This is honestly pretty brittle and cringe, because how will we keep the same NPC's between scenes? */
        GameObject[] npcs = GameObject.FindGameObjectsWithTag("NPC");

        foreach (GameObject npcGo in npcs)
        {
            NPC npc = npcGo.GetComponent<NPC>();

            CurrentNPCs.Add(npc);

            if (!AllNPCs.Find(x => x.Name.Equals(npc.Name)))
            {
                AllNPCs.Add(npc);
            }
        }
    }

    public NPC GetNPCByName(string name)
    {
        NPC npc = AllNPCs.Find(x => x.Name.Equals(name));

        return npc;
    }
}
