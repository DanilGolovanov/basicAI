using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatePointAI : MonoBehaviour
{
    #region Variables

    [Header("Player")]
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private PlayerController playerController;

    [Header("Waypoints")]
    [SerializeField]
    private GameObject[] waypoint;
    [SerializeField]
    private float minDistance;
    [SerializeField]
    private int index = 0;

    [Header("AI")]
    [SerializeField]
    private float maxDistanceAI;
    [SerializeField]
    private float speed = 5.0f;

    //list of available states
    public enum State
    {
        patrol,
        chase,
        flee
    }

    public State state;
    private float distanceBetweenPlayerAndAI;

    #endregion

    #region Default Methods

    private void Start()
    {
        playerController = player.GetComponent<PlayerController>();
        NextState();
    }

    #endregion

    #region States

    //default state of the AI when player is not nearby, AI walks from one waypoint to another
    private IEnumerator patrolState()
    {
        Debug.Log("patrol: Enter");
        while (state == State.patrol)
        {
            speed = 5;
            distanceBetweenPlayerAndAI = Vector2.Distance(transform.position, player.transform.position);
            Debug.Log("Patrolling");           
            yield return null;
            if (distanceBetweenPlayerAndAI < maxDistanceAI)
            {
                state = State.chase;
            }
            Patrol();
        }
        Debug.Log("patrol: Exit");
        NextState();
    }

    //AI start chasing player when player is within reaching distance for the AI
    //While chasing the player AI also reduces player's health by attacking him
    private IEnumerator chaseState()
    {
        Debug.Log("chase: Enter");
        while (state == State.chase)
        {
            distanceBetweenPlayerAndAI = Vector2.Distance(transform.position, player.transform.position);
            Debug.Log("Chasing");
            yield return null;
            //patrol if player is too far away
            if (distanceBetweenPlayerAndAI >= maxDistanceAI)
            {
                state = State.patrol;
            }
            //flee from the player if his health is less than 25
            if (playerController.health < 25)
            {
                state = State.flee;
            }
            Chase();
        }
        Debug.Log("chase: Exit");
        NextState();
    }

    //AI runs away from the player at increased speed when player's health is less than 25
    private IEnumerator fleeState()
    {
        Debug.Log("flee: Enter");
        while (state == State.flee)
        {
            distanceBetweenPlayerAndAI = Vector2.Distance(transform.position, player.transform.position);
            Debug.Log("Fleeing");
            yield return null;
            //patrol as soon as player is bit more far away than maxDistanceAI
            if (distanceBetweenPlayerAndAI >= maxDistanceAI)
            {
                speed = 5;
                state = State.patrol;
            }
            Flee();
        }
        Debug.Log("flee: Exit");
        NextState();
    }

    #endregion

    #region Custom Methods

    private void Chase()
    {
        //chase player
        MoveAI(player.transform.position);
        AttackPlayer();
    }

    private void Flee()
    {
        //run away from the player
        speed = 10;
        Vector3 directionToFlee = Vector3.one * 10;
        MoveAI(directionToFlee);
        AttackPlayer();
    }

    void Patrol()
    {
        float distance = Vector2.Distance(transform.position, waypoint[index].transform.position);

        if (distance < minDistance)
        {
            //set a random position for the waypoint that was last visited by the AI
            //Vector3 randomPosition = new Vector3();
            //randomPosition.Set(Random.Range(0, 10), Random.Range(0, 10), Random.Range(0, 10));
            //waypoint[index].transform.position = randomPosition;

            //set current waypoint to the next one
            index++;
        }
        if (index == waypoint.Length)
        {
            index = 0;
        }

        //move AI towards next waypoint
        MoveAI(waypoint[index].transform.position);
    }

    void MoveAI(Vector2 targetPosition)
    {
        //move AI towards selected position
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
    }

    void AttackPlayer()
    {
        //attack player (reduce his health)
        playerController.health -= 1 * Time.deltaTime;
    }

    private void NextState()
    {
        //assemble method name
        string methodName = state.ToString() + "State";
        System.Reflection.MethodInfo info = GetType().GetMethod(methodName,
                                                                System.Reflection.BindingFlags.NonPublic |
                                                                System.Reflection.BindingFlags.Instance);
        StartCoroutine((IEnumerator)info.Invoke(this, null));
    }

    #endregion

}
