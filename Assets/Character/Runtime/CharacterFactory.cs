namespace MC.Project.Character
{
    public class CharacterFactory
    {
        public static Character Create( CharacterData data )
        {
            return new Character();
        }
    }
}