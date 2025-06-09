using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject howToPlay;

    public void Start()
    {
        this.gameObject.SetActive(true);
        howToPlay.SetActive(false);
    }
    public void Play()
    {
        SceneManager.LoadScene("Dungeon Game");
    }

    public void BackToMenu()
    {
        this.gameObject.SetActive(true);
        howToPlay.SetActive(false);
    }

    public void HowToPlay()
    {
        this.gameObject.SetActive(false);
        howToPlay.SetActive(true);
    }
}
