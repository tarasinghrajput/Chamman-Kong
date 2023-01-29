using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
  private Rigidbody2D rb;
  private Vector2 direction;
  private Collider2D[] results;
  private Collider2D colli;
  private bool grounded;
  private bool climbing;
  private SpriteRenderer spriteRenderer;
  private int spriteIndex;


  public Sprite[] runSprites;
  public Sprite climbSprite;
  public float moveSpeed = 1f;
  public float jumpStrength = 1f;

  // private GameManager gameManagerScript = GameObject.Find("GameManager").GetComponent<Preload>();

  private void Awake() 
  {
    rb = gameObject.GetComponent<Rigidbody2D>();
    colli = gameObject.GetComponent<Collider2D>();
    spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    results = new Collider2D[4];
  } 


  private void OnEnable()
  {
    InvokeRepeating(nameof(AnimateSprite), 1f/12f, 1f/12f);
  }

  private void OnDisable() 
  {
    CancelInvoke();
  }


  private void CheckCollision()
  {
    grounded = false;
    climbing = false;

    Vector2 size = colli.bounds.size;
    size.y += 0.1f;
    size.x /= 2f;
    int amount = Physics2D.OverlapBoxNonAlloc(transform.position, size, 0f, results);

    for (int i = 0; i < amount; i++)
    {
      GameObject hit = results[i].gameObject;

      if(hit.layer == LayerMask.NameToLayer("Ground"))
      {
        grounded = hit.transform.position.y < (transform.position.y - 0.5f);
        Physics2D.IgnoreCollision(colli, results[i], !grounded);
      } else if(hit.layer == LayerMask.NameToLayer("Ladder"))
        {
          climbing = true;
        }
    }
  }

  private void Update() 
  {
    CheckCollision();

    if(climbing)
    {
      direction.y = Input.GetAxis("Vertical") * moveSpeed;
    } else if (grounded && Input.GetButtonDown("Jump"))
    {
      direction = Vector2.up * jumpStrength;
    }
    else
    {
      direction += Physics2D.gravity * Time.deltaTime;
    }

    direction.x = Input.GetAxis("Horizontal") * moveSpeed;
    
    if(grounded)
    {
    direction.y = Mathf.Max(direction.y, -1f);
    }


    if (direction.x > 0f)
    {
      transform.eulerAngles = Vector3.zero;
    }
    else if(direction.x < 0f)
    {
      transform.eulerAngles = new Vector3(0f, 180f, 0f);
    }
  }  

  private void FixedUpdate() 
  {
    rb.MovePosition(rb.position + direction * Time.fixedDeltaTime);
  }

  private void AnimateSprite()
  {
    if(climbing)
    {
      spriteRenderer.sprite = climbSprite;
    }
    else if(direction.x != 0)
    {
      spriteIndex++;

      if(spriteIndex >= runSprites.Length)
      {
        spriteIndex = 0;
      }

      spriteRenderer.sprite = runSprites[spriteIndex];
    }

  }

  private void OnCollisionEnter2D(Collision2D collision) 
  {
    if(collision.gameObject.CompareTag("Objective"))
    {
      enabled = false;
      // gameManagerScript.LevelComplete();
    } else if (collision.gameObject.CompareTag("Obstacle"))
    {
      enabled = false;
      // gameManagerScript.LevelFailed();
    }
  }
}
