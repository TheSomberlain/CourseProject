using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourseProjectMVC.Models;

namespace CourseProjectMVC.Interfaces
{
    public interface IСurrencyService
    {
        Task<string> GetCurrencies();
        List<CurrencyModel> ParseCurrenciesToModel(string json);
        Task<double> GetCurrencyByKey(string key);
    }
}
