using OurAssets.Scripts.Enemy;
using UnityEngine;

namespace OurAssets.Scripts
{
    [CreateAssetMenu(fileName = "WeaponStats", menuName = "AyoubEtMichael/WeaponStats", order = 1)]
    public class WeaponStats : ScriptableObject
    {
        public int damage = 10;
    }
}