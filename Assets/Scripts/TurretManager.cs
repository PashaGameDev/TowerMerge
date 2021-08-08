using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TurretManager : MonoBehaviour
{
    [SerializeField] private LayerMask layer = new LayerMask();

    private Vector3 startMousePosition;
    private Vector3 lastMousePosition;
    private Vector3 startTurretPosition; 

    private GameObject chousenTurret = null;
    private GameObject intersectedObject = null;
    private GameObject prefabToInstaniate = null;

    private float mouseOffset = 5f;

    private void Update()
    {
#if UNITY_EDITOR
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            startMousePosition = Input.mousePosition;
        }
        if (Mouse.current.leftButton.isPressed)
        {
            CheckTouchedObject(Input.mousePosition);
        }
        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            lastMousePosition = Input.mousePosition;
           // if (choosenUnit != null) { choosenUnit.layer = 6; }
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
       /* float mouseMoveDis = Vector3.Distance(startMousePosition, lastMousePosition);
        if (mouseMoveDis <= mouseOffset && chousenTurret != null)
        {
           
            choosenUnit.GetComponent<MyUnit>().myCell.SetuUnitOnPlace(null);

            choosenUnit = null;
            CleanChoosenUnit();
            return;
        }*/

        if (chousenTurret != null && intersectedObject != null &&
            chousenTurret.GetComponent<Turret>().level == intersectedObject.GetComponent<Turret>().level &&
            chousenTurret.GetComponent<Turret>().unitType == intersectedObject.GetComponent<Turret>().unitType
            )
        {
            prefabToInstaniate = GameManager.instance.getUnitToCreat(chousenTurret.GetComponent<Turret>().unitType,
                                 intersectedObject.GetComponent<Turret>().level++);

            if (prefabToInstaniate == null) { CleanChoosenUnit(); return; }
            GameObject newUnit = Instantiate(prefabToInstaniate, intersectedObject.transform.position, Quaternion.identity);

            newUnit.GetComponent<Turret>().cell =  intersectedObject.GetComponent<Turret>().cell;
            chousenTurret.GetComponent<Turret>().cell.GetComponent<CellTurret>().turretOnPlace = null;

            Destroy(chousenTurret);
            Destroy(intersectedObject);
        }
        else
        {
            CleanChoosenUnit();
        }
    }

    void CheckTouchedObject(Vector3 mPos)
    {
        Ray ray = Camera.main.ScreenPointToRay(mPos);

        if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layer)) { return; }

        if (chousenTurret != null)
        {
            chousenTurret.transform.position = new Vector3(hit.point.x, chousenTurret.transform.position.y, hit.point.z);
        }

        if (hit.collider.TryGetComponent<Turret>(out Turret turret))
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
        
        if (chousenTurret != null) { intersectedObject = t; return; }
        chousenTurret = t;
        chousenTurret.layer = 8;
        startTurretPosition = chousenTurret.transform.position;
    }

    void CleanChoosenUnit()
    {
        if (chousenTurret != null) { chousenTurret.transform.position = startTurretPosition; }

        chousenTurret = null;
        intersectedObject = null;
    }
}
