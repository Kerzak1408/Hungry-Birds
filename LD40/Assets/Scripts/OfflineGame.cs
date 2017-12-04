using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OfflineGame : MonoBehaviour
{
    public Button EndGameButton;
    public GameObject Goal;

    private GameObject[] birds;
    private int screenWidth;
    private int screenHeight;
    private GameObject dragged;
    private IEnumerable<Touch> activeTouches;
    

    // Use this for initialization
    private void Start ()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        screenWidth = Screen.width;
        screenHeight = Screen.height;
        List<GameObject> birdsList = new List<GameObject>();
        for (int i = 0; i < 2; i++)
        {
            GameObject newBird = Instantiate(Resources.Load<GameObject>(ResourcesPaths.BIRD_PATH + i));
            newBird.name = "Player " + (i + 1);
            birdsList.Add(newBird);
            float positionsDiff = (i == 0) ? 0.1f : -0.1f;
            Vector3 worldPosition = Camera.main.ViewportToWorldPoint(new Vector3(0, positionsDiff + 0.5f, 0));
            worldPosition.z = 0;
            newBird.transform.position = worldPosition;
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
            newBird.GetComponentInChildren<Camera>().gameObject.transform.eulerAngles = Vector3.zero;
#endif
        }
        birds = birdsList.ToArray();
        GenerateInsects();
        GenerateBackground();
    }

    private void GenerateBackground()
    {
        GameObject background = Instantiate(Resources.Load<GameObject>("Prefabs/Background"));

        for (Vector3 currentPosition = new Vector3(0, -2, 0);
            currentPosition.x < Goal.transform.position.x;
            currentPosition.x += background.GetComponent<SpriteRenderer>().bounds.size.x)
        {
            GameObject backgroundAnother = Instantiate(Resources.Load<GameObject>("Prefabs/Background"));
            backgroundAnother.transform.position = currentPosition;
        }
        background.SetActive(false);
    }

    private void GenerateInsects()
    {
        float goalX = Goal.transform.position.x;
        float step = goalX / 150f;
        float columnProbability = 0.4f;
        for (float x = 0; x < goalX; x += step)
        {
            if (Random.value < columnProbability)
            {
                GameObject newInsect = Instantiate(Resources.Load<GameObject>(ResourcesPaths.INSECT));
                newInsect.transform.position = new Vector3(x, 10 * (Random.value - 0.5f), 0);
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || 
            EndGameButton.isActiveAndEnabled && 
                (Input.GetKeyDown(KeyCode.Return) 
                || Input.GetKeyDown(KeyCode.Space)
                || Input.GetKeyDown(KeyCode.KeypadEnter)))
        {
            BackToMenu();
        }
    }

    private void GameEnded(string loserName)
    {
        EndGameButton.gameObject.SetActive(true);
        foreach (GameObject bird in birds)
        {
            bird.GetComponent<Bird>().Pause();
        }
        Transform left = EndGameButton.gameObject.transform.Find("ImageLeft");
        Transform right = EndGameButton.gameObject.transform.Find("ImageRight");
        if (loserName == "Player 1")
        {
            
            left.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/lost");
            right.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/achievement");
        }
#if UNITY_ANDROID && !UNITY_EDITOR
        left.transform.Rotate(0, 0, -90);
        right.transform.Rotate(0, 0, 90);
#endif
    }

    public void BirdDied(Bird loser)
    {
        GameObject winner = birds.First(b => b.name != loser.name);
        
        //EndGameButton.GetComponentInChildren<Text>().text = loser.name + " died. The winner is " + winner.name;

        GameEnded(loser.name);
    }

    public void GoalReached(string name)
    {
        //EndGameButton.GetComponentInChildren<Text>().text = name + " reached the goal and won.";
        GameObject loser = birds.First(b => b.name != name);
        GameEnded(loser.name);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
