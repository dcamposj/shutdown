using System.Diagnostics;

namespace shutdownsharp
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void BtnRun_Click(object sender, EventArgs e)
        {
            string input = txtHours.Text.Replace(',', '.');
            if (double.TryParse(input, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out double hours) && hours > 0)
            {
                // Create the modified PowerShell script
                string script =
                $@"$timerHours = {hours.ToString(System.Globalization.CultureInfo.InvariantCulture)}
                $timerSeconds = [math]::Round($timerHours * 3600)
                Write-Host ""Dejar esta ventana abierta la PC se apaga en $timerHours horas...""
                Start-Sleep -Seconds $timerSeconds
                Stop-Computer -Force";

                // Get temp file path
                string tempScriptPath = Path.Combine(Path.GetTempPath(), "ShutdownTimer.ps1");

                // Write the script to file
                File.WriteAllText(tempScriptPath, script);

                // Create process start info
                ProcessStartInfo psi = new()
                {
                    FileName = "powershell.exe",
                    Arguments = $"-NoExit -ExecutionPolicy Bypass -File \"{tempScriptPath}\"",
                    UseShellExecute = true
                };

                // Start the process
                Process.Start(psi);

                // Close the launcher
                this.Close();
            }
            else
            {
                MessageBox.Show("Por favor digite un valor válido mayor a 0.", "Error de entrada", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}