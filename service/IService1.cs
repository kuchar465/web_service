using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WcfService1
{
    
    [ServiceContract]
    public interface IService1
    {
        [OperationContract]
        string GetMatrix(String matrix);

        [OperationContract]
        string MultiplyMatrix(String matrixA, String matrixB);

        [OperationContract]
        string SendMatrix(String matrix, int row, int col);
    }

    [ServiceContract]
    public interface IService2
    {
        [OperationContract]
        string GenerateMandelbrottSet(int x, int y, int threadsAmount);
        [OperationContract]
        string GetMandelbrottSet(string name);

        
    }
   
}
