/// <summary>
/// Interface for entities that can be stunned (primarily enemies)
/// </summary>
public interface IStunnable
{
    /// <summary>
    /// Stun the entity for the specified duration
    /// </summary>
    /// <param name="duration">Duration of the stun in seconds</param>
    void Stun(float duration);
    
    /// <summary>
    /// Check if the entity is currently stunned
    /// </summary>
    bool IsStunned { get; }
} 