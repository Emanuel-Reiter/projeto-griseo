TMPro Drop Shadows
by EagleStudio
v1.0.0

===================

Overview
--------
A lightweight plug-and-play Unity asset that automatically adds a drop shadow to TextMeshProUGUI text elements. Add the TMPDropShadow component to any UI object with a TextMeshProUGUI component and it creates and maintains a synchronized shadow text for you.

Includes optional support for the Text Animator for Unity asset through the TMPAnimDropShadow component.

Features
--------
- Automatic TextMeshPro drop shadow generation
- Real-time text synchronization
- Supports rich text and color tags
- Fully customizable shadow offset and color
- Preserves most TextMeshPro settings automatically
- Optional compatibility with Text Animator for Unity
- No update loops required

Installation
------------
1. Import the package into your Unity project.
2. Ensure TextMeshPro is installed in the project.
3. (Optional) Install Febucci's Text Animator for Unity if you want animated text shadow support.

Usage
-----
Standard TMP Drop Shadow
1. Select a UI GameObject that contains a TextMeshProUGUI component.
2. Add the TMPDropShadow component
   (Assets/EagleStudio/TMP Drop Shadows/Scripts/TMPDropShadow.cs)
3. Configure the shadow settings in the Inspector.

Text Animator Compatible Drop Shadow
1. Select a UI GameObject that contains:
   - TextMeshProUGUI
   - TextAnimator_TMP
2. Add the TMPAnimDropShadow component
   (Assets/EagleStudio/TMP Drop Shadows/Scripts/TMPAnimDropShadow.cs)
3. Configure the shadow settings in the Inspector.

Inspector Options
-----------------
Shadow Offset
2D offset applied to the shadow relative to the main text.

Shadow Color
Color of the generated drop shadow.

Notes & Tips
-------------
- The component requires a TextMeshProUGUI component on the same GameObject.
- TMPAnimDropShadow additionally requires TextAnimator_TMP.
- The original text object becomes the shadow internally due to Unity UI hierarchy rendering order.
- Text updates are synchronized automatically through TextMeshPro events.
- Rich text color tags are overridden on the shadow to maintain consistent shadow coloring.
- If you are not using Text Animator for Unity, remove the TMPAnimDropShadow.cs script from the project to avoid compilation errors.
