using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Configuration.DkimSigner
{
    public partial class CustomTextBox : TextBox
    {
        public CustomTextBox()
        {
            this.InitializeComponent();
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
        }
    }
}
