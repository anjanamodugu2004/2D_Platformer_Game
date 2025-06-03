using UnityEngine;
using UnityEngine.Rendering;

public class PlayerMovement : MonoBehaviour
{
   [SerializeField] private float speed;
   [SerializeField] private LayerMask groundLayer;
   [SerializeField] private LayerMask wallLayer;
   [SerializeField] private float jumpPower;
   private Rigidbody2D body;
   private Animator anim;
   private BoxCollider2D boxCollider;
   private float wallJumpCooldown;
   private float horizontalInput;

   //grabs references for the rigidbody and animator components attached to the gameobject
   private void Awake()
   {
      body = GetComponent<Rigidbody2D>();
      anim = GetComponent<Animator>();
      boxCollider = GetComponent<BoxCollider2D>();
   }

   private void Update()
   {

      horizontalInput = Input.GetAxis("Horizontal");

      //flip player left or right    
      if (horizontalInput > 0.01f)
      {
         transform.localScale = Vector3.one;
      }
      else if (horizontalInput < -0.01f)
      {
         transform.localScale = new Vector3(-1, 1, 1);
      }

      //setting animator parameters 
      anim.SetBool("run", horizontalInput != 0);
      anim.SetBool("grounded", isGrounded());

      //wall jump logic 
      if (wallJumpCooldown > 0.2f)
      {
         body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

         if (onWall() && !isGrounded())
         {
            body.gravityScale = 0;
            body.velocity = Vector2.zero;
         }
         else
         {
            body.gravityScale = 7; //makes sure we dont levitate in air
         }

         if (Input.GetKey(KeyCode.Space))
         {
            Jump();
         }
      }
      else
      {
         wallJumpCooldown += Time.deltaTime; //calculates the time between 2 jumps  
      }
   }

   private void Jump()
   {
      //jump logic for if on ground by giving a number to substitute in y axix
      if (isGrounded())
      {
         body.velocity = new Vector2(body.velocity.x, jumpPower);
         anim.SetTrigger("jump");
      }

      //wall climbing jump 
      else if (onWall() && !isGrounded())
      {
         if (horizontalInput == 0)
         {
            body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 10, 0);//player over here climbing down the wall so force
            //applied in opp direction of x axis and no force on y axis 
            transform.localScale = new Vector3(-Mathf.Sign(transform.localScale.x), transform.localScale.y, transform.localScale.z);
         }
         else
         {
            body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 3, 6); //when player climbs wall, the force applied in opposite
                                                                                     //direction of x axis and upwards direction
         }
      }
      wallJumpCooldown = 0; //since done with the jump, set it back to 0
   }

   private bool isGrounded()
   {
      // emits a ray in the direction pointed and returns true if there is a collision by that ray in the given layer and specs
      RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
      return raycastHit.collider != null; // returns true when collision b/w player and ground i.e player on ground and vice versa
   }

   private bool onWall()
   {
      RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
      return raycastHit.collider != null; //returns true when player hits wall 
   }

   //gives true only when no arrow keys pressed, not on wall and when on ground
   public bool canAttack()
   {
      return horizontalInput == 0 && isGrounded() && !onWall();
   }


}
