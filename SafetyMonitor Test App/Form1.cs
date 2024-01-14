using System;
using System.Windows.Forms;

namespace ASCOM.TSOSM
{
    public partial class Form1 : Form
    {

        private ASCOM.DriverAccess.SafetyMonitor driver;

        public Form1()
        {
            InitializeComponent();
            SetUIState();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (IsConnected)
                driver.Connected = false;

            Properties.Settings.Default.Save();
        }

        private void buttonChoose_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.DriverId = ASCOM.DriverAccess.SafetyMonitor.Choose(Properties.Settings.Default.DriverId);
            SetUIState();
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            if (IsConnected)
            {
                driver.Connected = false;
            }
            else
            {
                driver = new ASCOM.DriverAccess.SafetyMonitor(Properties.Settings.Default.DriverId);
                driver.Connected = true;
            }
            SetUIState();
        }

        private void SetUIState()
        {
            buttonConnect.Enabled = !string.IsNullOrEmpty(Properties.Settings.Default.DriverId);
            buttonChoose.Enabled = !IsConnected;
            buttonConnect.Text = IsConnected ? "Disconnect" : "Connect";
        }

        private bool IsConnected
        {
            get
            {
                return ((this.driver != null) && (driver.Connected == true));
            }
        }

        private void btnSafe_Click(object sender, EventArgs e)
        {
            textBox1.Text = driver.IsSafe.ToString();
        }
    }
}
