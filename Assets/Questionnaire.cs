using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Questionnaire : MonoBehaviour
{
    [SerializeField] GameObject stimSphere = null;
    [SerializeField] GameObject questionBoard = null;
    [SerializeField] TMP_Text questionText;
    private bool firstStimulusUpdate = true;
    private bool questionSet = false;
    [SerializeField] string[] questions;
    // You can read in the questions from the sheet as well in the project.
    [SerializeField] int currQuestion = 0;
    [SerializeField] string id = "0";
    private string responseHeader = "qid, respnse";
    private static StreamWriter questionnaireDataStrem;

    private enum ExperimentPhase { stimulus, question, finished };
    private ExperimentPhase phase;
    // Start is called before the first frame update
    private void Start()
    {
        questionnaireDataStrem = new StreamWriter(string.Format("Data/response-{0}.csv", id));
        questionnaireDataStrem.WriteLine(responseHeader);
        questionnaireDataStrem.Flush();
        phase = ExperimentPhase.stimulus;
    }

    // Update is called once per frame
    private void Update()
    {
        switch(phase) {
            case ExperimentPhase.stimulus:
                if (firstStimulusUpdate) {
                    firstStimulusUpdate = false;
                    questionSet = false;
                    StartCoroutine(StartTrialDuration(2));
                }
                break;
            case ExperimentPhase.question:
                // we might disable meshrender of the stimsphere or the entire stimsphere object
                // depending on our need.
                stimSphere.GetComponent<MeshRenderer>().enabled = false;
                CheckQuestionResponse();
                break;
            case ExperimentPhase.finished:
                questionnaireDataStrem.Close();
                # if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
                # else
                Application.Quit();
                # endif
                break;
        }
    }

    private IEnumerator StartTrialDuration(int duration) {
        yield return new WaitForSeconds(duration);
        phase = ExperimentPhase.question;
    }

    private void CheckQuestionResponse() {
        if (questionBoard == null) return;
        if (!questionSet) {
            questionBoard.SetActive(true);

            // questionText should be the text attached to the question board, so it is also activated
            questionText.text = questions[currQuestion];
            questionSet = true;
        }

        Debug.Log("Checking for response");
        if (Input.GetKey(KeyCode.Space)) {
            Debug.Log("space pressed");
            questionnaireDataStrem.WriteLine(string.Format("{0}, Yes", currQuestion));
            questionnaireDataStrem.Flush();

            stimSphere.GetComponent<MeshRenderer>().enabled = true;
            questionBoard.SetActive(false);

            currQuestion++;
            if (currQuestion == questions.Length) {
                // we finished all the questions
                phase = ExperimentPhase.finished;
            } else {
                // go to next scene
                firstStimulusUpdate = true;
                phase = ExperimentPhase.stimulus;
            }
        } else if (Input.GetKey(KeyCode.Backspace)) {
            Debug.Log("backspace pressed");
            questionnaireDataStrem.WriteLine(string.Format("{0}, No", currQuestion));
            questionnaireDataStrem.Flush();

            stimSphere.GetComponent<MeshRenderer>().enabled = true;
            questionBoard.SetActive(false);

            currQuestion++;
            if (currQuestion == questions.Length) {
                // we finished all the questions
                phase = ExperimentPhase.finished;
            } else {
                // go to next scene
                firstStimulusUpdate = true;
                phase = ExperimentPhase.stimulus;
            }
        }
    }   
}
