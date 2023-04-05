using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Enemies;
using TMPro;

[System.Serializable]
public class Card : MonoBehaviour
{
    public int cost;
    [SerializeField]
    private TextMeshProUGUI costText;
    public Image image;
    public Sprite imageSprite;
    public Image cardBorder;
    public string myName;
    [SerializeField]
    private TextMeshProUGUI nameText;
    [SerializeField]
    private TextMeshProUGUI descriptionText;
    public int effectIteration;
    Card cardToDiscard;
    Enemy enemy;
    //public CardEffect posEffect;
    //public CardEffect negEffect;
    public List<EEffectType> CardEffects;
    //public ESelectionType ECardSelection;
    public List<ESelectionType> CardSelections;
    public int dmgToEnemy, amountOfHitsToEnemy = 1;
    public float enemyHitChance = 1f;
    [Space]
    public int dmgToPlayer;
    public int amountOfHitsToPlayer = 1;
    public float playerHitChance = 1f;
    [Space]
    public int drawAmount;
    public int discardAmount;
    [Space]
    public int healAmount;
    public int healRepetition = 1;
    [Space]
    public int ManaGain;
    [Space]
    public int weakRoundAmount;
    public int strengthRoundAmount;
    [Space]
    public int enemyHealStrength;
    public int enemyHealAmount = 1;
    [Space]
    public int enemyWeakAmount;
    public int enemyStrongAmount;

    public string descriptionTextTemp;
    private int discardCounter = 1;
    public bool isACombinedCard;

    private void Start()
    {
        InitiateCard();
    }

    #region cardNaming
    public void InitiateCard()
    {
        string tmpText = " ";
        if (dmgToEnemy > 0)
        {
            tmpText += $"\nDeal {dmgToEnemy} ";
        }
        if (CardEffects.Contains(EEffectType.ATTACKRANDOMENEMY))
        {
            tmpText += "to a random enemy ";
        }
        else if (CardEffects.Contains(EEffectType.ATTACKALL))
        {
            tmpText += "to all enemies ";
        }
        if (amountOfHitsToEnemy > 1)
        {
            tmpText += $"{amountOfHitsToEnemy}-times ";
        }
        if (enemyHitChance < 1)
        {
            tmpText += $"with a {(int)(enemyHitChance * 100)}% chance";
        }
        if (dmgToPlayer > 0)
        {
            tmpText += $"\nDeal {dmgToPlayer} to yourself ";
        }
        if (amountOfHitsToPlayer > 1)
        {
            tmpText += $"{amountOfHitsToPlayer}-times";
        }
        if (drawAmount > 0)
        {
            tmpText += $"\nDraw {drawAmount} cards";
        }
        if (discardAmount > 0)
        {
            tmpText += $"\nDiscard {discardAmount} cards";
        }
        if (healAmount > 0)
        {
            tmpText += $"\nHeal for {healAmount} ";
        }
        if (healRepetition > 1)
        {
            tmpText += $"{healRepetition}-times";
        }
        if (ManaGain != 0)
        {
            tmpText += $"\nGain {ManaGain} Mana";
        }
        if (weakRoundAmount > 0)
        {
            tmpText += $"\nBecome weak for {weakRoundAmount} rounds";
        }
        if (strengthRoundAmount > 0)
        {
            tmpText += $"\nBecome strong for {strengthRoundAmount} rounds";
        }
        if (CardEffects.Contains(EEffectType.HEALENEMY))
        {
            tmpText += $"\nHeal an enemy for {enemyHealStrength}";
        }
        else if (CardEffects.Contains(EEffectType.HEALALLENEMIES))
        {
            tmpText += $"\nHeal all enemies for {enemyHealStrength}";
        }
        else if (CardEffects.Contains(EEffectType.HEALRANDOMENEMY))
        {
            tmpText += $"\nHeal random enemies for {enemyHealStrength}";
        }
        if (enemyHealAmount > 1)
        {
            tmpText += $" {enemyHealAmount}-times";
        }
        if (enemyWeakAmount > 0)
        {
            tmpText += $"\nEnemies become weak for {enemyWeakAmount} rounds";
        }
        if (enemyStrongAmount > 0)
        {
            tmpText += $"\nEnemies become strong for {enemyStrongAmount} rounds";
        }
        descriptionTextTemp = tmpText;
        descriptionText.text = tmpText;

        image.sprite = imageSprite;
        nameText.text = myName;
        costText.text = cost.ToString();
    }
    #endregion cardNaming

    public void CheckIfSelectionNeeded()
    {
        effectIteration = 0;
        List<ESelectionType> tmpList = new List<ESelectionType>();
        //foreach (ESelectionType _cardSelection in CardSelections)
        //{
        for (int i = 0; i < CardSelections.Count; i++)
        {

            if (i == CardSelections.Count - 1)
            {
                SelectionManager.Instance.newCardwasPlayed = true;
            }
            switch (CardSelections[i])
            {
                case ESelectionType.NONE:
                    SelectionManager.Instance.ChangeToDoList(CardSelections[i], true);
                    //InvokeEffect();
                    break;
                case ESelectionType.ENEMY:
                    //tmpList.Add(_cardSelection);
                    SelectionManager.Instance.ChangeToDoList(CardSelections[i], true);
                    SelectionManager.Instance.StartSelection();
                    break;
                case ESelectionType.CARD:
                    for (int j = 0; j < discardAmount; j++)
                    {
                        SelectionManager.Instance.ChangeToDoList(CardSelections[i], true);
                        //tmpList.Add(_cardSelection);
                    }
                    SelectionManager.Instance.StartSelection();
                    break;
                default:
                    break;
            }
        }
    }

    public void InvokeEffect()
    {
        InvokeEffect(effectIteration);
        increaseIteration();
        SelectionManager.Instance.ChangeToDoList(SelectionManager.Instance.EffectToDoList[0], false);
    }

    public void GiveEnemy(Enemy _selectedEnemy)
    {
        enemy = _selectedEnemy;
        InvokeEffect(effectIteration);
        increaseIteration();
    }

    public void DiscardCard(Card _card)
    {
        cardToDiscard = _card;
        InvokeEffect(effectIteration);
        if (discardCounter < discardAmount)
        {
            discardCounter++;
        }
        else
        {
            increaseIteration();
            discardCounter = 1;
        }
    }

    public void increaseIteration()
    {
        effectIteration++;
        if (effectIteration >= CardEffects.Count)
        {
            effectIteration = 0;
        }
    }

    public void InvokeEffect(int _iteration)
    {
        switch (CardEffects[_iteration])
        {
            case EEffectType.ATTACKONE:
                EffectManager.Instance.Attack(dmgToEnemy, amountOfHitsToEnemy, enemyHitChance, enemy);
                break;
            case EEffectType.ATTACKALL:
                EffectManager.Instance.AttackAll(dmgToEnemy, enemyHitChance, amountOfHitsToEnemy);
                break;
            case EEffectType.DAMAGEPLAYER:
                EffectManager.Instance.DealDamageToPlayer(dmgToPlayer, amountOfHitsToPlayer);
                break;
            case EEffectType.DRAW:
                EffectManager.Instance.DrawCards(drawAmount);
                break;
            case EEffectType.DISCARD:
                EffectManager.Instance.DiscardCards(cardToDiscard);
                break;
            case EEffectType.HEAL:
                EffectManager.Instance.HealPlayer(healAmount, healRepetition);
                break;
            case EEffectType.GAINMANA:
                EffectManager.Instance.GainMana(ManaGain);
                break;
            case EEffectType.APPLYWEAK:
                EffectManager.Instance.ApplyWeak(weakRoundAmount);
                break;
            case EEffectType.APPLYSTRONG:
                EffectManager.Instance.ApplyStrong(strengthRoundAmount);
                break;
            case EEffectType.HEALENEMY:
                EffectManager.Instance.HealEnemy(enemy, enemyHealStrength, enemyHealAmount);
                break;
            case EEffectType.HEALALLENEMIES:
                EffectManager.Instance.HealAllEnemies(enemyHealStrength, enemyHealAmount);
                break;
            case EEffectType.HEALRANDOMENEMY:
                EffectManager.Instance.HealRandomEnemies(enemyHealStrength, enemyHealAmount);
                break;
            case EEffectType.ATTACKRANDOMENEMY:
                EffectManager.Instance.AttackRandomEnemy(dmgToEnemy, enemyHitChance, amountOfHitsToEnemy);
                break;
            case EEffectType.APPLYWEAKTOALLENEMIES:
                EffectManager.Instance.ApplyWeakToAllEnemies(enemyWeakAmount);
                break;
            case EEffectType.APPLYSTRONGTOALLENEMIES:
                EffectManager.Instance.ApplyStrongToAllEnemies(enemyStrongAmount);
                break;
            default:
                break;
        }
    }

    public bool IsLegalCard()
    {
        foreach (EEffectType _effect in CardEffects)
        {
            switch (_effect)
            {
                case EEffectType.ATTACKONE:
                    continue;
                case EEffectType.ATTACKALL:
                    continue;
                case EEffectType.DAMAGEPLAYER:
                    break;
                case EEffectType.DRAW:
                    continue;
                case EEffectType.DISCARD:
                    if (DeckManager.Instance.handCards.Count < discardAmount + 1)
                    {
                        return false;
                    }
                    break;
                case EEffectType.HEAL:
                    continue;
                default:
                    break;
            }
        }
        return true;
    }

    public void Click()
    {
        if (SelectionManager.Instance.waitingForCardSelection)
        {
            SelectionManager.Instance.SelectCard(this);
        }
        //text.text = HP.ToString();
    }
}
