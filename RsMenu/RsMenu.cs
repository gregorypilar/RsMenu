using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using RsMenu.Properties;

namespace RsMenu
{
    
    public  class RsMenu : MenuStrip
    {
        public class ShowReportsEventArgs : EventArgs
        {
            public ShowReportsEventArgs(string path)
            {
                ReportPath = path;
            }

            public string ReportPath;

            public override string ToString()
            {
                return ReportPath;
            }
        }

        public delegate void ShowReportEventHandler(object sender, ShowReportsEventArgs e);

        //
        [System.ComponentModel.SettingsBindable(true)]
        public string WebServiceUrl { get; set; }
        public Image FolderImage { get; set; }
        public Image ReportImage { get; set; }
        public bool UseDefaultCredentials { get; set; }
        [System.ComponentModel.SettingsBindable(true)]
        public string Reportpath { get; set; }
        [System.ComponentModel.DefaultValue( false)]
        public bool ShowHidenItems { get; set; }
        private ReportingService2005 ReportingService2005 { get; set; }
        
        public event EventHandler<ShowReportsEventArgs> ShowReport;
        //
        private readonly ToolStripMenuItem _menuItem;
        private readonly ToolStripDropDownItem _rItem;

        public RsMenu()
        {
            this.ReportImage = Resources.report;
            this.FolderImage = Resources.folder;
            this._menuItem = new ToolStripMenuItem("Reportes");
            //
            this._rItem = new ToolStripMenuItem("Refresh", Resources.refresh);
            _rItem.Click += RItemClick;
            this._menuItem.DropDownItems.Add(_rItem);
            //
            this.Items.Add(this._menuItem);
        }

        public RsMenu(string url)
        {
            this.WebServiceUrl = url;
            this.ReportingService2005 = new ReportingService2005
                                       {
                                           Url = WebServiceUrl,
                                           UseDefaultCredentials = UseDefaultCredentials
                                       };

            this.ReportImage = Resources.report;
            this.FolderImage = Resources.folder;
            this._menuItem = new ToolStripMenuItem("Reportes");
            //
            this._rItem = new ToolStripMenuItem("Refresh", Resources.refresh);
            _rItem.Click += RItemClick;
            this._menuItem.DropDownItems.Add(_rItem);
            //
            this.Items.Add(this._menuItem);
        }

        void RItemClick(object sender, EventArgs e)
        {
            this._menuItem.DropDownItems.Clear();
            this._menuItem.DropDownItems.Add(this._rItem);
            var temp = new Thread(Reportes);
            temp.Start();
        }

        public  void Reportes()
        {
            GetReports(this._rItem);
        }

        protected virtual void OnShow(object sender, ShowReportsEventArgs e)
        {
            EventHandler<ShowReportsEventArgs> handler = ShowReport;
            if (handler != null)
            {
                handler(sender, e);
            }
        }

        public void GetReports(ToolStripDropDownItem menuItem)
        {
            ReportingService2005 service2005 = ReportingService2005;
            //
            List<CatalogItem> list = service2005.ListChildren(Reportpath, false).ToList();
            list.RemoveAll(i => i.Hidden == ShowHidenItems || (i.Type != ItemTypeEnum.Report && i.Type != ItemTypeEnum.Folder));

            foreach (CatalogItem item in list)
            {
                if (item.Type == ItemTypeEnum.Report)
                {
                    string rName = item.Name.Replace("/", "").Replace("_", " ").ToProperCase().Trim();
                    string rTag = item.Path;
                    var rItem = new ToolStripMenuItem(rName) { Image = FolderImage, Tag = rTag };
                    rItem.Click += ((sender, e) => OnShow(sender, new ShowReportsEventArgs(((ToolStripMenuItem)sender).Tag.ToString())));
                    menuItem.DropDownItems.Add(rItem);
                }
                if (item.Type == ItemTypeEnum.Folder)
                {
                    string rName = item.Name.Replace("/", " ").Replace("_", " ").ToProperCase().Trim();
                    var fItem = new ToolStripMenuItem(rName) { Image = FolderImage };
                    menuItem.DropDownItems.Add(fItem);
                    GetItems(ref service2005, item, ref fItem);
                }
            }
        }


        private void GetItems(ref ReportingService2005 service2005, CatalogItem item,
                              ref ToolStripMenuItem menuItem)
        {
            List<CatalogItem> listDependentItems = service2005.ListChildren(item.Path, false).ToList();
            listDependentItems.RemoveAll(
                i => i.Hidden == ShowHidenItems || (i.Type != ItemTypeEnum.Report && i.Type != ItemTypeEnum.Folder));
            //
            foreach (CatalogItem catalogItem in listDependentItems)
            {
                if (catalogItem.Type == ItemTypeEnum.Report)
                {
                    string rName = catalogItem.Name.Replace("/", " ").Replace("_", " ").ToProperCase().Trim();
                    string rTag = catalogItem.Path;
                    var rItem = new ToolStripMenuItem(rName) { Image = ReportImage, Tag = rTag };
                    rItem.Click += ((sender, e) => OnShow(sender, new ShowReportsEventArgs(((ToolStripMenuItem)sender).Tag.ToString())));
                    menuItem.DropDownItems.Add(rItem);
                    continue;
                }
                if (catalogItem.Type == ItemTypeEnum.Folder)
                {
                    string rName = catalogItem.Name.Replace("/", " ").Replace("_", " ").ToProperCase().Trim();
                    var fItem = new ToolStripMenuItem(rName) { Image = FolderImage };
                    menuItem.DropDownItems.Add(fItem);
                    //
                    GetItems(ref service2005, catalogItem, ref fItem);
                }
            }
        }
    }
}
