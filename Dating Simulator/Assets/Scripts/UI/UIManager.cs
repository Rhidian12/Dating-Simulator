using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    /* Public Member Fields */
    public int MaxNrOfDialogueOptionsToDisplay { get; private set; } = 3;

    [SerializeField] private GameObject PlayerDialogueOptionPrefab;
    [SerializeField] private GameObject NPCDialogueOptionPrefab;
    [SerializeField] private List<Transform> PlayerDialogueOptionLocations;
    [SerializeField] private Transform NPCDialogueOptionLocation;

    private List<GameObject> _PlayerDialogueOptionGameObjects = new List<GameObject>();
    private List<DialogueOption> _CurrentPlayerDialogueOptions = new List<DialogueOption>();
    private List<TextMeshPro> _PlayerDialogueOptionsText = new List<TextMeshPro>();

    private GameObject _NPCDialogueOptionGameObject = null;
    private DialogueOption _CurrentNPCDialogueOption = null;
    private TextMeshPro _NPCDialogueOptionsText = null;

    private DialogueSystem _DialogueSystem;
    private LocationManager _LocationManager;

    private void Start()
    {
        if (PlayerDialogueOptionPrefab == null || PlayerDialogueOptionLocations.Count != MaxNrOfDialogueOptionsToDisplay)
        {
            Debug.LogError("UIManager::Start() > PlayerDialogueOption had an error in it");
            return;
        }

        if (NPCDialogueOptionPrefab == null || NPCDialogueOptionLocation == null)
        {
            Debug.LogError("UIManager::Start() > NPCDialogueOption had an error in it");
            return;
        }

        /* Spawn the Player Dialogue Boxes but disable them */
        for (int i = 0; i < MaxNrOfDialogueOptionsToDisplay; i++)
        {
            GameObject dialogueOption = Instantiate(PlayerDialogueOptionPrefab, PlayerDialogueOptionLocations[i].position, Quaternion.identity);
            _PlayerDialogueOptionGameObjects.Add(dialogueOption);
            dialogueOption.SetActive(false);

            _PlayerDialogueOptionsText.Add(dialogueOption.GetComponentInChildren<TextMeshPro>());
        }

        /* Spawn the NPC Dialogue Box */
        GameObject npcDialogue = Instantiate(NPCDialogueOptionPrefab, NPCDialogueOptionLocation.position, Quaternion.identity);
        _NPCDialogueOptionGameObject = npcDialogue;
        _NPCDialogueOptionGameObject.SetActive(false);
        _NPCDialogueOptionsText = _NPCDialogueOptionGameObject.GetComponentInChildren<TextMeshPro>();

        _DialogueSystem = GetComponent<DialogueSystem>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            OnClick();
        }
    }

    public void RenderDialogueOptions(DialogueOption npcDialogueOption, List<DialogueOption> playerDialogueOptions)
    {
        /* If there are no dialogue options to be rendered, make sure all the dialogue options aren't being rendered */
        if (playerDialogueOptions == null || playerDialogueOptions.Count == 0)
        {
            foreach (GameObject dialogueOption in _PlayerDialogueOptionGameObjects)
            {
                dialogueOption.SetActive(false);
            }

            Debug.LogError("UIManager::RenderDialogueOptions() > There were no player responses to render, function was terminated");

            return;
        }

        if (npcDialogueOption == null)
        {
            Debug.LogError("UIManager::RenderDialogueOptions() > There were no NPC responses to render, function was terminated");

            return;
        }

        /* Render the Player options */
        _CurrentPlayerDialogueOptions = playerDialogueOptions;

        /* Render the given dialogue options */
        for (int i = 0; i < playerDialogueOptions.Count; i++)
        {
            /* Make sure we're only displaying as many dialogue options as are allowed */
            if (i < MaxNrOfDialogueOptionsToDisplay)
            {
                _PlayerDialogueOptionGameObjects[i].SetActive(true);

                /* [CRINGE]: What if text goes out of bounds because it's too long? */
                /* Daphné's job though lmao */
                // _dialogueOptions[i].Text = dialogueOptions[i].Text;
                _PlayerDialogueOptionsText[i].text = playerDialogueOptions[i].Text;
            }
            else
            {
                Debug.LogError("UIManager::RenderDialogueOptions() > Too many dialogue options were passed!");
            }
        }

        /* Render the NPC dialogue */
        _CurrentNPCDialogueOption = npcDialogueOption;

        _NPCDialogueOptionGameObject.SetActive(true);

        /* [CRINGE]: What if text goes out of bounds because it's too long? */
        _NPCDialogueOptionsText.text = npcDialogueOption.Text;
    }

    public void StopAllDialogueRendering()
    {
        foreach (GameObject dialogueOption in _PlayerDialogueOptionGameObjects)
        {
            dialogueOption.SetActive(false);
        }

        _NPCDialogueOptionGameObject.SetActive(false);
    }

    private void OnClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);

        if (hit)
        {
            if (hit.collider.CompareTag("DialogueOption"))
            {
                string text = hit.collider.gameObject.GetComponentInChildren<TextMeshPro>().text;

                DialogueOption dialogueOption = _CurrentPlayerDialogueOptions.Find(x => x.Text.Equals(text));

                if (dialogueOption != null)
                {
                    _DialogueSystem.OnDialogueOptionClick(dialogueOption);
                }
                else
                {
                    Debug.LogError("UIManager::OnClick() > Clicked Dialogue Option could not be found!");
                }
            }
            else if (hit.collider.CompareTag("Location"))
            {
                _LocationManager.OnLocationSelected(hit.collider.gameObject.GetComponent<Location>());
            }
            else if (hit.collider.CompareTag("NPC"))
            {
                _DialogueSystem.OnNPCClick(hit.collider.GetComponent<NPC>());
            }
        }
    }
}
