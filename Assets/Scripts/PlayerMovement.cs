using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerPhysics))]

public class PlayerMovement : MonoBehaviour
{
    public float speed, acceleration;
    public Transform character, characterRotationIndicator;
    public GameObject FriendlyBullet, _gameOverText, _menuButton, _gameOverBackground;
    public AudioSource playerBulletAudioData, playerDamageAudioData, playerDeathAudioData;
    public Image _heart1, _heart2, _heart3, _heart4, _heart5;

    private float xcurrentSpeed, xtargetSpeed, ycurrentSpeed, ytargetSpeed;
    private int currentHealth;
    private Vector2 amountToMove;

    private PlayerPhysics playerPhysics;

    void Start()
    {
        if(VariableScript.Difficulty != 4)
        {
            currentHealth = 5;
        }
        else
        {
            currentHealth = 1;
        }
        
        playerPhysics = GetComponent<PlayerPhysics>();
    }

    void Update()
    {
        HealthCheckup();
        if (currentHealth == 0)
        {

        }
        else
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                Instantiate(FriendlyBullet, characterRotationIndicator.position, character.transform.rotation);
                playerBulletAudioData.Play(0);
            }

            xtargetSpeed = Input.GetAxisRaw("Horizontal")*speed;
            xcurrentSpeed = Increment(xcurrentSpeed, xtargetSpeed, acceleration);

            ytargetSpeed = Input.GetAxisRaw("Vertical") * speed;
            ycurrentSpeed = Increment(ycurrentSpeed, ytargetSpeed, acceleration);

            if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            {
                var x = Input.GetAxisRaw("Horizontal");
                var y = Input.GetAxisRaw("Vertical");
                var rotation = Mathf.Rad2Deg * -Mathf.Atan(x/y);
                if (y<0)
                {
                    rotation += 180;
                }
                Quaternion target = Quaternion.Euler(0, 0, rotation);
                character.transform.rotation = Quaternion.Slerp(character.transform.rotation, target, Time.deltaTime * 15.0f);
            }

            amountToMove = new Vector2(xcurrentSpeed, ycurrentSpeed);
            playerPhysics.Move(amountToMove * Time.deltaTime);
        }
    }

    private float Increment(float n, float target, float acc)
    {
        if(n==target)
        {
            return n;
        }
        else
        {
            float dir = Mathf.Sign(target - n);
            n += acc * Time.deltaTime * dir;
            return (dir == Mathf.Sign(target - n)) ? n : target;
        }
    }

    private void HealthCheckup()
    {
        if (_heart5.enabled == false && currentHealth == 5)
        {
            currentHealth = 4;
            playerDamageAudioData.Play();
        }
        else if (_heart4.enabled == false && currentHealth == 4)
        {
            currentHealth = 3;
            playerDamageAudioData.Play();
        }
        else if (_heart3.enabled == false && currentHealth == 3)
        {
            currentHealth = 2;
            playerDamageAudioData.Play();
        }
        else if (_heart2.enabled == false && currentHealth == 2)
        {
            currentHealth = 1;
            playerDamageAudioData.Play();
        }
        else if (_heart1.enabled == false && currentHealth == 1)
        {
            currentHealth = 0;
            playerDeathAudioData.Play();
            GameOver();
        }
    }
    private void GameOver()
    {
        _menuButton.SetActive(true);
        _gameOverText.SetActive(true);
        _gameOverBackground.SetActive(true);
    }
}
