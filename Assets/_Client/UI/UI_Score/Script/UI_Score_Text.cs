using UnityEngine;
using UnityEngine.UI;

public class UI_Score_Text : MonoBehaviour
{
    private Text textScore = null;

    private void Awake()
    {
        textScore = GetComponent<Text>();
    }

    public void SetTextScore(int score)
    {
        textScore.text = $"Score : {score}";
    }
}
