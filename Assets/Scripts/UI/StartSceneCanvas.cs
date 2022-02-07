using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneCanvas : MonoBehaviour
{
    [SerializeField] private Animator anim = null;
    [SerializeField] AudioSource audio = null;
    [SerializeField] AudioClip clickSFX = null;
    [SerializeField] private UnitDataBase unitData = null;
    [SerializeField] private Animator textAnim = null;
    [SerializeField] private MainMenuController mContoller = null;
   // public List<GameObject> sqaudCollection = new List<GameObject>();
   
    private string SceneToLoad = null;

    public void StartBattle(string SceneName)
    {
        if (!CheckSquad()) { return; }
        if (anim == null) { return; }
        if (audio != null && clickSFX != null)
        {
            audio.clip = clickSFX;
            audio.Play();
        }

        SceneToLoad = SceneName;
        anim.SetBool("ButtleRun",true);
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(1.8f);
        SceneManager.LoadScene(SceneToLoad,LoadSceneMode.Single);
    }

    bool CheckSquad()
    {
        if (unitData.squad.Count == unitData.GetSquadSize())
        {
            return true;
        }
        else
        {
            textAnim.SetTrigger("ErrorAlertText");
            ShakeSpots();
            return false;
        }
    }

    void ShakeSpots()
    {
        if (mContoller == null) { return; }

        foreach (var spot in mContoller.squadSpots)
        {
           
            if (spot.gameObject.GetComponent<SetUnitImage>().isAvailable)
            {
                spot.gameObject.GetComponent<Animator>().SetTrigger("isShake");
            }
        }

    }
    IEnumerator turnOffShake(GameObject spot)
    {
        yield return new WaitForSeconds(1.0f);
        spot.GetComponent<Animator>().SetBool("isClicked", false);
    }
}
