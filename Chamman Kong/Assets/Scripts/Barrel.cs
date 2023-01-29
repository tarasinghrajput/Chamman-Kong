using UnityEngine;

public class Barrel : MonoBehaviour
{
    private Rigidbody2D rb;
    public float speed = 1f;

    private void Awake() 
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collison) 
    {
        if(collison.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            rb.AddForce(collison.transform.right * speed, ForceMode2D.Impulse);
        }
    }
}
