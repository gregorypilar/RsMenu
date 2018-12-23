using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using RsMenu.Properties;

namespace RsMenu
{
    /// <summary>
    /// 
    /// </summary>
    /// 11/02/2011 by gpilar
    public class RsMenu : MenuStrip
    {
        /// <summary>
        /// 
        /// </summary>
        /// 11/02/2011 by gpilar
        public class ShowReportsEventArgs : EventArgs
        {
            /// <summary>
            /// Inicializa una nueva instancia de la class <see cref="ShowReportsEventArgs"/>.
            /// </summary>
            /// <param name="path">The path.</param>
            /// 11/02/2011 by gpilar
            public ShowReportsEventArgs(string path)
            {
                ReportPath = path;
            }

            /// <summary>
            /// 
            /// </summary>
            public string ReportPath;

            public override string ToString()
            {
                return ReportPath;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public delegate void ShowReportEventHandler(object sender, ShowReportsEventArgs e);

        //
        /// <summary>
        /// Gets or sets the web service URL.
        /// </summary>
        /// <value>The web service URL.</value>
        /// 11/02/2011 by gpilar
        [System.ComponentModel.SettingsBindable(true)]
        public string WebServiceUrl { get; set; }
        /// <summary>
        /// Gets or sets the folder image.
        /// </summary>
        /// <value>The folder image.</value>
        /// 11/02/2011 by gpilar
        public Image FolderImage { get; set; }
        /// <summary>
        /// Gets or sets the report image.
        /// </summary>
        /// <value>The report image.</value>
        /// 11/02/2011 by gpilar
        public Image ReportImage { get; set; }
        /// <summary>
        /// Gets or sets un valor indicando si [use default credentials].
        /// </summary>
        /// <value>
        /// 	<c>true</c> si [use default credentials]; en otro caso, <c>false</c>.
        /// </value>
        /// 11/02/2011 by gpilar
        public bool UseDefaultCredentials { get; set; }
        /// <summary>
        /// Gets or sets the reportpath.
        /// </summary>
        /// <value>The reportpath.</value>
        /// 11/02/2011 by gpilar
        [System.ComponentModel.SettingsBindable(true)]
        public string Reportpath { get; set; }
        /// <summary>
        /// Gets or sets un valor indicando si [show hiden items].
        /// </summary>
        /// <value><c>true</c> si [show hiden items]; en otro caso, <c>false</c>.</value>
        /// 11/02/2011 by gpilar
        [System.ComponentModel.DefaultValue(false)]
        public bool ShowHidenItems { get; set; }
        /// <summary>
        /// Gets or sets the reporting service2005.
        /// </summary>
        /// <value>The reporting service2005.</value>
        /// 11/02/2011 by gpilar
        private ReportingService2005 ReportingService2005 { get; set; }

        /// <summary>
        /// Ocurre cuando [show report].
        /// </summary>
        /// 11/02/2011 by gpilar
        public event EventHandler<ShowReportsEventArgs> ShowReport;
        //
        private readonly ToolStripMenuItem _menuItem;
        private readonly ToolStripDropDownItem _rItem;

        /// <summary>
        /// Inicializa una nueva instancia de la class <see cref="RsMenu"/> .
        /// </summary>
        /// 11/02/2011 by gpilar
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

        /// <summary>
        /// Inicializa una nueva instancia de la class <see cref="RsMenu"/>.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// 11/02/2011 by gpilar
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
        public void InicializeEngine()
        {
            this.WebServiceUrl = this.WebServiceUrl;
            this.ReportingService2005 = new ReportingService2005
            {
                Url = WebServiceUrl,
                UseDefaultCredentials = UseDefaultCredentials
            };
        }
        /// <summary>
        /// Rs the item click.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">La instancia de <see cref="System.EventArgs"/> que contiene los datos del evento.</param>
        /// 11/02/2011 by gpilar
        void RItemClick(object sender, EventArgs e)
        {
            this._menuItem.DropDownItems.Clear();
            this._menuItem.DropDownItems.Add(this._rItem);
            var temp = new Thread(Reportes);
            temp.Start();
        }

        /// <summary>
        /// Reporteses esta instancia.
        /// </summary>
        /// 11/02/2011 by gpilar
        public void Reportes()
        {
            GetReports(this._menuItem );
        }

        /// <summary>
        /// Se llama cuando [show].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">La instancia de <see cref="RsMenu.ShowReportsEventArgs"/> que contiene los datos del evento.</param>
        /// 11/02/2011 by gpilar
        protected virtual void OnShow(object sender, ShowReportsEventArgs e)
        {
            EventHandler<ShowReportsEventArgs> handler = ShowReport;
            if (handler != null)
            {
                handler(sender, e);
            }
        }

        /// <summary>
        /// Gets the reports.
        /// </summary>
        /// <param name="menuItem">The menu item.</param>
        /// 11/02/2011 by gpilar
        public void GetReports(ToolStripDropDownItem menuItem)
        {
            ReportingService2005 service2005 = ReportingService2005;
            //
            List<CatalogItem> list = service2005.ListChildren(Reportpath, false).ToList();
            list.RemoveAll(i => i.Hidden == !ShowHidenItems || (i.Type != ItemTypeEnum.Report && i.Type != ItemTypeEnum.Folder));

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


        /// <summary>
        /// Gets the items.
        /// </summary>
        /// <param name="service2005">The service2005.</param>
        /// <param name="item">The item.</param>
        /// <param name="menuItem">The menu item.</param>
        /// 11/02/2011 by gpilar
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
