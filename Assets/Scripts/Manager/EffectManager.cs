using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemies;
using TMPro;

public class EffectManager : MonoBehaviour
{
    public static EffectManager Instance;
    private int m_weakAmount, m_strongAmount;
    private Player.Player player;
    public int weakAmount
    {
        get => m_weakAmount;
        set
        {
            m_weakAmount = value;
            if (value > 0)
            {
                player.WeakUI.SetActive(true);
                player.WeakUI.GetComponentInChildren<TextMeshProUGUI>().text = $"Weak for {value} round(s)";
            }
            else
            {
                player.WeakUI.SetActive(false);
            }
        }
    }
    public int strongAmount
    {
        get => m_strongAmount;
        set
        {
            m_strongAmount = value;
            if (value > 0)
            {
                player.StrongUI.SetActive(true);
                player.StrongUI.GetComponentInChildren<TextMeshProUGUI>().text = $"Strong for {value} round(s)";
            }
            else
            {
                player.StrongUI.SetActive(false);
            }
        }
    }

    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        player = FindObjectOfType<Player.Player>();
    }

    public void Attack(int _dmg, int _amount, float _hitChance, Enemy _enemy)
    {
        float multiplier = 1;
        if (weakAmount > 0)
        {
            multiplier *= 0.66f;
        }
        if (strongAmount > 0)
        {
            multiplier *= 1.5f;
        }
        if (Random.Range(0f, 1f) <= _hitChance)
        {
            _enemy.HP -= (int)Mathf.Ceil(_dmg * _amount * multiplier);
        }
    }

    public void AttackAll(int _dmg, float _hitChance, int _amount)
    {
        float multiplier = 1;
        if (weakAmount > 0)
        {
            multiplier *= 0.66f;
        }
        if (strongAmount > 0)
        {
            multiplier *= 1.5f;
        }
        foreach (Enemy _enemy in FindObjectsOfType<Enemy>())
        {
            if (Random.Range(0f, 1f) <= _hitChance)
            {
                _enemy.HP -= (int)Mathf.Ceil(_dmg * _amount * multiplier);
            }
        }
    }

    public void AttackRandomEnemy(int _dmg, float _hitChance, int _amount)
    {
        float multiplier = 1;
        if (weakAmount > 0)
        {
            multiplier *= 0.66f;
        }
        if (strongAmount > 0)
        {
            multiplier *= 1.5f;
        }
        for (int i = 0; i < _amount; i++)
        {
            if (Random.Range(0f, 1f) <= _hitChance)
            {
                FindObjectsOfType<Enemy>().Random().HP -= (int)Mathf.Ceil(_dmg * multiplier);
            }
        }
    }

    public void DealDamageToPlayer(int _dmg, int _amount)
    {
        for (int i = 0; i < _amount; i++)
        {
            Player.Player.Instance.SetDamage(_dmg);
        }
    }

    public void HealPlayer(int _healAmount, int _amount)
    {
        for (int i = 0; i < _amount; i++)
        {
            Player.Player.Instance.SetHealing(_healAmount);
        }
    }

    public void HealEnemy(Enemy _enemy, int _healAmount, int _amount)
    {
        for (int i = 0; i < _amount; i++)
        {
            _enemy.HP += _healAmount;
        }
    }

    public void HealAllEnemies(int _healAmount, int _amount)
    {
        foreach (Enemy _enemy in FindObjectsOfType<Enemy>())
        {
            for (int i = 0; i < _amount; i++)
            {
                _enemy.HP += _healAmount;
            }
        }
    }

    public void HealRandomEnemies(int _healAmount, int _amount)
    {
        for (int i = 0; i < _amount; i++)
        {
            FindObjectsOfType<Enemy>().Random().HP += _healAmount * _amount;
        }
    }

    public void GainMana(int _amount)
    {
        Combat.CombatController.Instance.Mana += _amount;
    }

    public void ApplyWeak(int _amount)
    {
        weakAmount += _amount;
        ///TODO: Player UI WEAK
    }

    public void ApplyStrong(int _amount)
    {
        strongAmount += _amount;
    }

    public void ReduceBuffs()
    {
        if (weakAmount > 0)
        {
            weakAmount--;
        }
        if (strongAmount > 0)
        {
            strongAmount--;
        }
    }

    public void ApplyWeakToAllEnemies(int _amount)
    {
        foreach (Enemy _enemy in FindObjectsOfType<Enemy>())
        {
            _enemy.weakAmount += _amount;
        }
    }

    public void ApplyStrongToAllEnemies(int _amount)
    {
        foreach (Enemy _enemy in FindObjectsOfType<Enemy>())
        {
            _enemy.strongAmount += _amount;
        }
    }

    public void DrawCards(int _amount)
    {
        DeckManager.Instance.Draw(_amount);
    }

    public void DiscardCards(Card _cardToDiscard)
    {
        DeckManager.Instance.DiscardCard(_cardToDiscard.gameObject);
    }
}
