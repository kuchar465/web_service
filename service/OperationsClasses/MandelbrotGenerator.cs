using System;

using System.Linq;

using System.IO;
using System.Collections.Concurrent;
using System.Threading;

using System.Threading.Tasks;




namespace WcfService1
{
    public class MandelbrotGenerator
    {
        
        private const double _cxMin = -2.5;
        private const double _cxMax = 1.5;
        private const double _cyMin = -2.0;
        private const double _cyMax = 2.0;
        private const int _maxColorComponentValue = 255;
        //private byte[,,] color = new byte[1001, 1001, 4];
        private const int _iterationMax = 200;
        private const double _escapeRadius = 2;
        
        private double _eR2 = _escapeRadius * _escapeRadius;

        private string _docPath = "D:/studia/przetwarzanie rozproszone/projekt/wdsl/wynik/";

        public string Generate_set(int x, int y, int threadAmount)
        {
             int _iXmax = x;
             int _iYmax = y;
             double _pixelWidth = (_cxMax - _cxMin) / _iXmax;
             double _pixelHeight = (_cyMax - _cyMin) / _iYmax;

        Random rnd = new Random();
            byte[,,] color = new byte[_iXmax, _iYmax, 3];

            var allLength = color.Length;
            var total = 1;
            for (int i = 0; i < color.Rank; i++)
            {
                total *= color.GetLength(i);
            }

            for (int i = 0; i < _iXmax; i++)
                for (int j = 0; j < _iYmax; j++)
                {
                    color[i, j, 0] = 0;
                    color[i, j, 1] = 0;
                    color[i, j, 2] = 0;
                }


            byte[] R = new byte[threadAmount];
            byte[] G = new byte[threadAmount];
            byte[] B = new byte[threadAmount];
            for (int i = 0; i < threadAmount; i++) {
                 R[i] = Convert.ToByte(rnd.Next(0, 255));
                 G[i] = Convert.ToByte(rnd.Next(0, 255));
                 B[i] = Convert.ToByte(rnd.Next(0, 255));
            }
            

            ParallelOptions options = new ParallelOptions();
            options.MaxDegreeOfParallelism = threadAmount;
           

            var erwriter = new StreamWriter(Path.Combine(_docPath, "error.txt"));

            erwriter.WriteLine("{0} equals {1}", allLength, total);

            var source = Enumerable.Range(0, _iYmax).ToArray();
            var rangePartitioner = Partitioner.Create(0, source.Length);

            Parallel.ForEach(rangePartitioner, options, (range, loopState) =>
            {
               
                double Cx, Cy;
                double Zx, Zy;
                double Zx2, Zy2;
                int Iteration;
                Thread thread = Thread.CurrentThread;
                for (int iY = range.Item1; iY < range.Item2; iY++)
                {
                    int y = iY;
                    Cy = _cyMin + y * _pixelHeight;
                    if (Math.Abs(Cy) < _pixelHeight / 2)
                        Cy = 0.0;
                    for (int iX = 0; iX < _iXmax; iX++)
                    {
                        int x = iX;
                        Cx = _cxMin + x * _pixelWidth;
                        Zx = 0.0;
                        Zy = 0.0;
                        Zx2 = Zx * Zx;
                        Zy2 = Zy * Zy;
                        for (Iteration = 0; Iteration < _iterationMax && ((Zx2 + Zy2) < _eR2); Iteration++)
                        {
                            Zy = 2 * Zx * Zy + Cy;
                            Zx = Zx2 - Zy2 + Cx;
                            Zx2 = Zx * Zx;
                            Zy2 = Zy * Zy;
                        };
                        if (Iteration == _iterationMax)
                        {
                            color[x, y, 0] = 0;
                            color[x, y, 1] = 0;
                            color[x, y, 2] = 0;
                        }
                        else
                        {  
                            color[x, y, 0] = R[thread.ManagedThreadId%threadAmount];
                            color[x, y, 1] = G[thread.ManagedThreadId%threadAmount];
                            color[x, y, 2] = B[thread.ManagedThreadId%threadAmount];
                        };
                    }
                }
            });
            erwriter.Close();


            
            int id = 0;
            string curFile = "fractal" + id + ".ppm";
            while (true)
                if (File.Exists(Path.Combine(_docPath, curFile)))
                {
                    id++;
                    curFile = "fractal" + id + ".ppm";
                }
                else {
                    break;
                }
            string filename = curFile;
            var writer = new StreamWriter(Path.Combine(_docPath, filename));
            writer.WriteLine("P6");
            writer.WriteLine($"{_iXmax}\n{_iYmax}");
            writer.WriteLine($"{_maxColorComponentValue}");
            writer.Close();

            using (BinaryWriter writerB = new BinaryWriter(new FileStream(Path.Combine(_docPath, filename), FileMode.Append)))
            {
                for (int i = 0; i < _iXmax; i++)
                    for (int j = 0; j < _iYmax; j++)
                    {
                        writerB.Write(color[j, i, 0]);
                        writerB.Write(color[j, i, 1]);
                        writerB.Write(color[j, i, 2]);
                    }

            }

            return filename;

        }

        public string ReturnSet(string name) {
            int row;
            int col;
            string set ="";
            using (TextReader reader = File.OpenText(Path.Combine(_docPath, name)))
            {
                string lineOfText;
                string text = reader.ReadLine();
                text = reader.ReadLine();
                row = int.Parse(text);
                text = reader.ReadLine();
                col = int.Parse(text);
                set += row + "\n" + col + "\n";
                
                byte[] test = File.ReadAllBytes(Path.Combine(_docPath, name));
                foreach (var b in test)
                {
                    set+=(b + ", ");
                }

            }
            
            return set;
        }
    }
}