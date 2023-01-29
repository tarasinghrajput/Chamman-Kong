using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private int lives;
    private int score;
    private int level;

    private void Start() 
    {
        NewGame();
    }

    private void Awake() {
        DontDestroyOnLoad(gameObject);
    }

    public void NewGame()
    {
        lives = 3;
        score = 0;

        // Load Level....
        LoadLevel(1);
    }

    public void LoadLevel(int index)
    {
        level = index;

        Camera camera = Camera.main;

        if(camera != null)
        {
            camera.cullingMask = 0;
        }
        Invoke(nameof(LoadScene), 1f);

    }

    private void LoadScene()
    {
        SceneManager.LoadScene(level);
    }

    public void LevelComplete()
    {
        score += 1000;

        // Load next Level......
        int nextLevel =  level + 1;

        if (nextLevel < SceneManager.sceneCountInBuildSettings)
        {
        LoadLevel(nextLevel);
        } else {
            LoadLevel(1);
        }
    }

    public void LevelFailed()
    {
        lives--;

        if(lives <= 0)
        {
            NewGame();
        } else {
            // Reload Current Level.....
            LoadLevel(level);
        }
    }
}
