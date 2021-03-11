
// We use an interface here for an extra bit of future proofing
public interface IEnemy
{
    /// <summary>
    /// This is called when the enemy is allowed to do whatever action on their turn.
    /// Don't forget to call EnemyController.PassNextEnemyTurn() when done at the end.
    /// </summary>
    void ActOnTurn();
    
    /// <summary>
    /// This is called when the enemy can and has successfully finished this turn.
    /// Here any states or variables can be reset.
    /// </summary>
    void NextTurn();
}