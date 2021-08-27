using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    public Text resultTest = null;
    public void RestartLevel()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(0);
    }
}
