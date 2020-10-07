using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInfo : MonoBehaviour
{
    public float MAX_MOVABLE_DIS = 6;
    public float MAX_CLAMPHEIGHT = 1;
    public int MAX_HEALTH = 100;
    public int CURRENT_HEALTH = 100;

    public Vector2 CurrentPositionID { get { return currentPosID; } set { currentPosID = value; } }
    private Vector2 currentPosID = Vector2.zero;
}
