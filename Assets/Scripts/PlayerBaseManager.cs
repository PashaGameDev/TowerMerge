using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PlayerBaseManager : MonoBehaviour
{
    [SerializeField] private int extraErning = 30;
    [SerializeField] private float countDown = 5f;
    [SerializeField] private int superShotPower = 100;
    [SerializeField] private Image superShotImage = null;
    [SerializeField] private LayerMask touchLayer = new LayerMask();
    [SerializeField] private GameObject superShootVFX = null;
    [SerializeField] private GameObject superShotIcon = null;

    [SerializeField] private LineRenderer liser;
    private Vector3 liserStartPosition;


    private float currentTime = 0f;
    private int shootAmount = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SuperShotCountDown();
        SuperShot();
    }

    void SuperShotCountDown()
    {
        if (currentTime >= countDown)
        {
            shootAmount = 1;
           // superShotIcon.SetActive(true);
        }
        else
        {
            // superShotIcon.SetActive(false);
            liser.SetPosition(0, Vector3.zero);
            liser.SetPosition(1, Vector3.zero);

            currentTime += Time.deltaTime;
            float fillAmount = currentTime / countDown;
            superShotImage.fillAmount = fillAmount;
        }
    }

    void SuperShot()
    {
       
#if UNITY_EDITOR
        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            SuperShotAction(Input.mousePosition);
        }
#else
        if (Input.touchCount > 0)
        {
           Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case UnityEngine.TouchPhase.Began:
                     SuperShotAction(touch.position);
                    break;

                case UnityEngine.TouchPhase.Moved:
                    

                    break;

                case UnityEngine.TouchPhase.Ended:
                    

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
            if (shootAmount == 0) { return; }
            Enemy enemy = hit.transform.gameObject.GetComponent<Enemy>();
            if (enemy == null) return;

            GameObject VFX = Instantiate(superShootVFX, hit.transform.position, Quaternion.identity);
            Destroy(VFX, 1f);

            if ((enemy.GetComponent<Enemy>().getHelth() - superShotPower) <= 0) { GameManager.instance.IncreaseBalance(extraErning); }

            liser.SetPosition(0, gameObject.transform.position);
            liser.SetPosition(1, enemy.transform.position);

            enemy.GetDemage(superShotPower);
            shootAmount--;
            currentTime = 0f;
        }
        else if (hit.transform.gameObject.layer == 11)
        {
            tryBuildTurret(hit.transform.gameObject);
        }
    }

    void tryBuildTurret(GameObject turretCell)
    {
        if (turretCell.GetComponent<CellTurret>() == null) { return; }
        if (turretCell.GetComponent<CellTurret>().turretOnPlace != null) { return; }

        Vector3 turretPosition = turretCell.transform.position;
        turretPosition.y += 0.6f;

        GameObject turret = GameManager.instance.getUnitToCreat(0,0);

        turret.GetComponent<Turret>().cell = turretCell.GetComponent<CellTurret>();
        turretCell.GetComponent<CellTurret>().turretOnPlace = Instantiate(turret, turretPosition, Quaternion.identity);
    }
}
