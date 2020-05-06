﻿using System;
using UnityEngine;

namespace Character
{
    public class CharacterHealth : MonoBehaviour
    {
        [SerializeField] private int _maxHealth;
        [SerializeField] private int _currentHealth;
        [SerializeField] private float _invincibilityTime;
        [SerializeField] private AnimationCurve _glowCurve;
        [SerializeField] private Renderer _glowRenderer;
        private float _timeSinceHit;
        private static readonly int HealthId = Shader.PropertyToID("_Health");

        public int GetMaxHealth()
        {
            return _maxHealth;
        }

        public int GetCurrentHealth()
        {
            return _currentHealth;
        }

        private void ChangeHealth(int amount)
        {
            _currentHealth += amount;
            _currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth);
            if (_currentHealth <= 0)
                Die();
            // Set the emission of the object to the health.
            var material = _glowRenderer.material;
            var healthPercentage = _currentHealth / (float)_maxHealth;
            var glow = _glowCurve.Evaluate(healthPercentage);
            print(glow);
            material.SetFloat(HealthId, glow);
        }

        public void Damage(int amount)
        {
            // Ignore if hit too soon.
            if (_timeSinceHit < _invincibilityTime)
                return;
            // Reset the invincibility timer.
            _timeSinceHit = 0;
            ChangeHealth(-amount);
        }

        private void Die()
        {
            print("Die");
            Destroy(gameObject);
        }

        private void Start()
        {
            ChangeHealth(0);
        }

        private void Update()
        {
            _timeSinceHit += Time.deltaTime;
        }
    }
}