using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    static public int MaxRelationshipValue
    {
        get => _maxRelationshipValue;
    }
    static public int MinRelationshipValue
    {
        get => _minRelationshipValue;
    }

    public string Name;

    static protected int _maxRelationshipValue = 100;
    static protected int _minRelationshipValue = 0;

    public abstract void OnDialogueOptionSelected(PlayerDialogueOption dialogueOption);
}
