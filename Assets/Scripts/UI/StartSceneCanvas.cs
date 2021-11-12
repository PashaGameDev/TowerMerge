using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneCanvas : MonoBehaviour
{

    [SerializeField] private Animator anim = null;
    public List<GameObject> sqaudCollection = new List<GameObject>();
   
    private string SceneToLoad = null;

    public void StartBattle(string SceneName)
    {
        if (anim == null) { return; }

        SceneToLoad = SceneName;
        anim.SetBool("ButtleRun",true);
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(1.8f);
        SceneManager.LoadScene(SceneToLoad,LoadSceneMode.Single);
    }  
}
