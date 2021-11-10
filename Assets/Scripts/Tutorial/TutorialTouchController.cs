using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TutorialTouchController : MonoBehaviour
{

    [SerializeField] private TutorialController controller = null;
    [SerializeField] float mouseOffset = 0.7f;
    [SerializeField] LayerMask rayLayer = new LayerMask();

    Vector3 startPositionChoosenUnit = Vector3.zero;
    Vector3 startMousPosition = Vector3.zero;
    Vector3 lastMousePosition = Vector3.zero;

    private void Update()
    {

        if (controller.tutorialStep == 2 || controller.tutorialStep == 6 || controller.tutorialStep == 5)
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
                if (controller.choosenUnit != null) { controller.choosenUnit.layer = 6; }

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
                        if (controller.choosenUnit != null) { controller.choosenUnit.layer = 6; }
                        Merge();

                        break;
                }
            }

#endif
        }
    }

    void Merge()
    {
      
        float mouseMoveDis = Vector3.Distance(startMousPosition, lastMousePosition);
        if (mouseMoveDis <= mouseOffset && controller.choosenUnit != null)
        {
            
            if (controller.tutorialStep == 2 || controller.tutorialStep == 6)
            {
              
                controller.choosenUnit.GetComponent<TutorialUnit>().Touched();
                controller.SetPopupState(false, "",0f);

                controller.choosenUnit = null;
                controller.cell1.GetComponent<Renderer>().material.color = Color.white;
                controller.cell2.GetComponent<Renderer>().material.color = Color.white;

                CleanChoosenUnit();
                return;
            }


        }
        if (controller.choosenUnit != null && controller.intersectedObject != null)
        {
            Vector3 instPost = controller.cell2.transform.position;
            GameObject unit3 = Instantiate(controller.tutorialUnit2, instPost, Quaternion.identity);
            unit3.GetComponent<TutorialUnit>().SetController(this.gameObject.GetComponent<TutorialController>());

            controller.SetPopupState(true, "Great Job!!! Tap to send Unit on battle ",0f);
            controller.CreatEnemy(unit3.GetComponent<TutorialUnit>());
            controller.tutorialStep = 6;
            controller.cell1.GetComponent<Renderer>().material.color = Color.white;
            controller.cell2.GetComponent<Renderer>().material.color = Color.yellow;
            Destroy(controller.choosenUnit);
            Destroy(controller.intersectedObject);
        }
        else
        {
            CleanChoosenUnit();
        }
    }
    void CheckTouchedObject(Vector3 mPos)
    {
        Ray ray = Camera.main.ScreenPointToRay(mPos);

        if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, rayLayer)) { return; }

        if (controller.choosenUnit != null)
        {
            controller.choosenUnit.transform.position = new Vector3(hit.point.x, controller.choosenUnit.transform.position.y, hit.point.z);
            
        }

        if (hit.collider.TryGetComponent<TutorialUnit>(out TutorialUnit tUnit))
        {
            SetChoosenUnit(hit.collider.gameObject);
        }
        else
        {
            controller.intersectedObject = null;
        }

    }
    void SetChoosenUnit(GameObject t)
    {
        if (controller.choosenUnit != null) { controller.intersectedObject = t; return; }

        controller.choosenUnit = t;
        controller.choosenUnit.layer = 8;

        startPositionChoosenUnit = controller.choosenUnit.transform.position;
    }


    void CleanChoosenUnit()
    {
        if (controller.choosenUnit != null) {controller.choosenUnit.transform.position = startPositionChoosenUnit; }
        controller.choosenUnit = null;
        controller.intersectedObject = null;
    }

}
