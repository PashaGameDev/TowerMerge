using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneCanvas : MonoBehaviour
{

    [SerializeField] private Animator anim = null;
    [SerializeField] AudioSource audio = null;
    [SerializeField] AudioClip clickSFX = null;
    public List<GameObject> sqaudCollection = new List<GameObject>();
   
    private string SceneToLoad = null;

    public void StartBattle(string SceneName)
    {
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
}
