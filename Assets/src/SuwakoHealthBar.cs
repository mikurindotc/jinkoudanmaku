using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SuwakoHealthBar : MonoBehaviour{
    public Image healthBarImage;
    
    public float currentHealth;
    private float maxHealth = 15000f;
    private Suwako suwakoHandler;

    void Start() {
        suwakoHandler = GameObject.Find("Suwako").GetComponent<Suwako>();
    }

    void Update(){
        currentHealth = suwakoHandler.suwakoCurrentHP;
        healthBarImage.fillAmount = currentHealth / maxHealth;
    }
}
