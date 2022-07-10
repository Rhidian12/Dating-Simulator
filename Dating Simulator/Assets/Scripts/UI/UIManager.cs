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

    private List<GameObject> _playerDialogueOptionGameObjects = new List<GameObject>();
    private List<DialogueOption> _currentPlayerDialogueOptions = new List<DialogueOption>();
    private List<TextMeshPro> _playerDialogueOptionsText = new List<TextMeshPro>();

    private GameObject _npcDialogueOptionGameObject = null;
    private DialogueOption _currentNPCDialogueOption = null;
    private TextMeshPro _npcDialogueOptionsText = null;

    private DialogueSystem _dialogueSystem;
    private LocationManager _locationManager;

    public void RenderDialogueOptions(DialogueOption npcDialogueOption, List<DialogueOption> playerDialogueOptions)
    {
        /* If there are no dialogue options to be rendered, make sure all the dialogue options aren't being rendered */
        if (playerDialogueOptions == null || playerDialogueOptions.Count == 0)
        {
            foreach (GameObject dialogueOption in _playerDialogueOptionGameObjects)
            {
                dialogueOption.SetActive(false);
            }

            Debug.LogWarning("UIManager::RenderDialogueOptions() > There were no player responses to render, function was terminated");

            return;
        }

        if (npcDialogueOption == null)
        {
            Debug.LogWarning("UIManager::RenderDialogueOptions() > There were no NPC responses to render, function was terminated");

            return;
        }

        /* Render the Player options */
        _currentPlayerDialogueOptions = playerDialogueOptions;

        /* Render the given dialogue options */
        for (int i = 0; i < playerDialogueOptions.Count; i++)
        {
            /* Make sure we're only displaying as many dialogue options as are allowed */
            if (i < MaxNrOfDialogueOptionsToDisplay)
            {
                _playerDialogueOptionGameObjects[i].SetActive(true);

                /* [CRINGE]: What if text goes out of bounds because it's too long? */
                /* Daphné's job though lmao */
                // _dialogueOptions[i].Text = dialogueOptions[i].Text;
                _playerDialogueOptionsText[i].text = playerDialogueOptions[i].Text;
            }
            else
            {
                Debug.LogError("UIManager::RenderDialogueOptions() > Too many dialogue options were passed!");
            }
        }

        /* Render the NPC dialogue */
        _currentNPCDialogueOption = npcDialogueOption;

        _npcDialogueOptionGameObject.SetActive(true);

        /* [CRINGE]: What if text goes out of bounds because it's too long? */
        _npcDialogueOptionsText.text = npcDialogueOption.Text;
    }

    public void StopAllDialogueRendering()
    {
        foreach (GameObject dialogueOption in _playerDialogueOptionGameObjects)
        {
            dialogueOption.SetActive(false);
        }

        _npcDialogueOptionGameObject.SetActive(false);
    }

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
            _playerDialogueOptionGameObjects.Add(dialogueOption);
            dialogueOption.SetActive(false);

            _playerDialogueOptionsText.Add(dialogueOption.GetComponentInChildren<TextMeshPro>());
        }

        /* Spawn the NPC Dialogue Box */
        GameObject npcDialogue = Instantiate(NPCDialogueOptionPrefab, NPCDialogueOptionLocation.position, Quaternion.identity);
        _npcDialogueOptionGameObject = npcDialogue;
        _npcDialogueOptionGameObject.SetActive(false);
        _npcDialogueOptionsText = _npcDialogueOptionGameObject.GetComponentInChildren<TextMeshPro>();

        _dialogueSystem = GetComponent<DialogueSystem>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            OnClick();
        }
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

                DialogueOption dialogueOption = _currentPlayerDialogueOptions.Find(x => x.Text.Equals(text));

                if (dialogueOption != null)
                {
                    _dialogueSystem.OnDialogueOptionClick(dialogueOption);
                }
                else
                {
                    Debug.LogError("UIManager::OnClick() > Clicked Dialogue Option could not be found!");
                }
            }
            else if (hit.collider.CompareTag("Location"))
            {
                _locationManager.OnLocationSelected(hit.collider.gameObject.GetComponent<Location>());
            }
            else if (hit.collider.CompareTag("NPC"))
            {
                _dialogueSystem.OnNPCClick(hit.collider.GetComponent<NPC>());
            }
        }
    }
}
