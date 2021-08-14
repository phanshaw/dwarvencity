using System;
using System.Collections;
using System.Collections.Generic;
using AmplifyShaderEditor;
using HHG.Scripts;
using UnityEngine;

public struct UnitStats
{
    private int _maxHealth;
    private int _currentHealth;
    public int CurrentHealth => _currentHealth;

    private bool _isAlive;
    public bool IsAlive;

    public void Init(UnitDefinition definition)
    {
        _maxHealth = definition.MaxHealth;
        _currentHealth = definition.MaxHealth;
    }

    public void TransferFrom(UnitStats other)
    {
        _maxHealth = other._maxHealth;
        _currentHealth = other._currentHealth;
    }

    public void TakeDamage(int amount)
    {
        Debug.Assert(amount > 0, "damage was above 0");
        _currentHealth -= Mathf.Abs(amount);

        if (_currentHealth < 0)
            _isAlive = false;
    }

    public bool NeedsHealing()
    {
        return _currentHealth < _maxHealth;
    }

    public void Heal(int amount)
    {
        _currentHealth += amount;
        _currentHealth = Mathf.Min(_currentHealth, _maxHealth);
    }
}
