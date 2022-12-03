using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Suwako : MonoBehaviour {
    public bool isSuwakoDead;
    public float suwakoHP = 15000f;
    public float suwakoCurrentHP;
    public int suwakoAttackPhase = 0;
    public Player playerHandler;

    private int currentSector;
    private float moveSpeed;
    private bool moveRight;

    void Start() 
    {
        suwakoHP = 15000f;
        moveSpeed = 6f;
        currentSector = 3;
        isSuwakoDead = false;
        suwakoCurrentHP = suwakoHP;
        playerHandler = GameObject.Find("PlayerPrefab").GetComponent<Player>();
    }

    void Update() 
    {
        SetSector();
        CheckBounds();
        Move();
    }

    private void Move()
    {
        if (moveRight)
        {
            transform.position = new Vector2(transform.position.x + moveSpeed * Time.deltaTime, transform.position.y);
        }
        else
        {
            transform.position = new Vector2(transform.position.x - moveSpeed * Time.deltaTime, transform.position.y);
        }
    }

    private void SetSector()
    {
        if (transform.position.x <= -5.2f)
        {
            currentSector = 1;
            return;
        }

        if (transform.position.x > -5.2f && transform.position.x <= -1.7f)
        {
            currentSector = 2;
            return;
        }

        if (transform.position.x > -1.7f && transform.position.x <= 1.7f)
        {
            currentSector = 3;
            return;
        }

        if (transform.position.x > 1.7f && transform.position.x <= 5.2f)
        {
            currentSector = 4;
            return;
        }

        if (transform.position.x > 5.2f)
        {
            currentSector = 5;
            return;
        }
    }    

    public int GetSection()
    {
        return currentSector;
    }

    private void CheckBounds()
    {
        if (transform.position.x > 8.1f)
        {
            moveRight = false;
        }
        else if (transform.position.x < -8.1f)
        {
            moveRight = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Spell"){
            Destroy(other.gameObject);
            Spell spellHandler = other.GetComponent<Spell>();
            suwakoCurrentHP -= spellHandler.spellDamage;
            playerHandler.playerScore += spellHandler.spellDamage;

            if (suwakoCurrentHP <= 0){
                isSuwakoDead = true;
                //gameObject.SetActive(false);
            }
        }
    }

    public void ResetSuwako()
    {
        suwakoHP = 15000f;
        moveSpeed = 6f;
        currentSector = 3;
        isSuwakoDead = false;
        suwakoCurrentHP = 15000f;
        transform.position = new Vector3(0f, 3.1f, 0f);
    }
}
