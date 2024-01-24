using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public enum GameState {
    None,
    MainMenu,
    Playing,
    Victory,
    GameOver
}
public class GameManager : MonoBehaviour
{
    public static GameManager _instance;
    GameState gameState;

    void Awake() {
        _instance = this;
        gameState = GameState.None;
    }
    private void Start() {
        changeGameState(GameState.Playing);
    }
    public void changeGameState(GameState newstate) {
        gameState = newstate;
        switch (gameState) {
            case GameState.None:
                break;
            case GameState.MainMenu:
                break;
            case GameState.Playing:
                break;
            case GameState.Victory:
                break;
            case GameState.GameOver:
                Restart();
                break;
        }
    }
    public GameState getGameState() {
        return gameState;
    }
    public bool isPlaying() {
        return gameState == GameState.Playing;
    }
    public void changeGameStateFromEditor(string newState) {
        changeGameState((GameState)System.Enum.Parse(typeof(GameState), newState));
    }
    public void Restart() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
