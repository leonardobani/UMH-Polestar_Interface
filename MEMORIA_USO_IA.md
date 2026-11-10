# Anexo: Registro y Declaración de Uso de Inteligencia Artificial

**Asignatura:** Práctica 1 - Desarrollo de Interfaces de Usuario  
**Universidad:** Universidad Miguel Hernández de Elche (UMH)  
**Herramientas utilizadas:** Asistente IA (Gemini 3.8 Flash High)  
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

### Interacción 1: Evaluación de librerías de animación (XamlFlair) y desarrollo de `AnimationHelper` nativo
* **Fecha:** 01/11/2026 - 09/11/2026
* **Objetivo:** Simplificar la sintaxis de las animaciones en WPF evitando la verbosidad de los `Storyboard` manuales, sin comprometer la estabilidad en .NET 8.
* **Primer prompt:**
  > *"Las animaciones nativas con Storyboard y DoubleAnimation en C# me quedan con código muy largo y repetitivo. ¿Existe alguna librería para WPF (como XamlFlair o similar) que permita aplicar efectos de fade, zoom y rotación de forma más limpia?"*
* **Respuestaal primer prompt:**
  Recomendó la biblioteca `XamlFlair.WPF` a través de NuGet, proponiendo la declaración de recursos en `App.xaml` y el uso de `Animations.SetPrimary(...)`.
* **Problema detectado:**
  Al compilar el proyecto en Visual Studio bajo .NET 8, `XamlFlair.WPF` generó errores de compilación (`error MC3074: The tag 'XamlFlairResources' does not exist` y `error MC3072: Animations.Primary does not exist`) debido a que la librería fue creada para versiones antiguas (.NET Core 3.1) y su analizador XAML colisiona con el compilador moderno de .NET 8.
* **Segundo prompt:**
  > *"La librería XamlFlair me está dando errores MC3074 de compilación en .NET 8 y además prefiero no depender de paquetes externos que puedan dar problemas de restauración en la corrección. ¿Podemos descartar XamlFlair y crear una clase estática de utilidad en C# (tipo AnimationHelper) que encapsule DoubleAnimation con curvas de aceleración (CubicEase, QuadraticEase) de forma limpia?"*
* **Respuesta al segundo prompt:**
  Validó la decisión de prescindir de dependencias externas potencialmente incompatibles y propuso una solución nativa basada en la API de animación de WPF (`System.Windows.Media.Animation`). Diseñó la arquitectura de la clase estática `AnimationHelper` estructurada en tres métodos clave:
  1. `FadeTo(UIElement, toOpacity, durationMs, easing)`: encapsula `DoubleAnimation` sobre `UIElement.OpacityProperty`, aplicando una curva `QuadraticEase` (`EaseOut`) por defecto para transiciones suaves de visibilidad.
  2. `RotateTo(UIElement, toAngle, durationMs)`: comprueba e inicializa automáticamente un `RotateTransform` con pivote centrado (`RenderTransformOrigin = (0.5, 0.5)`) y anima `RotateTransform.AngleProperty` con aceleración `CubicEase` (`EaseInOut`) para rotaciones de iconos e indicadores.
  3. `ScaleAndFade(UIElement, toScale, toOpacity, durationMs)`: configura un `ScaleTransform` y sincroniza concurrentemente tres animaciones (`ScaleX`, `ScaleY` y `OpacityProperty`) con curvas `CubicEase` (`EaseIn`), permitiendo efectos combinados de zoom y desvanecimiento en una única línea de llamada.
  Suministró la implementación completa en C# lista para ser alojada en la carpeta `Utils/`.
* **Criterio y adaptación del alumno:**
  Se descartó y eliminó definitivamente el paquete `XamlFlair.WPF` del archivo `.csproj`, limpiando los espacios de nombres `xmlns:xf` y recursos obsoletos en `App.xaml`, `LoginWindow.xaml` y `MainWindow.xaml`. Se creó el archivo `polestar/polestar/Utils/AnimationHelper.cs` con la arquitectura propuesta y se integró en `LoginWindow.xaml.cs` (fade-in inicial y animación compuesta de salida tras el login) y `MainWindow.xaml.cs`. Esto garantizó una experiencia visual idéntica y fluida, desacoplada de librerías externas de terceros y con 0 advertencias y 0 errores en .NET 8.

---

### Interacción 2: Depuración del ciclo de vida de ventanas (Bug de doble apertura)
* **Fecha:** 03/11/2026
* **Objetivo:** Solucionar un fallo por el cual `MainWindow` se instanciaba dos veces tras el login.
* **Prompt:**
  > *"Al pulsar el botón de login, tengo una animación de zoom en una imagen con ScaleX y ScaleY. Cuando termina la animación abro MainWindow con `new MainWindow().Show(); this.Close();`. Sin embargo, la ventana principal se me abre dos veces. ¿Por qué ocurre esto?"*
* **Respuesta:**
  Explicó que si se suscribe el evento `Completed` a una misma instancia de animación aplicada tanto a `ScaleX` como a `ScaleY`, el manejador se dispara dos veces (una por cada propiedad animada). Recomendó usar una tarea asíncrona (`await Task.Delay`) o suscribir el evento solo a uno de los timelines.
* **Criterio y adaptación:**
  Se refactorizó el método `AnimarYEntrar()` en `LoginWindow.xaml.cs` para sincronizar el tiempo de apertura de forma asíncrona mediante `await Task.Delay(600);`, eliminando la duplicación de la ventana y quitando los diálogos modales innecesarios.

---

### Interacción 3: Refactorización a arquitectura MVVM
* **Fecha:** 05/11/2026
* **Objetivo:** Separar los datos de los modelos de vehículos del code-behind de la ventana.
* **Prompt:**
  > *"En mi MainWindow.xaml.cs tengo una lista fija de coches con nombre, descripción e imagen, y voy cambiando el texto de los TextBlock manualmente por código. ¿Cómo puedo estructurar esto de forma más profesional siguiendo el patrón MVVM usando CommunityToolkit.Mvvm sin complicarlo en exceso?"*
* **Respuesta:**
  Propuso crear una clase modelo `CarModel` y un `MainViewModel` que herede de `ObservableObject`, utilizando los atributos `[ObservableProperty]` y `[RelayCommand]` para exponer los comandos `NextCar` y `SelectCar`.
* **Criterio y adaptación:**
  Se crearon las carpetas `Models/` y `ViewModels/`. Se conectó el `DataContext` de la ventana al ViewModel y se sustituyeron las asignaciones manuales en C# por DataBinding en XAML (`{Binding SelectedCar.Name}`, `{Binding SelectedCar.Description}`, etc.), manteniendo el código limpio y desacoplado.

---

### Interacción 4: Maquetación geométrica de la vista de búsqueda
* **Fecha:** 09/11/2026
* **Objetivo:** Ajustar los elementos visuales de la vista de búsqueda (`SearchView`).
* **Prompt:**
  > *"Quiero diseñar una pantalla de búsqueda minimalista en WPF inspirada en una carretera nocturna con un cartel de tráfico que sirva como barra de búsqueda. ¿Cómo puedo dibujar con Canvas en XAML una carretera horizontal con líneas discontinuas y centrar un Border estilizado encima?"*
* **Respuesta:**
  Proporcionó un esquema XAML utilizando elementos `Rectangle` y `Line` con la propiedad `StrokeDashArray="20 16"` para las líneas viales discontinuas, junto a un `Border` con esquinas redondeadas.
* **Criterio y adaptación:**
  Se integraron las líneas en `SearchView.xaml`, ajustando las coordenadas y los colores a la paleta monocromática de Polestar (#111114, blanco y tonos de gris), agregando además el placeholder dinámico al escribir y la respuesta a la tecla Enter y Escape.

---

## 3. Resumen de Aportación Propia y Validación
1. **Lógica docente intacta:** El algoritmo de validación de credenciales (`txtUsuario.Text`, `txtPassword.Password`, usuario `"Alumno"` y contraseña `"1234"`) se mantuvo estrictamente según las directrices y código suministrado por el profesor para la Práctica 1.
2. **Control del ciclo de vida:** Se verificó y resolvió manualmente la sincronización entre ventanas y el manejo del cierre de formularios.
3. **Criterio técnico frente a dependencias:** Tras detectar problemas de compatibilidad de `XamlFlair.WPF` con el compilador XAML de .NET 8, el alumno tomó la decisión de descartar la librería externa y diseñar la clase modular nativa `AnimationHelper`
4. **Optimización de recursos:** Se comprobaron todas las rutas relativas de recursos de imagen (`/images/...`) y la compatibilidad con el compilador .NET 8 WPF.
