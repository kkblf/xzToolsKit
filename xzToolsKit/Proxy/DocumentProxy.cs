using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using xzToolsKit.model;
using System.Web;
using System.Net;
using System.Windows.Forms;
using System.IO;
using System.Text;

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



        public void upload(String workspaceId, String parentId, String fileName, long size, System.IO.Stream stream, FrmMain form)
        {
            String url = "https://api.glodon.com/v3/doc/{0:s}/file/data?parentId={1:s}&fileName={2:s}&size={3:d}";

            var request = HttpWebRequest.CreateHttp(String.Format(url, workspaceId, parentId, fileName, size));
            request.Method = "POST";
            request.AllowWriteStreamBuffering = false;
            request.ContentLength = size;
            request.Headers.Add(HttpRequestHeader.Authorization, "Bearer " + DataCenter.loginResponse.access_token);
            Stream postStream = request.GetRequestStream();

            try
            {
                var progressBar = form.getToolStripProgressBar1();
                progressBar.Maximum =(int)size;
                progressBar.Minimum = 0;
                progressBar.Value = 0;

                var sb = new StringBuilder();

                //每次上传4k 
                int bufferLength = 4096;
                byte[] buffer = new byte[bufferLength];

                //已上传的字节数 
                long offset = 0;

                //开始上传时间 
                DateTime startTime = DateTime.Now;
                int readSize = stream.Read(buffer, 0, bufferLength);
                while (readSize > 0)
                {
                    postStream.Write(buffer, 0, readSize);

                    offset += readSize;
                    progressBar.Value = (int)offset;
                    TimeSpan span = DateTime.Now - startTime;
                    double second = span.TotalSeconds;

                    calProgress(size, form, offset, second);
                    Application.DoEvents();

                    readSize = stream.Read(buffer, 0, bufferLength);
                }
                postStream.Close();

                //获取服务器端的响应 
                WebResponse webRespon = request.GetResponse();
                Stream s = webRespon.GetResponseStream();
                StreamReader sr = new StreamReader(s);

                //读取服务器端返回的消息 
                String sReturnString = sr.ReadToEnd();
                s.Close();
                sr.Close();
                WorkspaceResponse<FileItem> _response = JsonConvert.DeserializeObject<WorkspaceResponse<FileItem>>(sReturnString);
                if (_response.code == 0)
                {
                    form.showMessage("上传成功！");
                } 
                else if (_response != null)
                {
                    form.showMessage("上传失败！["+_response.code+","+_response.message!=null?_response.message:""+"]");

                }
                else 
                {
                    form.showMessage("上传失败！[null]");
                }
            }
            catch(Exception ept)
            {
                form.showMessage("上传失败！" + ept.Message);
            }
            finally
            {
                stream.Close();
            }

        }

        private static void calProgress(long size, FrmMain form, long offset, double second)
        {
            StringBuilder sb = new StringBuilder();
            sb.Clear();
            sb.Append("已用时：" + second.ToString("F2") + "秒 ");
            if (second > 0.001)
            {
                sb.Append(" 平均速度：" + (offset / 1024 / second).ToString("0.00") + "KB/秒 ");
            }
            else
            {
                sb.Append(" 正在连接… ");
            }
            sb.Append("已上传：" + (offset * 100.0 / size).ToString("F2") + "% ");
            sb.Append((offset / 1048576.0).ToString("F2") + "M/" + (size / 1048576.0).ToString("F2") + "M ");
            form.showMessage(sb.ToString());
        }
    }
}
