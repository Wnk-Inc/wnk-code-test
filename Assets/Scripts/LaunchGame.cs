using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LaunchGame : MonoBehaviour {

    public Text level_field;
    public Dropdown DifficultyDropboxMenu;
    public void StartGame()
    {
        if(int.TryParse(level_field.text, out int result) == true)
        {
            VariableScript.Level = int.Parse(level_field.text);
        }
        else
        {
            VariableScript.Level = 1;
        }

        VariableScript.Difficulty = DifficultyDropboxMenu.value;
        SceneManager.LoadScene("Level");
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Level"));
        SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName("Menu"));
    }
}
