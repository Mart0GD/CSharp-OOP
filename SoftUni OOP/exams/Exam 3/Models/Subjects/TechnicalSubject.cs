using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversityCompetition.Models.Subjects
{
    public class TechnicalSubject : Subject
    {
        private const double TechnicalRate = 1.3;
        public TechnicalSubject(int id, string name) : base(id, name, TechnicalRate)
        {
        }
    }
}
