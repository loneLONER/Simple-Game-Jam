using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    public SpriteRenderer sr;
    public Color color;
    public Animator anim;
    public bool IsGrounded;
    public float runSpeed = 5f;
    public int Coins = 0;
    public Text CoinsText;
    public Transform spwan;
    //public Transform GroundCheck;
    //public Transform GroundCheck_L;
    //public Transform GroundCheck_R;
    public Transform music;
    public Transform coinPick;
    public GameObject bullet;
    public GameObject shootPosleft;
    public GameObject shootPosright;
    public static bool canShoot = true;
    public int health = 3;
    public int leftLives = 3;
    public float fireCooldown = 1f;
    public float bulletSpeed;

    public bool isWalking = false;

    private bool lookRight = true;
    public GameObject panel;
    //public GameObject deadPanel;


    void Start()
    {
        shootPosleft.SetActive(false);
        shootPosright.SetActive(true); 
        rb = GetComponent<Rigidbody2D>();
        //color = gameObject.GetComponent<Color>();
        //deadPanel.SetActive(false);
        //sr = gameObject.GetComponent<SpriteRenderer>();
        //anim = GetComponent<Animator>();
        //Instantiate(music);
    }

    void Update()
    {
        if (health <= 0)
        {
            StartCoroutine(Death());
            leftLives -= 1;
            transform.position = spwan.transform.position;
            if(leftLives <= 0)
            {
                //deadPanel.SetActive(true);
            }

        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PanelActivate();
        }
        float move = Input.GetAxis("Horizontal");
        transform.rotation = Quaternion.Euler(0, 0, 0);
        //anim.SetBool("isWalking", isWalking);
        //anim.SetBool("isGrounded", IsGrounded);
        //anim.SetInteger("health", health);

        /*if (Physics2D.Linecast(transform.position, GroundCheck.position, 1 << LayerMask.NameToLayer("Ground")) ||
            Physics2D.Linecast(transform.position, GroundCheck_L.position, 1 << LayerMask.NameToLayer("Ground")) ||
            Physics2D.Linecast(transform.position, GroundCheck_R.position, 1 << LayerMask.NameToLayer("Ground")))
        {
            IsGrounded = true;
        }
        else
        {
            IsGrounded = false;
        }*/
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            rb.velocity += new Vector2(runSpeed * Time.deltaTime, 0);
            sr.flipX = false;
            shootPosleft.SetActive(false);
            shootPosright.SetActive(true);

        }
        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            rb.velocity += new Vector2(-runSpeed * Time.deltaTime, 0);
            sr.flipX = true;
            shootPosleft.SetActive(true);
            shootPosright.SetActive(false);
        }
        else if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            rb.velocity += new Vector2(0, runSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            rb.velocity += new Vector2(0, -runSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            isWalking = true;
        }
        else
        {
            isWalking = false;
        }
        if (move > 0 && !lookRight)
        {
            //Flip();
        }
        else if (move < 0 && lookRight)
        {
            //Flip();
        }
        if (Input.GetKeyDown(KeyCode.Mouse0) && canShoot)
        {
            if (shootPosleft.active)
            {
                GameObject cloneBullet = Instantiate(bullet, shootPosleft.transform.position, shootPosleft.transform.rotation);
                cloneBullet.GetComponent<Rigidbody2D>().AddForce(Vector2.left * bulletSpeed);
                
            }
            else if (shootPosright.active)
            {
                GameObject cloneBullet = Instantiate(bullet, shootPosright.transform.position, shootPosright.transform.rotation);
                cloneBullet.GetComponent<Rigidbody2D>().AddForce(Vector2.right * bulletSpeed);
            }
            //Instantiate(bullet, shootPos.position, Quaternion.identity);
            
            canShoot = false;
            StartCoroutine(FireCoolDown());
        }
    }

    public void PanelActivate()
    {
        if (panel.active)
        {
            panel.SetActive(false);
        }
        else
        {
            panel.SetActive(true);
        }
    }

    public void Flip()
    {
        lookRight = !lookRight;
        transform.Rotate(0, 180f, 0);
    }

    IEnumerator FireCoolDown()
    {
        yield return new WaitForSeconds(fireCooldown);
        canShoot = true;
        yield return null;
    }

    IEnumerator Death()
    {
        yield return new WaitForSeconds(1f);
        health = 3;
        yield return null;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.name.Contains("spike"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            StartCoroutine(Immunity());
        }
        if(other.gameObject.tag == "Enemy") {
            Damage(1);
            //add little knockback im not doing it rn bcs im tired -mrpotato
            StartCoroutine(Immunity());
        }
        if (other.gameObject.name.Contains("coin"))
        {
            Destroy(other.gameObject);
            Instantiate(coinPick);
            Coins++;
            if (Coins < 10)
            {
                CoinsText.text = ("0") + Coins.ToString();
            }
            else
            {
                CoinsText.text = Coins.ToString();
            }
        }
    }

    public void Damage(int dmg)
    {
        sr.color = color;
        health -= dmg;
        StartCoroutine(DamageColor());
        sr.color = Color.white; ;

    }

    public int GetHealth()
    {
        return health;
    }

    IEnumerator Immunity()
    {
        //play anim
        yield return new WaitForSecondsRealtime(1.5f);
        yield return null;
    }

    IEnumerator DamageColor()
    {
        yield return new WaitForSecondsRealtime(0.3f);
        yield return null;
    }
}
