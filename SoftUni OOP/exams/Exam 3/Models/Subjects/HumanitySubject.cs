using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversityCompetition.Models.Subjects
{
    public class HumanitySubject : Subject
    {
        private const double HumanityRate = 1.15;
        public HumanitySubject(int id, string name) : base(id, name, HumanityRate)
        {
        }
    }
}
