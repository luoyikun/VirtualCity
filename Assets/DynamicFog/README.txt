************************************
*        DYNAMIC FOG & MIST        *
* (C) Copyright 2015-2017 Kronnect * 
*           README FILE            *
************************************


How to use this asset
---------------------
Firstly, you should run the Demo Scenes provided to get an idea of the overall functionality.
Later, you should read the documentation and experiment with the plugin.

Hint: to quick start using the asset just add DynamicFog script to your camera. It will show up in the Game view. Customize it using the custom inspector.


Demo Scenes
-----------
There're at several demo scenes, located in "Demos" folder. Just go there from Unity, open them and run it. Remember to remove the Demos folder from your project to reduce size.

Important: since Unity 5.4, the Game View allows to scale the window. Choose "Free Aspect" to easily read the text of some of the demo scenes, otherwise due to the scaling feature demo texts could not be visible inside the window depending on the chosen resolution and your screen size.



Documentation/API reference
---------------------------
The PDF is located in the Documentation folder. It contains additional instructions on how to use this asset as well as a useful Frequent Asked Question section.


Support
-------
Please read the documentation PDF and browse/play with the demo scene and sample source code included before contacting us for support :-)

* Support: contact@kronnect.me
* Website-Forum: http://kronnect.me
* Twitter: @KronnectGames


Future updates
--------------

All our assets follow an incremental development process by which a few beta releases are published on our support forum (kronnect.com).
We encourage you to signup and engage our forum. The forum is the primary support and feature discussions medium.

Of course, all updates of Dynamic Fog & Mist will be eventually available on the Asset Store.


Version history
---------------

V5.1
- VR: support for Single Pass Instanced (Unity 2017.2+)
- Support for Timeline animations
- Added wind direction

V5.0
- New Fog Profiles
- Fog volumes now handle full set of fog properies by setting target fog profile
- Dithering option extended to all fog variants

V4.3
- Added light scattering option to Desktop Plus variant

V4.2
- New fog variant: orthogonal - takes depth and height separately for computing fog
- New fog variant: desktop plus orthogonal - takes depth and height separately for computing fog
- Added noise scale parameter to some fog variants

V4.1
- Fog volumes: added option to specify custom fog colors

V4.0.1
- [Fix] Fixed issue with Post-Processing Stack

V4.0
- New fog of war prefab and mode!
- Updated demo scene "Orthographic" with fog of war functionality

V3.1:
- New fog variant "Basic" for low end mobile devices
- [Fix] Fixed Single Pass Stereo with OpenVR SDK

V3.0:
- Support for orthographic camera

V2.4:
- Compatibility with Unity 5.5
- VR: Compatibility with Single Pass Stereo Rendering

V2.3:
- Additional fog materials for reflection support
- Improved scene 5 (game) with sounds
- Fixed baseline height issue with fog materials
- Added distance falloff to Desktop Fog Plus, Mobile (Simplified) and fog material shaders

V2.2:
- New fog materials for using on geometry (see demo scene 5)
- Updated documentation

V2.0:
- New Desktop Fog Plus variant which improves fog effect
- New custom inspector

V1.8:
- Added Sun property to assign a directional light and implement day light cycle

V1.7:
- Enhanced fog blending for desktop variant
- Fog is now clipped above fog height, saving GPU cycles when using low fog
- Added option “Clip under baseline height”
- Fixed “fog soup” issue when using low camera far clip values

V1.6.1:
- Added compatibility with Gaia Extension System

V1.6:
- Added max distance and max distance falloff parameters.
- Added support for second color to mobile shaders.

V1.5:
- Support for custom void areas (“Fog of War”). See demo scene 4.
- New secondary color to create artistic/gradient fog effects.

V1.4:
- Compatibility with RenderTexture targets

V1.3:
- New option to specify a baseline for the height of the fog

V1.2:
- Support for fog volumes
- Improved performance (even more!)
- Fixed non-advanced fog shader to take into account new sky alpha setting

V1.1:
- Fixed opaque sky haze issue with billboard trees

V1.0
- Initial release

