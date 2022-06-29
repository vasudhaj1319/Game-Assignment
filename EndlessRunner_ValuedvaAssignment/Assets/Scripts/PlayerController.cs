using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    // PRIVATE DATA MEMBERS

    // Save current Road position for achive endless game functionality
    private float currentPosition = 100;
    // Save current score
    private int score = 0;
    // Insted of creating new vector every time we created one vector for camera new position for follow player
    private Vector3 cameraNewPosition;



    // PUBLIC DATA MEMBERS

    //Player forward speed
    public float playerForwardSpeed = 10;
    // Player Left Right speed
    public float PlayerLeftRightSpeed = 15;

    // Reference of Road 1 and Road 2 for achive endless game feature
    public Transform Road1;
    public Transform Road2;

    // Reference of Main Camera for follow plaer
    public Camera mainCamera;

    // List of Road 1 and Road 2 Green Cubes for setting true after reposition it becouse while score achiving cubes are going to visible false
    public List<GameObject> Road1GreenCubes;
    public List<GameObject> Road2GreenCubes;

    // Offset Main camera  position
    public Vector3 offsetCamera;

    // Reference of Game screen Text 
    public Text scoreText;

    // Reference of Game Scrren UI
    public GameObject gameScreen;

    // Reference of Game Over Screen UI
    public GameObject gameOverScreen;

    // Reference of Game Over Screen Text 
    public Text gameOverScreenScoreText;


    // Start is called before the first frame update
    void Start()
    {
        // Be default Game Screen is active and Game Over Scrren is Inactive
        gameScreen.SetActive(true);
        gameOverScreen.SetActive(false);

        // Created new vector for Main Camera position
        cameraNewPosition = new Vector3();
    }

    // Update is called once per frame
    void Update()
    {
        // If Game Over Screen is active then player forward and left right movement must be stop
        if (!gameOverScreen.active)
        {
            // Player Forward Speed up by 5 While Up arrow button pressed
            if (Input.GetKey(KeyCode.UpArrow))
                transform.position += Vector3.forward * (playerForwardSpeed + 10) * Time.deltaTime;

           else // Player is continusely moving forward 
            transform.position += Vector3.forward * playerForwardSpeed * Time.deltaTime;

            // set the Player current y and z position 
            cameraNewPosition.y = transform.position.y;
            cameraNewPosition.z = transform.position.z;

            // Update the Main Camera position for follow camera functionality
            mainCamera.transform.position = cameraNewPosition + offsetCamera;

            // Player Left and Right Movement 
            if (Input.GetKey(KeyCode.RightArrow) && transform.position.x < 4)
            {
                transform.position += Vector3.right * PlayerLeftRightSpeed * Time.deltaTime;
            }
            else if (Input.GetKey(KeyCode.LeftArrow) && transform.position.x > -4)   
            {
                transform.position += Vector3.left * PlayerLeftRightSpeed * Time.deltaTime;
            }
        }
    }

    // Trigger event for achiving endless game and score count
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "0")
        {
            other.gameObject.SetActive(false);
            score++;
            
            // Player forward speed will automatically increase by 2 after evey 10 point score achived
            if (score % 10 == 0)
            {
                playerForwardSpeed += 2;
            }
            scoreText.text = score + "";
        }
        else if (other.name == "Trigger 1")
        {
            currentPosition += 100;
            Road1.position = new Vector3(0, 0, currentPosition);

            for (int i = 0; i < Road1GreenCubes.Count; i++)
            {
                Road1GreenCubes[i].SetActive(true);
            }
        }

        else if (other.name == "Trigger 2")
        {
            currentPosition += 100;
            Road2.position = new Vector3(0, 0, currentPosition);
            for (int i = 0; i < Road2GreenCubes.Count; i++)
            {
                Road2GreenCubes[i].SetActive(true);
            }
        }


    }

    // Collision event is used to detect Obstacle collision and Display Game Over Screen
    private void OnCollisionEnter(Collision collision)
    {
        // Display Game Over Scrren if Player is collied other than Road
        if (collision.gameObject.name != "Road")
        {
            gameOverScreenScoreText.text = score + "";
            gameOverScreen.SetActive(true);
            gameScreen.SetActive(false);
        }


    }

    // Restart Button clicked for restart game
    public void ButtonClicked_Restart()
    {
        // For restart just reload current scene so automaticcaly reset all gameobject and functionaliy
        SceneManager.LoadScene("SampleScene");
    }

    // Exit button clicked for Exit Game
    public void ButtonClicked_Exit()
    {
        // Application quit 
        Application.Quit();
    }

}
