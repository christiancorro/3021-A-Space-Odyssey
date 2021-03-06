using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour {

    // Main FSM wrapper. Brain of the game on a Dont destory object. Animator (FSM) for game state transitions

    public static GameStateManager instance;
    public static Animator gameStates;
    public static Vector3 checkpoint;

    void Awake() {
        if (!instance) {
            instance = this;
            gameStates = GetComponent<Animator>();
            DontDestroyOnLoad(gameObject);
        }
    }
    private void Start() {
        Time.timeScale = 1;
    }

    private void Update() {
    }

    void OnEnable() {
        // Debug.Log("Init");
    }

    public static void SetCheckpoint(Vector3 position) {
        checkpoint = position;
    }

    public static bool isIntro() {
        return gameStates.GetCurrentAnimatorStateInfo(0).IsName("Initial Story");
    }

    public static void TestGame() {
        gameStates.SetTrigger("testGame");
        StartGame();
    }

    public static bool isTutorial() {
        return gameStates.GetCurrentAnimatorStateInfo(0).IsName("Tutorial");
    }


    public static bool isGameover() {
        return gameStates.GetCurrentAnimatorStateInfo(0).IsName("Game Over");
    }

    public static bool isStartGame() {
        return gameStates.GetCurrentAnimatorStateInfo(0).IsName("Start Game");
    }

    public static bool isMidGame() {
        return gameStates.GetCurrentAnimatorStateInfo(0).IsName("Mid Game");
    }

    public static bool isPaused() {
        return gameStates.GetCurrentAnimatorStateInfo(0).IsName("Pause");
    }

    public static bool startPlayList() {
        return gameStates.GetCurrentAnimatorStateInfo(0).IsName("Start Game") || gameStates.GetCurrentAnimatorStateInfo(0).IsName("Mid Game");
    }

    public static bool isPausable() {
        return gameStates.GetCurrentAnimatorStateInfo(0).IsName("Pause")
                || gameStates.GetCurrentAnimatorStateInfo(0).IsName("Initial Story")
                || gameStates.GetCurrentAnimatorStateInfo(0).IsName("Tutorial")
                || gameStates.GetCurrentAnimatorStateInfo(0).IsName("Start Game")
                || gameStates.GetCurrentAnimatorStateInfo(0).IsName("Mid Game")
                || gameStates.GetCurrentAnimatorStateInfo(0).IsName("End Game");
    }

    public static bool isInGame() {
        return gameStates.GetCurrentAnimatorStateInfo(0).IsName("Tutorial")
                || gameStates.GetCurrentAnimatorStateInfo(0).IsName("Start Game")
                || gameStates.GetCurrentAnimatorStateInfo(0).IsName("Mid Game");
    }

    public static bool isCheckpoint() {
        return checkpoint != Vector3.zero;
    }

    public static bool isStartMenu() {
        return gameStates.GetCurrentAnimatorStateInfo(0).IsName("Start Menu") && !(gameStates.GetBool("tutorial") || gameStates.GetBool("startGame") || gameStates.GetBool("midGame"));
    }

    public static bool isInit() {
        return gameStates.GetCurrentAnimatorStateInfo(0).IsName("Start Menu");
    }

    public static void TogglePause() {
        if (gameStates.GetBool("pause")) {
            gameStates.SetBool("pause", false);
        } else {
            gameStates.SetBool("pause", true);
        }

        Debug.Log("Pause");
    }

    public static void Restart() {
        // if (isInGame()) {
        //     StartGame();
        // }
        gameStates.SetTrigger("restart");
    }

    public static void StartMenu() {
        gameStates.SetTrigger("startMenu");
        checkpoint = Vector3.zero;
    }

    public static void StartGame() {
        gameStates.SetBool("startGame", true);
        Debug.Log("Start Game");
    }

    public static void StartMidGame() {
        gameStates.SetBool("midGame", true);
        Debug.Log("Mid Game");
    }


    public static void EndGame() {
        gameStates.SetTrigger("endGame");
        Debug.Log("End Game");
    }

    public static void StartCredits() {
        gameStates.SetTrigger("credits");
        Debug.Log("Credits");
    }

    public static bool canStarShipMove() {
        return gameStates.GetBool("allowStarShipMovements");
    }

    public static bool CanStarShipHook() {
        return gameStates.GetBool("allowHook");
    }

    public static bool isHUDVisible() {
        return gameStates.GetBool("showHUD");
    }

    public static bool isFuelNavigationSystemActive() {
        return gameStates.GetBool("showPlanetNavigationSystem");
    }

    public static bool isVestaNavigationSystemActive() {
        return gameStates.GetBool("showVestaNavigationSystem");
    }

    public static void ShowPlanetNavigationSystem() {
        gameStates.SetBool("showPlanetNavigationSystem", true);
    }

    public static void HidePlanetNavigationSystem() {
        gameStates.SetBool("showPlanetNavigationSystem", false);
    }

    public static void ShowVestaNavigationSystem() {
        gameStates.SetBool("showVestaNavigationSystem", true);
    }

    public static void HideVestaNavigationSystem() {
        gameStates.SetBool("showVestaNavigationSystem", false);
    }

    public static void ShowFuelNavigationSystem() {
        gameStates.SetBool("showFuelNavigationSystem", true);
    }

    public static void HideFuelNavigationSystem() {
        gameStates.SetBool("showFuelNavigationSystem", false);
    }


    public static void ShowHUD() {
        gameStates.SetBool("showHUD", true);
    }

    public static void HideHUD() {
        gameStates.SetBool("showHUD", false);
    }

    public static void AllowStarShipMovements() {
        gameStates.SetBool("allowStarShipMovements", true);
    }

    public static void BlockStarShipMovements() {
        gameStates.SetBool("allowStarShipMovements", false);
    }

    public static void AllowStarShipHook() {
        gameStates.SetBool("allowHook", true);
    }

    public static void BlockStarShipHook() {
        gameStates.SetBool("allowHook", false);
    }

    public static void NewGame() {
        gameStates.SetBool("newGame", true);
        Debug.Log("New Game");
    }

    public static void ExitGame() {
        gameStates.SetTrigger("exitGame");
        checkpoint = Vector3.zero;
        Debug.Log("Exit");
    }

    public static void Gameover() {
        gameStates.SetTrigger("gameOver");
        Debug.Log("Gameover");
    }

    public static void StartTutorial() {
        gameStates.SetBool("tutorial", true);
        Debug.Log("Tutorial started");
    }


}
