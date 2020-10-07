using UnityEngine.AI;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class CharacterControllor : MonoBehaviour
{
    public bool AllowToMove = false;

    public NavMeshAgent agent;
    public Slider healthBar;

    public List<Tile> movableTiles = new List<Tile>();

    private void FixedUpdate()
    {
        if (!AllowToMove)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 1000f))
            {
                if (hit.transform != null)
                {
                    var tileX = (int)hit.transform.position.x;
                    var tileY = (int)hit.transform.position.z;
                    var tile = movableTiles.FirstOrDefault(x => x.X == tileX && x.Y == tileY);
                    SetChacterDest(tile);
                }
            }
        }
    }

    #region Moving Character
    public void ShowMovableGrids()
    {
        movableTiles.Clear();
        var characterinfo = GetComponent<CharacterInfo>();
        movableTiles = MapControllor.Instance.ShowMovableTiles(characterinfo);
        AllowToMove = true;
    }

    public void HideMovableGrids()
    {
        AllowToMove = false;
        MapControllor.Instance.ActivateAllGrids(false);
    }

    public void SetChacterDest(Tile tile)
    {
        if (tile == null)
            return;

        var Position = MapControllor.Instance.Grids[tile.Y, tile.X].transform.position;

        agent.SetDestination(Position);

        GetComponent<CharacterInfo>().CurrentPositionID = new Vector2(tile.X, tile.Y);

        HideMovableGrids();
    }
    #endregion
}
