using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Diagnostics;
using System.Threading.Tasks;

namespace WcfService1
{
    public class MatrixOperations
    {
        //private string _docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        private string _docPath = "D:/studia/przetwarzanie rozproszone/projekt/wdsl/wynik/";
        private MatrixInfo _matrixA;
        private MatrixInfo _matrixB;

        private int _LastMatrixRow = 0;
        private int _LastMatrixCol = 0;

        struct MatrixInfo {
            public MatrixInfo(float[,] matrix, int row, int col)
            {
                Matrix = matrix;
                Row = row;
                Col = col;
            }
            public float[,] Matrix { get; }
            public int Row { get; }
            public int Col { get; }
        }

        public string WriteMatrixToFile(string[] matrix, int row, int col, string name = "matrix")
        {
            int id = 0;
            string curFile = name + id + ".txt";
            while (true)
            {
                if (File.Exists(Path.Combine(_docPath, curFile)))
                {
                    id++;
                    curFile = name + id + ".txt";
                }
                else
                {
                    using (StreamWriter outputFile = new StreamWriter(Path.Combine(_docPath, curFile)))
                    {
                        int counter = 0;
                        outputFile.Write(row + " " + col);
                        foreach (string line in matrix)
                        {
                            if (counter % col == 0)
                            {
                                outputFile.Write("\n");
                            }
                            outputFile.Write(line + " ");
                            counter++;
                        }

                    }
                    break;
                }
            }

            string[] matrixName = curFile.Split('.');
            return matrixName[0];
        }

        public string MultiplyMatrix(string A, string B)
        {
             _matrixA = ReadMatrixFromFile(A);
             _matrixB = ReadMatrixFromFile(B);
            float[,] C = new float[_matrixA.Row, _matrixA.Col];
            string message = "";

            for (int i = 0; i < _matrixA.Row; ++i)
                for (int j = 0; j < _matrixB.Col; ++j)
                {
                    C[i, j] = 0;
                }

            Parallel.For(0, _matrixA.Row, i =>
            {
                for (int j = 0; j < _matrixB.Col; ++j)
                    for (int k = 0; k < _matrixA.Col; ++k)
                    {
                        C[i, j] += _matrixA.Matrix[i, k] * _matrixB.Matrix[k, j];
                    }
            });

            for (int i = 0; i < _matrixA.Row; ++i)
            {
                for (int j = 0; j < _matrixB.Col; ++j)
                {
                    message += C[i, j] + " ";
                }
            }
            _LastMatrixCol = _matrixB.Col;
            _LastMatrixRow = _matrixA.Row;
            return message;
        }

        public int GetLastMultipliedMatrixColSize()
        {
            return _LastMatrixCol;
        }

        public int GetLastMultipliedMatrixRowSize()
        {
            return _LastMatrixRow;
        }

        private MatrixInfo ReadMatrixFromFile(string fileName)
        {
            int row;
            int col;
            float[,] matrix;
            using (TextReader reader = File.OpenText(Path.Combine(_docPath, fileName + ".txt")))
            {
                string text = reader.ReadLine();
                string[] bits = text.Split(' ');
    
                row = int.Parse(bits[0]);
                col = int.Parse(bits[1]);

                matrix = new float[row,col];

                for (int i = 0; i < row; i++) {
                    text = reader.ReadLine();
                    bits = text.Split(' ');
                    for (int j = 0; j < col; j++) {
                        matrix[i, j] = float.Parse(bits[j]);
                    }
                }
            }
            MatrixInfo matrixInfo = new MatrixInfo(matrix, row, col);
            return matrixInfo;
        }

        public string ReadMatrix(string fileName)
        {
            int row;
            int col;
            float[,] matrix;
            string message = "";
            using (TextReader reader = File.OpenText(Path.Combine(_docPath, fileName + ".txt")))
            {
                string text = reader.ReadLine();
                string[] bits = text.Split(' ');

                row = int.Parse(bits[0]);
                col = int.Parse(bits[1]);

                matrix = new float[row, col];

                for (int i = 0; i < row; i++)
                {
                    text = reader.ReadLine();
                    message += text + "\n"; 
                }
            }
            return message;
        }

    }
}