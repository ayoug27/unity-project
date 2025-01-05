using System;
using System.Collections;
using System.Collections.Generic;
using OurAssets.Scripts.Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KeysUIManager : MonoBehaviour
{
    public PlayerComponent player;
    
    private TextMeshProUGUI m_KeyText;
    
    private void Awake()
    {
        m_KeyText = GetComponent<TextMeshProUGUI>();
    }
    
    private void Update()
    {
        if (!m_KeyText) return;
        var key = player.GetCollectibleCount("Key");
        m_KeyText.SetText(key + " CLÉ" + (key > 1 ? "S" : ""));
    }
}
