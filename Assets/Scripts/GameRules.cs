using UnityEngine;

[CreateAssetMenu(fileName = "Default Game Rules", menuName = "Data/Game Rules")]
public class GameRules : ScriptableObject
{
    public int InitialHandSize => m_initialHandSize;
    public int RoundHandSize => m_roundHandSize;
    public int MaxMana => m_maxMana;

    [SerializeField]
    private int m_initialHandSize = 0;
    [SerializeField]
    private int m_roundHandSize = 1;
    [SerializeField]
    private int m_maxMana = 5;
}
