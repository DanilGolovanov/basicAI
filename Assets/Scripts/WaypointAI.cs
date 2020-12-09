using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointAI : MonoBehaviour
{
    public float speed = 5.0f;
    public GameObject[] waypoint;
    public float minDistance;
    public int index = 0;
    Vector3 random;
    public float maxDistanceAI;
    public GameObject player;
    public PlayerController playerController;

    // Start is called before the first frame update
    private void Start()
    {
        playerController = player.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        float distanceBetweenPlayerAndAI = Vector2.Distance(transform.position, player.transform.position);

        if (distanceBetweenPlayerAndAI < maxDistanceAI)
        {
            MoveAI(player.transform.position);
            playerController.health -= 1 * Time.deltaTime;
        }
        else
        {
            Patrol();
        }
        
    }

    void Patrol()
    {
        float distance = Vector2.Distance(transform.position, waypoint[index].transform.position);

        if (distance < minDistance)
        {
            Vector3 randomPosition = random;
            randomPosition.Set(Random.Range(0, 2), Random.Range(0, 1), Random.Range(0, 2));
            waypoint[index].transform.position = randomPosition;
            index++;     
        }
        if (index == waypoint.Length)
        {
            index = 0;
        }

        MoveAI(waypoint[index].transform.position);
    }

    void MoveAI(Vector2 targetPosition)
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
    }
}
