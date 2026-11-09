using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using polestar.Utils;
using polestar.ViewModels;

namespace polestar
{
    public partial class MainWindow : Window
    {
        public MainViewModel ViewModel { get; }

        private const double RotationStep = 180.0;
        private double _currentAngle = 0.0;
        private bool _isAnimating = false;

        private Image[] _menuCarImages = null!;

        public MainWindow(string initialSection = "")
        {
            InitializeComponent();

            ViewModel = new MainViewModel();
            DataContext = ViewModel;

            _menuCarImages = new[] { MenuCar0, MenuCar1, MenuCar2, MenuCar3 };

            // Eventos del ViewModel para animaciones y scroll
            ViewModel.CarTransitionRequested += OnCarTransitionRequested;
            ViewModel.SearchOpenRequested += OpenSearchOverlay;
            ViewModel.SearchCloseRequested += CloseSearchOverlay;
            ViewModel.ScrollRequested += OnScrollRequested;

            if (!string.IsNullOrEmpty(initialSection))
            {
                Loaded += (s, e) => OnScrollRequested(initialSection);
            }
        }

        // Navegacion del header
        private void BtnDrawer_Click(object sender, RoutedEventArgs e) => SectionBigText?.BringIntoView();
        private void LogoHeader_Click(object sender, MouseButtonEventArgs e) => SectionHero?.BringIntoView();
        private void HeaderLink1_Click(object sender, MouseButtonEventArgs e) => SectionHero?.BringIntoView();
        private void HeaderLink2_Click(object sender, MouseButtonEventArgs e) => SectionShowcase?.BringIntoView();
        private void HeaderLink3_Click(object sender, MouseButtonEventArgs e) => SectionBigText?.BringIntoView();

        private void HeaderLogin_Click(object sender, MouseButtonEventArgs e)
        {
            LoginWindow login = new LoginWindow();
            login.Show();
            this.Close();
        }

        private void HeaderSearch_Click(object sender, MouseButtonEventArgs e)
        {
            ViewModel.OpenSearchCommand.Execute(null);
        }

        // Overlay de busqueda
        private void OpenSearchOverlay()
        {
            SearchOverlay.Visibility = Visibility.Visible;
            AnimationHelper.FadeTo(SearchOverlay, 1.0, 280);
            SearchOverlay.FocusInput();
        }

        private async void CloseSearchOverlay()
        {
            AnimationHelper.FadeTo(SearchOverlay, 0.0, 200);
            await Task.Delay(200);
            SearchOverlay.Visibility = Visibility.Collapsed;
        }

        private void SearchView_CloseRequested(object? sender, EventArgs e)
        {
            ViewModel.CloseSearchCommand.Execute(null);
        }

        private void SearchView_SearchSubmitted(object? sender, string query)
        {
            ViewModel.ExecuteSearchCommand.Execute(query);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape && SearchOverlay.Visibility == Visibility.Visible)
            {
                ViewModel.CloseSearchCommand.Execute(null);
                e.Handled = true;
            }
        }

        // Showcase: transicion al cambiar de coche
        private void BtnNextCar_Click(object sender, RoutedEventArgs e)
        {
            if (_isAnimating) return;
            ViewModel.NextCarCommand.Execute(null);
        }

        private async void OnCarTransitionRequested(int targetIndex)
        {
            _isAnimating = true;

            // Rotar logo
            double targetAngle = _currentAngle + RotationStep;
            AnimationHelper.RotateTo(LogoGeometryPath, targetAngle, 600);
            _currentAngle = targetAngle;

            // Fade out
            AnimationHelper.FadeTo(ShowcaseContentPanel, 0.0, 250);
            AnimationHelper.FadeTo(ShowcaseCarImage, 0.0, 250);

            await Task.Delay(250);

            // Fade in (los datos se actualizan por binding)
            AnimationHelper.FadeTo(ShowcaseContentPanel, 1.0, 250);
            AnimationHelper.FadeTo(ShowcaseCarImage, 1.0, 250);

            await Task.Delay(250);
            _isAnimating = false;
        }

        // Efectos hover en los modelos
        private void HeroCard_MouseEnter(object sender, MouseEventArgs e)
        {
            if (sender is FrameworkElement elem && int.TryParse(elem.Tag?.ToString(), out int idx))
            {
                var filtro = idx switch { 1 => FiltroScuro1, 2 => FiltroScuro2, 3 => FiltroScuro3, _ => FiltroScuro4 };
                var contenuto = idx switch { 1 => ContenutoHover1, 2 => ContenutoHover2, 3 => ContenutoHover3, _ => ContenutoHover4 };
                AnimationHelper.FadeTo(filtro, 0.6, 300);
                AnimationHelper.FadeTo(contenuto, 1.0, 300);
            }
        }

        private void HeroCard_MouseLeave(object sender, MouseEventArgs e)
        {
            if (sender is FrameworkElement elem && int.TryParse(elem.Tag?.ToString(), out int idx))
            {
                var filtro = idx switch { 1 => FiltroScuro1, 2 => FiltroScuro2, 3 => FiltroScuro3, _ => FiltroScuro4 };
                var contenuto = idx switch { 1 => ContenutoHover1, 2 => ContenutoHover2, 3 => ContenutoHover3, _ => ContenutoHover4 };
                AnimationHelper.FadeTo(filtro, 0.0, 300);
                AnimationHelper.FadeTo(contenuto, 0.0, 300);
            }
        }

        private void ScrollToPolestar2_Click(object sender, RoutedEventArgs e) => ViewModel.SelectCarCommand.Execute(0);
        private void ScrollToPolestar3_Click(object sender, RoutedEventArgs e) => ViewModel.SelectCarCommand.Execute(1);
        private void ScrollToPolestar4_Click(object sender, RoutedEventArgs e) => ViewModel.SelectCarCommand.Execute(2);
        private void ScrollToPolestar5_Click(object sender, RoutedEventArgs e) => ViewModel.SelectCarCommand.Execute(3);

        private void OnScrollRequested(string sectionName)
        {
            if (sectionName == "SectionShowcase")
            {
                SectionShowcase?.BringIntoView();
            }
            else if (sectionName == "SectionBigText")
            {
                SectionBigText?.BringIntoView();
            }
            else if (sectionName == "SectionHero")
            {
                SectionHero?.BringIntoView();
            }
        }

        // Seccion Big Text
        private void MenuCar_MouseEnter(object sender, MouseEventArgs e)
        {
            if (sender is TextBlock tb)
            {
                tb.Foreground = Brushes.Black;
                tb.FontWeight = FontWeights.SemiBold;
            }

            if (sender is FrameworkElement elem && int.TryParse(elem.Tag?.ToString(), out int targetIndex))
            {
                if (_menuCarImages == null) return;

                for (int i = 0; i < _menuCarImages.Length; i++)
                {
                    double targetOpacity = (i == targetIndex) ? 1.0 : 0.0;
                    AnimationHelper.FadeTo(_menuCarImages[i], targetOpacity, 350);
                }
            }
        }

        private void MenuCar_MouseLeave(object sender, MouseEventArgs e)
        {
            if (sender is TextBlock tb)
            {
                tb.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#111111"));
                tb.FontWeight = FontWeights.Normal;
            }
        }

        private void MenuCar_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is FrameworkElement elem && int.TryParse(elem.Tag?.ToString(), out int targetIndex))
            {
                ViewModel.SelectCarCommand.Execute(targetIndex);
            }
        }
    }
}
