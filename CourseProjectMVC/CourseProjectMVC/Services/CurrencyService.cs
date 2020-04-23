using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CourseProjectMVC.Interfaces;
using CourseProjectMVC.Models;
using Newtonsoft.Json;

namespace CourseProjectMVC.Services
{
    public class CurrencyService : IСurrencyService
    {
        public async Task<string> GetCurrencies()
        {
            using (var client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync("https://bank.gov.ua/NBUStatService/v1/statdirectory/exchange?json");
                if (response.IsSuccessStatusCode)
                {
                    string str = await response.Content.ReadAsStringAsync();
                    return str;
                }
            }

            return null;
        }

        public List<CurrencyModel> ParseCurrenciesToModel(string json)
        { 
            var list =JsonConvert.DeserializeObject<List<CurrencyModel>>(json);
            return list;
        }

    }
}
