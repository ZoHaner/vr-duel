using System;
using UnityEngine;

namespace CodeBase.Behaviours.Player
{
    public class PlayerDeath : MonoBehaviour
    {
        public PlayerHealth _playerHealth;
        public Action PlayerDead;

        private void Start()
        {
            SubscribeEvents();
        }

        private void OnDestroy()
        {
            UnsubscribeEvents();
        }
        
        private void SubscribeEvents()
        {
            _playerHealth.HealthChanged += HealthChanged;
        }
        
        private void UnsubscribeEvents()
        {
            _playerHealth.HealthChanged -= HealthChanged;
        }

        private void HealthChanged(int health)
        {
            if (health <= 0)
            {
                PlayerDead?.Invoke();
            }
        }
    }
}