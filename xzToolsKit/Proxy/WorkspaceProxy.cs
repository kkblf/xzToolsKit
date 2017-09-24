using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using xzToolsKit.model;

namespace xzToolsKit.proxy
{
    public class WorkspaceProxy
    {
        public List<WorkspaceMeta> list()
        {
            String url = "https://api.glodon.com/v3/ws/";

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", DataCenter.loginResponse.access_token);

                var response = client.GetAsync(new Uri(url));
                response.Result.EnsureSuccessStatusCode();
                string responseStr = response.Result.Content.ReadAsStringAsync().Result.ToString();

                WorkspaceResponse<List<WorkspaceMeta>> _response = JsonConvert.DeserializeObject<WorkspaceResponse<List<WorkspaceMeta>>>(responseStr);

                return _response.data;
            }
        }
       
    }
}
