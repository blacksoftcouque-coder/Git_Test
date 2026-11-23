using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // 간단한 슈팅 게임.
    
    // Player, Enemy, Item.
    
    // Player
    // WASD로 움직일 수 있어야 함.
    // 마우스로 플레이어를 회전 할 수 있게 해야 함.
    // 마우스 왼쪽 버튼 누르면, 총에서 총알 발사.
    // 데미지 받기.
    // Item 먹기.
    
    // Enemy
    // Player를 추적하는 기능.
    // 일정 거리 안에 오면 Player를 공격.
    // 데미지 받기.
    // 죽을 때, 게임 점수 올려주기.

    // Item
    // 필드에 랜덤 생성.
    // Player와 충돌 시 플레이어에게 효과 부여.
    // Heal Item. (시간이 나면 이속 등 Item 효과 다양화해보기.)

    // Game
    // 게임 시작,
    // 게임 클리어, (점수가 100점 이상)
    // 게임 종료. (플레이어가 죽었을 때.)
    // 게임에 관련된 모든 것들을 관리하는 역할.

    // Sound, Effect.
    
    // UI
    // 게임 시작, 게임 클리어, 게임 종료 시 UI를 띄워주는 역할.
    private Player player = null;
    private EnemyManager enemyManager = null;
    private UIManager uiManager = null;


    private int score = 0;

    private void Awake()
    {
        score = 0;
        player = GetComponentInChildren<Player>();
        enemyManager = GetComponentInChildren<EnemyManager>();
        uiManager = GetComponentInChildren<UIManager>();
    }

    private void Start()
    {
        player.Init(OnGameOver);
        enemyManager.Init(player, AddScore);
        uiManager.Init();
        uiManager.UpdateScoreUI(score);
    }

    public void AddScore(int score)
    {
        this.score += score;
        uiManager.UpdateScoreUI(this.score);

        if (this.score > 100) 
            ClearGame();
    }

    private void ClearGame()
    {
        uiManager.ShowGameClearUI();
        Time.timeScale = 0;
    }

    private void OnGameOver()
    {
        uiManager.ShowGameOverUI();
        Time.timeScale = 0;
    }
}
