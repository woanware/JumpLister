using System.Windows.Forms;

namespace woanware
{
    /// <summary>
    /// 
    /// </summary>
    public partial class FormOptions : Form
    {
        #region Member Variables
        public Settings Settings {get; private set;}
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="settings"></param>
        public FormOptions(Settings settings)
        {
            InitializeComponent();

            Settings = settings;

            chkUseDecimal.Checked = Settings.UseDecimal;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOk_Click(object sender, System.EventArgs e)
        {
            Settings.UseDecimal = chkUseDecimal.Checked;

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, System.EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        } 
    }
}
