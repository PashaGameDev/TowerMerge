using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameController : MonoBehaviour
{
    [SerializeField] LayerMask rayLayer = new LayerMask();
    [SerializeField] GameObject prefabToInstaniate = null;
    [SerializeField] float mouseOffset = 0.3f;

    private GameObject choosenUnit = null;
    private GameObject intersectedObject = null;
    private Vector3 startPositionChoosenUnit =  Vector3.zero;
    private Vector3 startMousPosition;
    private Vector3 lastMousePosition; 
   
    
    void Update()
    {
#if UNITY_EDITOR
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            startMousPosition = Input.mousePosition;
        }
        if (Mouse.current.leftButton.isPressed)
        {
            CheckTouchedObject(Input.mousePosition);
        }
        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            lastMousePosition = Input.mousePosition;
            if (choosenUnit != null) { choosenUnit.layer = 6; }
            Merge();
        }
#else

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case UnityEngine.TouchPhase.Began:
                    startMousPosition = touch.position;
                    CheckTouchedObject(touch.position);
                    break;

                case UnityEngine.TouchPhase.Moved:
                    CheckTouchedObject(touch.position);

                    break;

                case UnityEngine.TouchPhase.Ended:
                    lastMousePosition = touch.position;
                    if (choosenUnit != null) { choosenUnit.layer = 6; }
                    Merge();

                    break;
            }
        }

#endif
    }

    void Merge()
    {
        float mouseMoveDis = Vector3.Distance(startMousPosition,lastMousePosition);
        if (mouseMoveDis <= mouseOffset && choosenUnit != null)
        {
            choosenUnit.GetComponent<MyUnit>().isCanMove = true;
            choosenUnit.GetComponent<MyUnit>().myCell.SetuUnitOnPlace(null);
                
            choosenUnit = null;
            CleanChoosenUnit();
            return;
        }

        if (choosenUnit != null && intersectedObject != null &&
            choosenUnit.GetComponent<MyUnit>().level == intersectedObject.GetComponent<MyUnit>().level &&
            choosenUnit.GetComponent<MyUnit>().unitType == intersectedObject.GetComponent<MyUnit>().unitType
            )
        {
            prefabToInstaniate = GameManager.instance.getUnitToCreat(choosenUnit.GetComponent<MyUnit>().unitType,
                                 intersectedObject.GetComponent<MyUnit>().level++);

            if(prefabToInstaniate == null){ CleanChoosenUnit(); return; }
            GameObject newUnit = Instantiate(prefabToInstaniate, intersectedObject.transform.position, Quaternion.identity);

            newUnit.GetComponent<MyUnit>().SetCell(intersectedObject.GetComponent<MyUnit>().GetCell());
            choosenUnit.GetComponent<MyUnit>().cleanCell();

            Destroy(choosenUnit);
            Destroy(intersectedObject);
        }
        else
        {
            CleanChoosenUnit();
        }
    }

    void CleanChoosenUnit()
    {
        if (choosenUnit != null) { choosenUnit.transform.position = startPositionChoosenUnit; }

        choosenUnit = null;
        intersectedObject = null;
    }

    void CheckTouchedObject(Vector3 mPos)
    {
        Ray ray = Camera.main.ScreenPointToRay(mPos);

        if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, rayLayer)) { return; }

        if (choosenUnit != null)
        {
            choosenUnit.transform.position = new Vector3(hit.point.x, choosenUnit.transform.position.y, hit.point.z);
        }

        if (hit.collider.TryGetComponent<MyUnit>(out MyUnit myUnit))
        {
            SetChoosenUnit(hit.collider.gameObject);
        }
        else
        {
            intersectedObject = null;
        }

    }
    void SetChoosenUnit(GameObject t)
    {
        //if (t.GetComponent<MyUnit>().isCanMove) { CleanChoosenUnit(); return; }
        if (choosenUnit != null) { intersectedObject = t; return; }
        choosenUnit = t;
        choosenUnit.layer = 8;
        startPositionChoosenUnit = choosenUnit.transform.position;
    }
}
