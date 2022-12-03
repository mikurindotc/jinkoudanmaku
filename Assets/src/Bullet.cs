using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector2 moveDirection;
    [SerializeField] private float moveSpeed = 50f;
    [SerializeField] private float bulletLifetime = 5f;
    Player playerHandler;
    int avoidValue;
    private void OnEnable()
    {
        Invoke("Destroy", bulletLifetime);
    }

    private void Start()
    {
        avoidValue = Random.Range(1, 300);
        playerHandler = GameObject.Find("PlayerPrefab").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
    }

    public void SetMoveDirection(Vector2 direction)
    {
        moveDirection = direction;
    }

    private void Destroy()
    {
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            //Debug.Log("Hit!");
            playerHandler.DamagePlayer();
        }

        if (other.tag == "PerceptionHitbox")
        {
            avoidValue = Random.Range(1, 300);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "PerceptionHitbox")
        {
            playerHandler.BulletPosition(transform.position);
            playerHandler.Perception(avoidValue);
        }
    }
}
