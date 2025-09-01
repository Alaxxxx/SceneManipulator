# SceneManipulator for Unity

<p align="center">
  <a href="https://github.com/Alaxxxx/SceneManipulator/stargazers"><img src="https://img.shields.io/github/stars/Alaxxxx/SceneManipulator?style=flat-square&logo=github&color=FFC107" alt="GitHub Stars"></a>
  &nbsp;
  <a href="https://github.com/Alaxxxx?tab=followers"><img src="https://img.shields.io/github/followers/Alaxxxx?style=flat-square&logo=github&label=Followers&color=282c34" alt="GitHub Followers"></a>
  &nbsp;
  <a href="https://github.com/Alaxxxx/SceneManipulator/commits/main"><img src="https://img.shields.io/github/last-commit/Alaxxxx/SceneManipulator?style=flat-square&logo=github&color=blueviolet" alt="Last Commit"></a>
</p>

<p align="center">
  <a href="https://github.com/Alaxxxx/SceneManipulator/releases"><img src="https://img.shields.io/github/v/release/Alaxxxx/SceneManipulator?style=flat-square" alt="Release"></a>
  &nbsp;
  <a href="https://unity.com/"><img src="https://img.shields.io/badge/Unity-2021.3+-2296F3.svg?style=flat-square&logo=unity" alt="Unity Version"></a>
  &nbsp;
  <a href="https://github.com/Alaxxxx/SceneManipulator/blob/main/LICENSE"><img src="https://img.shields.io/github/license/Alaxxxx/SceneManipulator?style=flat-square" alt="License"></a>
</p>

<table>
  <tr>
    <td width="40%" valign="middle" align="center">
      <img  alt="Screenshot" src="https://github.com/user-attachments/assets/fb7a0c07-7b86-4440-a055-85c3b0ef6862" />
    </td>
    <td width="40%" valign="top">

### A scene manipulation toolkit for Unity.

<p><strong>SceneManipulator</strong> - a powerful editor overlay that brings advanced object manipulation tools directly to your scene view.</p>

#### ✨ Key Features
- **Precision Alignment:** Align objects perfectly along any axis with visual preview and instant feedback.
- **Smart Arrangements:** Create lines, grids, circles, or random patterns with customizable spacing.
- **Advanced Snapping:** Snap to ground, bounds, grid, or stack objects with intelligent collision detection.
- **Real-time Measurements:** Display distances, bounds, volume, and surface area with visual gizmos.
- **Live Preview:** See changes before applying them with hover-to-preview functionality.
- **Comprehensive Tools:** Position, rotate, duplicate, group, and organize objects effortlessly.

<br>

<p align="center">
  <a href="#-installation"><strong>🚀 Get Started &raquo;</strong></a>
</p>

  </tr>
</table>

<br>

## ✨ Features

* **🎯 Precision Alignment:** Align objects to minimum, center, or maximum bounds on any axis with visual icons and real-time preview.
* **📐 Smart Arrangements:** Organize objects in lines, grids, circles, or random patterns with customizable spacing controls.
* **🧲 Advanced Snapping:** Comprehensive snapping system including ground detection, surface alignment, grid snapping, and intelligent stacking.
* **📏 Real-time Measurements:** Display distances between objects, total bounds, volume calculations, and surface area with optional visual gizmos.
* **👁️ Live Preview System:** Hover over any button to see a real-time preview of the transformation before applying it.
* **🔄 Smart Rotation Tools:** Set absolute rotations, add relative rotations, reset to zero, or randomize Y-axis with preview support.
* **📊 Detailed Statistics:** Get comprehensive information about selected objects including chain distances, direct distances, and bounding box data.
* **🛠️ Utility Functions:** Quick duplicate, delete, and grouping operations with proper undo support.
* **🎨 Visual Feedback:** Scene view gizmos show distance measurements and connections between selected objects.
* **⚡ Seamless Integration:** Works as a Unity overlay, always accessible in your scene view without cluttering the interface.

<br>

## 🚀 Installation

Choose your preferred installation method below. The recommended method is via Git URL.

<details>
<summary><strong>1. Install via Git URL (Recommended)</strong></summary>
<br>

This method installs the package directly from GitHub and allows you to update it easily.

1.  In Unity, open the **Package Manager** (`Window > Package Manager`).
2.  Click the **+** button and select **"Add package from git URL..."**.
3.  Enter the following URL and click "Add":
    ```
    https://github.com/Alaxxxx/SceneManipulator.git
    ```
</details>

<details>
<summary><strong>2. Install via .unitypackage</strong></summary>
<br>

Ideal if you prefer a specific, stable version of the asset.

1.  Go to the [**Releases**](https://github.com/Alaxxxx/SceneManipulator/releases) page.
2.  Download the `.unitypackage` file from the latest release.
3.  In your Unity project, go to `Assets > Import Package > Custom Package...` and select the downloaded file.
</details>

<details>
<summary><strong>3. Manual Installation (from .zip)</strong></summary>
<br>

1.  Download this repository as a ZIP file by clicking `Code > Download ZIP`.
2.  Unzip the file.
3.  Drag the unzipped package folder into your project's `Assets` directory.
</details>

<br>

## 📋 Usage

Once installed, SceneManipulator automatically adds an overlay to your Unity Scene View:

### Getting Started
1. **Open Scene View:** The SceneManipulator overlay appears automatically
2. **Select Objects:** Select one or more GameObjects in your scene
3. **Use Tools:** Access positioning, alignment, arrangement, and snapping tools
4. **Preview Changes:** Hover over buttons to see live previews before applying

### Tool Categories

**🎯 Positioning**
- Set absolute world positions for all selected objects
- Center selection around world origin
- Snap objects to ground with collision detection
- Move objects directly to origin

**📐 Alignment**  
- Align objects along X, Y, or Z axes
- Choose minimum, center, or maximum alignment targets
- Visual icons for intuitive operation
- Real-time preview on hover

**🔄 Rotation**
- Set absolute rotations or add relative rotations
- Quick reset to zero rotation
- Randomize Y-axis rotation for natural variation
- Preview rotation changes before applying

**📊 Arrangement**
- **Line:** Arrange objects along X, Y, or Z axes
- **Grid:** Create uniform grids on the XZ plane  
- **Circle:** Distribute objects in circular patterns
- **Random:** Scatter objects around the first selection

**🧲 Snapping**
- **Ground Snap:** Intelligent ground detection with surface alignment
- **Bounds Snap:** Position objects relative to reference bounds
- **Grid Snap:** Align to customizable grid points
- **Stack:** Vertical stacking with padding control

**📏 Measurement**
- Distance calculations between objects
- Bounding box dimensions and volume
- Surface area calculations
- Visual gizmo display (toggleable)

<br>

## 🎛️ Advanced Features

### Measurement Gizmos
Enable "Show Distance Gizmos" to display:
- Distance lines between consecutive objects
- Directional arrows showing object relationships  
- Floating distance labels in scene view
- Comprehensive measurement statistics

### Smart Bounds Detection
The system intelligently calculates object bounds using:
- Renderer components for visual objects
- Fallback to transform position for objects without renderers
- Hierarchical bounds calculation for complex objects
- Proper handling of scaled and rotated objects

<br>

## 🤝 Contributing & Supporting

This project is open-source under the **MIT License**, and any form of contribution is welcome and greatly appreciated!

If you find `SceneManipulator` useful in your workflow, the easiest way to show your support is by **giving it a star ⭐️ on GitHub!** It helps a lot with the project's visibility and motivates continued development.

Here are other ways you can get involved:

* **💡 Share Ideas & Report Bugs:** Have a great idea for a new manipulation tool or found a bug? [Open an issue](https://github.com/Alaxxxx/SceneManipulator/issues) to share the details.
* **🔌 Contribute Code:** Feel free to fork the repository and submit a pull request for bug fixes or new features.
* **🗣️ Spread the Word:** Know other Unity developers? Share this tool with them!
* **📹 Create Content:** Made a tutorial or showcase? We'd love to feature it!

Every contribution, from a simple star to a pull request, is incredibly valuable. Thank you for your support!
