using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour {

    [Header("Step1 Movements")]
    [SerializeField] GameObject step1Trigger;

    [SerializeField] TypeWriter step1Writer1;
    [SerializeField] Sentences step1Sentence1;

    [SerializeField] TypeWriter step1Writer2;
    [SerializeField] Sentences step1Sentence2;

    [SerializeField] TypeWriter step1Writer3;
    [SerializeField] Sentences step1Sentence3;

    [Header("Step2 Fuel")]
    [SerializeField] GameObject step2Trigger;

    [SerializeField] TypeWriter step2Writer1;
    [SerializeField] Sentences step2Sentence1;
    [SerializeField] TypeWriter step2Writer2;
    [SerializeField] Sentences step2Sentence2;

    [SerializeField] TypeWriter step2Writer3;
    [SerializeField] Sentences step2Sentence3;


    [Header("Step3 Hook")]
    [SerializeField] GameObject step3Trigger;
    [SerializeField] TypeWriter step3Writer1;
    [SerializeField] Sentences step3Sentence1;

    [Header("Step4 Planets")]
    [SerializeField] GameObject step4Trigger;

    [Header("Step5 Vesta")]
    [SerializeField] GameObject step5Trigger;

    [Header("HUD Navigation System")]
    [SerializeField] HUDNavigationSystem tutorialTargetsNavigationSystem;

    [SerializeField] GameObject tutorialObjects;

    void Start() {
        step1Trigger.SetActive(false);
        step2Trigger.SetActive(false);
        step3Trigger.SetActive(false);
        step4Trigger.SetActive(false);
        step5Trigger.SetActive(false);
    }

    void Update() {

        // Starts tutorial
        if (GameStateManager.isTutorial()) {

            tutorialTargetsNavigationSystem.gameObject.SetActive(true);

            if (TutorialStateManager.isTutorialWaiting()) {
                GameStateManager.BlockStarShipMovements();
                TutorialStateManager.StartTutorial();
                tutorialObjects.SetActive(true);
                step1Writer1.Write(step1Sentence1);
            }

            if (TutorialStateManager.isTutorialEnded()) {
                // firstPersonWriter.Write(fpSentence1);
                Debug.Log("Step ended");
                hideTutorialObjects();
            }


            if (!GameStateManager.isPaused() && Input.GetButtonDown("Back")) {
                // TutorialStateManager.EndTutorial();
                // TODO: Implement Skip tutorial
            }
        } else {
            tutorialTargetsNavigationSystem.gameObject.SetActive(false);
        }
    }

    private void hideTutorialObjects() {
        for (int i = 0; i < tutorialObjects.transform.childCount; i++) {
            tutorialObjects.transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    public void StartStep1_Movements() {
        // GameStateManager.BlockStarShipMovements();
        GameStateManager.AllowStarShipMovements();
        TutorialStateManager.Step1();
        step1Trigger.SetActive(true);
        step1Writer2.Write(step1Sentence2);
        tutorialTargetsNavigationSystem.Show();
        tutorialTargetsNavigationSystem.SetTarget(step1Trigger);
    }

    public void EndStep1() {
        step1Trigger.SetActive(false);
        step1Writer3.Write(step1Sentence3);
        tutorialTargetsNavigationSystem.Hide();
        GameStateManager.BlockStarShipMovements();
    }

    public void ShowHUD() {
        GameStateManager.ShowHUD();
        step2Writer1.Write(step2Sentence1);
    }

    public void StartStep2_Fuel() {
        GameStateManager.AllowStarShipMovements();
        TutorialStateManager.Step2();
        step2Trigger.SetActive(true);
        tutorialTargetsNavigationSystem.Show();
        tutorialTargetsNavigationSystem.SetTarget(step2Trigger);
    }

    public void Step2_Attract_Message() {
        if (!step2Writer2.HasAlreadyWritten()) {
            step2Writer2.Write(step2Sentence2);
            GameStateManager.BlockStarShipMovements();
        }
    }

    public void EndStep2() {
        step2Trigger.SetActive(false);
        step2Writer3.Write(step2Sentence3);
        tutorialTargetsNavigationSystem.Hide();
    }

    public void StartStep3_Hook() {
        TutorialStateManager.Step3();
        step3Trigger.SetActive(true);
        step3Writer1.Write(step3Sentence1);
        tutorialTargetsNavigationSystem.Show();
        tutorialTargetsNavigationSystem.SetTarget(step3Trigger);
    }

    public void EndStep3() {
        step3Trigger.SetActive(false);

    }

    public void StartStep4_Planets() {
        TutorialStateManager.Step4();
        step4Trigger.SetActive(true);
    }

    public void EndStep4() {
        step4Trigger.SetActive(false);
        step1Writer3.Write(step1Sentence3);
        GameStateManager.ShowHUD();
    }

    public void StartStep5_Vesta_Indicator() {
        TutorialStateManager.Step5();
        step5Trigger.SetActive(true);
    }

    public void EndStep5() {
        step5Trigger.SetActive(false);
    }




}
