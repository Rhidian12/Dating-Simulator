using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    static public int MaxRelationshipValue
    {
        get => _MaxRelationshipValue;
    }
    static public int MinRelationshipValue
    {
        get => _MinRelationshipValue;
    }

    /* Properties aren't available in the editor cus Unity cringe */
    public string Name;

    static protected int _MaxRelationshipValue = 100;
    static protected int _MinRelationshipValue = 0;

    public abstract void OnDialogueOptionSelected(PlayerDialogueOption dialogueOption);
}
