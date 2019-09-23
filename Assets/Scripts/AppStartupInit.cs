using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AppStartupInit : MonoBehaviour
{
    private Button _loadWaveButton;

    void Start()
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Menu"));
        if(SceneManager.sceneCount > 1)
        {
            SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName("Level"));
        }
        GameObject _backButton = GameObject.Find("BackButton");
        GameObject _howToText = GameObject.Find("HowToText");
        GameObject _instructionsText = GameObject.Find("InstructionsText");
        GameObject _howToBackground = GameObject.Find("HowToBackground");
        _loadWaveButton = GameObject.Find("LoadWaveButton").GetComponent<Button>();
        _loadWaveButton.interactable = false;
        _backButton.SetActive(false);
        _howToText.SetActive(false);
        _instructionsText.SetActive(false);
        _howToBackground.SetActive(false);
    }
}
