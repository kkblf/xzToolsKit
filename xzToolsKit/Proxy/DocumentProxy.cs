using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using xzToolsKit.model;

namespace xzToolsKit.proxy
{
    public class DocumentProxy
    {
        public TrialInfo getTrialInfo(String key)
        {
            String url = "https://xz.glodon.com/document/trial/{0:s}";

            using (HttpClient client = new HttpClient())
            {
                var response = client.GetAsync(new Uri(String.Format(url, key)));
                response.Result.EnsureSuccessStatusCode();
                string responseStr = response.Result.Content.ReadAsStringAsync().Result.ToString();

                TrialInfo _response = JsonConvert.DeserializeObject<TrialInfo>(responseStr);

                return _response;
            }

        }
        public Page<List<FileItem>> list(String workspaceId,String fileId)
        {
            String url = "https://api.glodon.com/v3/doc/{0:s}/file/children?fileId={1:s}";

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", DataCenter.loginResponse.access_token);

                var response = client.GetAsync(new Uri(String.Format(url, workspaceId, fileId)));
                response.Result.EnsureSuccessStatusCode();
                string responseStr = response.Result.Content.ReadAsStringAsync().Result.ToString();

                WorkspaceResponse<Page<List<FileItem>>> _response = JsonConvert.DeserializeObject<WorkspaceResponse<Page<List<FileItem>>>>(responseStr);

                return _response.data;
            }
        }


        public FileItem file(String workspaceId, String fileId)
        {
            if (fileId == null) return null;
            String url = "https://api.glodon.com/v3/doc/{0:s}/file/meta?fileId={1:s}";

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", DataCenter.loginResponse.access_token);

                var response = client.GetAsync(new Uri(String.Format(url, workspaceId, fileId)));
                response.Result.EnsureSuccessStatusCode();
                string responseStr = response.Result.Content.ReadAsStringAsync().Result.ToString();

                WorkspaceResponse<FileItem> _response = JsonConvert.DeserializeObject<WorkspaceResponse<FileItem>>(responseStr);

                return _response.data;
            }
        }

        public void download(String workspaceId, String fileId, System.IO.FileStream os,FrmMain form)
        {
            String url = "https://api.glodon.com/v3/doc/{0:s}/file/data?fileId={1:s}";

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", DataCenter.loginResponse.access_token);
                
                var response = client.GetStreamAsync(new Uri(String.Format(url, workspaceId, fileId)));
                var inputStream = response.Result;
                byte[] buffer = new byte[1024];
                int totalCount = 0;
                int curCount = 0;
                //下面判断结束的标志不可用
                //先用粗糙的方式判断
                int totalNotRead = 0;
                while ((curCount = inputStream.Read(buffer, 0, 1024)) != -1)
                {
                    os.Write(buffer, 0, curCount);
                    totalCount += curCount;
                    form.showProgress(totalCount);
                    if (totalNotRead > 100)
                        break;
                    if(curCount==0)
                      totalNotRead++;

                }
                
            }

        }



        public void upload(String workspaceId, String parentId, String fileName,long size, System.IO.Stream stream, FrmMain form)
        {
            String url = "https://api.glodon.com/v3/doc/{0:s}/file/data?parentId={1:s}&fileName={2:s}&size={3:d}";           


            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", DataCenter.loginResponse.access_token);

                StreamContent streamContent = new StreamContent(stream);

                var response = client.PostAsync(new Uri(String.Format(url, workspaceId, parentId,fileName,size)), streamContent);
                response.Result.EnsureSuccessStatusCode();
            }

        }
    }
}
