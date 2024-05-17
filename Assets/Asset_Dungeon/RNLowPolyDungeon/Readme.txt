Thank you for purchasing the Red Nebula Studios Low Poly Dungeon set! Please let us know if you have any questions using the contact form on our website:

https://games.rednebulastudios.com/contact

If you use this pack in a finished product and wish to provide a credit, you may credit Red Nebula Studios. We'd appreciate it!

The current version of this set is supported with Unity version 2019.4 and above. The main download supports the Built-in Render Pipeline. If you need support for the Universal Render Pipeline (URP), import the RNLowPolyDungeon_URP_2019.4.unitypackage file into your project instead. I highly recommend using Unity version 2020.2 or higher for URP, as that is when Screen Space Ambient Occlusion (SSAO) was introduced, and it makes props like these look a lot better!

The High Definition Render Pipeline (HDRP) is not currently supported, but you are free to import the files and update the materials as you see fit.

This modular low poly dungeon building set contains:

- 230 FBX models
- 1 color swatch texture
- 1 sprite sheet with 16 sprites used for particle effects and decals
- 294 total prefabs, including:
  - 238 static props
  - 19 activatable animated props
  - 18 lit props (torches/candles)
  - 12 decals
  - 2 lights with flicker effect
  - 5 particle effects
- 2 demo scenes (1 asset showcase and one demo build, plus 3 supporting scripts)
- 1 script (LightFlicker.cs)
- 1 WAV music file (44.1 kHz, 16 bit)

Model poly counts:
- Min: 5
- Max: 760
- Avg: 177

********************

LICENSE

With purchase of this asset pack, you are granted a royalty-free license to use all assets contained herein for your own projects, commercial or non-commercial.

You are not permitted to:
- Claim credit for the creation of these asset files
- Resell, redistribute, or transfer the pack or individual asset files

********************

DEMO SCENES

Two demo scenes (Asset_Showcase and Demo_Build) are provided to preview all of the items in this set. When you press play in each one, you can fly around the scene using the WASD and Q/E keys, and hold down the right mouse button and move the mouse to look around. Props that are activatable will display a target cursor when you hover. Click to activate and de-activate them.

In the Demo_Build scene, you can also toggle the ceiling to view the assets from above as though playing a top-down style game.

********************

PLACING OBJECTS

This set is modular and built so that the pieces fit together when moved incrementally. I strongly recommend using Unity's snap settings when building your scene. Walls are built on a 4m wide by 3m tall scale, so it's generally easiest to move objects by 1m at a time. Go to Edit -> Grid and Snap Settings and change the Move setting to 1. In your scene, activate grid snap by holding down the Ctrl key (or Command on a Mac) as you move the GameObjects around.

********************

MATERIALS

All of the props in this pack utilize a single color swatch texture for better performance. This also makes it very easy to adjust the color palette for your entire scene, by pulling the texture into an image editing program like Photoshop or GIMP and adjusting the colors.

The color swatch texture is used in three materials: RNLowPolyDungeon_Main, _SoftEmissive, and _BrightEmissive, each with a different amount of emission. Note that you will need to have a Post Processing Bloom effect to see emission materials properly. You can see how this is set up in the demo scenes.

In addition, there are three other supplemental materials:

RNLowPolyDungeon_SpriteLit: Used for sprite decals, such as magic symbols and spiderwebs.
RNLowPolyDungeon_ParticleGlow: Used for particle systems. (Omitted in the URP package where the built-in ParticlesLit material is used instead.)
RNLowPolyDungeon_Blackout: An unlit, completely black material, used to black out the area underneath the Floor_Stone_Grate building piece prefab. You can remove the Plane subobject from that prefab if you'd prefer the floor grate be see-through.

********************

ACTIVATED PROPS

All animated props are triggered using a boolean parameter called "Activated". To activate them via script, make sure you have access to the GameObjects Animator component, and update it using SetBool.

Example:

public class ActivateProp : MonoBehaviour
{
    Animator anim;

    void Start()
    {
	anim = GetComponent<Animator>();
    }

    void Update()
    {
	if (Input.GetKeyDown(KeyCode.O))
	{
	    anim.SetBool("Activated", true);
	}
	if (Input.GetKeyDown(KeyCode.P))
	{
	    anim.SetBool("Activated", false);
	}
    }
}

This simple example script would activate the object when you press O on your keyboard, and deactivate it when you press P. For GetComponent to work, this script would need to be placed on the same GameObject as the Animator component. For a more detailed example that also includes activating and deactivating lights and particle effects, take a look at the included ActivateProp.cs demo script.

Note that ActivateProp.cs is for demonstration purposes only in the example scenes and is not included in the prop prefabs. You can reuse it to suit the needs of your project or write an entirely new script, whichever works better.

********************

COLLIDERS

All mesh object prefabs are set up with colliders. Primitive colliders are used where possible, and in some cases, custom convex or non-convex mesh colliders are used. For very simple objects that primitives are unsuitable for, the existing mesh is used as a mesh collider instead. Again, these are set to convex for performance reasons wherever possible.

********************

LIGHT FLICKER SCRIPT

A script is included in the light prefabs for both candles and torch lights called LightFlicker.cs. This script makes randomly adjusts the light's range property within a minimum and maximum range variation, giving it the appearance of flickering fire light. You can use this script as-is, adjust it to work for your own project, or remove it entirely if you wish.

Flicker Speed:
The speed (in seconds) at which the light source flickers.

Range Variation:
How much the range varies from the light's default range value. For example, if the initial range is 2 and the range variation is 1, the light will flicker between 1 and 3.

********************

LIGHTING NOTES

Each lit prop prefab (candles and torches) contains a point light. This is fine if you have a very small number of these lights in your scene. However, point lights - and indeed, a large number of realtime light sources in general - are inefficient. if you intend to have a lot of light sources in your scene, you may wish to remove some or all of the point lights from individual prop prefabs and arrange your own lighting solution.

Because the props in this pack all use a single color swatch texture, the UV maps are very compact. This may result in you getting a warning message about overlapping UVs if you attempt to bake a lightmap with default settings. If you choose to use baked lightmaps, I recommend a lightmap resolution of at least 30. If you need to include very small objects in your baked lightmap for any reason, you may also need to increase their lightmap scale to avoid warnings. For more information about lightmaps, please see this video on Unity's YouTube channel: https://www.youtube.com/watch?v=KJ4fl-KBDR8

********************

MUSIC

The music track, "Gloomy Dungeon" by Robert Wilkinson of Red Nebula Studios, is included as a bonus at no extra cost. This track is in WAV format at 44.1kHz, 16 bit. Enjoy!

********************

IN CLOSING

Thank you once again for purchasing this asset pack. Red Nebula Studios is a small business run by myself (Sarrah) and my husband (Robert). Your support means the world to us!

Follow us:
@RNStudiosGames on Facebook and Twitter
https://games.rednebulastudios.com