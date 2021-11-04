using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
  

    public void RestartLevel()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("StartScene");
    }
}
