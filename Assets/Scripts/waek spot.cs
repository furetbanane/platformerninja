using UnityEngine;

public class waekspot : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {

            if (collision.CompareTag("player"))
            {
            Destroy(transform.parent.gameObject);
        }

    }

    
}
