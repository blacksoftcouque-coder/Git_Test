using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class UI_Title : MonoBehaviour
{
    private UI_Title_Button_GameStart buttonGameStart = null;

    private void Awake()
    {
        buttonGameStart = GetComponentInChildren<UI_Title_Button_GameStart>();
    }
    
    private void Start()
    {
        buttonGameStart.SetEvent(GameStart);
    }

    private void GameStart()
    {
        SceneManager.LoadScene("GameScene");
    }
}
