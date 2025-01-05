using System;
using System.Collections;
using System.Collections.Generic;
using OurAssets.Scripts.Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUIManager : MonoBehaviour
{
    private TextMeshProUGUI m_ScoreText;

    public PlayerComponent player;

    private void Awake()
    {
        m_ScoreText = GetComponent<TextMeshProUGUI>();
    }

    private void FixedUpdate()
    {
        if (!m_ScoreText) return;
        var score = player.GetCollectibleCount("Coin");
        m_ScoreText.SetText(score + " POINT" + (score > 1 ? "S" : ""));
    }
}
