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
        private readonly IRedisService _redisService;
        public CurrencyService(IRedisService s)
        {
            _redisService = s;
        }
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

        public async Task<double> GetCurrencyByKey(string key)
        {
            if (key == null)
                return 1;
            var str = await _redisService.Consume("currency");
            var currenciesString = str.Value;
            var currenciesModel = ParseCurrenciesToModel(currenciesString);
            var nums = currenciesModel.Where(z => z.cc == key).Select(x => x.rate);
            return nums.Any() ? nums.First() : 1;
        }

        public List<CurrencyModel> ParseCurrenciesToModel(string json)
        { 
            var list =JsonConvert.DeserializeObject<List<CurrencyModel>>(json);
            return list;
        }

    }
}
