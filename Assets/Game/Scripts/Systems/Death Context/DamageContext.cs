namespace SketchFleets.Systems.DeathContext
{
    /// <summary>
    /// An enumeration of death contexts
    /// </summary>
    public enum DamageContext
    {
        Unknown,
        PlayerBullet,
        PlayerCollision,
        EnemyBullet,
        EnemyCollision,
        ObstacleCollision,
        MindControlled
    }
}