using UnityEngine;

using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour

{

    public void BackToMenu()

    {

        SceneManager.LoadScene("MainMenu");

    }

}

