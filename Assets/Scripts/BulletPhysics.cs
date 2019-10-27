using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class BulletPhysics : MonoBehaviour
{
    public Transform BulletType;
    public Transform OriginRotationIndicator;
    public Transform OriginEntity;
    public AudioSource enemyDeathAudioData;
    public Image _heart1;
    public Image _heart2;
    public Image _heart3;
    public Image _heart4;
    public Image _heart5;

    public float bulletSpeed;

    private Vector3 direction;
    void Start()
    {
        if (transform.name == "FriendlyBullet" || transform.name == "EnemyBullet")
        {

        }
        else
        {
            direction = xyComponents();

        }
    }

    void Update()
    {
        if (transform.name != "FriendlyBullet" && transform.name != "EnemyBullet")
        {
            float timeShift = timeSlow();
            BulletType.transform.position += direction * Time.deltaTime * timeShift * bulletSpeed;
        }
            
    }

    private float timeSlow()
    {
        float x = Mathf.Abs(Input.GetAxisRaw("Horizontal"));
        float y = Mathf.Abs(Input.GetAxisRaw("Vertical"));
        float timeShift;

        if (x > y)
        {
            timeShift = x / 90;
        }
        else
        {
            timeShift = y / 90;
        }

        return timeShift;
    }

    private Vector3 xyComponents()
    {
        float x = OriginRotationIndicator.transform.position.x - OriginEntity.transform.position.x;
        float y = OriginRotationIndicator.transform.position.y - OriginEntity.transform.position.y;
        Vector3 xyComp = new Vector3(x, y, 0);
        return xyComp;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Wall")
        {
            DestroyProjectile();
        }
        else if (other.tag == "Player" && transform.tag != "FBullet" && transform.name != "EnemyBullet")
        {
            if(VariableScript.Difficulty != 0)
            {
                if(_heart5.enabled == true)
                {
                    _heart5.enabled = false;
                }
                else if(_heart4.enabled == true)
                {
                    _heart4.enabled = false;
                }
                else if (_heart3.enabled == true)
                {
                    _heart3.enabled = false;
                }
                else if (_heart2.enabled == true)
                {
                    _heart2.enabled = false;
                }
                else if (_heart1.enabled == true)
                {
                    _heart1.enabled = false;
                }
            }

            DestroyProjectile();
        }
        else if (other.tag == "Enemy" && transform.tag != "EBullet")
        {
            enemyDeathAudioData.Play();
            Destroy(other.gameObject);
            DestroyProjectile();
            if(VariableScript.Difficulty == 1)
            {
                if (_heart2.enabled == false && _heart1.enabled == true)
                {
                    _heart2.enabled = true;
                }
                else if(_heart3.enabled == false && _heart2.enabled == true)
                {
                    _heart3.enabled = true;
                }
                else if(_heart4.enabled == false && _heart3.enabled == true)
                {
                    _heart4.enabled = true;
                }
                else if(_heart5.enabled == false && _heart4.enabled == true)
                {
                    _heart5.enabled = true;
                }
            }
        }
    }

    void DestroyProjectile()
    {
        if (transform.name != "FriendlyBullet" && transform.name != "EnemyBullet")
        {
            Destroy(gameObject);
        }
    }
}