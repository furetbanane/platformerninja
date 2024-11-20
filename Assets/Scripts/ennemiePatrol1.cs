using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ennemiePatrol1 : MonoBehaviour
{
    public Transform[] waypoints;

    public SpriteRenderer graphique;
    [SerializeField]
    float speed;
    Transform target;
    int desPoint = 0;
    void Start()
    {
        target = waypoints[0];
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dir = target.position - transform.position;
        transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);

        if (Vector3.Distance(transform.position, target.position) < 0.3f)
        {
            desPoint = (desPoint + 1) % waypoints.Length;
            target = waypoints[desPoint];
            graphique.flipX = !graphique.flipX;
        }
    }

    
}
