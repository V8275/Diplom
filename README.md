# OpenTK 3D Engine

3D model visualization in C# (UI - Windows Forms) using OpenTK, featuring Bullet Physics integration, PBR rendering, dynamic shadows, and a modular object system.

[![.NET](https://img.shields.io/badge/.NET-9.0-512BD4)](https://dotnet.microsoft.com/)
[![OpenTK](https://img.shields.io/badge/OpenTK-4.8.2-blue)](https://opentk.net/)

## Features

### Graphics and Rendering
- **PBR (Physically Based Rendering)** shaders with support for:
  - Albedo / Diffuse maps
  - Normal maps
  - Metallic maps
  - Roughness maps
- **Dynamic shadows** using Shadow Mapping
- Support for model formats: **OBJ** and **glTF 2.0**

### Physics
- Integration with **Bullet Physics**
- Support for static, dynamic, and kinematic bodies
- Automatic collision generation based on model geometry
- Various movement modes (`MovementMode`):
  - `Direct` - direct position modification
  - `Force` - via force application (F = m * a)
  - `Impulse` - via impulse application
  - `Velocity` - via direct velocity control
- Movement damping and speed limiting
- Jump support with ground detection

### Camera System
- **Free mode** (`CameraMode.Free`) - unrestricted movement in space
- **Orbit mode** (`CameraMode.Orbit`) - rotation around a target object
- Smooth control with adjustable mouse sensitivity
- Zoom with configurable min/max distance limits in orbit mode

### Modular System
Scene objects can be extended with modules by inheriting from `ObjectModule`. Currently available:

| Module | Purpose |
|--------|---------|
| `PhysicsModule` | Physical behavior, mass, velocity, forces |
| `CollisionModule` | Static collision handling |
| `MoveModule` | Movement control with physics support and smooth rotation |
| `LightModule` | Light sources |

### Lighting
- Support for up to **10 simultaneous light sources**
- Configurable parameters: `Ambient`, `Diffuse`, `Specular`
- Automatic `lightSpaceMatrix` calculation for shadows
- Light manager (`LightManager`) for centralized control

## Dependencies

| Package | Version | Purpose |
|---------|---------|---------|
| OpenTK | 4.9.4 | Core graphics framework |
| BulletSharp | 0.11.1 | Physics engine |
| StbImageSharp | 2.30.15 | Texture loading |
| SharpGLTF.Core | 1.0.6 | glTF model loading |
| JeremyAnsel.Media.WavefrontObj | 3.0.58 | OBJ model loading |
| Microsoft.Data.Sqlite.Core | 10.0.3 | Preset database operations |

## Implementation Details

### PBR Shader
- Distribution function: **GGX / Trowbridge-Reitz**
- Geometry function: **Schlick-GGX + Smith**
- Fresnel: **Schlick approximation**
- Tone mapping: **Reinhard + Gamma correction**

### Shadow Mapping
- PCF filtering with 3x3 kernel
- Dynamic bias based on the angle between normal and light direction
- Configurable shadow map resolution (default: 2048x2048)
