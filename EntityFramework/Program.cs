using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFramework
{
    class Program
    {
        static void Main(string[] args)
        {
            ejemplos();

            //foreach (var item in GetTeachersList())
            //{
            //    Console.WriteLine(item.courseName);
            //    Console.WriteLine(item.teacherName);
            //}

            //Console.ReadKey();
        }

        public static List<TeacherEntities> GetTeachersList()
        {
            //EJEMPLO DE INNER JOIN, TRAER NOMBRE DE TEACHER Y COURSE 
            //Y CASTEARLO EN UNA ENTIDAD NUEVA
            using (var ctx = new SchoolDBEntities())
            {

                var Teacher = (from t in ctx.Teachers
                               join c in ctx.Courses on t.TeacherId equals c.TeacherId
                               where c.CourseName == "Science"
                               select new TeacherEntities
                               {
                                   teacherName = t.TeacherName,
                                   courseName = c.CourseName
                               }).ToList();

                return Teacher;
            }
        }

        public class TeacherEntities
        {
            public string teacherName { get; set; }
            public string courseName { get; set; }
        }

        public static void ejemplos()
        {
            //EJEMPLOS QUERIES
            using (var dbcontext = new SchoolDBEntities())
            {
                //EJEMPLO SQL QUERY:
                var studentEntity = dbcontext.Students.SqlQuery("select * from student").ToList<Student>();

                //EJEMPLO LIKE 
                var Teacher = (from t in dbcontext.Teachers
                               join c in dbcontext.Courses on t.TeacherId equals c.TeacherId
                               where c.CourseName.Contains("Science")
                               select new TeacherEntities
                               {
                                   teacherName = t.TeacherName,
                                   courseName = c.CourseName
                               }).ToList();
            }

            //EJEMPLO Eager Loading 
            //Trae las entidades relacionadas, en este caso Standard y Teachers
            using (var context = new SchoolDBEntities())
            {
                var stud1 = (from s in context.Students.Include("Standard.Teachers")
                             select s).ToList<Student>();
            }

            //EJEMPLO LAZY Loading es lo contrario al eager loading, trae la propiedad especificada 
            //de forma independiente
            using (var ctx = new SchoolDBEntities())
            {
                //Loading students only
                IList<Student> studList = ctx.Students.ToList<Student>();

                Student std = studList[0];

                //Loads Student address for particular Student only (seperate SQL query)
                StudentAddress add = std.StudentAddress;
            }

            //EJEMPLO Explicit Loading
            //Trae entitdades relacionadas de forma explicita
            using (var context = new SchoolDBEntities())
            {
                var student = context.Students
                                    .Where(s => s.StudentName == "Student5")
                                    .FirstOrDefault<Student>();

                context.Entry(student).Reference(s => s.StudentAddress).Load(); // loads StudentAddress
                context.Entry(student).Collection(s => s.Courses).Load(); // loads Courses collection 
            }

            //EJEMPLO UPDATE
            //using (var context = new SchoolDBEntities())
            //{
            //    var std = (from s in context.Students where s.StudentID==5 select s).FirstOrDefault<Student>();
            //    std.StandardId = 1;
            //    context.SaveChanges();
            //}

            //EJEMPLO INSERT
            //using (var context = new SchoolDBEntities())
            //{
            //    var std = new Student()
            //    {
            //        StudentName = "Walitas"
            //    };
            //    context.Students.Add(std);

            //    context.SaveChanges();
            //}

            //EJEMPLO DELETE
            //using (var context = new SchoolDBEntities())
            //{
            //    var std = context.Students.First<Student>();
            //    context.Students.Remove(std);

            //    context.SaveChanges();
            //}
        }

    }

}
