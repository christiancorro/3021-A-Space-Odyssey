using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using System.Linq;

[System.Serializable]
public class EndWritingEvent : UnityEvent { }

[System.Serializable]
public class TypeWriter : MonoBehaviour {

    public Sentences currentText;
    [SerializeField] float timePerCharacter = 0.04f;
    [SerializeField] bool skipable = true;
    [SerializeField] bool autoForward = false;
    [SerializeField] float autoForwardPause = 3;
    [SerializeField] TMP_Text page;
    [SerializeField] TMP_Text alert;
    [SerializeField] bool dontClean = false;

    [SerializeField] EndWritingEvent endWritingEvent;

    private Animator alertAnimation;
    private Queue<string> sentences;

    private bool active = false;
    private bool isTyping = false;
    private bool hasAlreadyWritten = false;
    private AudioSource SFX_Typing;

    void Start() {
        sentences = new Queue<string>();
        SFX_Typing = GetComponent<AudioSource>();
        alertAnimation = GetComponent<Animator>();
        hasAlreadyWritten = false;
        // StartWriting();

        if (Input.GetJoystickNames().Length > 0) {
            Debug.Log("Joystick detected");
            alert.text = "Press A to continue...";
        }
    }

    public void Write(Sentences text) {
        hasAlreadyWritten = true;
        StopAllCoroutines();
        currentText = text;
        StartWriting();
    }

    public void StartWriting() {
        Debug.Log("Start Writing");
        active = true;
        sentences.Clear(); // clears old sentences
                           // load sentences
        foreach (string sentence in currentText.sentences) {
            sentences.Enqueue(sentence);
        }

        SFX_Typing.Play();
        WriteNextSentence();
    }

    private void WriteNextSentence() {
        CloseAlert();
        SFX_Typing.UnPause();
        if (sentences.Count == 0) {
            EndWriting();
        } else {
            string sentence = sentences.Dequeue();
            StartCoroutine(TypeSentence(sentence));
        }

    }

    private void EndWriting() {
        SFX_Typing.Stop();
        active = false;
        if (!dontClean) {
            page.text = "";
        }
        endWritingEvent.Invoke();
    }

    public void Clean() {
        page.text = "";
    }


    IEnumerator TypeSentence(string sentence) {

        string currentText = "";
        page.text = "";
        isTyping = true;

        for (int i = 0; i <= sentence.Length; i++) {
            currentText = sentence.Substring(0, i);
            currentText += "<color=#00000000>" + sentence.Substring(i) + "</color>"; // alpha 0
            page.text = currentText;
            if (isTyping) {
                if (i > 0 && sentence[i - 1] == '.') {
                    SFX_Typing.Pause();
                    yield return new WaitForSeconds(timePerCharacter * 10); // pause typing on full stop
                } else {
                    SFX_Typing.UnPause();
                    yield return new WaitForSeconds(timePerCharacter);
                }
            }
        }

        SFX_Typing.Pause();
        isTyping = false;


        if (autoForward) {
            yield return new WaitForSeconds(autoForwardPause);
            WriteNextSentence();
        } else {
            ShowAlert();
            yield return null;
        }
    }

    private void ShowAlert() {
        alertAnimation.SetBool("showAlert", true);
    }

    private void CloseAlert() {
        alertAnimation.SetBool("showAlert", false);
    }

    void Update() {
        if (active) {
            if (skipable && Input.GetButtonDown("Skip") && !GameStateManager.isPaused()) {
                if (isTyping) {
                    isTyping = false;
                    SFX_Typing.Pause();
                } else {
                    WriteNextSentence();
                }
            }
        }

    }

    public bool HasAlreadyWritten() {
        return hasAlreadyWritten;
    }
}
