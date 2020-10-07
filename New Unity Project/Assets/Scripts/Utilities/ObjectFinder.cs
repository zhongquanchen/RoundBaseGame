using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFinder : MonoBehaviour
{
    public static ObjectFinder Instance;

    public Material[] walkMaterial;
    public GameObject[] characterTemplates;

    private void Start() => Instance = this;

    public Material FindMaterial(WalkDescription describe)
    {
        switch (describe)
        {
            case WalkDescription.Walkable:
                return walkMaterial[0];
            default:
                return walkMaterial[1];
        }
    } 

    public GameObject FindCharacterByID(CharacterID characterID)
    {
        return characterTemplates[(int)characterID];
    }
}
