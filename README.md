# Leyenda de El Dorado

**Aplicación interactiva 3D — Universidad Militar Nueva Granada**

Una experiencia narrativa en tercera persona ambientada en la cultura muisca precolombina, donde el jugador explora una aldea, interactúa con sus habitantes y descubre fragmentos de la leyenda de El Dorado a través del diálogo y la exploración.

---

## Tecnologías

| Herramienta | Versión |
|---|---|
| Unity | 6000.3.9f1 |
| Render Pipeline | Universal Render Pipeline (URP) 17.3.0 |
| Input System | 1.18.0 |
| AI Navigation | 2.0.10 |
| Timeline | 1.8.10 |
| Visual Scripting | 1.9.9 |
| Lenguaje | C# |

---

## Estructura del proyecto

```
Assets/
├── Character/                # Modelos 3D, animaciones, texturas y scripts de personajes
│   ├── Player/               # Personaje principal (movimiento, cámara, animación)
│   ├── AGRICULTORA/          # NPC: Agricultora (CHIBCHAZHUM)
│   ├── ARTISTA/              # NPC: Artista (CHISAKUI)
│   ├── CACIQUE/              # NPC: Cacique (HUE)
│   ├── CONQUISTADOR/         # NPC: Conquistador (FRANCISCO)
│   ├── ESCLAVA/              # NPC: Esclava (BAIA)
│   ├── GUERRERO/             # NPC: Guerrero (NYKY)
│   └── SACERDOTE/            # NPC: Sacerdote (CHYKY)
├── Prefabs/                  # Prefabs reutilizables (árboles animados, UI, activadores)
├── Scenary/                  # Escenario 3D (terreno, árboles, letreros, fragmentos)
│   ├── MAPA/                 # Malla principal del terreno y texturas
│   ├── ESPACIOS_NPC/         # Espacios decorados por cada NPC
│   └── Trees/                # Árboles con animación
├── Scenes/                   # Escenas del juego
│   ├── MenuPrincipal.unity   # Menú principal e introducción
│   └── EscenaPrincipal.unity # Mundo del juego
├── Scripts/                  # Lógica del juego
│   ├── Character/            # Movimiento, cámara y animación del jugador
│   ├── Eventos/              # Sistema de audio y zonas musicales
│   └── UI/                   # Menús, pausa, control de audio y botones
├── UI/                       # Interfaces de usuario (diálogos, inventario, menú)
├── SoundEffects/             # Pistas musicales (BGM)
├── Sounds/                   # Efectos de sonido (ambiente, SFX)
├── Items/                    # Arte gráfico para UI, fondos e introducción
└── skyIlumination/           # [Asset externo] Skybox Cubemap Extended (Boxophobic)
```

---

## Mecánicas del juego

### Movimiento del jugador
- Control en tercera persona con WASD y física basada en Rigidbody
- Caminar, correr (Shift) y saltar (Espacio)
- Cámara orbitable con el ratón, inclinación vertical limitada (-20° a 60°)
- Transiciones suaves de rotación con velocidades diferenciadas por estado

### Interacción con NPC
- El jugador detecta NPCs mediante SphereCast desde la cámara
- Al presionar el botón de interacción, se muestra un diálogo con efecto de máquina de escribir
- Cada NPC tiene múltiples líneas de diálogo que avanzan secuencialmente
- Al hablar con un NPC por primera vez, se registra en el inventario/diario del jugador

### Recolección de notas
- 6 notas (fragmentos del mapa de El Dorado) dispersas por la aldea
- Sistema de detección por SphereCast
- Contador visual en pantalla (X/6)
- Al recolectar las 6, se activa la escena final

### Sistema de audio
- Gestor de música singleton con transiciones suaves (crossfade)
- Zonas musicales por proximidad (triggers) con modos overlay y directo
- Mezclador de audio de 3 canales: General, Música y SFX
- Sonidos ambientales por ubicación (agua, fuego, naturaleza, rituales)

### Menús e interfaz
- **Menú principal**: Fondos animados con cambio de color secuencial
- **Introducción**: Secuencia de 5 imágenes con fundido, avance con tecla
- **Menú de pausa**: Reanudar, ajustes de audio (3 sliders), volver al menú
- **Inventario/Diario**: Visualización de personajes descubiertos con retratos

---

## Personajes

| Nombre | Rol | Descripción |
|---|---|---|
| CHIBCHAZHUM | Agricultora | Habitante de la aldea dedicada al cultivo |
| CHISAKUI | Artista | Creador y expresor de la cultura visual muisca |
| HUE | Cacique | Autoridad y líder de la comunidad |
| BAIA | Esclava | Personaje en situación de servidumbre |
| NYKY | Guerrero | Defensor y protector de la aldea |
| CHYKY | Sacerdote | Guía espiritual y conocedor de los rituales |
| FRANCISCO | Conquistador | El español que busca El Dorado |

---

## Assets externos

- **Skybox Cubemap Extended** (Boxophobic) — Shader profesional de cielo con modos día/noche/mezcla

---

## Cómo ejecutar

1. Abrir el proyecto con **Unity 6000.3.9f1**
2. En la ventana *Scenes*, abrir `Assets/Scenes/MenuPrincipal.unity`
3. Presionar **Play**

También hay una compilación pregenerada en `Build/Windows/LeyendaElDorado.exe`.

---

## Créditos

- **Lider** Leonardo Herrera
- **Programador** Nicolas Pinilla Forero
- **Modelador** Kevin Alexander Castellanos
- **Diseñador UI** Andres Alcazar
- **Tester** Juan Benavides
