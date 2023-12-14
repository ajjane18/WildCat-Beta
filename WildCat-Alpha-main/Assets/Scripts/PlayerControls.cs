using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.ParticleSystem;

public class PlayerControls : MonoBehaviour
{
    //Declaring variables and referencing the animator component
    private Rigidbody cat;
    public float jumpForce;
    public float gravityModifer;
    public bool isOnGround = true;
    public bool gameOver = false;
    public Animator animator;
    public bool isGameOver;
    public GameObject particleF;
    public GameObject particleE;


    void Start()
    {
        //Getting the components physics and animations
        cat = GetComponent<Rigidbody>();
        Physics.gravity *= gravityModifer;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        //If space is pressed and is on the ground, object will jump with animation
        //prevent double jumping
        if (Input.GetKeyDown(KeyCode.Space) && isOnGround)
        {
            cat.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isOnGround = false;
            animator.SetBool("IsJumping", true);
        }

        //Otherwise.. cat is on the ground running at default
        else if(!Input.GetKeyDown(KeyCode.Space) && isOnGround)
        {
            isOnGround = true;
        }
    }
    void PowerUp()
    {
        Instantiate(particleF, transform.position, particleF.transform.rotation);
    }

    void Explosion()
    {
        Instantiate(particleE, transform.position, particleF.transform.rotation);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //When cat collided with the ground, animation JUMP stops
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
            animator.SetBool("IsJumping", false);
            
        }

        //If cat hits bin, cat gets destroyed
        else if (collision.gameObject.CompareTag("bin"))
        {
            gameOver = true;
            Destroy(gameObject);
            ScoreBoard.score -= 1;
            Debug.Log("Game Over!");
            SceneManager.LoadScene("GameOver");
            Explosion();

        }
        
        //If cat collects fish coin, fish coin gets destroyed
        else if (collision.gameObject.CompareTag("fish"))
        {
            Destroy(collision.gameObject);
            Debug.Log("MEOW~");
            PowerUp();
            ScoreBoard.score += 1;
        }

        else if (collision.gameObject.CompareTag("gekko"))
        {
            Destroy(collision.gameObject);
            Explosion();
            ScoreBoard.score -= 1;
        }

        //If cat gets hit by trash, cat gets destroyed
        else if(collision.gameObject.CompareTag("trash"))
        {
            gameOver = true;
            Destroy(gameObject);
            ScoreBoard.score -= 1;
            Debug.Log("Game Over!");
            SceneManager.LoadScene("GameOver");
            Explosion();
        }
    }

}
