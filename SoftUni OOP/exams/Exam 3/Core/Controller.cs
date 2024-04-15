using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UniversityCompetition.Core.Contracts;
using UniversityCompetition.Models;
using UniversityCompetition.Models.Contracts;
using UniversityCompetition.Models.Subjects;
using UniversityCompetition.Repositories;
using UniversityCompetition.Utilities.Messages;

namespace UniversityCompetition.Core
{
    public class Controller : IController
    {
        private StudentRepository studentRepository;
        private SubjectRepository subjectRepository;
        private UniversityRepository universityRepository;

        public Controller()
        {
            studentRepository = new StudentRepository();
            subjectRepository = new SubjectRepository();
            universityRepository = new UniversityRepository();
        }

        public string AddStudent(string firstName, string lastName)
        {
            if (studentRepository.FindByName($"{firstName} {lastName}") is not null)
            {
                return String.Format(OutputMessages.AlreadyAddedStudent, firstName, lastName);
            }

            IStudent student = new Student(studentRepository.Models.Count + 1, firstName, lastName);

            studentRepository.AddModel(student);

            return String.Format(OutputMessages.StudentAddedSuccessfully, firstName, lastName, nameof(StudentRepository)).TrimEnd();
        } // Done

        public string AddSubject(string subjectName, string subjectType)
        {
            if (subjectType != nameof(EconomicalSubject) && subjectType != nameof(TechnicalSubject) && subjectType != nameof(HumanitySubject))
            {
                return String.Format(OutputMessages.SubjectTypeNotSupported, subjectType);
            }
            if (subjectRepository.FindByName(subjectName) is not null)
            {
                return String.Format(OutputMessages.AlreadyAddedSubject, subjectName);
            }

            ISubject subject = null;
            if (subjectType == nameof(HumanitySubject))
            {
                subject = new HumanitySubject(subjectRepository.Models.Count + 1, subjectName);
            }
            if (subjectType == nameof(TechnicalSubject))
            {
                subject = new TechnicalSubject(subjectRepository.Models.Count + 1, subjectName);
            }
            if (subjectType == nameof(EconomicalSubject))
            {
                subject = new EconomicalSubject(subjectRepository.Models.Count + 1, subjectName);
            }

            subjectRepository.AddModel(subject);

            return String.Format(OutputMessages.SubjectAddedSuccessfully, subjectType, subjectName, nameof(SubjectRepository)).TrimEnd();
        } // Done

        public string AddUniversity(string universityName, string category, int capacity, List<string> requiredSubjects)
        {
            if (universityRepository.FindByName(universityName) is not null)
            {
                return String.Format(OutputMessages.AlreadyAddedUniversity, universityName).TrimEnd();
            }

            List<int> requiredSubjectsToAdd = new();

            foreach (var subject in requiredSubjects)
            {
                requiredSubjectsToAdd.Add(subjectRepository.FindByName(subject).Id);
            }

            IUniversity university = new University(universityRepository.Models.Count + 1, universityName, category, capacity, requiredSubjectsToAdd);

            universityRepository.AddModel(university);

            return String.Format(OutputMessages.UniversityAddedSuccessfully, universityName, nameof(UniversityRepository)).TrimEnd();
        } // Done

        public string ApplyToUniversity(string studentName, string universityName)
        {
            string fisrtName = studentName.Split().First();
            string lastName = studentName.Split().Last();

            IStudent student = studentRepository.FindByName(studentName);
            IUniversity university = universityRepository.FindByName(universityName);

            if (student is null)
            {
                return String.Format(OutputMessages.StudentNotRegitered, fisrtName, lastName).TrimEnd();
            }
            if (university is null)
            {
                return String.Format(OutputMessages.UniversityNotRegitered, universityName).TrimEnd();
            }
            if (!university.RequiredSubjects.All(s => student.CoveredExams.Any(ex => ex == s)))
            {
                return String.Format(OutputMessages.StudentHasToCoverExams, studentName, universityName).TrimEnd();
            }
            if (student.University is not null && student.University.Name == universityName)
            {
                return String.Format(OutputMessages.StudentAlreadyJoined, fisrtName, lastName, universityName).TrimEnd();
            }

            student.JoinUniversity(university);

            return String.Format(OutputMessages.StudentSuccessfullyJoined, fisrtName, lastName, universityName).TrimEnd();
        }

        public string TakeExam(int studentId, int subjectId)
        {
            IStudent student = studentRepository.FindById(studentId);
            ISubject subject = subjectRepository.FindById(subjectId);

            if (student is null)
            {
                return String.Format(OutputMessages.InvalidStudentId).TrimEnd();
            }
            if (subject is null)
            {
                return String.Format(OutputMessages.InvalidSubjectId).TrimEnd();
            }
            if (student.CoveredExams.Any(x => x == subjectId))
            {
                return String.Format(OutputMessages.StudentAlreadyCoveredThatExam, student.FirstName, student.LastName, subject.Name).TrimEnd();
            }

            student.CoverExam(subject);

            return String.Format(OutputMessages.StudentSuccessfullyCoveredExam, student.FirstName, student.LastName, subject.Name).TrimEnd();
        }

        public string UniversityReport(int universityId)
        {
            IUniversity university = universityRepository.FindById(universityId);

            int studentsCount = studentRepository.Models.Where(x => x.University == university).Count();

            StringBuilder sb = new();

            sb.AppendLine($"*** {university.Name} ***");
            sb.AppendLine($"Profile: {university.Category}");
            sb.AppendLine($"Students admitted: {studentsCount}");
            sb.AppendLine($"University vacancy: {university.Capacity - studentsCount}");

            return sb.ToString().TrimEnd();
        }
    }
}
