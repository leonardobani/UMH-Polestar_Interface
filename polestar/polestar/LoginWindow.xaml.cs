using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using XamlFlair;

namespace polestar
{
    public partial class LoginWindow : Window
    {
        private bool _isLoggingIn = false;

        public LoginWindow()
        {
            InitializeComponent();
        }

        private void NavInicio_Click(object sender, MouseButtonEventArgs e)
        {
            MainWindow main = new MainWindow("SectionHero");
            main.Show();
            this.Close();
        }

        private void NavGaleria_Click(object sender, MouseButtonEventArgs e)
        {
            MainWindow main = new MainWindow("SectionShowcase");
            main.Show();
            this.Close();
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
                    AnimarYEntrar();
                }
            }
        }

        private async void AnimarYEntrar()
        {
            _isLoggingIn = true;
            btnLogin.IsEnabled = false;

            // Animacion de salida y zoom del logo
            Animations.SetPrimary(FormControlsPanel, new AnimationSettings
            {
                Kind = AnimationKind.FadeTo,
                Opacity = 0.0,
                Duration = 250
            });

            Animations.SetPrimary(LogoImage, new AnimationSettings
            {
                Kind = AnimationKind.FadeTo | AnimationKind.ScaleXTo | AnimationKind.ScaleYTo,
                ScaleX = 5.0,
                ScaleY = 5.0,
                Opacity = 0.0,
                Duration = 600,
                Easing = EasingType.Cubic,
                EasingMode = EasingMode.EaseIn
            });

            await Task.Delay(600);

            // Abrir ventana principal y cerrar login
            MainWindow ventanaPrincipal = new MainWindow();
            ventanaPrincipal.Show();
            this.Close();
        }
    }
}
