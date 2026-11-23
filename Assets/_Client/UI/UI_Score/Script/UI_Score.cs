using UnityEngine;

public class UI_Score : MonoBehaviour
{
    // GameManager에서 점수를 세고 있음.
    // UIManager를 통해서 UIScore에 점수를 전달하고, UIScore는 자식에 있는 UIScoreText의 Text에서 Score의 값을 변경하면 됨.

    private UI_Score_Text textScore = null;

    private void Awake()
    {
        textScore = GetComponentInChildren<UI_Score_Text>();
    }

    public void UpdateScore(int score)
    {
        textScore.SetTextScore(score);
    }
}
