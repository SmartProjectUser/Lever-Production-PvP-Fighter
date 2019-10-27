using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BotAI : MonoBehaviour
{
    private PlayerController playerController;
    private List<PlayerController> enemies;
    private Transform _enemyToAttack;
    private Transform enemyToAttack {
        get=>_enemyToAttack;
        set
        {
            _enemyToAttack = value;
            _enemyToAttack.GetComponent<PlayerController>().OnCharacterLeaveField += EnemyDied;
        }
    }

    public float safetyArenaDiametr = 3f;
    private Vector3 pointToImpulse = new Vector3(0,0,0);

    enum AiAction
    {
        Attack,
        Impulse,
        WinDance
    }

    AiAction aiAction = AiAction.Attack;
    void Start()
    {
        playerController = GetComponent<PlayerController>();
        UpdateEnemiesList();
        SelectEnemyToAttack();
        StartCoroutine(FireLoop());
    }

    void Update()
    {
        switch (aiAction)
        {
            case AiAction.Attack:
                PushEnemy();
                break;
            case AiAction.Impulse:
                GetImpulse();
                break;
            case AiAction.WinDance:
                WinDance();
                break;
            default:
                break;
        }
    }

    void UpdateEnemiesList()
    {
        if (enemies?.Count > 0)
        {
            enemies.Clear();
        }
        enemies = FindObjectsOfType<PlayerController>().ToList();
        enemies.Remove(enemies.Find(x => x.Equals(playerController)));
        enemies.Remove(enemies.Find(x => x.Playable==false));
    }

    void SelectEnemyToAttack()
    {
        if (enemies.Count > 1)
        {
            float[] enemyPriority = new float[enemies.Count];
            for (int i = 0; i < enemies.Count; i++)
            {
                float distanceToEnemy = Vector3.Distance(enemies[i].transform.position, transform.position);
                float distanceToEdge = Vector3.Distance(enemies[i].transform.position, Vector3.zero);
                enemyPriority[i] = -distanceToEnemy + distanceToEdge;
            }
            float maxPriority = enemyPriority.Max();
            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemyPriority[i] == maxPriority)
                {
                    enemyToAttack = enemies[i].transform;
                }
            }
            aiAction = AiAction.Attack;
        }
        else if (enemies.Count == 1)
        {
            enemyToAttack = enemies[0].transform;
            aiAction = AiAction.Attack;
        }
        else
        {
            aiAction = AiAction.WinDance;
        }
    }

    void SelectEnemyToAttack(Transform enemy)
    {
        enemyToAttack = enemy;
    }

    void PushEnemy()
    {
        if (enemyToAttack != null)
        {
            playerController.MoveCharacter(GetDirection(enemyToAttack.position));
        }
        else
        {
            UpdateEnemiesList();
            SelectEnemyToAttack();
        }
    }

    void WinDance()
    {
        if (Vector3.Distance(transform.position, Vector3.zero) > 1.5f)
        {
            playerController.MoveCharacter(GetDirection(Vector3.zero));
        }
        else
        {
            playerController.RotateCharacter(new Vector3(0, 5, 0));
        }
    }

    void GetImpulse()
    {
        if (transform.GetComponent<Rigidbody>().velocity.magnitude < 0.1f)
        {
            NewPointToImpulse();
        }
        if (Vector3.Distance(transform.position, pointToImpulse) > 1.5f)
        {
            playerController.MoveCharacter(GetDirection(pointToImpulse));
        }
        else { aiAction = AiAction.Attack; }
    }

    void EnemyDied(object sender, PlayerController enemy)
    {
        UpdateEnemiesList();
        SelectEnemyToAttack();
    }

    public Vector3 GetDirection(Vector3 target)
    {
        var heading = target - transform.position;
        var distance = heading.magnitude;
        var direction = heading / distance;
        return direction;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.GetComponent<PlayerController>() != null)
        {
            if (collision.relativeVelocity.magnitude > 5)
            {
                if (Random.Range(0, 2) > 0)
                {
                    SelectEnemyToAttack(collision.transform);
                }
            }
            else
            {
                NewPointToImpulse();
                aiAction = AiAction.Impulse;
            }
        }
    }

    void NewPointToImpulse()
    {
        pointToImpulse = new Vector3(Random.Range(-safetyArenaDiametr,safetyArenaDiametr),0, Random.Range(-safetyArenaDiametr, safetyArenaDiametr));
    }

    IEnumerator FireLoop()
    {
        while (enemyToAttack != null)
        {
            yield return new WaitForSeconds(0.1f);
            RaycastHit hit;
            Ray ray = new Ray(playerController.gun.position, playerController.gun.forward);
            Physics.Raycast(ray, out hit);
            if (hit.collider != null)
            {
                playerController.Fire();
                Debug.DrawLine(ray.origin, hit.point, Color.red);
            }
        }
    }
}
