using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{   // Setup Variables 
    // In "Scripts" object's inseprctor -> component -> GameManager (Script)
    public List<GameObject> zombies;
    public GameObject selectedZombie;
    public Vector3 selectedSize; // Vector is x axis, y axis, z axis
    public Vector3 defaultSize; // size when not selected
    public float pforce; // allow to set the amout of force to throw the zombie back up 
    public int lives; // decide how many lives to start with 
    public int framerate;
    public TextMeshProUGUI livestext; // where we reference text
    public TextMeshProUGUI scoretext; // where we reference score

    private Rigidbody rb; // define Rigidbody to apply force
    private int index; // to cycle through the list of zombies
    private int count;

    public GameObject floor;
    public float rotatefactor; // cuz might want to rotatex.xx so use float 
    public int score;
    public int scoredec;

    public AudioSource audioSource;
    public AudioClip backgroundMusic;
    public AudioClip selectZombieSound;
    public AudioClip bounceZombieSound;
    public AudioClip jumpZombieSound;
    public float volume = 0.5f;
    public Button muteButton;

    public Button PauseButton;
    public Button PlayButton;
    public Button ResetButton;
    public float jumpForce;
    public string GameScene;
    public string GameOverScene;
    public float cameraMoveSpeed = 5f;
    public float cameraMoveRangeX = 5f;
    public float cameraMoveRangeY = 5f;
    public float cameraMoveRangeZ = 5f;

    public Transform floorAngle;
    private bool tiltDown;

    public float floorRotateAngle;
    public float tiltSpeed = 10f;
    private bool tiltingToTarget = true;
    private float startAngle;
    private float targetAngle;

    // Start is called before the first frame update
    void Start()
    {
        // Initialise variables
        Camera mainCamera = Camera.main;
        index = 0;
        livestext.text = "Lives: " + lives;
        selectedZombie.transform.localScale = selectedSize; // Make the values we put in selectedSize play

        Button Pause = PauseButton.GetComponent<Button>();
        Pause.onClick.AddListener(TaskOnClickPause);
        Button Play = PlayButton.GetComponent<Button>();
        Play.onClick.AddListener(TaskOnClickPlay);
        Button resetbtn = ResetButton.GetComponent<Button>();
        resetbtn.onClick.AddListener(TaskOnClickReset);

        audioSource.clip = backgroundMusic;
        audioSource.loop = true;
        audioSource.volume = volume;
        audioSource.Play();
        Button muteBtn = muteButton.GetComponent<Button>();
        muteBtn.onClick.AddListener(ToggleMuteMusic);

        startAngle = floor.transform.localRotation.eulerAngles.x;
        targetAngle = startAngle + floorRotateAngle;
    }

    void TaskOnClickPause()
    {
        Debug.Log("You have clicked Pause Button!");
        Time.timeScale = 0;  // setting time to 0
    }

    void TaskOnClickPlay()
    {
        Debug.Log("You have clicked Play Button!");
        Time.timeScale = 1;  // setting time to 0
    }

    void TaskOnClickReset()
    {
        Debug.Log("You have clicked Restart Button!");
        SceneManager.LoadScene(GameScene);
    }


    // Take lives off the player
    public void decreaseLives()
    {
        lives = lives - 1;
        livestext.text = "Lives: " + lives;

        //Game Over - when life hits 0
        if (lives <= 0)
        {
            lives = 0;
            PlayerPrefs.SetFloat("FinalScore", score); // Save the score as "FinalScore"
            SceneManager.LoadScene(GameOverScene); // Load the GameOver scene
        }
    }


    //Function to decrease the score
    public void decreaseScore()
    {
        score = score - scoredec; // can change the penalty when hit the tile to more than 1
        scoretext.text = "Score: " + score;

        if (score < 0)
        {
            score = 0;
        }
    }


    //Roteta floor
    private void RotateFloor(float fromAngle, float toAngle)
    {
        float currentAngle = floor.transform.localRotation.eulerAngles.x;
        float newAngle = Mathf.MoveTowardsAngle(currentAngle, toAngle, tiltSpeed * Time.deltaTime);
        floor.transform.localRotation = Quaternion.Euler(newAngle, 0f, 0f);
    }

    //Roteta floor
    public void PlaySelectZombieSound()
    {
        audioSource.PlayOneShot(selectZombieSound, volume);
    }

    public void PlayBounceZombieSound()
    {
        audioSource.PlayOneShot(bounceZombieSound, volume);
    }

    public void PlayPressSpaceKeySound()
    {
        audioSource.PlayOneShot(jumpZombieSound, volume);
    }

    public void ToggleMuteMusic()
    {
        audioSource.mute = !audioSource.mute;
    }

    private void OnMuteToggleChanged(bool isMuted)
    {
        audioSource.mute = isMuted;
    }


    // Update is called once per frame
    void Update()
    {
        //rotate floor
        floor.transform.Rotate(-rotatefactor, 0f, 0f);

        //Score section
        //Counter for framerate
        count = count + 1;

        // If count hits (equal) framerate, assume one second has passed increment score and reset count
        if (count == framerate)
        {
            score = score + 1;
            scoretext.text = "Score: " + score;
            count = 0;
        }


        // Control the tile angle
        if (tiltingToTarget)
        {
            RotateFloor(startAngle, targetAngle);
            if (Mathf.Approximately(floor.transform.localRotation.eulerAngles.x, targetAngle))
            {
                tiltingToTarget = false;
            }
        }
        else
        {
            RotateFloor(targetAngle, startAngle);
            if (Mathf.Approximately(floor.transform.localRotation.eulerAngles.x, startAngle))
            {
                tiltingToTarget = true;
            }
        }

        if (lives <= 0)
        {
            PlayerPrefs.SetFloat("Final Score", score);
            SceneManager.LoadScene(GameOverScene);
        }



        //Keystroke section
        if (Input.GetKeyDown("left"))
        {
            index = index - 1;  // do -1 before the test, after we change the course, a matter of position 

            if (index < 0)   // starts 0 then -1 which was conditioned that if its < 0 will immediately go to element 3
            {
                index = 3;
            }

            selectedZombie.transform.localScale = defaultSize;
            selectedZombie = zombies[index];
            selectedZombie.transform.localScale = selectedSize;
            Debug.Log("Left Arrow pressed");

            PlaySelectZombieSound();
        }

        else if (Input.GetKeyDown("right"))  // else if -> cuz only do the rigth key if haven't done the left key
        {
            index = index + 1; // index = index + 1;  =   index++;
                               // move to next postition each time we press the key

            if (index >= 4) // starts 0 then +1, when the program reaches 4, it will loop back to element 0 
            {
                index = 0;
            }

            selectedZombie.transform.localScale = defaultSize; // change the zombie that was selected to default size ->
            selectedZombie = zombies[index]; // select new zombie ->
            selectedZombie.transform.localScale = selectedSize;  // change selected to new size
            Debug.Log("Right Arrow pressed");

            PlaySelectZombieSound();
        }
        else if (Input.GetKeyDown("up"))
        {
            Debug.Log("up Arrow pressed");
            rb = selectedZombie.GetComponent<Rigidbody>();
            rb.AddForce(new Vector3(0, 0, pforce), ForceMode.Impulse);

            PlayBounceZombieSound();
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Space key pressed");
            rb = selectedZombie.GetComponent<Rigidbody>();
            rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);

            PlayPressSpaceKeySound();
        }


        float cameraRangeControlSpeed = 1f;

        if (Input.GetKey(KeyCode.W))
        {
            cameraMoveRangeY += cameraRangeControlSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            cameraMoveRangeY -= cameraRangeControlSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A))
        {
            cameraMoveRangeX -= cameraRangeControlSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            cameraMoveRangeX += cameraRangeControlSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.Q))
        {
            Camera.main.transform.position += Vector3.forward * cameraRangeControlSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.E))
        {
            Camera.main.transform.position -= Vector3.forward * cameraRangeControlSpeed * Time.deltaTime;
        }

        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        // Control X, Y, and Z axis separately for range
        float xMovement = horizontalInput * cameraMoveSpeed * Time.deltaTime;
        float yMovement = verticalInput * cameraMoveSpeed * Time.deltaTime;

        cameraMoveRangeX = Mathf.Clamp(cameraMoveRangeX, 0f, float.MaxValue);
        cameraMoveRangeY = Mathf.Clamp(cameraMoveRangeY, 0f, float.MaxValue);


        // Assuming the camera is orthographic, let's move it in the X-Z plane only
        Vector3 cameraMovement = new Vector3(xMovement, yMovement, 0f);

        Camera.main.transform.position += cameraMovement;
    }
}