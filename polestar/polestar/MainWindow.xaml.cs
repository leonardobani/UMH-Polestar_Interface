using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace polestar
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private class CarInfo
        {
            public string Name { get; set; } = "";
            public string Subtitle { get; set; } = "";
            public string Description { get; set; } = "";
            public string ImageSource { get; set; } = "";
        }

        private readonly CarInfo[] _cars = new[]
        {
            new CarInfo
            {
                Name = "Polestar 2",
                Subtitle = "100% elettrica. Fastback ad alte prestazioni con dinamica di guida coinvolgente.",
                Description = "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Design scandinavo puro, materiali innovativi riciclati e accelerazione istantanea.",
                ImageSource = "/images/2side.png"
            },
            new CarInfo
            {
                Name = "Polestar 3",
                Subtitle = "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.",
                Description = "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.",
                ImageSource = "/images/3side.png"
            },
            new CarInfo
            {
                Name = "Polestar 4",
                Subtitle = "Il SUV coupé elettrico trasformato. Comfort e aerodinamica senza lunotto posteriore.",
                Description = "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Lo spazio interno di una berlina di lusso si unisce alla linea affilata di un coupé sportivo.",
                ImageSource = "/images/4side.png"
            },
            new CarInfo
            {
                Name = "Polestar 5",
                Subtitle = "La GT elettrica a 4 porte ad altissime prestazioni. L'evoluzione di Polestar Precept.",
                Description = "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Architettura a 800V, fino a 884 CV e telaio in alluminio incollato sviluppato per il motorsport.",
                ImageSource = "/images/5side.png"
            }
        };

        // Angolo di rotazione per ciascun cambio auto (180 o 90 gradi)
        private const double RotationStep = 180.0;

        private int _currentCarIndex = 1; // Inizia con Polestar 3 come nel mockup
        private double _currentAngle = 0.0;
        private bool _isAnimating = false;

        private Image[] _menuCarImages = null!;

        public MainWindow()
        {
            InitializeComponent();
            _menuCarImages = new[] { MenuCar0, MenuCar1, MenuCar2, MenuCar3 };
        }

        // ==========================================
        // HEADER STICKY INTERACTIONS
        // ==========================================

        private void BtnDrawer_Click(object sender, RoutedEventArgs e)
        {
            SectionBigText?.BringIntoView();
        }

        private void LogoHeader_Click(object sender, MouseButtonEventArgs e)
        {
            SectionHero?.BringIntoView();
        }

        private void HeaderLink1_Click(object sender, MouseButtonEventArgs e)
        {
            SectionHero?.BringIntoView();
        }

        private void HeaderLink2_Click(object sender, MouseButtonEventArgs e)
        {
            SectionShowcase?.BringIntoView();
        }

        private void HeaderLink3_Click(object sender, MouseButtonEventArgs e)
        {
            SectionBigText?.BringIntoView();
        }

        private void HeaderSearch_Click(object sender, MouseButtonEventArgs e)
        {
            OpenSearchOverlay();
        }

        // ==========================================
        // SEARCH VIEW (GESTIONE OVERLAY)
        // ==========================================

        private void OpenSearchOverlay()
        {
            SearchOverlay.Visibility = Visibility.Visible;
            DoubleAnimation fadeIn = new DoubleAnimation(0.0, 1.0, TimeSpan.FromSeconds(0.28));
            fadeIn.Completed += (s, ev) =>
            {
                SearchOverlay.FocusInput();
            };
            SearchOverlay.BeginAnimation(UIElement.OpacityProperty, fadeIn);
        }

        private void CloseSearchOverlay()
        {
            DoubleAnimation fadeOut = new DoubleAnimation(1.0, 0.0, TimeSpan.FromSeconds(0.2));
            fadeOut.Completed += (s, ev) =>
            {
                SearchOverlay.Visibility = Visibility.Collapsed;
            };
            SearchOverlay.BeginAnimation(UIElement.OpacityProperty, fadeOut);
        }

        private void SearchView_CloseRequested(object? sender, EventArgs e)
        {
            CloseSearchOverlay();
        }

        private void SearchView_SearchSubmitted(object? sender, string query)
        {
            ExecuteSearch(query);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape && SearchOverlay.Visibility == Visibility.Visible)
            {
                CloseSearchOverlay();
                e.Handled = true;
            }
        }

        private void ExecuteSearch(string query)
        {
            string q = query.ToLowerInvariant();

            int targetIndex = -1;
            if (q.Contains("2")) targetIndex = 0;
            else if (q.Contains("3")) targetIndex = 1;
            else if (q.Contains("4")) targetIndex = 2;
            else if (q.Contains("5")) targetIndex = 3;

            CloseSearchOverlay();

            if (targetIndex >= 0)
            {
                SwitchToCarAndScroll(targetIndex);
            }
            else
            {
                SectionBigText?.BringIntoView();
            }
        }

        // ==========================================
        // SEZIONE SHOWCASE: ROTAZIONE E DISSOLVENZA
        // ==========================================

        private void BtnNextCar_Click(object sender, RoutedEventArgs e)
        {
            if (_isAnimating) return;
            int nextIndex = (_currentCarIndex + 1) % _cars.Length;
            TransitionToCar(nextIndex);
        }

        private void TransitionToCar(int targetIndex)
        {
            _isAnimating = true;

            // 1. Animazione di rotazione della geometria del logo
            double targetAngle = _currentAngle + RotationStep;
            DoubleAnimation rotateAnim = new DoubleAnimation
            {
                From = _currentAngle,
                To = targetAngle,
                Duration = TimeSpan.FromSeconds(0.6),
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseInOut }
            };
            LogoRotateTransform.BeginAnimation(RotateTransform.AngleProperty, rotateAnim);
            _currentAngle = targetAngle;

            // 2. Dissolvenza in uscita (Fade Out) per auto e testo
            DoubleAnimation fadeOut = new DoubleAnimation
            {
                From = 1.0,
                To = 0.0,
                Duration = TimeSpan.FromSeconds(0.25)
            };

            fadeOut.Completed += (s, ev) =>
            {
                _currentCarIndex = targetIndex;
                var car = _cars[_currentCarIndex];

                ShowcaseHeading.Text = car.Name;
                ShowcaseSubtitle.Text = car.Subtitle;
                ShowcaseDescription.Text = car.Description;
                ShowcaseCarImage.Source = new BitmapImage(new Uri(car.ImageSource, UriKind.Relative));
                CarIndexText.Text = $"0{_currentCarIndex + 1} / 0{_cars.Length}";

                // 3. Dissolvenza in entrata (Fade In)
                DoubleAnimation fadeIn = new DoubleAnimation
                {
                    From = 0.0,
                    To = 1.0,
                    Duration = TimeSpan.FromSeconds(0.25)
                };
                fadeIn.Completed += (s2, ev2) => { _isAnimating = false; };

                ShowcaseContentPanel.BeginAnimation(UIElement.OpacityProperty, fadeIn);
                ShowcaseCarImage.BeginAnimation(UIElement.OpacityProperty, fadeIn);
            };

            ShowcaseContentPanel.BeginAnimation(UIElement.OpacityProperty, fadeOut);
            ShowcaseCarImage.BeginAnimation(UIElement.OpacityProperty, fadeOut);
        }

        // ==========================================
        // HERO GRID: SCROLL ALLA SEZIONE SHOWCASE
        // ==========================================

        private void ScrollToPolestar2_Click(object sender, RoutedEventArgs e) => SwitchToCarAndScroll(0);
        private void ScrollToPolestar3_Click(object sender, RoutedEventArgs e) => SwitchToCarAndScroll(1);
        private void ScrollToPolestar4_Click(object sender, RoutedEventArgs e) => SwitchToCarAndScroll(2);
        private void ScrollToPolestar5_Click(object sender, RoutedEventArgs e) => SwitchToCarAndScroll(3);

        private void SwitchToCarAndScroll(int index)
        {
            if (_currentCarIndex != index)
            {
                TransitionToCar(index);
            }
            SectionShowcase?.BringIntoView();
        }

        // ==========================================
        // SEZIONE BIG TEXT: HOVER PER FADE AUTO
        // ==========================================

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
                    DoubleAnimation anim = new DoubleAnimation
                    {
                        To = targetOpacity,
                        Duration = TimeSpan.FromSeconds(0.35),
                        EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
                    };
                    _menuCarImages[i].BeginAnimation(UIElement.OpacityProperty, anim);
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
                SwitchToCarAndScroll(targetIndex);
            }
        }
    }
}
