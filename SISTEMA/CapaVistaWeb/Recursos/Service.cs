//using RestSharp;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;

namespace MadereraCarocho.Recursos
{
    public class Service
    {
        //public async Task<bool> IsValidEmail(string email, int timeEspera)
        //{
        //    try
        //    {
        //        var client = new RestClient("https://api.apilayer.com/email_verification/check?email=reyesanticona25%40gmail.com");
        //        client.Timeout = -1;

        //        var request = new RestRequest(Method.GET);
        //        request.AddHeader("apikey", "ZeYgPtPHw55Pr5g8uoWg6x3rTNHKX8TH");

        //        IRestResponse response = await client.ExecuteAsync(request);
        //        if (response.IsSuccessful)
        //        {
        //            dynamic data = JObject.Parse(response.Content);
        //            if (data == null || data.format_valid == null || data.smtp_check == null || data.disposable == null)
        //            {
        //                return false;
        //            }

        //            return (bool)data.format_valid && (bool)data.smtp_check && (bool)data.disposable;
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //    catch
        //    {
        //        // Si ocurre una excepción, devolver falso
        //        return false;
        //    }
        //}
    }
}
