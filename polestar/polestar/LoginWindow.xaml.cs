using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace polestar
{
    /// <summary>
    /// Logica di interazione per LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private bool _isLoggingIn = false;

        public LoginWindow()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (_isLoggingIn) return;

            string User = this.txtUsuario.Text;
            string Password = this.txtPassword.Password;

            if (User == "" || Password == "")
            {
                MessageBox.Show("Inserte el nombre del usuario o el Password");
                this.txtUsuario.Focus();
                return;
            }
            else
            {
                if (txtUsuario.Text != "Alumno" || txtPassword.Password != "1234")
                {
                    MessageBox.Show("El nombre de usuario o el password es incorrecto");
                    this.txtUsuario.Focus();
                    return;
                }
                else
                {
                    MessageBox.Show("Usuario correcto");
                    AvviaTransizioneZoom();
                    return;
                }
            }
        }

        /// <summary>
        /// Esegue l'effetto di zoom sul logo Polestar e apre la finestra principale
        /// </summary>
        private void AvviaTransizioneZoom()
        {
            _isLoggingIn = true;
            btnLogin.IsEnabled = false;

            // 1. Dissolvenza rapida degli input del form
            DoubleAnimation fadeForm = new DoubleAnimation
            {
                From = 1.0,
                To = 0.0,
                Duration = TimeSpan.FromSeconds(0.25)
            };

            // 2. Animazione Zoom (ingrandimento progressivo) del logo Polestar
            DoubleAnimation zoomAnim = new DoubleAnimation
            {
                From = 1.0,
                To = 5.0,
                Duration = TimeSpan.FromSeconds(0.6),
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseIn }
            };

            // 3. Dissolvenza morbida del logo verso la fine dell'ingrandimento
            DoubleAnimation fadeLogo = new DoubleAnimation
            {
                From = 1.0,
                To = 0.0,
                Duration = TimeSpan.FromSeconds(0.3),
                BeginTime = TimeSpan.FromSeconds(0.3)
            };

            // 4. Apertura della pagina principale al termine dello zoom
            zoomAnim.Completed += (s, e) =>
            {
                MainWindow main = new MainWindow();
                main.Show();
                this.Close();
            };

            // Avvio sincronizzato delle animazioni
            FormControlsPanel.BeginAnimation(UIElement.OpacityProperty, fadeForm);
            LogoImage.BeginAnimation(UIElement.OpacityProperty, fadeLogo);
            LogoScale.BeginAnimation(ScaleTransform.ScaleXProperty, zoomAnim);
            LogoScale.BeginAnimation(ScaleTransform.ScaleYProperty, zoomAnim);
        }
    }
}
