namespace Combat
{
    public enum ECombatState
    {
        NONE,
        INITIALIZE,
        PLAYER_TURN_START,
        PLAYER_TURN_END,
        ENEMY_TURN_START,
        ENEMY_TURN_END,
        CHOOSE_REWARD,
        COMBAT_END
    }
}
