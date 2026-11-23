using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UI_Title_Button_GameStart : MonoBehaviour
{
    private Button button = null;

    private void Awake()
    {
        button = GetComponent<Button>();    
    }

    public void SetEvent(UnityAction onGameStart)
    {
        button.onClick.AddListener(onGameStart);
    }
}
