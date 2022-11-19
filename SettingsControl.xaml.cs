using System.Windows.Controls;

namespace DahlDesign.Plugin
{
    /// <summary>
    /// Logique d'interaction pour SettingsControlDemo.xaml
    /// </summary>
    public partial class SettingsControl : UserControl
    {
        public DahlDesign Plugin { get; }

        public SettingsControl()
        {
            InitializeComponent();
        }

        public SettingsControl(DahlDesign plugin) : this()
        {
            this.Plugin = plugin;
        }

        private void ControlsEditor_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {

        }
    }
}
