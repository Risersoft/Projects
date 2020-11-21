﻿using Acr.UserDialogs;
using risersoft.shared;
using risersoft.shared.Extensions;
using Risersoft.Framework.Global;
using Risersoft.Framework.Pages.Framework;
using Risersoft.Framework.XFView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PrjNirvana.XF.Pages
{

    public partial class frmPIDUnitMan : BasePage
    {
        public frmPIDUnitMan()
        {
            InitializeComponent();
        }
        public override async Task<bool> PrepForm( clsXamView oView, EnumfrmMode prepMode, string prepIDX, string strXML = "")
        {
            this.FormPrepared = false;
            var objModel = await this.InitData("frmPIDUnitManModel", oView, prepMode, prepIDX, strXML);
            if (this.BindModel(objModel, oView))
            {
                this.FormPrepared = true;
            }
            return this.FormPrepared;
        }

        public override bool BindModel(clsFormDataModel NewModel, clsView oView)
        {
            if (base.BindModel(NewModel, oView))
            {
                var bnd = this.Model.Bindable();
                this.BindingContext = bnd;
                return true;
            }
            return false;
        }

        public override async Task<bool> VSave()
        {
            try
            {
                UserDialogs.Instance.ShowLoading("Please Wait");
                bool result = false;
                this.InitError();
                if (this.ValidateData())
                {
                    if (await this.SaveModel()) result = true;
                }
                else this.SetError();
                UserDialogs.Instance.HideLoading();
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await this.VSave();
        }
    }
}
