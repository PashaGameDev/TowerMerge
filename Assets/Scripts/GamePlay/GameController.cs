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
                   // if (choosenUnit != null) { choosenUnit.layer = 6; }
                    Merge();

                    break;
            }
        }

#endif
    }

    void Merge()
    {
      
        float mouseMoveDis = Vector3.Distance(startMousPosition,lastMousePosition);
        if (mouseMoveDis <= mouseOffset && choosenUnit != null && !choosenUnit.GetComponent<MyUnit>().isOnMerge)
        {
            choosenUnit.GetComponent<MyUnit>().myCell.SetuUnitOnPlace(null, Color.white);
            choosenUnit.GetComponent<MyUnit>().isCanMove = true;
            choosenUnit = null;
            CleanChoosenUnit();
            return;
        }

       
        if (choosenUnit != null && intersectedObject != null &&
            choosenUnit.GetComponent<MyUnit>().level == intersectedObject.GetComponent<MyUnit>().level &&
            choosenUnit.GetComponent<MyUnit>().unitType == intersectedObject.GetComponent<MyUnit>().unitType
            )
        {
            prefabToInstaniate = GameManager.instance.getUnitToCreat(choosenUnit.GetComponent<MyUnit>().supTypeID,
                                 intersectedObject.GetComponent<MyUnit>().level++);

            if(prefabToInstaniate == null){ CleanChoosenUnit(); return; }

            choosenUnit.GetComponent<MyUnit>().isOnMerge = true;
            intersectedObject.GetComponent<MyUnit>().isOnMerge = true;
            choosenUnit.GetComponent<Unit>().anim.SetTrigger("leftMerge");
            intersectedObject.GetComponent<Unit>().anim.SetTrigger("rightMerge");
            StartCoroutine(waitForMergeVFX());

        }
        else
        {
            CleanChoosenUnit();
        }
    }

    IEnumerator waitForMergeVFX()
    {
        yield return new WaitForSeconds(0.3f);

        if (intersectedObject != null)
        {

            Vector3 pos = intersectedObject.transform.position;
            intersectedObject.GetComponent<MyUnit>().GetCell().SpawnMegreEffect();
            CellManager cell = intersectedObject.GetComponent<MyUnit>().GetCell();
            choosenUnit.GetComponent<MyUnit>().cleanCell();

            Destroy(choosenUnit);
            Destroy(intersectedObject);

            GameObject newUnit = Instantiate(prefabToInstaniate, pos, Quaternion.identity);
            newUnit.GetComponent<MyUnit>().isOnMerge = false;
            newUnit.GetComponent<MyUnit>().SetCell(cell);
        }
    }
    void CleanChoosenUnit()
    {
        if (choosenUnit != null) {
            choosenUnit.layer = 6;
            choosenUnit.GetComponent<MyUnit>().isOnMerge = false;
            choosenUnit.transform.position = startPositionChoosenUnit;
        }

        
        //intersectedObject.GetComponent<MyUnit>().isOnMerge = false;

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

        if (hit.collider.TryGetComponent<MyUnit>(out MyUnit myUnit) )
        {
            if (hit.collider.gameObject.GetComponent<MyUnit>().isOnMerge) { return; }
            SetChoosenUnit(hit.collider.gameObject);
        }
        else
        {
            if (hit.collider.gameObject.GetComponent<MyUnit>() == null || hit.collider.gameObject.GetComponent<MyUnit>().isOnMerge) { return; }
            intersectedObject = null;
        }

    }
    void SetChoosenUnit(GameObject t)
    {
        //if (t.GetComponent<MyUnit>().isCanMove) { CleanChoosenUnit(); return; }
        if (choosenUnit != null) { intersectedObject = t; return; }
        choosenUnit = t;
      //  t.GetComponent<MyUnit>().isOnMerge = true;
        choosenUnit.layer = 8;
        startPositionChoosenUnit = choosenUnit.transform.position;
    }
}
