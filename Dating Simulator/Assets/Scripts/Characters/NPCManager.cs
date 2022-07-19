using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManager : MonoBehaviour
{
    public List<GameObject> NPCGameObjects
    {
        get { return _NPCGameObjects; }
    }
    public List<NPC> NPCs
    {
        get { return _AllNPCs; }
    }
    private List<NPC> _AllNPCs = new List<NPC>();
    [SerializeField] private List<string> _NPCNames;
    [SerializeField] private List<GameObject> _NPCPrefabs;
    private List<GameObject> _NPCGameObjects = new List<GameObject>();

    /* ============== UNITY MESSAGES ============== */
    private void Awake()
    {
        if (_NPCPrefabs.Count != _NPCNames.Count)
        {
            Debug.LogError("NPCManager::Awake() > There is an unequal amount of names and prefabs!");
            return;
        }

        for (int i = 0; i < _NPCPrefabs.Count; ++i)
        {
            /* If the name is not in the list of NPC's add it */
            if (!_AllNPCs.Find(x => x.Name.Equals(_NPCNames[i])))
            {
                GameObject go = Instantiate(_NPCPrefabs[i]);
                NPC npc = go.GetComponent<NPC>();

                npc.Name = name;

                _AllNPCs.Add(npc);

                go.SetActive(false);

                _NPCGameObjects.Add(go);
            }
        }

        GameObject[] npcs = GameObject.FindGameObjectsWithTag("NPC");

        foreach (GameObject gameObject in npcs)
        {
            /* Find corresponding NPC in our List */
            NPC npc = _AllNPCs.Find(x => x.gameObject.name.Contains(gameObject.name));

            if (npc == null)
            {
                Debug.LogError("NPCManager::Awake() > A NPC with the name " + gameObject.name + " could not be found!");
                break;
            }

            npc.gameObject.transform.position = gameObject.transform.position;
            npc.gameObject.SetActive(true);
            Destroy(gameObject);
        }
    }

    public NPC GetNPCByName(string name)
    {
        NPC npc = _AllNPCs.Find(x => x.Name.Equals(name));

        return npc;
    }
}
