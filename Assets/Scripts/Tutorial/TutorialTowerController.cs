using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class TutorialTowerController : MonoBehaviour
{
    [SerializeField] TutorialController _tController = null;
    [SerializeField] private LayerMask touchLayer = new LayerMask();
    [SerializeField] private LineRenderer liser;
    [SerializeField] private GameObject superShootVFX = null;
    [SerializeField] private Transform partToRotate = null;

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            if (_tController.tutorialStep < 4) { return; }
            SuperShotAction(Input.mousePosition);
        }
#else
        if (Input.touchCount > 0)
        {
           Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case UnityEngine.TouchPhase.Began:
                    
                    break;

                case UnityEngine.TouchPhase.Moved:
                    break;

                case UnityEngine.TouchPhase.Ended:
                 SuperShotAction(touch.position);
                    break;
            }
        }
#endif

    }

    void SuperShotAction(Vector3 inputPos)
    {
        Ray ray = Camera.main.ScreenPointToRay(inputPos);

        if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, touchLayer)) { return; }
        if (hit.transform.gameObject.layer == 9)
        {
            
            DummyEnemy enemy = hit.transform.gameObject.GetComponent<DummyEnemy>();
            if (enemy == null) return;

            GameObject VFX = Instantiate(superShootVFX, hit.transform.position, Quaternion.identity);
            Destroy(VFX, 2f);

            liser.SetPosition(0, partToRotate.position);
            liser.SetPosition(1, enemy.transform.position);

            StartCoroutine(cleanLaser(0.7f));

            partToRotate.LookAt(enemy.transform);
            enemy.gameObject.GetComponent<DummyEnemy>().GetDemage(1000);
            //Destroy(enemy.gameObject);
        } 
    }
    IEnumerator cleanLaser(float t)
    {
        yield return new WaitForSeconds(t);
        liser.SetPosition(0, Vector3.zero);
        liser.SetPosition(1, Vector3.zero);

    }
}
