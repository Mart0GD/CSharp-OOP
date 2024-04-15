namespace SimpleSnake.GameObjects.Foods
{
    public class AsteriksFood : Food
    {
        private const char Asteriks_Symbol = '#';
        private const int Asteriks_Points = 1;

        public AsteriksFood(Border borders) : base(Asteriks_Symbol, Asteriks_Points, borders)
        {
        }
    }
}
