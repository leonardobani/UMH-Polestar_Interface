using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using polestar.ViewModels;
using XamlFlair;

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
            Animations.SetPrimary(SearchOverlay, new AnimationSettings
            {
                Kind = AnimationKind.FadeTo,
                Opacity = 1.0,
                Duration = 280
            });
            SearchOverlay.FocusInput();
        }

        private async void CloseSearchOverlay()
        {
            Animations.SetPrimary(SearchOverlay, new AnimationSettings
            {
                Kind = AnimationKind.FadeTo,
                Opacity = 0.0,
                Duration = 200
            });
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
            Animations.SetPrimary(LogoGeometryPath, new AnimationSettings
            {
                Kind = AnimationKind.RotateTo,
                Rotation = targetAngle,
                Duration = 600,
                Easing = EasingType.Cubic,
                EasingMode = EasingMode.EaseInOut
            });
            _currentAngle = targetAngle;

            // Fade out
            var fadeOut = new AnimationSettings
            {
                Kind = AnimationKind.FadeTo,
                Opacity = 0.0,
                Duration = 250
            };
            Animations.SetPrimary(ShowcaseContentPanel, fadeOut);
            Animations.SetPrimary(ShowcaseCarImage, fadeOut);

            await Task.Delay(250);

            // Fade in (los datos se actualizan por binding)
            var fadeIn = new AnimationSettings
            {
                Kind = AnimationKind.FadeTo,
                Opacity = 1.0,
                Duration = 250
            };
            Animations.SetPrimary(ShowcaseContentPanel, fadeIn);
            Animations.SetPrimary(ShowcaseCarImage, fadeIn);

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
                Animations.SetPrimary(filtro, new AnimationSettings { Kind = AnimationKind.FadeTo, Opacity = 0.6, Duration = 300 });
                Animations.SetPrimary(contenuto, new AnimationSettings { Kind = AnimationKind.FadeTo, Opacity = 1.0, Duration = 300 });
            }
        }

        private void HeroCard_MouseLeave(object sender, MouseEventArgs e)
        {
            if (sender is FrameworkElement elem && int.TryParse(elem.Tag?.ToString(), out int idx))
            {
                var filtro = idx switch { 1 => FiltroScuro1, 2 => FiltroScuro2, 3 => FiltroScuro3, _ => FiltroScuro4 };
                var contenuto = idx switch { 1 => ContenutoHover1, 2 => ContenutoHover2, 3 => ContenutoHover3, _ => ContenutoHover4 };
                Animations.SetPrimary(filtro, new AnimationSettings { Kind = AnimationKind.FadeTo, Opacity = 0.0, Duration = 300 });
                Animations.SetPrimary(contenuto, new AnimationSettings { Kind = AnimationKind.FadeTo, Opacity = 0.0, Duration = 300 });
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
                    Animations.SetPrimary(_menuCarImages[i], new AnimationSettings
                    {
                        Kind = AnimationKind.FadeTo,
                        Opacity = targetOpacity,
                        Duration = 350,
                        Easing = EasingType.Quadratic,
                        EasingMode = EasingMode.EaseOut
                    });
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
