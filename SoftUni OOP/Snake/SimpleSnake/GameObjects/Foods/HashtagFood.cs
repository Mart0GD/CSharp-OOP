using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleSnake.GameObjects.Foods
{
    public class HashtagFood : Food
    {
        private const char Hashtag_Symbol = '#';
        private const int Hashtag_Points = 1;

        public HashtagFood(Border borders) : base(Hashtag_Symbol, Hashtag_Points, borders)
        {
        }
    }
}
