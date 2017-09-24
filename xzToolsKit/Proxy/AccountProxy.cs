using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using xzToolsKit.model;
using xzToolsKit.Handler;

namespace xzToolsKit.proxy
{
    public class AccountProxy
    {
        public LoginResponse login(String grandType, String userName, String password,String auth)
        {
            String url = "https://account.glodon.com/oauth2/token";

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", auth);

                var content = new FormUrlEncodedContent(new Dictionary<string, string>()       
                    {    {"grant_type",grandType},
                            {"username",userName},
                            {"password",password}
                        });

                var response = client.PostAsync(new Uri(url), content);
                ExceptionHandler.handle( response,1);
                string responseStr = response.Result.Content.ReadAsStringAsync().Result.ToString();

                LoginResponse loginResponse = JsonConvert.DeserializeObject<LoginResponse>(responseStr);

                return loginResponse;
            }
        }
       
    }
}
