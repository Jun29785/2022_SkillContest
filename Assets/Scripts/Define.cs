namespace Define
{
    public enum Scenes
    {
        Start,
        Main,
        Stage1,
        Stage2,
        Finish
    }

    public enum EnemyType
    {
        Bacteria,
        Germ,
        Virus,
        Cancer
    }

    public enum ItemType
    {
        BulletUpgrade,
        God,
        Heal,
        Pain,
        BulletSpeed,
        BomberMod
    }

    public enum Direction
    {
        Left = -1,
        Center = 0,
        Right = 1
    }

    public enum ButtonType
    {
        Start,
        Register,
        Help
    }

    public enum PlayerBulletType
    {
        Straight,
        Side,
        Bomb,
        Parent,
        Frag
    }

    public enum NPCType
    {
        White,
        Red
    }

    public enum BossType
    {
        Covid_19,
        Evolved_Covid_19,
        Mini_Covid_19,
    }
}