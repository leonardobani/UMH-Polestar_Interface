# UMH - Polestar Interface (Práctica 1)

Prototipo de aplicación de escritorio interactiva en **WPF (.NET 8)** para **Polestar Automotive**, desarrollado en el marco de la asignatura **Interfaces de Usuario** del *Grado en Ingeniería Informática en Tecnologías de la Información* de la **Universidad Miguel Hernández de Elche (UMH)**.

---

## 📌 Documentación Oficial de la Entrega

* 📄 **[Memoria Técnica Completa (MEMORIA_TECNICA.md)](MEMORIA_TECNICA.md):** Justificación de diseño, heurísticas de usabilidad de Nielsen, accesibilidad, arquitectura MVVM, plan de pruebas y licencias.
* 🤖 **[Registro de Uso de Inteligencia Artificial (MEMORIA_USO_IA.md)](MEMORIA_USO_IA.md):** Declaración ética, registro cronológico de prompts, respuestas de la IA y análisis crítico de aportación del alumno según las directrices de la UMH.

---

## 🚗 Descripción del Proyecto

La aplicación reproduce la experiencia digital y el lenguaje de diseño escandinavo de la marca de vehículos eléctricos Polestar, adaptada a terminales comerciales y tiendas (*Polestar Spaces*):

1. **Pantalla de Inicio (Novedades y Ofertas):** Mosaico cuadricular con la gama de vehículos (Polestar 2, 3, 4 y 5), distintivos de ofertas estacionales y campañas publicitarias (*0% TAE*, *Entrega inmediata*).
2. **Pantalla de Identificación (Login):** Formulario de acceso con control de foco, ocultación de contraseña, gestión de errores en credenciales y animación de transición.
   * *Credenciales de evaluación:* **Usuario:** `Alumno` | **Contraseña:** `1234`
3. **Galería Comercial Interactiva:**
   * Carrusel interactivo con especificaciones técnicas detalladas, autonomía WLTP, aceleración y rotación coordinada del emblema.
   * Selector tipográfico a gran escala con previsualización dinámica en hover.
4. **Búsqueda Rápida Integrada:** UserControl modal accesible mediante atajo o botón con filtrado en tiempo real.
5. **Navegación Unificada:** Barra superior y logotipo corporativo presentes de forma homogénea en todas las pantallas.

---

## 🛠️ Tecnologías y Arquitectura

* **Plataforma:** .NET 8.0 WPF (Windows Presentation Foundation)
* **Patrón de Arquitectura:** MVVM (Model-View-ViewModel)
* **Bibliotecas NuGet:**
  * `CommunityToolkit.Mvvm` (v8.4.2) - Gestión de estado, propiedades observables (`ObservableProperty`) y comandos (`RelayCommand`).
  * `XamlFlair.WPF` (v1.2.13) - Animaciones declarativas de alto rendimiento con curvas de aceleración naturales.

---

## 🚀 Instrucciones de Ejecución

1. Clonar el repositorio:
   ```bash
   git clone https://gitlab.com/leorise25/umh-polestar_interface.git
   ```
2. Abrir la solución `polestar/polestar.sln` en **Microsoft Visual Studio 2022** o **Microsoft Blend**.
3. Restaurar paquetes NuGet y compilar la solución (`Ctrl + Shift + B`).
4. Ejecutar el proyecto (`F5`).

---

## 👨‍💻 Datos del Alumno

* **Alumno:** `[Nombre y Apellidos del Alumno]`
* **Titulación:** Grado en Ingeniería Informática en Tecnologías de la Información
* **Asignatura:** Interfaces de Usuario (Curso 2026/2027)
* **Universidad:** Universidad Miguel Hernández de Elche (UMH)
