using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ESelectionType
{
    NONE,
    ENEMY,
    CARD
}
public enum EEffectType
{
    NONE,
    ATTACKONE,
    ATTACKALL,
    DAMAGEPLAYER,
    DRAW,
    DISCARD,
    HEAL,
    GAINMANA,
    APPLYWEAK,
    APPLYSTRONG,
    HEALENEMY,
    HEALALLENEMIES,
    HEALRANDOMENEMY,
    ATTACKRANDOMENEMY,
    APPLYWEAKTOALLENEMIES,
    APPLYSTRONGTOALLENEMIES
}


[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/NewEffect", order = 1)]
public class CardEffect : ScriptableObject
{

//    public int dmgToEnemy, amountOfHitsToEnemy, dmgToPlayer, amountOfHitsToPlayer, drawAmount, discardAmount;

//    public bool selectionNeeded;

//    public Enemy enemy;
//    public Card cardToDiscard;

//    //public EEffectType ECardEffect;
//    public List<EEffectType> CardEffects;
//    //public ESelectionType ECardSelection;
//    public List<ESelectionType> CardSelections;

//    public void InvokeEffect(int _iteration)
//    {
//        switch (CardEffects[_iteration])
//        {
//            case EEffectType.ATTACKONE:
//                EffectManager.Instance.Attack(dmgToEnemy, amountOfHitsToEnemy, enemy);
//                break;
//            case EEffectType.ATTACKALL:
//                EffectManager.Instance.AttackAll(dmgToEnemy, amountOfHitsToEnemy);
//                break;
//            case EEffectType.DAMAGEPLAYER:
//                break;
//            case EEffectType.DRAW:
//                EffectManager.Instance.DrawCards(drawAmount);
//                break;
//            case EEffectType.DISCARD:
//                EffectManager.Instance.DiscardCards(cardToDiscard);
//                break;
//            case EEffectType.HEAL:
//                break;
//            default:
//                break;
//        }
//    }
}
