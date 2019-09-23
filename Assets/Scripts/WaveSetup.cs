using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveSetup : MonoBehaviour
{
    public Text _leftNum, _elimNum, _currentWave;
    public GameObject Enemy;
    public Transform player, playerSprite;
    public Image _heart1, _heart2, _heart3, _heart4, _heart5;
    public AudioSource nextWaveAudioData;

    private int lastKnownEnemies, currentEnemies, eliminatedEnemies, currentWave;
    private Quaternion initialRotation;
    private Vector3 initialPosition;
    private bool firstrun;
    public void Start()
    {
        GameObject _menuButton = GameObject.Find("GameOverMenuButton");
        GameObject _gameOverText = GameObject.Find("GameOverText");
        GameObject _gameOverBackground = GameObject.Find("GameOverBackground");
        _menuButton.SetActive(false);
        _gameOverText.SetActive(false);
        _gameOverBackground.SetActive(false);
        firstrun = true;
        currentWave = VariableScript.Level;
        initialPosition = player.transform.position;
        initialRotation = playerSprite.transform.rotation;
        ResetLevel(currentWave);
        firstrun = false;
    }

    void Update()
    {
        currentEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length - 1;
        if (currentEnemies == 0)
        {
            currentWave++;
            nextWaveAudioData.Play();
            ResetLevel(currentWave);
        }
        if (currentEnemies != lastKnownEnemies)
        {
            _leftNum.text = currentEnemies.ToString();
            eliminatedEnemies = currentWave - currentEnemies;
            _elimNum.text = eliminatedEnemies.ToString();
        }
    }

    void ResetLevel(int levelNumber)
    {
        _currentWave.text = "Wave " + levelNumber;
        _leftNum.text = levelNumber.ToString();
        _elimNum.text = "0";
        currentEnemies = currentWave;
        lastKnownEnemies = currentWave;
        player.transform.position = initialPosition;
        playerSprite.transform.rotation = initialRotation;

        if(VariableScript.Difficulty < 3 || firstrun == true)
        {
            _heart1.enabled = true;

            if(VariableScript.Difficulty != 4)
            {
                _heart2.enabled = true;
                _heart3.enabled = true;
                _heart4.enabled = true;
                _heart5.enabled = true;
            }
            else
            {
                _heart2.enabled = false;
                _heart3.enabled = false;
                _heart4.enabled = false;
                _heart5.enabled = false;
            }
        }

        EnemyMovement.routesTaken = 0;

        for (int i=0; i<currentWave; i++)
        {
            Instantiate(Enemy);
        }
    }
}

