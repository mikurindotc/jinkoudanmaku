using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour{
    public int lives = 3;
    public int frogsFreezed = 0;
    public int playerScore;
    public float fireRate = 0.5f;

    [SerializeField] private GameObject spellPrefab;

    private GameObject SuwakoGO;
    private bool playerGotHit;
    private bool playerInputEnabled;
    private float playerMovSpeed = 20.0f;
    private UIManager uiManagerHandler;
    private Game_Manager gameManagerHandler;
    private Vector3 bulletPosition;
    private float spellShootCooldown = 0.0f;
    private int currentSector = 3;
    [SerializeField] private int playerSkillLevel;

    void MovePlayer(){
        MovePlayerHorizontally();
        MovePlayerVertically();
    }

    void GetPlayerActions(){
        ShootSpell();
        TurnOnSlowDown();
    }

    void MovePlayerHorizontally(){
        float horizontalInput = Input.GetAxisRaw("Horizontal");

        if(playerInputEnabled){
            transform.Translate(Time.deltaTime * playerMovSpeed * horizontalInput * Vector3.right);
        }
    }

    void MovePlayerVertically(){
        float verticalInput = Input.GetAxisRaw("Vertical");

        if(playerInputEnabled){
            transform.Translate(Time.deltaTime * playerMovSpeed * verticalInput * Vector3.right);
        }
    }

    void LimitPlayerMovementPositiveX(){
        if(transform.position.x > 8.37f){
            transform.position = new Vector3(8.37f, transform.position.y, transform.position.z);
        }
    }

    void LimitPlayerMovementNegativeX(){
        if(transform.position.x < -8.37f){
            transform.position = new Vector3(-8.37f, transform.position.y, transform.position.z);
        }
    }

    void LimitPlayerMovementPositiveY(){
        if(transform.position.y > 4.4f){
            transform.position = new Vector3(transform.position.x, 4.4f, transform.position.z);
        }
    }

    void LimitPlayerMovementNegativeY(){
        if(transform.position.y < -4.4f){
            transform.position = new Vector3(transform.position.x, -4.4f, transform.position.z);
        }
    }

    void LimitPlayerMovement(){
        LimitPlayerMovementPositiveX();
        LimitPlayerMovementNegativeX();
        LimitPlayerMovementPositiveY();
        LimitPlayerMovementNegativeY();
    }

    void ApplySpellCooldown(){
        if(Time.time > spellShootCooldown){
            Instantiate(spellPrefab, transform.position, Quaternion.identity);
            spellShootCooldown = Time.time + fireRate;
        }
    }

    void ShootSpell(){
        ApplySpellCooldown();
    }

    void TurnOnSlowDown(){
        GameObject hitboxGO = gameObject.transform.Find("Hitbox").gameObject;
        if(playerInputEnabled && Input.GetKey(KeyCode.LeftShift)){
            hitboxGO.SetActive(true);
            playerMovSpeed = 5.0f;
        } else{
            hitboxGO.SetActive(false);
            playerMovSpeed = 20.0f;
        }
    }

    void MovePlayerWhileInvunerable(){
        if(playerGotHit){
            transform.position = new Vector3(0, -3.75f, 0);
        }
    }

    IEnumerator ChangePlayerPropertiesOnHit(){
        SpriteRenderer playerSpriteRenderer = GetComponent<SpriteRenderer>();
        CircleCollider2D playerHitbox = GetComponent<CircleCollider2D>();

        playerMovSpeed = 0f;
        playerHitbox.enabled = false;
        playerInputEnabled = false;
        playerGotHit = true;
        playerSpriteRenderer.color = new Color(1f, 1f, 1f, .3f);
        yield return new WaitForSeconds(1.5f);

        playerSpriteRenderer.color = new Color(1f, 1f, 1f, .7f);
        playerMovSpeed = 20f;
        playerInputEnabled = true;
        playerGotHit = false;

        yield return new WaitForSeconds(1.5f);
        playerSpriteRenderer.color = new Color(1f, 1f, 1f, 1f);
        playerHitbox.enabled = true;
    }

    public void DamagePlayer(){
        lives--;
        uiManagerHandler.UpdateLives(lives);

        if(lives == 0){
            SpriteRenderer playerSpriteRenderer = GetComponent<SpriteRenderer>();
            CircleCollider2D playerHitbox = GetComponent<CircleCollider2D>();

            playerInputEnabled = false;
            playerSpriteRenderer.color = new Color(1f, 1f, 1f, 0f);
            playerHitbox.enabled = false;
            return;
        }

        StartCoroutine(ChangePlayerPropertiesOnHit());
    }

    private float StraightLineGeneralEquation(Vector2 pointA)
    {
        Vector2 pointB = transform.position;
        float a = pointA.y - pointB.y;
        float b = pointA.x - pointB.x;
        float c = pointA.x * pointB.y - pointB.x * pointA.y;

        return a * pointB.x + b * pointB.y + c;
    }

    private void FollowSuwako()
    {
        int suwakoSector = SuwakoGO.GetComponent<Suwako>().GetSection();
        if (currentSector != suwakoSector)
        {
            Vector3 targetPos;
            switch (suwakoSector)
            {
                case 1:
                    targetPos = new Vector3(Random.Range(-5.2f, -8.1f), transform.position.y, transform.position.z);
                    break;
                case 2:
                    targetPos = new Vector3(Random.Range(-1.7f, -5.1f), transform.position.y, transform.position.z);
                    break;
                case 3:
                    targetPos = new Vector3(Random.Range(-1.7f, 1.7f), transform.position.y, transform.position.z);
                    break;
                case 4:
                    targetPos = new Vector3(Random.Range(1.7f, 5.1f), transform.position.y, transform.position.z);
                    break;
                case 5:
                    targetPos = new Vector3(Random.Range(5.2f, 8.1f), transform.position.y, transform.position.z);
                    break;
                default:
                    targetPos = new Vector3(0, 0, 0);
                    break;
            }
            transform.position = Vector3.MoveTowards(transform.position, targetPos, playerMovSpeed * Time.deltaTime);
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


    public void Perception(int avoidValue)
    {
        if (StraightLineGeneralEquation(bulletPosition) >= -1.5f && StraightLineGeneralEquation(bulletPosition) <= 1.5f)
        {
            bool shouldAvoidBullet = avoidValue <= playerSkillLevel;
            int value = Random.Range(0, 50);

            if (shouldAvoidBullet)
            {
                if (value >= 0 && value <= 25)
                {
                    transform.position += Vector3.left;
                }

                if (value > 25 && value <= 50)
                {
                    transform.position += Vector3.right;
                }
            }
        } 
    }

    public void BulletPosition(Vector3 _bulletPosition)
    {
        bulletPosition = _bulletPosition;
    }

    void Start()
    {
        playerSkillLevel = Random.Range(1, 300);
        bulletPosition = new Vector3(0f, 0f, 0f);
        playerGotHit = false;
        playerScore = 0;
        playerInputEnabled = true;
        transform.position = new Vector3(0f, -3.75f, 0f);

        SuwakoGO = GameObject.Find("Suwako");
        gameManagerHandler = GameObject.Find("GameManager").GetComponent<Game_Manager>();
        uiManagerHandler = GameObject.Find("Canvas").GetComponent<UIManager>();

        if(uiManagerHandler != null){
            uiManagerHandler.UpdateLives(lives);
        }
    }

    void Update()
    {
        SetSector();
        FollowSuwako();
        MovePlayer();
        GetPlayerActions();
        LimitPlayerMovement();
        MovePlayerWhileInvunerable();
    }

    public void ResetPlayer(bool shouldResetPlayerSkillLevel)
    {
        if (shouldResetPlayerSkillLevel)
        {
            playerSkillLevel = Random.Range(1, 300);
        }

        lives = 3;
        bulletPosition = new Vector3(0f, 0f, 0f);
        playerGotHit = false;
        playerScore = 0;
        playerInputEnabled = true;
        transform.position = new Vector3(0f, -3.75f, 0f);
        SpriteRenderer playerSpriteRenderer = GetComponent<SpriteRenderer>();
        CircleCollider2D playerHitbox = GetComponent<CircleCollider2D>();

        playerSpriteRenderer.color = new Color(1f, 1f, 1f, 1f);
        playerHitbox.enabled = true;
    }
}
