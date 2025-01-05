using System.Collections;
using System.Collections.Generic;
using OurAssets.Scripts.Player;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthUIManager : MonoBehaviour
{
    public PlayerComponent player;

    public Image healthBarImage;
    public TextMeshProUGUI healthText;

    // Update is called once per frame
    private void Update()
    {
        var health = (float)player.GetCurrentHealth();
        var maxHealth = (float)player.GetMaxHealth();
        healthBarImage.fillAmount = health / maxHealth;
        healthText.text = health + " / " + maxHealth;
    }
}
