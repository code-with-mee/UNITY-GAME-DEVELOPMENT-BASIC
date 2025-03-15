using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    private NavMeshAgent enemyAgent;
    private PlayerController playerMovement;

    [SerializeField]
    private float distance = 30;

    private void Start()
    {
        enemyAgent = GetComponent<NavMeshAgent>();
        playerMovement = FindObjectOfType<PlayerController>();
    }

    private void Update()
    {
       if(enemyAgent != null && playerMovement != null)
        {
            Vector3 posEnemy = enemyAgent.transform.position;
            Vector3 posPlayer = playerMovement.transform.position;
            if (Vector3.Distance(posEnemy,posPlayer)< distance)
            {
                enemyAgent.SetDestination(playerMovement.transform.position);
            }
            
        }
    }
}
