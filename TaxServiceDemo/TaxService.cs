using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Web;
using System.ServiceModel.Activation;
using System.ServiceModel;
using System.Globalization;

namespace TaxServiceDemo
{
    /// <summary>
    /// Basically this code is developed for HTTP GET, PUT, POST & DELETE operation.
    /// </summary>
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class TaxService : ITaxService
    {
        public List<Tax> CreateTaxes(List<Tax> createTaxes)
        {
            var dc = new TaxClassesDataContext();
            dc.ExecuteCommand("TRUNCATE TABLE Tax");
            Table<Tax> taxes = dc.Taxes;
            foreach (Tax t in createTaxes)
            {
                try
                {
                    taxes.InsertOnSubmit(t);
                }
                catch (Exception)
                {
                    return null;
                }
            }
            taxes.Context.SubmitChanges();
            return createTaxes;
        }

        public Tax CreateTax(Tax createTax)
        {
            Table<Tax> taxes = new TaxClassesDataContext().Taxes;
            try
            {
                taxes.InsertOnSubmit(createTax);
            }
            catch (Exception)
            {
                return null;
            }
            taxes.Context.SubmitChanges();
            return createTax;
        }

        public double GetRate(string town, string date)
        {
            DateTime dt;
            DateTime.TryParseExact(date, 
                                   "yyyy.MM.dd", 
                                   CultureInfo.InvariantCulture, 
                                   DateTimeStyles.None, 
                                   out dt);
            if (dt != null)
            {
                TaxClassesDataContext dc = new TaxClassesDataContext();
                List<Tax> result = dc.Taxes.Where(t => t.Town.Equals(town) && dt >= t.DateBegin && dt <= t.DateEnd).OrderBy(t => t.TimeSpan).ToList();
                if (result.Count > 0)
                {
                    return result.Last().Rate;
                }
                else
                {
                    return 0.0;
                }
            }
            else
            {
                return 0.0;
            }
        }

        public List<Tax> GetAllTaxes()
        {
            TaxClassesDataContext dc = new TaxClassesDataContext();
            return dc.Taxes.ToList();
        }

        public List<Tax> GetAllTaxesOfTown(string town)
        {
            TaxClassesDataContext dc = new TaxClassesDataContext();
            return dc.Taxes.Where(t => t.Town.Equals(town)).ToList();
        }

        public void DeleteTaxes(string town)
        {
            TaxClassesDataContext dc = new TaxClassesDataContext();
            List<Tax> matchedTaxes = dc.Taxes.Where(t => t.Town.Equals(town)).ToList();
            dc.Taxes.DeleteAllOnSubmit(matchedTaxes);
            dc.SubmitChanges();
        }

        public void DeleteAll()
        {
            new TaxClassesDataContext().ExecuteCommand("TRUNCATE TABLE Tax");
        }
    }
}
