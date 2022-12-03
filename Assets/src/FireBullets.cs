using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBullets : MonoBehaviour
{
    [SerializeField] private int bulletsAmount;
    [SerializeField] private float startAngle = 0f;
    [SerializeField] private float endAngle = 360f;
    [SerializeField] private float fireRate = 0.5f;
    private Game_Manager gameManager;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<Game_Manager>();
        InvokeRepeating(nameof(Fire), 0f, fireRate);
    }

    private void Update()
    {
        if (gameManager.Difficulty() < 100)
        {
            fireRate = 1f;
            bulletsAmount = 10;
        }
        else if (gameManager.Difficulty() >= 100 && gameManager.Difficulty() <= 200)
        {
            fireRate = 0.8f;
            bulletsAmount = 15;
        }
        else
        {
            fireRate = 0.65f;
            bulletsAmount = 20;
        }
    }

    private void Fire()
    {
        float angleStep = (endAngle - startAngle) / bulletsAmount;
        float angle = startAngle;

        for (int i = 0; i < bulletsAmount + 1; i++)
        {
            float bulletDirectionX = transform.position.x + Mathf.Sin((angle * Mathf.PI) / 180f);
            float bulletDirectionY = transform.position.y + Mathf.Cos((angle * Mathf.PI) / 180f);

            Vector3 bulletMoveVector = new Vector3(bulletDirectionX, bulletDirectionY, 0f);
            Vector2 bulletDirection = (bulletMoveVector - transform.position).normalized;

            GameObject bullet = BulletPool.bulletPoolInstance.GetBullet();

            bullet.transform.position = transform.position;
            bullet.transform.rotation = transform.rotation;
            bullet.SetActive(true);
            bullet.GetComponent<Bullet>().SetMoveDirection(bulletDirection);

            angle += angleStep;
        }

        if(startAngle != 0 && endAngle != 360)
        {
            startAngle = 0f;
            endAngle = 360f;
        }
        else
        {
            startAngle = 15f;
            endAngle = 375f;
        }
    }

    private void OnDisable()
    {
        bulletsAmount = 0;
    }
}
