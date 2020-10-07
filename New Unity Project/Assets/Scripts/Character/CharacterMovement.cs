using UnityEngine.AI;
using UnityEngine;
using System.Collections.Generic;

public class CharacterMovement : MonoBehaviour
{
    public List<Vector3> points;
    private int destPoint = 0;
    private NavMeshAgent agent;
    private bool Arrive = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // Disabling auto-braking allows for continuous movement
        // between points (ie, the agent doesn't slow down as it
        // approaches a destination point).
        agent.autoBraking = false;

        GotoNextPoint();
    }


    void GotoNextPoint()
    {
        // Returns if no points have been set up
        if (points.Count == 0)
            return;

        // Set the agent to go to the currently selected destination.
        agent.destination = points[destPoint];

        // Choose the next point in the array as the destination,
        // cycling to the start if necessary.
        destPoint = destPoint + 1;
        if (destPoint >= points.Count)
            Arrive = true;

    }


    void FixedUpdate()
    {
        // Choose the next destination point when the agent gets
        // close to the current one.
        if (!agent.pathPending && agent.remainingDistance < 0.5f && !Arrive)
            GotoNextPoint();
    }

    public void SetUpDestinations(List<Vector3> destinations)
    {
        points = destinations;
        Arrive = false;
    }
}
