using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork
{
    class Laplacian
    {
        public void Start()
        {
            Person p1 = new Person("p1", new List<string>(), new List<string>());
            Person p2 = new Person("p2", new List<string>(), new List<string>());
            Person p3 = new Person("p3", new List<string>(), new List<string>());
            Person p4 = new Person("p4", new List<string>(), new List<string>());
            Person p5 = new Person("p5", new List<string>(), new List<string>());
            Person p6 = new Person("p6", new List<string>(), new List<string>());

            p1.AddFriends(new List<Person>() { p2, p3 });
            p2.AddFriends(new List<Person>() { p4, p5 });
            p3.AddFriends(new List<Person>() { p4 });
            p4.AddFriends(new List<Person>() { p5 });
            p5.AddFriends(new List<Person>() { p6 });
            p6.AddFriends(new List<Person>() { });

            List<Person> Network = new List<Person>() { p1, p2, p3, p4, p5, p6 };

            Matrix<double> laplacian = MakeLaplacian(Network);

            Console.WriteLine(laplacian);
            Console.Read();
        }

        public Matrix<double> MakeLaplacian(List<Person> n)
        {
            List<List<double>> ARows = new List<List<double>>();
            List<List<double>> DRows = new List<List<double>>();
            List<double> currentRow = new List<double>();
            List<double> DRow = new List<double>();
            foreach (var person in n)
            {
                currentRow.Clear();
                foreach (var p in n) currentRow.Add(0);
                DRow = new List<double> (currentRow);
                DRow[n.IndexOf(person)] = person.friends.Count();
                DRows.Add(new List<double> (DRow));
                foreach (var friend in person.friends) currentRow[n.IndexOf(friend)] = 1;
                ARows.Add(new List<double>(currentRow));
            }
            Matrix<double> A = DenseMatrix.OfRows(ARows);
            Matrix<double> D = DenseMatrix.OfRows(DRows);
            return D - A;
        }

        public List<Person> GetPersons()
        {
            List<string> network = File.ReadAllLines(Directory.GetCurrentDirectory().Replace("bin\\Debug", "friendships")).ToList();
            return new List<Person>();
        }
    }
}
