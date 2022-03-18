using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MyToggle : MonoBehaviour
{
    [SerializeField] Toggle yesToggle;
    [SerializeField] Toggle noToggle;
    [SerializeField] TMP_Text warningText;
    [SerializeField] Button submitButton;
    [SerializeField] string id = "0";

    private string currUserSelection = ""; // used string rather than boolean to account for user not selecting anything

    void Start()
    {
        // these listeners can also be hooked up in the Unity scene. See document.
        submitButton.onClick.AddListener(() => OnSubmitClicked());
        yesToggle.onValueChanged.AddListener(delegate {
            OnYesToggle(yesToggle);
        });
        noToggle.onValueChanged.AddListener(delegate {
            OnNoToggle(noToggle);
        });
    }

    private void OnSubmitClicked() {
        if (currUserSelection == "") {
            // user did not select anything, warn the user and do not proceed
            warningText.text = "Please select at least one response.";
            return;
        }
        // otherwise record the selection
        Debug.Log(string.Format("{0}, selected {1}", id, currUserSelection));
    }

    public void OnYesToggle(Toggle toggle) {
        if (toggle.isOn) {
            noToggle.isOn = false;
            Debug.Log("user selected yes");
            currUserSelection = "Yes";
        } else {
            currUserSelection = "";
        }
    }

    public void OnNoToggle(Toggle toggle) {
        if (toggle.isOn) {
            yesToggle.isOn = false;
            Debug.Log("user selected no");
            currUserSelection = "No";
        } else {
            currUserSelection = "";
        }
    }

}
