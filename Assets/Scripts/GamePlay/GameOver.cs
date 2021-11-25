using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    [SerializeField] private AudioSource audio = null;

    public void RestartLevel()
    {
        audio.Play();
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("StartScene");
    }
}
