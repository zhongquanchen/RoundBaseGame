using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridControlor : MonoBehaviour
{
    public GameObject GridsTemplate;
    public Vector2 GridID;

    public List<int> NeighborTiles = new List<int>();

    public void InstantiateGrid()
    {
        var gb = Instantiate(GridsTemplate);
        var pos = transform.position;
        gb.name = "tile";
        gb.transform.position = new Vector3(pos.x, pos.y + 0.1f, pos.z);
        gb.transform.SetParent(gameObject.transform);

        int X = (int)gb.transform.position.x;
        int Y = (int)gb.transform.position.z;
        MapControllor.Instance.SetupGrids(X, Y, gameObject);
    }

    public void ActivateGrids(bool activate)
    {
        transform.Find("tile").gameObject.SetActive(activate);
    }

    public void SetUpGridMat(WalkDescription walk)
    {
        var gb = transform.Find("tile").gameObject;
        gb.GetComponent<Renderer>().material =
            ObjectFinder.Instance.FindMaterial(walk);
    }
}
