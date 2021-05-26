﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WcfService1
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IService1
    {
        [OperationContract]
        string GetMatrix(String matrix);

        [OperationContract]
        string MultiplyMatrix(String matrixA, String matrixB);

        [OperationContract]
        string SendMatrix(String matrix, int row, int col);



        // TODO: Add your service operations here
    }

    [ServiceContract]
    public interface IService2
    {
        [OperationContract]
        string GenerateMandelbrottSet(int x, int y, int threadsAmount);
        [OperationContract]
        string GetMandelbrottSet(string name);

        // TODO: Add your service operations here
    }


    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    [DataContract]
    public class CompositeType
    {
        bool boolValue = true;
        string stringValue = "Hello ";

        [DataMember]
        public bool BoolValue
        {
            get { return boolValue; }
            set { boolValue = value; }
        }

        [DataMember]
        public string StringValue
        {
            get { return stringValue; }
            set { stringValue = value; }
        }
    }

   
}