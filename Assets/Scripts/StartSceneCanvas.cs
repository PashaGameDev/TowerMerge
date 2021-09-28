using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneCanvas : MonoBehaviour
{

    [SerializeField] private Animator anim = null;

    public void StartBattle()
    {
        if (anim == null) { return; }

        anim.SetBool("ButtleRun",true);
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(1.8f);
        SceneManager.LoadScene(1,LoadSceneMode.Single);
    }


   
}
