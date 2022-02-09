class Weapon
{
    private int _damage;
    private int _bullets;

    public bool CanShoot => _bullets > 0;

    public Weapon(int damage, int bullets)
    {
        _damage = damage;
        _bullets = bullets;
    }

    public void Fire(Player player)
    {
        player.TakeDamage(_damage);
        _bullets -= 1;
    }
}

class Player
{
    private int _health;

    public bool IsAlive => _health > 0;

    public Player(int health)
    {
        _health = health;
    }

    public void TakeDamage(int damage)
    {
        if (damage > 0)
            _health -= damage;
        else
            throw new System.FormatException();
    }
}

class Bot
{
    private Weapon _weapon = new Weapon(15,100);

    public void OnSeePlayer(Player player)
    {
        if (player.IsAlive)
            if (_weapon.CanShoot)
                _weapon.Fire(player);
    }
}
