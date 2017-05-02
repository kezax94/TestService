using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Globalization;

namespace TaxServiceDemo
{
    [ServiceContract]
    public interface ITaxService
    {
        //POST operation
        [OperationContract]
        [WebInvoke(UriTemplate = "import", Method = "POST", RequestFormat = WebMessageFormat.Json)]
        List<Tax> CreateTaxes(List<Tax> createTaxes);

        [OperationContract]
        [WebInvoke(UriTemplate = "", Method = "POST", RequestFormat = WebMessageFormat.Json)]
        Tax CreateTax(Tax createTax);

        //Get Operation
        [OperationContract]
        [WebGet(UriTemplate = "", ResponseFormat = WebMessageFormat.Json)]
        List<Tax> GetAllTaxes();

        [OperationContract]
        [WebGet(UriTemplate = "{town}", ResponseFormat = WebMessageFormat.Json)]
        List<Tax> GetAllTaxesOfTown(string town);

        [OperationContract]
        [WebGet(UriTemplate = "{town}/{date}", ResponseFormat = WebMessageFormat.Json)]
        double GetRate(string town, string date);

        //DELETE Operations
        [OperationContract]
        [WebInvoke(UriTemplate = "{town}", Method = "DELETE")]
        void DeleteTaxes(string town);

        [OperationContract]
        [WebInvoke(UriTemplate = "", Method = "DELETE")]
        void DeleteAll();
    }

    public enum TaxTimeSpan
    {
        Year,
        Month,
        Week,
        Day
    }
}
