﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetNuke.Services.Localization;

namespace Bitboxx.DNNModules.BBStore.Providers.Payment
{
    public partial class PaymentProviderPaypal : PaymentProviderBase
    {
        public override string Title
        {
            get { return lblTitle.Text; }
            set { lblTitle.Text = value; }
        }

        public override bool IsValid
        {
            get { return true;}
        }

        public override string Properties
        {
            get { return txtPPUser.Text + "," + txtPPPassword.Text + "," + txtPPSignature.Text + "," + chkSandbox.Checked.ToString(); }
            set
            {
                string[] props = value.Split(',');
                if (props.Length > 2)
                {
                    txtPPUser.Text = props[0];
                    txtPPPassword.Text = props[1];
                    txtPPSignature.Text = props[2];
                }

                if (props.Length > 3)
                    chkSandbox.Checked = Convert.ToBoolean(props[3]);
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            switch (DisplayMode)
            {
                case ViewMode.View:
                    pnlShow.SetActiveView(View);
                    break;
                case ViewMode.Edit:
                    pnlShow.SetActiveView(Edit);
                    break;
                case ViewMode.Summary:
                    pnlShow.SetActiveView(Summary);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            string description = "";
            decimal fixedAmount = 0.0m;
            if (this._cost > 0)
            {
                description = LocalizeString("lblCost.Text");
                fixedAmount = this._cost * (ShowNetprice ? 1 : (100 + TaxPercent) / 100);
            }
            else if (this._cost < 0)
            {
                description = LocalizeString("lblDiscount.Text");
                fixedAmount = (-1) * this._cost * (ShowNetprice ? 1 : (100 + TaxPercent) / 100);
            }
            else
            {
                description = LocalizeString("lblFree.Text");
            }

            string costText = "";
            if (this._cost != 0 && this._costPercent != 0)
                costText = String.Format(LocalizeString("lblFixed.Text"), fixedAmount) + " " + LocalizeString("lblConcat.Text") + " " + String.Format(LocalizeString("lblPercentage.Text"), this._costPercent);
            else if (this._cost != 0)
                costText = String.Format(LocalizeString("lblFixed.Text"), fixedAmount);
            else if (this._costPercent != 0)
                costText = String.Format(LocalizeString("lblPercentage.Text"), this._costPercent);

            lblDescription.Text = String.Format(LocalizeString("lblDescription.Text"), String.Format(description, costText));
        }
    }
}