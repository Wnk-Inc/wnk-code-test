using System.Collections;
using UnityEngine;

[RequireComponent(typeof(EnemyPhysics))]

public class EnemyMovement : MonoBehaviour
{
    public static int routesTaken;
    public float speed, acceleration, stoppingDistance, retreatDistance,waitTimeBtwBullets, waitTimeBtwPatrol;
    public Transform enemy, enemySprite, player, enemyRotationIndicator;
    public Transform[] sideMoveSpots, diagMoveSpots;
    public GameObject EnemyBullet;
    public AudioSource enemyBulletAudioData, inEnemyViewAudioData;

    private float xcurrentSpeed, xtargetSpeed, ycurrentSpeed, ytargetSpeed, currentWaitTime;
    private bool fromPointA, seen;
    private Vector3 pointA, pointB;
    private Vector2 amountToMove;

    private EnemyPhysics enemyPhysics;

    void Start()
    {
        if(transform.name != "Enemy")
        {
            enemyPhysics = GetComponent<EnemyPhysics>();
            currentWaitTime = 0;
            seen = false;
            int pointAIndex;
            int pointBIndex;

            if (routesTaken < 4)
            {
                pointAIndex = routesTaken;
                pointBIndex = pointAIndex+1;
                if(pointBIndex > sideMoveSpots.Length-1)
                {
                    pointBIndex = 0;
                }
                pointA = sideMoveSpots[pointAIndex].position;
                pointB = sideMoveSpots[pointBIndex].position;
                routesTaken++;
            }

            else if (routesTaken >= 4 && routesTaken < 8)
            {
                pointAIndex = routesTaken - 4;
                pointBIndex = pointAIndex+1;
                if (pointBIndex > diagMoveSpots.Length-1)
                {
                    pointBIndex = 0;
                }
                pointA = diagMoveSpots[pointAIndex].position;
                pointB = diagMoveSpots[pointBIndex].position;
                routesTaken++;
            }

            else
            {
                pointA = new Vector3(Random.Range(100, 1400), Random.Range(100, 1400), -24);
                pointB = new Vector3(Random.Range(100, 1400), Random.Range(100, 1400), -24);
            }

            enemy.transform.position = pointA;
            fromPointA = true;
        }
    }

    void Update()
    {
        if(transform.name != "Enemy")
        {
            float timeShift = timeSlow();

            if (timeShift > 0)
            {
                bool enemySpotted = withinSight();

                if(seen == true && enemySpotted == false)
                {
                    inEnemyViewAudioData.Play();
                }
                float x;
                float y;

                if(enemySpotted)
                {
                    seen = true;

                    if ((Vector2.Distance(transform.position, player.position) > stoppingDistance) || (Vector2.Distance(transform.position, player.position) < retreatDistance))
                    {
                        Vector3 ang2Player = (enemy.transform.position - player.transform.position).normalized;
                        float angle = Mathf.Atan2(ang2Player.y, ang2Player.x) * Mathf.Rad2Deg + 180;

                        x = 90 * Mathf.Cos(angle / Mathf.Rad2Deg);
                        y = 90 * Mathf.Sin(angle / Mathf.Rad2Deg);

                        if(Vector2.Distance(transform.position, player.position) < retreatDistance)
                        {
                            x *= -1;
                            y *= -1;
                        }

                        xtargetSpeed = x * speed;
                        xcurrentSpeed = Increment(xcurrentSpeed, xtargetSpeed, acceleration);

                        ytargetSpeed = y * speed;
                        ycurrentSpeed = Increment(ycurrentSpeed, ytargetSpeed, acceleration);

                        amountToMove = new Vector2(xcurrentSpeed, ycurrentSpeed);
                        enemyPhysics.Move(amountToMove * Time.deltaTime * timeShift);


                    }

                    rotation(player.position.x - enemy.position.x, player.position.y - enemy.position.y, timeShift);

                    if (currentWaitTime <= 0)
                    {
                        Instantiate(EnemyBullet, enemyRotationIndicator.position, enemySprite.transform.rotation);
                        enemyBulletAudioData.Play(0);
                        currentWaitTime = waitTimeBtwBullets;
                    }
                    else
                    {
                        currentWaitTime -= Time.deltaTime * timeShift;
                    }
                }
                else
                {
                    seen = false;

                    if(Vector3.Distance(enemy.transform.position, pointA) < 1.5f)
                    {
                        fromPointA = true;
                    }

                    if(Vector3.Distance(enemy.transform.position, pointB) < 1.5f)
                    {
                        fromPointA = false;
                    }

                    Vector3 nextPosition = new Vector3(0,0,0);

                    if(fromPointA)
                    {
                        nextPosition = pointB;
                    }
                    else
                    {
                        nextPosition = pointA;
                    }

                    Vector3 toDestination = (enemy.transform.position - nextPosition).normalized;
                    float angle = Mathf.Atan2(toDestination.y, toDestination.x) * Mathf.Rad2Deg + 180;

                    x = 90 * Mathf.Cos(angle / Mathf.Rad2Deg);
                    y = 90 * Mathf.Sin(angle / Mathf.Rad2Deg);

                    xtargetSpeed = x * speed;
                    xcurrentSpeed = Increment(xcurrentSpeed, xtargetSpeed, acceleration);

                    ytargetSpeed = y * speed;
                    ycurrentSpeed = Increment(ycurrentSpeed, ytargetSpeed, acceleration);

                    amountToMove = new Vector2(xcurrentSpeed, ycurrentSpeed);
                    enemyPhysics.Move(amountToMove * Time.deltaTime * timeShift);
                    rotation(nextPosition.x - enemy.position.x, nextPosition.y - enemy.position.y, timeShift);

                }
            }
            else
            {
                
            }  
        }  
    }

    private float Increment(float n, float target, float acc)
    {
        if (n == target)
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

    private bool withinSight()
    {
        bool spotted = false;

        if ((Vector3.Distance(player.position, enemy.position) < 250))
        {

            Vector3 rotation = (enemy.transform.position - player.transform.position).normalized;
            float angle = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg + 180;

            Vector3 rotation2 = ( enemy.transform.position - enemyRotationIndicator.transform.position).normalized;
            float angle2 = Mathf.Atan2(rotation2.y, rotation2.x) * Mathf.Rad2Deg + 180;

            float intervalMin = angle2 - 90;
            float intervalMax = angle2 + 90;

            if(intervalMin < 0)
            {
                if(angle <= intervalMax || (angle <= 360 && angle >= (intervalMin + 360)))
                {
                    spotted = true;
                }
            }
            else if(intervalMax > 360)
            {
                if(angle >= intervalMin || (angle >= 0 && angle <= (intervalMax - 360)))
                {
                    spotted = true;
                }
            }
            else
            {
                if(angle >= intervalMin && angle <= intervalMax)
                {
                    spotted = true;
                }
            }   
        }
        return spotted;
    }

    private void rotation(float x, float y, float timeShift)
    {
        if (x != 0 || y != 0)
        {
            var rotation = Mathf.Rad2Deg * -Mathf.Atan(x / y);
            if (y < 0)
            {
                rotation += 180;
            }
            Quaternion target = Quaternion.Euler(0, 0, rotation);
            enemySprite.transform.rotation = Quaternion.Slerp(enemySprite.transform.rotation, target, Time.deltaTime * 15.0f);
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
}