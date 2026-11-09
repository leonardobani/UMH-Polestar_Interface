# Anexo: Registro y Declaración de Uso de Inteligencia Artificial

**Asignatura:** Práctica 1 - Desarrollo de Interfaces de Usuario  
**Universidad:** Universidad Miguel Hernández de Elche (UMH)  
**Herramientas utilizadas:** Asistente IA (ChatGPT / Claude / Gemini)  
**Ámbito de aplicación:** Soporte para resolución de dudas técnicas en WPF, refactorización de arquitectura a MVVM, depuración de eventos y diseño de animaciones.

---

## 1. Declaración de Uso Ético y Metodología
En cumplimiento de las normativas académicas sobre el uso ético de herramientas de inteligencia artificial generativa, se hace constar que la IA ha sido empleada exclusivamente como asistente de consulta técnica y soporte de depuración. 

Todo el código generado fue revisado críticamente, probado y adaptado manualmente para garantizar:
- La correcta integración con los requisitos docentes de la práctica.
- La eliminación de cualquier código innecesario o redundante.
- El pleno entendimiento de la arquitectura implementada por parte del alumno.

---

## 2. Registro Cronológico de Interacciones

### Interacción 1: Selección e integración de librería de animaciones
* **Fecha:** 08/11/2026
* **Objetivo:** Evitar la complejidad y verbosidad de los `Storyboard` nativos de WPF en C#.
* **Prompt del alumno:**
  > *"Estoy desarrollando una interfaz en WPF para una práctica universitaria. Las animaciones nativas con Storyboard y DoubleAnimation en C# me quedan con demasiado código repetitivo. ¿Existe alguna librería ligera y moderna para WPF (como XamlFlair o similar) que permita aplicar efectos de fade y zoom fácilmente tanto en XAML como desde C#?"*
* **Respuesta de la IA:**
  Recomendó el uso del paquete NuGet `XamlFlair.WPF`, mostrando cómo añadir los recursos en `App.xaml` (`<xf:XamlFlairResources />`) y la sintaxis para aplicar animaciones con `Animations.SetPrimary(...)`.
* **Criterio y adaptación del alumno:**
  Se instaló el paquete en el proyecto (`polestar.csproj`), se adaptó la transición del logo en la ventana de login (`AnimarYEntrar`) y se reemplazaron las animaciones manuales anteriores, reduciendo el código en más de 50 líneas.

---

### Interacción 2: Depuración del ciclo de vida de ventanas (Bug de doble apertura)
* **Fecha:** 09/11/2026
* **Objetivo:** Solucionar un fallo por el cual `MainWindow` se instanciaba dos veces tras el login.
* **Prompt del alumno:**
  > *"Al pulsar el botón de login, tengo una animación de zoom en una imagen con ScaleX y ScaleY. Cuando termina la animación abro MainWindow con `new MainWindow().Show(); this.Close();`. Sin embargo, la ventana principal se me abre dos veces. ¿Por qué ocurre esto?"*
* **Respuesta de la IA:**
  Explicó que si se suscribe el evento `Completed` a una misma instancia de animación aplicada tanto a `ScaleX` como a `ScaleY`, el manejador se dispara dos veces (una por cada propiedad animada). Recomendó usar una tarea asíncrona (`await Task.Delay`) o suscribir el evento solo a uno de los timelines.
* **Criterio y adaptación del alumno:**
  Se refactorizó el método `AnimarYEntrar()` en `LoginWindow.xaml.cs` para sincronizar el tiempo de apertura de forma asíncrona mediante `await Task.Delay(600);`, eliminando la duplicación de la ventana y quitando los diálogos modales innecesarios.

---

### Interacción 3: Refactorización a arquitectura MVVM
* **Fecha:** 09/11/2026
* **Objetivo:** Separar los datos de los modelos de vehículos del code-behind de la ventana.
* **Prompt del alumno:**
  > *"En mi MainWindow.xaml.cs tengo una lista fija de coches con nombre, descripción e imagen, y voy cambiando el texto de los TextBlock manualmente por código. ¿Cómo puedo estructurar esto de forma más profesional siguiendo el patrón MVVM usando CommunityToolkit.Mvvm sin complicarlo en exceso?"*
* **Respuesta de la IA:**
  Propuso crear una clase modelo `CarModel` y un `MainViewModel` que herede de `ObservableObject`, utilizando los atributos `[ObservableProperty]` y `[RelayCommand]` para exponer los comandos `NextCar` y `SelectCar`.
* **Criterio y adaptación del alumno:**
  Se crearon las carpetas `Models/` y `ViewModels/`. Se conectó el `DataContext` de la ventana al ViewModel y se sustituyeron las asignaciones manuales en C# por DataBinding en XAML (`{Binding SelectedCar.Name}`, `{Binding SelectedCar.Description}`, etc.), manteniendo el código limpio y desacoplado.

---

### Interacción 4: Maquetación geométrica de la vista de búsqueda
* **Fecha:** 09/11/2026
* **Objetivo:** Ajustar los elementos visuales de la vista de búsqueda (`SearchView`).
* **Prompt del alumno:**
  > *"Quiero diseñar una pantalla de búsqueda minimalista en WPF inspirada en una carretera nocturna con un cartel de tráfico que sirva como barra de búsqueda. ¿Cómo puedo dibujar con Canvas en XAML una carretera horizontal con líneas discontinuas y centrar un Border estilizado encima?"*
* **Respuesta de la IA:**
  Proporcionó un esquema XAML utilizando elementos `Rectangle` y `Line` con la propiedad `StrokeDashArray="20 16"` para las líneas viales discontinuas, junto a un `Border` con esquinas redondeadas.
* **Criterio y adaptación del alumno:**
  Se integraron las líneas en `SearchView.xaml`, ajustando las coordenadas y los colores a la paleta monocromática de Polestar (#111114, blanco y tonos de gris), agregando además el placeholder dinámico al escribir y la respuesta a la tecla Enter y Escape.

---

## 3. Resumen de Aportación Propia y Validación
1. **Lógica docente intacta:** El algoritmo de validación de credenciales (`txtUsuario.Text`, `txtPassword.Password`, usuario `"Alumno"` y contraseña `"1234"`) se mantuvo estrictamente según las directrices y código suministrado por el profesor para la Práctica 1.
2. **Control del ciclo de vida:** Se verificó y resolvió manualmente la sincronización entre ventanas y el manejo del cierre de formularios.
3. **Optimización de recursos:** Se comprobaron todas las rutas relativas de recursos de imagen (`/images/...`) y la compatibilidad con el compilador .NET 8 WPF.
