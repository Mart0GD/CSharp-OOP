namespace SimpleSnake.GameObjects.Foods
{
    public class DollarFood : Food
    {
        private const char Dollar_Symbol = '$';
        private const int Dollar_Points = 1;

        public DollarFood(Border borders) : base(Dollar_Symbol, Dollar_Points, borders)
        {
        }
    }
}
