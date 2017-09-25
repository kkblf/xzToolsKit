using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using xzToolsKit.model;
using xzToolsKit.proxy;
using xzToolsKit.Service;

namespace xzToolsKit
{
    public partial class FrmMain : Form
    {
        /**
         * 登录服务 
         **/
        private LoginService loginService = null;
        private WorkspaceProxy workspaceProxy;
        private DocumentProxy documentProxy;
        private WorkspaceService workspaceService;

        public FrmMain()
        {
            InitializeComponent();
        }

        public void init()
        {
            loginService = new LoginService();
            if (loginService.needLogin())
            {
                FrmLogin frmLogin = new FrmLogin();
                DialogResult dialogResult = frmLogin.ShowDialog();
                if (dialogResult == DialogResult.OK)
                {
                    this.Show();
                }
            } else
            {
                this.Show();
            }


            workspaceProxy = new WorkspaceProxy();
            documentProxy = new DocumentProxy();
            workspaceService = new WorkspaceService(workspaceProxy);
            DataCenter.currentWorkspaceId = DataCenter.wsId;

            load();
        }

        public void load()
        {
            try
            {
                workspaceService.checkWorkspaceIsRight(DataCenter.currentWorkspaceId);
                loadChild(DataCenter.currentWorkspaceId);

            } catch(Exception ept)
            {
                MessageBox.Show("加载数据出错："+ept.Message);
            }

        }

        private void loadWorkspaceList()
        {
            clearListView();
            DataCenter.parentId = null;
            DataCenter.currentWorkspaceId = null;
            this.listView1.View = View.LargeIcon;
            DataCenter.workspaceList.ForEach(_ =>
            {
                ListViewItem item = new ListViewItem(_.name);
                item.ImageIndex = 0;
                item.Tag = _;
                this.listView1.Items.Add(item);

            });
        }

        private void clearListView()
        {
            this.listView1.Clear();
        }


        public void showProgress(Int32 value)
        {
            toolStripProgressBar1.Value = value;
            Application.DoEvents();
        }



        public void loadChild(String fileId)
        {
            Page<List<FileItem>> page = documentProxy.list(DataCenter.currentWorkspaceId, fileId);
            // if (page.items.Count > 0)
            // {
            DataCenter.parentId = fileId;
            loadFileItemListView(page.items);
            //}
        }




        public void loadFileItemListView(List<FileItem> fileItemList)
        {
            clearListView();
            if (fileItemList == null)
                return;
            this.listView1.View = View.List;
            fileItemList.OrderByDescending(_ => _.folder).ThenBy(_ => _.name).ToList().ForEach(_ =>
            {
                ListViewItem item = new ListViewItem(_.name);
                if (_.folder)
                {
                    item.ImageKey = "folder";
                }
                else
                {
                    item.ImageKey = "file";
                }
                item.Tag = _;
                this.listView1.Items.Add(item);

            });
        }

        private void 列表ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.listView1.View = View.List;
        }

        private void 缩略图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.listView1.View = View.LargeIcon;
        }

        private void 其它ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int viewValue = (int)this.listView1.View;
            this.listView1.View = (View)((viewValue + 1) % 5);
        }



        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.listView1.SelectedItems != null && this.listView1.SelectedItems.Count > 0)
            {
                Node node = this.listView1.SelectedItems[0].Tag as Node;
                String fileId = "";
                if (node.type == 1 || node.type == 0)
                {
                    //节点表示空间
                    WorkspaceMeta workspaceMeta = node as WorkspaceMeta;
                    DataCenter.currentWorkspaceId = workspaceMeta.id;
                    fileId = workspaceMeta.id;
                    loadChild(fileId);
                }
                else if (node.type == 2)
                {
                    //节点表示文件或文件夹
                    FileItem fileItem = node as FileItem;
                    fileId = fileItem.fileId;

                    if (fileItem.folder)
                    {
                        //文件夹加载子文件
                        loadChild(fileId);
                    }
                    else
                    {
                        //文件执行下载
                        download(fileId);
                    }
                }

            }
        }



        private void btnHome_Click(object sender, EventArgs e)
        {
            load();
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            FileItem fileItem = documentProxy.file(DataCenter.currentWorkspaceId, DataCenter.parentId);
            if (fileItem == null || fileItem.parentId == null || fileItem.parentId == DataCenter.currentWorkspaceId)
            {
                load();
            }
            else
            {
                loadChild(fileItem.parentId);
            }
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            if (this.listView1.SelectedItems != null && this.listView1.SelectedItems.Count > 0)
            {
                Node node = this.listView1.SelectedItems[0].Tag as Node;
                String fileId = "";
                if (node.type == 2)
                {
                    FileItem fileItem = node as FileItem;
                    if (fileItem.folder)
                    {
                        MessageBox.Show("请选择文件");
                    }
                    else
                    {
                        fileId = fileItem.fileId;
                        download(fileId);

                    }
                }
            }

        }

        public void download(String fileId)
        {
            //设置进度
            FileItem _file = documentProxy.file(DataCenter.currentWorkspaceId, fileId);
            toolStripProgressBar1.Maximum = _file.length.Value;
            toolStripProgressBar1.Value = 0;

            //下载文件
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.RestoreDirectory = true;
            sfd.FileName = _file.name + "." + _file.suffix;
            sfd.DefaultExt = _file.suffix;
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                using (System.IO.FileStream fs = (System.IO.FileStream)sfd.OpenFile())
                {
                    documentProxy.download(DataCenter.currentWorkspaceId, fileId, fs, this);
                    fs.Flush();
                    fs.Close();
                }
                MessageBox.Show("下载完成");
            }
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            if (DataCenter.parentId == null) return;
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.RestoreDirectory = true;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                System.IO.Stream file = ofd.OpenFile();
                toolStripProgressBar1.Maximum = Convert.ToInt32(file.Length);
                toolStripProgressBar1.Value = 0;
                documentProxy.upload(DataCenter.currentWorkspaceId, DataCenter.parentId, System.IO.Path.GetFileName(ofd.FileName), file.Length, file, this);
                MessageBox.Show("上传完成");
                loadChild(DataCenter.parentId);
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            DataCenter.clear();
            this.Close();
        }
    }
}
