using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOver : MonoBehaviour
{
    public GameManager gm;
    public Button YesButton;
    public string GameScene1;

    public Button NoButton;
    public string GameScene2;

    public float score;
    public TextMeshProUGUI scoretext; // where we reference score

    // Start is called before the first frame update
    void Start()
    {
        float finalScore = PlayerPrefs.GetFloat("FinalScore", 0f); // Get the saved "FinalScore" value with a default of 0

        Button yesbtn = YesButton.GetComponent<Button>();
        yesbtn.onClick.AddListener(TaskOnClickBeg);

        Button nobtn = NoButton.GetComponent<Button>();
        nobtn.onClick.AddListener(TaskOnClickBeg2);

        scoretext.text = "Final Score: " + finalScore; // Show the final score in the text
    }

    void TaskOnClickBeg()
    {
        Debug.Log("You have clicked the restart button!");
        SceneManager.LoadScene(GameScene1);

    }

    void TaskOnClickBeg2()
    {
        Debug.Log("You have clicked the Quit button!");
        Application.Quit();

    }

    // Update is called once per frame
    void Update()
    {

    }
}
