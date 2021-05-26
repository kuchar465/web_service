using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WcfService1
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1, IService2
    {
        private MatrixOperations _matrixOperations = new MatrixOperations();
        private MandelbrotGenerator _generator = new MandelbrotGenerator();

        public string SendMatrix(string matrix, int row, int col)
        {
            string[] subs = matrix.Split(' ');
            string message = _matrixOperations.WriteMatrixToFile(subs, row, col);
            return string.Format("Matrix file name: {0}", message);
        }

        public string GetMatrix(string matrix)
        {
            string message = _matrixOperations.ReadMatrix(matrix);
            return string.Format("You have chosen: {0}", message);
        }

        public string MultiplyMatrix(string matrixA, string matrixB)
        {
            string matrix = _matrixOperations.MultiplyMatrix(matrixA, matrixB);
            string[] subs = matrix.Split(' ');
            string message = _matrixOperations.WriteMatrixToFile(subs, _matrixOperations.GetLastMultipliedMatrixRowSize(), _matrixOperations.GetLastMultipliedMatrixColSize(), "result");
            return string.Format("Matrix file name: {0}", message);
        }

        public string GenerateMandelbrottSet(int x, int y, int threadsAmount)
        {
            string message =_generator.Generate_set(x, y, threadsAmount);
            return string.Format("Matrix file name: {0}", message);
        }

        public string GetMandelbrottSet(string name)
        {
            string message = _generator.ReturnSet(name+".ppm");
            return string.Format("Matrix file name: {0}", message);
        }
    }

}
