using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PlayerBaseManager : MonoBehaviour
{
    [SerializeField] private int extraErning = 30;
    [SerializeField] private float countDown = 5f;

    [SerializeField] private Image superShotImage = null;
    [SerializeField] private LayerMask touchLayer = new LayerMask();
    [SerializeField] private GameObject superShootVFX = null;
    [SerializeField] private GameObject superShotIcon = null;
    [SerializeField] private GameObject preShotVFX = null;
    [SerializeField] private Transform partToRotate = null;
    [SerializeField] private GameObject coinsExpVFX = null;
    [SerializeField] private GameObject superShotHightLight = null;

    [SerializeField] private LineRenderer liser;

    [SerializeField] private AudioSource audio = null;
    [SerializeField] private AudioClip superShootSFX = null;
    [SerializeField] private AudioClip creatTurretSFX = null;

    private Vector3 liserStartPosition;

   
    private int superShotPower = 0;
    private float currentTime = 0f;
    private int shootAmount = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        liser.SetPosition(0, Vector3.zero);
        liser.SetPosition(1, Vector3.zero);
        preShotVFX.SetActive(false);
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
            superShotPower = GameManager.instance.superShotPower;
            GameManager.instance.SetSuperShotAmount(1);
            shootAmount = GameManager.instance.GetSuperShotAmount();
            CheckEnemyHighLights();
            superShotImage.GetComponent<Image>().color = new Color(0.8588236f, 0.4705883f, 0.2313726f, 1f);

            if (superShotHightLight != null)
            { superShotHightLight.SetActive(true); }
        }
        else
        {
            if (superShotHightLight != null)
            { superShotHightLight.SetActive(false); }
            currentTime += Time.deltaTime;
            float fillAmount = currentTime / countDown;
            superShotImage.fillAmount = fillAmount;
            superShotImage.GetComponent<Image>().color = new Color(0f, 0.9693694f, 1f, 0.6862745f);
        }
    }

    IEnumerator cleanLaser(float t)
    {
        yield return new WaitForSeconds(t);

        preShotVFX.SetActive(false);
        liser.SetPosition(0, Vector3.zero);
        liser.SetPosition(1, Vector3.zero);

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

    void playSFX(AudioClip clip)
    {
        audio.clip = clip;
        audio.Play();
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
            Destroy(VFX, 2f);

            if ((enemy.GetComponent<Enemy>().getHelth() - superShotPower) <= 0)
            {
                Instantiate(coinsExpVFX, enemy.transform.position, Quaternion.identity);
                GameManager.instance.IncreaseBalance(extraErning);
            }

            preShotVFX.SetActive(true);
            liser.SetPosition(0, partToRotate.position);
            liser.SetPosition(1, enemy.transform.position);

            StartCoroutine(cleanLaser(0.7f));
           
            partToRotate.LookAt(enemy.transform);
            enemy.GetDemage(superShotPower);
            shootAmount--;
            GameManager.instance.SetSuperShotAmount(shootAmount);
            playSFX(superShootSFX);
            //   CheckEnemyHighLights();
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

        GameObject turret = GameManager.instance.getUnitToCreat(0, 0);

        if (turret.GetComponent<Turret>().price > GameManager.instance.currentBalance)
        { return; }

        GameManager.instance.decreaseBalace(turret.GetComponent<Turret>().price);

        Vector3 turretPosition = turretCell.transform.position;
        turretPosition.y += 0.6f;

        turret.GetComponent<Turret>().cell = turretCell.GetComponent<CellTurret>();
        turretCell.GetComponent<CellTurret>().turretOnPlace = Instantiate(turret, turretPosition, Quaternion.identity);
        turretCell.GetComponent<CellTurret>().SetState(true);
        playSFX(creatTurretSFX);
    }

    void LookAtTarget(Transform targetPoit)
    {
            if (partToRotate == null) { partToRotate = transform; }
            Vector3 dir = targetPoit.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(dir);
            Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * (10 * 2.75f)).eulerAngles;
            partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);   
    }

    void CheckEnemyHighLights()
    {
        foreach (var enemy in GameManager.instance.AllEnemies)
        {
            enemy.GetComponent<Helth>().TargetHighLigt();
        }
    }    

}
