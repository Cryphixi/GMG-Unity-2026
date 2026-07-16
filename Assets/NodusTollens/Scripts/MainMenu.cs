using UnityEngine;

using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour

{

    public void StartNewGame()

    {

        // Wipe any old save so a new game starts from line 0

        PlayerPrefs.DeleteKey("SavedIndex");

        PlayerPrefs.DeleteKey("SavedBranch");

        SceneManager.LoadScene("Game");

    }

    public void ContinueGame()

    {

        // Loads the Game scene. The DialogueManager will read the save itself.

        SceneManager.LoadScene("Game");

    }

    public void QuitGame()

    {

        Debug.Log("Quit pressed! (Only works in a built game, not the editor)");

        Application.Quit();

    }

}
