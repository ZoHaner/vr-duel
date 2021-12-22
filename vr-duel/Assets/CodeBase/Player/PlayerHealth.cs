namespace CodeBase.Player
{
    public class PlayerHealth
    {
        public PlayerVfx PlayerVfx;
        public float Current { get; set; }
        public float Max { get; set; }

        public void TakeDamage(float damage)
        {
            if (Current <= 0)
                return;

            Current -= damage;
            
            PlayerVfx.PlayDamageEffect();
        }
    }
}