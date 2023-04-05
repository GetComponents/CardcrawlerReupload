using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CombineManager : MonoBehaviour
{
    public static CombineManager Instance;
    [SerializeField]
    GameObject cardPrefab, hand;
    bool m_combinedThisTurn;
    public bool CombinedThisTurn
    {
        get => m_combinedThisTurn;
        set
        {
            m_combinedThisTurn = value;
            if (value)
            {
                hasCombinedText.text = "Combines this turn 0/1";
            }
            else
            {
                hasCombinedText.text = "Combines this turn 1/1";
            }
        }
    }
    [SerializeField]
    TextMeshProUGUI hasCombinedText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void CombineCards(Card _base, Card _mutator)
    {
        GameObject newCard = Instantiate(cardPrefab, hand.transform);
        Card tmp = newCard.GetComponent<Card>();
        tmp.cost = _mutator.cost + _base.cost;

        tmp.dmgToEnemy = _mutator.dmgToEnemy + _base.dmgToEnemy;
        tmp.amountOfHitsToEnemy = _mutator.amountOfHitsToEnemy + _base.amountOfHitsToEnemy - 1;
        tmp.enemyHitChance = _mutator.enemyHitChance * _base.enemyHitChance;

        tmp.dmgToPlayer = _mutator.dmgToPlayer + _base.dmgToPlayer;
        tmp.amountOfHitsToPlayer = _mutator.amountOfHitsToPlayer + _base.amountOfHitsToPlayer - 1;
        tmp.playerHitChance = _mutator.playerHitChance * _base.playerHitChance;

        tmp.drawAmount = _mutator.drawAmount + _base.drawAmount;
        tmp.discardAmount = _mutator.discardAmount + _base.discardAmount;

        tmp.healAmount = _mutator.healAmount + _base.healAmount;
        tmp.healRepetition = _mutator.healRepetition + _base.healRepetition - 1;

        tmp.ManaGain = _mutator.ManaGain + _base.ManaGain;

        tmp.strengthRoundAmount = _mutator.strengthRoundAmount + _base.strengthRoundAmount;
        tmp.weakRoundAmount = _mutator.weakRoundAmount + _base.weakRoundAmount;

        tmp.enemyHealStrength = _mutator.enemyHealStrength + _base.enemyHealStrength;
        tmp.enemyHealAmount = _mutator.enemyHealAmount + _base.enemyHealAmount - 1;

        tmp.enemyStrongAmount = _mutator.enemyStrongAmount + _base.enemyStrongAmount;
        tmp.enemyWeakAmount = _mutator.enemyWeakAmount + _base.enemyWeakAmount;

        combineLists(tmp, _base, _mutator);
        tmp.isACombinedCard = true;
        tmp.myName = _base.myName += "+";

        tmp.imageSprite = _base.imageSprite;
        tmp.InitiateCard();
        DeckManager.Instance.handCards.Remove(_base.gameObject);
        DeckManager.Instance.handCards.Remove(_mutator.gameObject);
        Destroy(_base.gameObject);
        Destroy(_mutator.gameObject);

        DeckManager.Instance.handCards.Add(tmp.gameObject);
        CombinedThisTurn = true;
        
}

    private void combineLists(Card _tmp, Card _base, Card _mutator)
    {
        for (int mutatorCount = 0; mutatorCount < _mutator.CardEffects.Count; mutatorCount++)
        {
            for (int baseCount = 0; baseCount < _base.CardEffects.Count; baseCount++)
            {
                if (!_tmp.CardEffects.Contains(_base.CardEffects[baseCount]))
                {
                    _tmp.CardEffects.Add(_base.CardEffects[baseCount]);
                    _tmp.CardSelections.Add(_base.CardSelections[baseCount]);
                }
                if (!_tmp.CardEffects.Contains(_mutator.CardEffects[mutatorCount]))
                {
                    _tmp.CardEffects.Add(_mutator.CardEffects[mutatorCount]);
                    _tmp.CardSelections.Add(_mutator.CardSelections[mutatorCount]);
                }
            }
        }
        //Sonderfälle
        if (_tmp.CardEffects.Contains(EEffectType.ATTACKONE) && _tmp.CardEffects.Contains(EEffectType.ATTACKALL))
        {
            _tmp.CardEffects.Remove(EEffectType.ATTACKONE);
            _tmp.CardSelections.Remove(ESelectionType.ENEMY);
        }
        if (_tmp.CardEffects.Contains(EEffectType.ATTACKRANDOMENEMY) && _tmp.CardEffects.Contains(EEffectType.ATTACKALL))
        {
            _tmp.CardEffects.Remove(EEffectType.ATTACKRANDOMENEMY);
            _tmp.CardSelections.Remove(ESelectionType.NONE);
        }
        if (_tmp.CardEffects.Contains(EEffectType.ATTACKRANDOMENEMY) && _tmp.CardEffects.Contains(EEffectType.ATTACKONE))
        {
            _tmp.CardEffects.Remove(EEffectType.ATTACKONE);
            _tmp.CardSelections.Remove(ESelectionType.ENEMY);
        }
    }
}
