using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ReadLine();
            string _docPath = "D:/studia/przetwarzanie rozproszone/projekt/wdsl/wynik/";

            ServiceReference1.Service1Client _Client = new ServiceReference1.Service1Client();
            ServiceReference1.Service2Client _Client2 = new ServiceReference1.Service2Client();

            //fractal test

            string test = _Client2.GenerateMandelbrottSet(200, 200, 4);
            string test2 = _Client2.GetMandelbrottSet(test);


            Console.WriteLine(test2 + "\n");
            Console.WriteLine(test);

            //matrix test

            Console.WriteLine("Enter row:");
            string rowS = Console.ReadLine();
            Console.WriteLine("Enter col:");
            string colS = Console.ReadLine();

            int col = Int32.Parse(colS);
            int row = Int32.Parse(rowS);
            Random rnd = new Random();

            

            

            String matrix = "";

            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    matrix += Math.Round((rnd.NextDouble() * 5), 2).ToString() + " ";
                }
            }
            String _RetValue = _Client.SendMatrix(matrix, row, col);

            String _RetValue3 = _Client.GetMatrix(_RetValue);

            Console.WriteLine("Enter name:");
            string name = Console.ReadLine();
            String _RetValue2 = _Client.MultiplyMatrix(name, name);

            

            Console.WriteLine(_RetValue);
            Console.WriteLine(_RetValue2 + "\n\n");
            Console.WriteLine(_RetValue3 + "\n\n");

            Console.ReadLine();
        }



    }
}
