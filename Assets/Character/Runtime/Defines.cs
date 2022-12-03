namespace MC.Project.Character
{
    public static class CharacterMenu
    {
        public const string MENU_PATH = "Character";
    }
    
    public enum CharacterType
    {
        Player,
        NonPlayer,
    }
    
    public enum StatType
    {
        Offensive,
        Defensive,
        
        AttackSpeed,
        
        CriticalProbability,
        CriticalDamage
    }

    public enum BuffEffectType
    {
        Positive,
        Negative
    }

    public enum BuffType
    {
        Offensive,
        Defensive,
        
        
    }

    public enum BuffOperatorType
    {
        Plus,
        Multiply,
        Bigger,
        Smaller,
    }
}