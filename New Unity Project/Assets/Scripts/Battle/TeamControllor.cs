using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterID { WARRIOR, ARCHOR, MAGE }

public class TeamControllor : MonoBehaviour
{
    private int teamNumber = 0;
    public Transform[] teamSpone;
    public GameObject[] CharacterTemplates;

    public void InstantiateCharacter( CharacterID characterID )
    {
        var gb = Instantiate(CharacterTemplates[(int)characterID]);
        gb.transform.position = teamSpone[teamNumber].position;
        gb.transform.SetParent(transform);

        teamNumber += 1;
    }

    public void ActivateMovement(bool activate)
    {

    }
}
