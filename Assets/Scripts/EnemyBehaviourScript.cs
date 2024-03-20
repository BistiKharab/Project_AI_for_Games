using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviourScript : MonoBehaviour
{

    public enum EnemyState { Patrol, Chase};

    [Header("NPC Elements")]

    [SerializeField] private float _enemyHealth;
    [SerializeField] private float _distancefromPlayer;
    [SerializeField] private float _timeLastSeen;

  
    [SerializeField] private float _moveSpeed;
    [SerializeField] private GameObject player;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private float radius;
    [SerializeField] private Transform centrePoint;

    [Space(02)]
    [Header("Utility Elements")]


    [SerializeField] private int _patrolBaseUtility;
    [SerializeField] private int _chaseBaseUtility;

    [Space(02)]
    [Header("Utility weights")]


    [SerializeField] private float _healthWeight;
    [SerializeField] private float _distanceWeight;
    [SerializeField] private float _timeWeight;

    private EnemyState state;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        state = EnemyState.Patrol;
        _distancefromPlayer = Vector3.Distance(player.transform.position, transform.position);
        
    }

    // Update is called once per frame
    void Update()
    {
        EnemyDecision(_enemyHealth, _distancefromPlayer, TimeCalculation());
        EnemyBehaviour();
       
      
    }

    void EnemyBehaviour()
    {
        switch (state)
        {
            case EnemyState.Patrol:
                Patrol();
                break;

            case EnemyState.Chase:
                Chase();
                break;
        }
    }

    void Patrol()
    {
       
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            Vector3 point;
            if (RandomPoint(centrePoint.position, radius, out point))
            {
                Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f);
                agent.SetDestination(point);
            }
        }


    } 
    
    void Chase()
    {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, _moveSpeed*Time.deltaTime);

    }

    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {

        Vector3 randomPoint = center + Random.insideUnitSphere * range;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
        {
           
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;
    }



    float CalculateUtilityChase(float health, float distance)
    {
        float healthFactor = _enemyHealth / 100;
        float distanceFactor = Mathf.Max(10 - distance, 0);

        return _chaseBaseUtility + (_healthWeight*healthFactor*10)+(_distanceWeight*distanceFactor);
    }

    float CalculateUtilityPatrol(float lastSeenTime)
    {
        float timeFactor = Mathf.Min(lastSeenTime, 10);

        return _patrolBaseUtility + (_timeWeight * timeFactor);
    }

    void EnemyDecision(float health, float distance, float lastSeenTime)
    {
        var patrolUtility = CalculateUtilityPatrol(lastSeenTime);
        var chaseUtility = CalculateUtilityChase(health, distance);
        var decision = "";

        if (patrolUtility > chaseUtility)
        {
            state = EnemyState.Patrol;
            decision = "Patrol";

        }
        else
        {
            state = EnemyState.Chase;
            decision = "Chase";

        }

        Debug.Log("Decision: " + decision + " Utility Patrol: " + patrolUtility + " Chase Utility: " + chaseUtility);
    }


    float TimeCalculation()
    {
        _timeLastSeen = Time.time - transform.GetChild(0).GetComponent<PlayerDetectionScript>().time;
        return _timeLastSeen;
    }
}
