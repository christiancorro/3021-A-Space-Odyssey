using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour {

    [SerializeField] AudioMixerSnapshot startMenu;
    [SerializeField] AudioMixerSnapshot intro;
    [SerializeField] AudioMixerSnapshot tutorial;
    [SerializeField] AudioMixerSnapshot pause;
    [SerializeField] AudioMixerSnapshot gameOver;
    [SerializeField] AudioMixerSnapshot danger;
    [SerializeField] AudioMixerSnapshot extremeDanger;

    [SerializeField] AudioSource introAudio;
    [SerializeField] AudioSource tutorialAudio;
    [SerializeField] AudioSource extremeDangerAudio;

    void Start() {
    }

    void Update() {
        if (GameStateManager.isStartMenu()) {
            startMenu.TransitionTo(6f);
        }

        if (GameStateManager.isIntro() && !GameStateManager.isPaused()) {
            if (!introAudio.isPlaying) {
                introAudio.Play();
            }
            intro.TransitionTo(0f);
        }

        if (GameStateManager.isTutorial()) {
            if (!tutorialAudio.isPlaying) {
                tutorialAudio.Play();
            }
            tutorial.TransitionTo(0.3f);
        } else if (!GameStateManager.isPaused()) {
            tutorialAudio.Stop();
        }

        if (GameStateManager.isPaused()) {
            pause.TransitionTo(0.3f);
        }

        if (GameStateManager.isGameover()) {
            gameOver.TransitionTo(0.4f);
        }



        if(EnemiesManager.isDanger() && !GameStateManager.isPaused()){
            danger.TransitionTo(3f);
        }

        if(EnemiesManager.isExtremeDanger() && !GameStateManager.isPaused()){
            extremeDanger.TransitionTo(3f);
        }



    }
}
