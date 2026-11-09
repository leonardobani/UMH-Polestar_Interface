using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using polestar.Models;

namespace polestar.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        public ObservableCollection<CarModel> Cars { get; } = new();

        [ObservableProperty]
        private CarModel _selectedCar = null!;

        [ObservableProperty]
        private int _selectedCarIndex = 0;

        [ObservableProperty]
        private string _carIndexDisplay = "01 / 04";

        [ObservableProperty]
        private bool _isSearchOpen = false;

        public event Action<int>? CarTransitionRequested;
        public event Action? SearchOpenRequested;
        public event Action? SearchCloseRequested;
        public event Action<string>? ScrollRequested;

        public MainViewModel()
        {
            Cars.Add(new CarModel
            {
                Name = "Polestar 2",
                Subtitle = "100% eléctrica. Fastback de alto rendimiento con una dinámica de conducción envolvente.",
                Description = "Diseño escandinavo puro, silueta fastback definida y faros LED Pixel. Hasta 476 CV en la versión Dual Motor con Performance Pack, amortiguadores regulables Öhlins y frenos Brembo. Sistema de infoentretenimiento con Google integrado y autonomía de hasta 654 km WLTP.",
                ImageSource = "/images/2side.png",
                NatureImageSource = "/images/2nature.png",
                Tag = "0"
            });

            Cars.Add(new CarModel
            {
                Name = "Polestar 3",
                Subtitle = "El SUV para la era eléctrica. Diseño escandinavo puro, prestaciones deportivas y tecnología avanzada.",
                Description = "Líneas aerodinámicas esculpidas con alerón integrado en el capó y spoiler trasero. Tracción total con motor dual de hasta 517 CV, suspensión neumática activa y sensores de radar avanzados. Autonomía de hasta 610 km WLTP e interiores con materiales sostenibles certificados.",
                ImageSource = "/images/3side.png",
                NatureImageSource = "/images/3nature.png",
                Tag = "1"
            });

            Cars.Add(new CarModel
            {
                Name = "Polestar 4",
                Subtitle = "El SUV coupé eléctrico transformado. Confort y aerodinámica sin luneta trasera.",
                Description = "Innovación radical que redefine la aerodinámica eliminando la luneta trasera a favor de un retrovisor digital HD. De 0 a 100 km/h en 3,8 segundos, techo panorámico de cristal electrocrómico y un habitáculo trasero espacioso con asientos reclinables.",
                ImageSource = "/images/4side.png",
                NatureImageSource = "/images/4nature.png",
                Tag = "2"
            });

            Cars.Add(new CarModel
            {
                Name = "Polestar 5",
                Subtitle = "El GT eléctrico de 4 puertas de muy alto rendimiento. La evolución de Polestar Precept.",
                Description = "Inspirado en el concept car Precept, combina la elegancia de un gran turismo de 4 puertas con prestaciones de superdeportivo. Arquitectura de 800V, motor dual síncrono de 884 CV y 900 Nm de par con chasis de aluminio adherido derivado de la competición.",
                ImageSource = "/images/5side.png",
                NatureImageSource = "/images/5nature.png",
                Tag = "3"
            });

            _selectedCarIndex = 1;
            _selectedCar = Cars[1];
            UpdateIndexDisplay();
        }

        [RelayCommand]
        private void NextCar()
        {
            int nextIndex = (SelectedCarIndex + 1) % Cars.Count;
            ChangeCar(nextIndex);
        }

        [RelayCommand]
        private void SelectCar(int index)
        {
            if (index >= 0 && index < Cars.Count)
            {
                if (SelectedCarIndex != index)
                {
                    ChangeCar(index);
                }
                ScrollRequested?.Invoke("SectionShowcase");
            }
        }

        [RelayCommand]
        private void OpenSearch()
        {
            IsSearchOpen = true;
            SearchOpenRequested?.Invoke();
        }

        [RelayCommand]
        private void CloseSearch()
        {
            IsSearchOpen = false;
            SearchCloseRequested?.Invoke();
        }

        [RelayCommand]
        private void ExecuteSearch(string? query)
        {
            CloseSearch();
            if (string.IsNullOrWhiteSpace(query))
            {
                ScrollRequested?.Invoke("SectionBigText");
                return;
            }

            string q = query.ToLowerInvariant();
            int targetIndex = -1;

            if (q.Contains("2")) targetIndex = 0;
            else if (q.Contains("3")) targetIndex = 1;
            else if (q.Contains("4")) targetIndex = 2;
            else if (q.Contains("5")) targetIndex = 3;

            if (targetIndex >= 0)
            {
                SelectCar(targetIndex);
            }
            else
            {
                ScrollRequested?.Invoke("SectionBigText");
            }
        }

        private void ChangeCar(int newIndex)
        {
            SelectedCarIndex = newIndex;
            SelectedCar = Cars[newIndex];
            UpdateIndexDisplay();
            CarTransitionRequested?.Invoke(newIndex);
        }

        private void UpdateIndexDisplay()
        {
            CarIndexDisplay = $"0{SelectedCarIndex + 1} / 0{Cars.Count}";
        }
    }
}
