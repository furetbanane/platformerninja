
using UnityEngine;

public class EnnemiePatrol : MonoBehaviour
{
    public Transform[] waypoints;
    public float health = 5f;
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
        }
    }
}
