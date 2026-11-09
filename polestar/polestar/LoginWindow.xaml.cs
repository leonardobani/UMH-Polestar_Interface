using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using polestar.Utils;

namespace polestar
{
    public partial class LoginWindow : Window
    {
        private bool _isLoggingIn = false;

        public LoginWindow()
        {
            InitializeComponent();

            Loaded += (s, e) =>
            {
                AnimationHelper.FadeTo(FormLoginPanel, 1.0, 550);
            };
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
            AnimationHelper.FadeTo(FormControlsPanel, 0.0, 250);
            AnimationHelper.ScaleAndFade(LogoImage, 5.0, 0.0, 600);

            await Task.Delay(600);

            // Abrir ventana principal y cerrar login
            MainWindow ventanaPrincipal = new MainWindow();
            ventanaPrincipal.Show();
            this.Close();
        }
    }
}
