using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject infoView;

    public void OpenInfo()
    {
        infoView.SetActive(true);
    }

    public void CloseInfo()
    {
        infoView.SetActive(false);
    }
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
}
