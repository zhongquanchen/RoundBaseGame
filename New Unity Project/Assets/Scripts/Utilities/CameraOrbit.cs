using UnityEngine;
using System.Collections;

public class CameraOrbit : MonoBehaviour
{
#if UNITY_IOS || UNITY_ANDROID
    private Camera cam;
    [SerializeField] private Transform target;
    private Vector3 previousPos;
    private Plane Plane;
    public float ZoomSensitive = .1f;
    public static bool AllowCamera = true;
    private void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        if (!AllowCamera)
            return;

        //if (Input.touchCount >= 1 && Input.touchCount < 2)
        //    Plane.SetNormalAndPosition(transform.up, transform.position);

        //if (Input.GetMouseButtonDown(0) && Input.touchCount == 1)
        //    previousPos = cam.ScreenToViewportPoint(Input.mousePosition);

        //if (Input.GetMouseButton(0) && Input.touchCount == 1)
        //{
        //    Vector3 dir = previousPos - cam.ScreenToViewportPoint(Input.mousePosition);
        //    target.transform.Rotate(new Vector3(0, 1, 0), -dir.x * 180, Space.World);
        //    previousPos = cam.ScreenToViewportPoint(Input.mousePosition);
        //}

        //if (Input.touchCount >=2)
        //{
        //    var pos1b = Input.GetTouch(0).deltaPosition;
        //    cam.orthographicSize += pos1b.x * ZoomSensitive;
        //    cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, 5f, 40f);
        //}

        if (Input.GetKey(KeyCode.UpArrow))
            target.transform.position += new Vector3(.5f, 0, .5f);
        if (Input.GetKey(KeyCode.DownArrow))
            target.transform.position -= new Vector3(.5f, 0, .5f);
        if (Input.GetKey(KeyCode.RightArrow))
            target.transform.position += new Vector3(.5f, 0, -0.5f);
        if (Input.GetKey(KeyCode.LeftArrow))
            target.transform.position += new Vector3(-.5f, 0, .5f);
    }

    private Vector3 PlanePositionDelta(Touch touch)
    {
        if (touch.phase != TouchPhase.Moved)
            return Vector3.zero;

        var rayBefore = cam.ScreenPointToRay(touch.position - touch.deltaPosition);
        var rayNow = cam.ScreenPointToRay(touch.position);
        if (Plane.Raycast(rayBefore, out var enterBefore) && Plane.Raycast(rayNow, out var enterNow))
            return rayBefore.GetPoint(enterBefore) - rayNow.GetPoint(enterNow);

        return Vector3.zero;
    }

    private Vector3 PlanePosition(Vector2 screenPos)
    {
        var rayNow = cam.ScreenPointToRay(screenPos);
        if (Plane.Raycast(rayNow, out var enterNow))
            return rayNow.GetPoint(enterNow);
        return Vector3.zero;
    }
#endif
}