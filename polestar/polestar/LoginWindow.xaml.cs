using System;
using System.Windows;

namespace polestar
{
    /// <summary>
    /// Logica di interazione per LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
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
                    MainWindow main = new MainWindow();
                    main.Show();
                    this.Close();
                    return;
                }
            }
        }
    }
}
