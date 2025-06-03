using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.Rendering;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed;
    private float direction;
    private bool hit; //if fireball collides then hit true and vice versa
    private Animator anim;
    private BoxCollider2D boxCollider;
    private float lifetime;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (hit) return; //if fireball already hit smtg, then ignore all further logic
        float movementSpeed = speed * Time.deltaTime * direction; //estimates the speed of the fireball based on time and direction of player
        transform.Translate(movementSpeed, 0, 0);

        lifetime += Time.deltaTime; //time taken by fireball since suspended

        //deactivates fireball if not collided with anything for more than 5 fps
        if (lifetime > 5)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //fireball explodes on collision
        hit = true;
        boxCollider.enabled = false;
        anim.SetTrigger("explode");
    }

    //sets the fireball in the direction of the player
    public void SetDirection(float _direction)
    {
        lifetime = 0;
        direction = _direction;
        gameObject.SetActive(true);
        hit = false;
        boxCollider.enabled = true;

        //flips the fireball in the player's direction if incase it is facing the other way
        float localScaleX = transform.localScale.x;
        if (Mathf.Sign(localScaleX) != direction)
        {
            localScaleX = -localScaleX;
        }

        transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);
    }



    private void Deactivate()
    {
        gameObject.SetActive(false);
    }


}
