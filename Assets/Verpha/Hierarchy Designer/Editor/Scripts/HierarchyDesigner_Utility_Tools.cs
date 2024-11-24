#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Verpha.HierarchyDesigner
{
    internal static class HierarchyDesigner_Utility_Tools
    {
        #region Properties
        #region Const Values
        private const string inspectorWindow = "InspectorWindow";
        private const string separatorTag = "EditorOnly";
        private const string separatorPrefix = "//";
        #endregion
        #region Integrations
        private static bool? _isTMPAvailable;
        private static bool IsTMPAvailable()
        {
            if (!_isTMPAvailable.HasValue)
            {
                _isTMPAvailable = AssetDatabase.FindAssets("t:TMP_Settings").Length > 0;
            }
            return _isTMPAvailable.Value;
        }
        #endregion
        #endregion

        #region Menu Items
        #region Activate
        #region General
        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Activate_General + "/Activate Selected GameObjects", false, HierarchyDesigner_Shared_MenuItems.LayerSeven)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Activate_SelectedGameObjects() => Activate_SelectedGameObjects(true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Activate_General + "/Activate All GameObjects", false, HierarchyDesigner_Shared_MenuItems.LayerEight)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Activate_AllGameObjects() => Activate_AllGameObjects(true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Activate_General + "/Activate All Parent GameObjects", false, HierarchyDesigner_Shared_MenuItems.LayerEight)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Activate_ParentGameObjects() => Activate_AllParentGameObjects(true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Activate_General + "/Activate All Empty GameObjects", false, HierarchyDesigner_Shared_MenuItems.LayerNine)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Activate_EmptyGameObjects() => Activate_AllEmptyGameObjects(true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Activate_General + "/Activate All Locked GameObjects", false, HierarchyDesigner_Shared_MenuItems.LayerNine)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Activate_LockedGameObjects() => Activate_AllLockedGameObjects(true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Activate_General + "/Activate All Folders", false, HierarchyDesigner_Shared_MenuItems.LayerTen)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Activate_Folders() => Activate_AllFolders(true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Activate_General + "/Deactivate Selected GameObjects", false, HierarchyDesigner_Shared_MenuItems.LayerEleven)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Deactivate_SelectedGameObjects() => Activate_SelectedGameObjects(false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Activate_General + "/Deactivate All GameObjects", false, HierarchyDesigner_Shared_MenuItems.LayerTwelve)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Deactivate_AllGameObjects() => Activate_AllGameObjects(false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Activate_General + "/Deactivate All Parent GameObjects", false, HierarchyDesigner_Shared_MenuItems.LayerTwelve)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Deactivate_ParentGameObjects() => Activate_AllParentGameObjects(false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Activate_General + "/Deactivate All Empty GameObjects", false, HierarchyDesigner_Shared_MenuItems.LayerThirteen)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Deactivate_EmptyGameObjects() => Activate_AllEmptyGameObjects(false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Activate_General + "/Deactivate All Locked GameObjects", false, HierarchyDesigner_Shared_MenuItems.LayerThirteen)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Deactivate_LockedGameObjects() => Activate_AllLockedGameObjects(false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Activate_General + "/Deactivate All Folders", false, HierarchyDesigner_Shared_MenuItems.LayerFourteen)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Deactivate_Folders() => Activate_AllFolders(false);
        #endregion

        #region Types (Activate)
        #region 2D Objects
        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Activate_Type_2D + "/Activate All Sprites", false, HierarchyDesigner_Shared_MenuItems.LayerEight)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Activate_Sprites() => Activate_AllComponentOfType<SpriteRenderer>(true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Activate_Type_2D + "/Activate All Sprite Masks", false, HierarchyDesigner_Shared_MenuItems.LayerEight + 1)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Activate_SpriteMasks() => Activate_AllComponentOfType<SpriteMask>(true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Activate_Type_2D_Sprites + "/Activate All 9-Sliced Sprites", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 1)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Activate_9SlicedSprites() => Activate_All2DSpritesByType("9-Sliced", true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Activate_Type_2D_Sprites + "/Activate All Capsule Sprites", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 1)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Activate_CapsuleSprites() => Activate_All2DSpritesByType("Capsule", true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Activate_Type_2D_Sprites + "/Activate All Circle Sprites", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 1)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Activate_CircleSprites() => Activate_All2DSpritesByType("Circle", true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Activate_Type_2D_Sprites + "/Activate All Hexagon Flat-Top Sprites", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 1)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Activate_HexagonFlatTopSprites() => Activate_All2DSpritesByType("Hexagon Flat-Top", true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Activate_Type_2D_Sprites + "/Activate All Hexagon Pointed-Top Sprites", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 1)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Activate_HexagonPointedTopSprites() => Activate_All2DSpritesByType("Hexagon Pointed-Top", true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Activate_Type_2D_Sprites + "/Activate All Isometric Diamond Sprites", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 1)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Activate_IsometricDiamondSprites() => Activate_All2DSpritesByType("Isometric Diamond", true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Activate_Type_2D_Sprites + "/Activate All Square Sprites", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 1)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Activate_SquareSprites() => Activate_All2DSpritesByType("Square", true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Activate_Type_2D_Sprites + "/Activate All Triangle Sprites", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 1)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Activate_TriangleSprites() => Activate_All2DSpritesByType("Triangle", true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Activate_Type_2D_Physics + "/Activate All Dynamic Sprites", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 2)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Activate_PhysicsDynamicSprites() => Activate_AllPhysicsDynamicSprites(true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Activate_Type_2D_Physics + "/Activate All Static Sprites", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 2)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Activate_PhysicsStaticSprites() => Activate_AllPhysicsStaticSprites(true);
        #endregion

        #region 3D Objects
        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Activate_Type_3D + "/Activate All Mesh Filters", false, HierarchyDesigner_Shared_MenuItems.LayerEight)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Activate_MeshFilters() => Activate_AllComponentOfType<MeshFilter>(true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Activate_Type_3D + "/Activate All Mesh Renderers", false, HierarchyDesigner_Shared_MenuItems.LayerEight)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Activate_MeshRenderers() => Activate_AllComponentOfType<MeshRenderer>(true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Activate_Type_3D + "/Activate All Skinned Mesh Renderer", false, HierarchyDesigner_Shared_MenuItems.LayerEight)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Activate_SkinnedMeshRenderers() => Activate_AllComponentOfType<SkinnedMeshRenderer>(true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Activate_Type_3D + "/Activate All Cubes", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 1)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Activate_CubesObjects() => Activate_All3DObjectsByType("Cube", true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Activate_Type_3D + "/Activate All Spheres", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 1)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Activate_SpheresObjects() => Activate_All3DObjectsByType("Sphere", true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Activate_Type_3D + "/Activate All Capsules", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 2)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Activate_CapsulesObjects() => Activate_All3DObjectsByType("Capsule", true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Activate_Type_3D + "/Activate All Cylinders", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 2)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Activate_CylindersObjects() => Activate_All3DObjectsByType("Cylinder", true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Activate_Type_3D + "/Activate All Planes", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 2)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Activate_PlanesObjects() => Activate_All3DObjectsByType("Plane", true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Activate_Type_3D + "/Activate All Quads", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 3)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Activate_QuadsObjects() => Activate_All3DObjectsByType("Quad", true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Activate_Type_3D + "/Activate All Texts - TextMeshPro", false, HierarchyDesigner_Shared_MenuItems.LayerTen + 3)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Activate_TextMeshProObjects() => Activate_All3DObjectsByType("TextMeshPro Mesh", true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Activate_Type_3D_Legacy + "/Activate All Text Meshes", false, HierarchyDesigner_Shared_MenuItems.LayerTen + 4)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Activate_TextMeshesObjects() => Activate_AllComponentOfType<TextMesh>(true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Activate_Type_3D + "/Activate All Terrains", false, HierarchyDesigner_Shared_MenuItems.LayerEleven + 4)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Activate_TerrainsObjects() => Activate_AllComponentOfType<Terrain>(true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Activate_Type_3D + "/Activate All Trees", false, HierarchyDesigner_Shared_MenuItems.LayerEleven + 4)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Activate_TreesObjects() => Activate_AllComponentOfType<Tree>(true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Activate_Type_3D + "/Activate All Wind Zones", false, HierarchyDesigner_Shared_MenuItems.LayerEleven + 4)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Activate_WindZonesObjects() => Activate_AllComponentOfType<WindZone>(true);
        #endregion

        #region Audio
        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Activate_Type_Audio + "/Activate All Audio Sources", false, HierarchyDesigner_Shared_MenuItems.LayerEight + 1)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Activate_AudioSources() => Activate_AllComponentOfType<AudioSource>(true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Activate_Type_Audio + "/Activate All Audio Reverb Zones", false, HierarchyDesigner_Shared_MenuItems.LayerEight + 2)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Activate_AudioReverbZones() => Activate_AllComponentOfType<AudioReverbZone>(true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Activate_Type_Audio + "/Activate All Audio Chorus Filters", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 2)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Activate_AudioChorusFilters() => Activate_AllComponentOfType<AudioChorusFilter>(true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Activate_Type_Audio + "/Activate All Audio Distortion Filters", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 2)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Activate_AudioDistortionFilters() => Activate_AllComponentOfType<AudioDistortionFilter>(true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Activate_Type_Audio + "/Activate All Audio Echo Filters", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 2)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Activate_AudioEchoFilters() => Activate_AllComponentOfType<AudioEchoFilter>(true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Activate_Type_Audio + "/Activate All Audio High Pass Filters", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 2)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Activate_AudioHighPassFilters() => Activate_AllComponentOfType<AudioHighPassFilter>(true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Activate_Type_Audio + "/Activate All Audio Listeners", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 2)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Activate_AudioListeners() => Activate_AllComponentOfType<AudioListener>(true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Activate_Type_Audio + "/Activate All Audio Low Pass Filters", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 2)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Activate_AudioLowPassFilters() => Activate_AllComponentOfType<AudioLowPassFilter>(true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Activate_Type_Audio + "/Activate All Audio Reverb Filters", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 2)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Activate_AudioReverbFilters() => Activate_AllComponentOfType<AudioReverbFilter>(true);
        #endregion

        #region Effects
        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Activate_Type_Effects + "/Activate All Particle Systems", false, HierarchyDesigner_Shared_MenuItems.LayerEight + 1)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Activate_ParticleSystems() => Activate_AllComponentOfType<ParticleSystem>(true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Activate_Type_Effects + "/Activate All Particle System Force Fields", false, HierarchyDesigner_Shared_MenuItems.LayerEight + 2)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Activate_ParticleSystemForceFields() => Activate_AllComponentOfType<ParticleSystemForceField>(true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Activate_Type_Effects + "/Activate All Trail Renderers", false, HierarchyDesigner_Shared_MenuItems.LayerEight + 2)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Activate_TrailRenderers() => Activate_AllComponentOfType<TrailRenderer>(true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Activate_Type_Effects + "/Activate All Line Renderers", false, HierarchyDesigner_Shared_MenuItems.LayerEight + 3)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Activate_LineRenderers() => Activate_AllComponentOfType<LineRenderer>(true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Activate_Type_Effects + "/Activate All Halos", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 3)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Activate_Halos() => Activate_AllHalos(true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Activate_Type_Effects + "/Activate All Lens Flares", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 3)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Activate_LensFlares() => Activate_AllComponentOfType<LensFlare>(true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Activate_Type_Effects + "/Activate All Projectors", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 3)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Activate_Projectors() => Activate_AllComponentOfType<Projector>(true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Activate_Type_Effects + "/Activate All Visual Effects", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 3)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Activate_VisualEffects() => Activate_AllComponentOfType<UnityEngine.VFX.VisualEffect>(true);
        #endregion

        #region Lights
        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Activate_Type_Light + "/Activate All Lights", false, HierarchyDesigner_Shared_MenuItems.LayerEight + 1)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Activate_Lights() => Activate_AllComponentOfType<Light>(true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Activate_Type_Light + "/Activate All Directional Lights", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 1)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Activate_DirectionalLights() => Activate_AllComponentOfType<Light>(true, light => light.type == LightType.Directional);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Activate_Type_Light + "/Activate All Point Lights", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 1)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Activate_PointLights() => Activate_AllComponentOfType<Light>(true, light => light.type == LightType.Point);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Activate_Type_Light + "/Activate All Spot Lights", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 1)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Activate_SpotLights() => Activate_AllComponentOfType<Light>(true, light => light.type == LightType.Spot);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Activate_Type_Light + "/Activate All Rectangle Area Lights", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 2)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Activate_RectangleAreaLights() => Activate_AllComponentOfType<Light>(true, light => light.type == LightType.Rectangle);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Activate_Type_Light + "/Activate All Disc Area Lights", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 3)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Activate_DiscAreaLights() => Activate_AllComponentOfType<Light>(true, light => light.type == LightType.Disc);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Activate_Type_Light + "/Activate All Reflection Probes", false, HierarchyDesigner_Shared_MenuItems.LayerTen + 3)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Activate_ReflectionProbes() => Activate_AllComponentOfType<ReflectionProbe>(true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Activate_Type_Light + "/Activate All Light Probe Groups", false, HierarchyDesigner_Shared_MenuItems.LayerTen + 4)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Activate_LightProbeGroups() => Activate_AllComponentOfType<LightProbeGroup>(true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Activate_Type_Light + "/Activate All Light Probe Proxy Volumes", false, HierarchyDesigner_Shared_MenuItems.LayerTen + 4)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Activate_LightProbeProxyVolumes() => Activate_AllComponentOfType<LightProbeProxyVolume>(true);
        #endregion

        #region Video
        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Activate_Type_Video + "/Activate All Video Players", false, HierarchyDesigner_Shared_MenuItems.LayerEight + 1)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Activate_VideoPlayers() => Activate_AllComponentOfType<UnityEngine.Video.VideoPlayer>(true);
        #endregion

        #region UI Toolkit
        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Activate_Type_UIToolkit + "/Activate All UI Documents", false, HierarchyDesigner_Shared_MenuItems.LayerEight + 2)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Activate_UIDocuments() => Activate_AllComponentOfType<UnityEngine.UIElements.UIDocument>(true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Activate_Type_UIToolkit + "/Activate All Panel Event Handlers", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 2)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Activate_PanelEventHandlers() => Activate_AllComponentOfType<UnityEngine.UIElements.PanelEventHandler>(true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Activate_Type_UIToolkit + "/Activate All Panel Raycasters", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 2)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Activate_PanelRaycasters() => Activate_AllComponentOfType<UnityEngine.UIElements.PanelRaycaster>(true);
        #endregion

        #region Cameras
        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Activate_Type + "/Activate All Cameras", false, HierarchyDesigner_Shared_MenuItems.LayerEight + 3)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Activate_Cameras() => Activate_AllComponentOfType<Camera>(true);
        #endregion

        #region UI
        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Activate_Type_UI + "/Activate All Images", false, HierarchyDesigner_Shared_MenuItems.LayerEight + 3)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Activate_Images() => Activate_AllComponentOfType<Image>(true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Activate_Type_UI + "/Activate All Texts - TextMeshPro", false, HierarchyDesigner_Shared_MenuItems.LayerEight + 3)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Activate_TextMeshPro() => Activate_AllTMPComponentIfAvailable<TMPro.TMP_Text>(true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Activate_Type_UI + "/Activate All Raw Images", false, HierarchyDesigner_Shared_MenuItems.LayerEight + 4)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Activate_RawImages() => Activate_AllComponentOfType<RawImage>(true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Activate_Type_UI + "/Activate All Toggles", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 4)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Activate_Toggles() => Activate_AllComponentOfType<Toggle>(true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Activate_Type_UI + "/Activate All Sliders", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 5)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Activate_Sliders() => Activate_AllComponentOfType<Slider>(true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Activate_Type_UI + "/Activate All Scrollbars", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 6)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Activate_Scrollbars() => Activate_AllComponentOfType<Scrollbar>(true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Activate_Type_UI + "/Activate All Scroll Views", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 7)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Activate_ScrollViews() => Activate_AllComponentOfType<ScrollRect>(true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Activate_Type_UI + "/Activate All Dropdowns - TextMeshPro", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 8)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Activate_DropdownTextMeshPro() => Activate_AllTMPComponentIfAvailable<TMPro.TMP_Dropdown>(true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Activate_Type_UI + "/Activate All Input Fields - TextMeshPro", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 8)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Activate_InputFieldTextMeshPro() => Activate_AllTMPComponentIfAvailable<TMPro.TMP_InputField>(true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Activate_Type_UI + "/Activate All Canvases", false, HierarchyDesigner_Shared_MenuItems.LayerTen + 8)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Activate_Canvases() => Activate_AllComponentOfType<Canvas>(true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Activate_Type_UI + "/Activate All Event Systems", false, HierarchyDesigner_Shared_MenuItems.LayerTen + 8)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Activate_EventSystems() => Activate_AllComponentOfType<UnityEngine.EventSystems.EventSystem>(true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Activate_Type_UI_Legacy + "/Activate All Texts", false, HierarchyDesigner_Shared_MenuItems.LayerEleven + 8)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Activate_Texts() => Activate_AllComponentOfType<Text>(true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Activate_Type_UI_Legacy + "/Activate All Buttons", false, HierarchyDesigner_Shared_MenuItems.LayerEleven + 9)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Activate_Buttons() => Activate_AllComponentOfType<Button>(true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Activate_Type_UI_Legacy + "/Activate All Dropdowns", false, HierarchyDesigner_Shared_MenuItems.LayerEleven + 9)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Activate_Dropdowns() => Activate_AllComponentOfType<Dropdown>(true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Activate_Type_UI_Legacy + "/Activate All Input Fields", false, HierarchyDesigner_Shared_MenuItems.LayerEleven + 9)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Activate_InputFields() => Activate_AllComponentOfType<InputField>(true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Activate_Type_UI + "/Activate All Masks", false, HierarchyDesigner_Shared_MenuItems.LayerTwelve + 9)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Activate_Masks() => Activate_AllComponentOfType<Mask>(true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Activate_Type_UI + "/Activate All Rect Masks 2D", false, HierarchyDesigner_Shared_MenuItems.LayerTwelve + 9)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Activate_RectMasks2D() => Activate_AllComponentOfType<RectMask2D>(true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Activate_Type_UI + "/Activate All Selectables", false, HierarchyDesigner_Shared_MenuItems.LayerTwelve + 9)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Activate_Selectables() => Activate_AllComponentOfType<Selectable>(true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Activate_Type_UI + "/Activate All Toggle Groups", false, HierarchyDesigner_Shared_MenuItems.LayerTwelve + 9)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Activate_ToggleGroups() => Activate_AllComponentOfType<ToggleGroup>(true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Activate_Type_UI_Effects + "/Activate All Outlines", false, HierarchyDesigner_Shared_MenuItems.LayerThirteen + 9)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Activate_Outlines() => Activate_AllComponentOfType<Outline>(true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Activate_Type_UI_Effects + "/Activate All Positions As UV1", false, HierarchyDesigner_Shared_MenuItems.LayerThirteen + 9)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Activate_PositionsAsUV1() => Activate_AllComponentOfType<PositionAsUV1>(true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Activate_Type_UI_Effects + "/Activate All Shadows", false, HierarchyDesigner_Shared_MenuItems.LayerThirteen + 9)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Activate_Shadows() => Activate_AllComponentOfType<Shadow>(true);
        #endregion
        #endregion

        #region Types (Deactivate)
        #region 2D Objects
        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Deactivate_Type_2D + "/Deactivate All Sprites", false, HierarchyDesigner_Shared_MenuItems.LayerEight)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Deactivate_AllSprites() => Activate_AllComponentOfType<SpriteRenderer>(false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Deactivate_Type_2D + "/Deactivate All Sprite Masks", false, HierarchyDesigner_Shared_MenuItems.LayerEight + 1)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Deactivate_SpriteMasks() => Activate_AllComponentOfType<SpriteMask>(false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Deactivate_Type_2D_Sprites + "/Deactivate All 9-Sliced Sprites", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 1)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Deactivate_9SlicedSprites() => Activate_All2DSpritesByType("9-Sliced", false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Deactivate_Type_2D_Sprites + "/Deactivate All Capsule Sprites", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 1)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Deactivate_CapsuleSprites() => Activate_All2DSpritesByType("Capsule", false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Deactivate_Type_2D_Sprites + "/Deactivate All Circle Sprites", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 1)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Deactivate_CircleSprites() => Activate_All2DSpritesByType("Circle", false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Deactivate_Type_2D_Sprites + "/Deactivate All Hexagon Flat-Top Sprites", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 1)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Deactivate_HexagonFlatTopSprites() => Activate_All2DSpritesByType("Hexagon Flat-Top", false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Deactivate_Type_2D_Sprites + "/Deactivate All Hexagon Pointed-Top Sprites", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 1)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Deactivate_HexagonPointedTopSprites() => Activate_All2DSpritesByType("Hexagon Pointed-Top", false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Deactivate_Type_2D_Sprites + "/Deactivate All Isometric Diamond Sprites", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 1)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Deactivate_IsometricDiamondSprites() => Activate_All2DSpritesByType("Isometric Diamond", false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Deactivate_Type_2D_Sprites + "/Deactivate All Square Sprites", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 1)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Deactivate_SquareSprites() => Activate_All2DSpritesByType("Square", false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Deactivate_Type_2D_Sprites + "/Deactivate All Triangle Sprites", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 1)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Deactivate_TriangleSprites() => Activate_All2DSpritesByType("Triangle", false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Deactivate_Type_2D_Physics + "/Deactivate All Dynamic Sprites", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 2)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Deactivate_PhysicsDynamicSprites() => Activate_AllPhysicsDynamicSprites(false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Deactivate_Type_2D_Physics + "/Deactivate All Static Sprites", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 2)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Deactivate_PhysicsStaticSprites() => Activate_AllPhysicsStaticSprites(false);
        #endregion

        #region 3D Objects
        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Deactivate_Type_3D + "/Deactivate All Mesh Filters", false, HierarchyDesigner_Shared_MenuItems.LayerEight)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Deactivate_MeshFilters() => Activate_AllComponentOfType<MeshFilter>(false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Deactivate_Type_3D + "/Deactivate All Mesh Renderers", false, HierarchyDesigner_Shared_MenuItems.LayerEight)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Deactivate_MeshRenderers() => Activate_AllComponentOfType<MeshRenderer>(false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Deactivate_Type_3D + "/Deactivate All Skinned Mesh Renderer", false, HierarchyDesigner_Shared_MenuItems.LayerEight)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Deactivate_SkinnedMeshRenderers() => Activate_AllComponentOfType<SkinnedMeshRenderer>(false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Deactivate_Type_3D + "/Deactivate All Cubes", false, HierarchyDesigner_Shared_MenuItems.LayerNine)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Deactivate_CubesObjects() => Activate_All3DObjectsByType("Cube", false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Deactivate_Type_3D + "/Deactivate All Spheres", false, HierarchyDesigner_Shared_MenuItems.LayerNine)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Deactivate_SpheresObjects() => Activate_All3DObjectsByType("Sphere", false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Deactivate_Type_3D + "/Deactivate All Capsules", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 1)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Deactivate_CapsulesObjects() => Activate_All3DObjectsByType("Capsule", false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Deactivate_Type_3D + "/Deactivate All Cylinders", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 1)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Deactivate_CylindersObjects() => Activate_All3DObjectsByType("Cylinder", false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Deactivate_Type_3D + "/Deactivate All Planes", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 1)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Deactivate_PlanesObjects() => Activate_All3DObjectsByType("Plane", false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Deactivate_Type_3D + "/Deactivate All Quads", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 2)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Deactivate_QuadsObjects() => Activate_All3DObjectsByType("Quad", false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Deactivate_Type_3D + "/Deactivate All Texts - TextMeshPro", false, HierarchyDesigner_Shared_MenuItems.LayerTen + 2)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Deactivate_TextMeshProObjects() => Activate_All3DObjectsByType("TextMeshPro Mesh", false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Deactivate_Type_3D_Legacy + "/Deactivate All Text Meshes", false, HierarchyDesigner_Shared_MenuItems.LayerTen + 3)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Deactivate_TextMeshesObjects() => Activate_AllComponentOfType<TextMesh>(false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Deactivate_Type_3D + "/Deactivate All Terrains", false, HierarchyDesigner_Shared_MenuItems.LayerEleven + 3)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Deactivate_TerrainsObjects() => Activate_AllComponentOfType<Terrain>(false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Deactivate_Type_3D + "/Deactivate All Trees", false, HierarchyDesigner_Shared_MenuItems.LayerEleven + 3)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Deactivate_TreesObjects() => Activate_AllComponentOfType<Tree>(false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Deactivate_Type_3D + "/Deactivate All Wind Zones", false, HierarchyDesigner_Shared_MenuItems.LayerEleven + 3)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Deactivate_WindZonesObjects() => Activate_AllComponentOfType<WindZone>(false);
        #endregion

        #region Audio
        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Deactivate_Type_Audio + "/Deactivate All Audio Sources", false, HierarchyDesigner_Shared_MenuItems.LayerEight + 1)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Deactivate_AudioSources() => Activate_AllComponentOfType<AudioSource>(false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Deactivate_Type_Audio + "/Deactivate All Audio Reverb Zones", false, HierarchyDesigner_Shared_MenuItems.LayerEight + 2)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Deactivate_AudioReverbZones() => Activate_AllComponentOfType<AudioReverbZone>(false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Deactivate_Type_Audio + "/Deactivate All Audio Chorus Filters", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 2)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Deactivate_AudioChorusFilters() => Activate_AllComponentOfType<AudioChorusFilter>(false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Deactivate_Type_Audio + "/Deactivate All Audio Distortion Filters", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 2)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Deactivate_AudioDistortionFilters() => Activate_AllComponentOfType<AudioDistortionFilter>(false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Deactivate_Type_Audio + "/Deactivate All Audio Echo Filters", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 2)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Deactivate_AudioEchoFilters() => Activate_AllComponentOfType<AudioEchoFilter>(false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Deactivate_Type_Audio + "/Deactivate All Audio High Pass Filters", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 2)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Deactivate_AudioHighPassFilters() => Activate_AllComponentOfType<AudioHighPassFilter>(false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Deactivate_Type_Audio + "/Deactivate All Audio Listeners", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 2)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Deactivate_AudioListeners() => Activate_AllComponentOfType<AudioListener>(false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Deactivate_Type_Audio + "/Deactivate All Audio Low Pass Filters", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 2)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Deactivate_AudioLowPassFilters() => Activate_AllComponentOfType<AudioLowPassFilter>(false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Deactivate_Type_Audio + "/Deactivate All Audio Reverb Filters", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 2)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Deactivate_AudioReverbFilters() => Activate_AllComponentOfType<AudioReverbFilter>(false);
        #endregion

        #region Effects
        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Deactivate_Type_Effects + "/Deactivate All Particle Systems", false, HierarchyDesigner_Shared_MenuItems.LayerEight + 1)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Deactivate_ParticleSystems() => Activate_AllComponentOfType<ParticleSystem>(false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Deactivate_Type_Effects + "/Deactivate All Particle System Force Fields", false, HierarchyDesigner_Shared_MenuItems.LayerEight + 2)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Deactivate_ParticleSystemForceFields() => Activate_AllComponentOfType<ParticleSystemForceField>(false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Deactivate_Type_Effects + "/Deactivate All Trail Renderers", false, HierarchyDesigner_Shared_MenuItems.LayerEight + 2)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Deactivate_TrailRenderers() => Activate_AllComponentOfType<TrailRenderer>(false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Deactivate_Type_Effects + "/Deactivate All Line Renderers", false, HierarchyDesigner_Shared_MenuItems.LayerEight + 3)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Deactivate_LineRenderers() => Activate_AllComponentOfType<LineRenderer>(false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Deactivate_Type_Effects + "/Deactivate All Halos", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 3)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Deactivate_Halos() => Activate_AllHalos(false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Deactivate_Type_Effects + "/Deactivate All Lens Flares", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 3)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Deactivate_LensFlares() => Activate_AllComponentOfType<LensFlare>(false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Deactivate_Type_Effects + "/Deactivate All Projectors", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 3)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Deactivate_Projectors() => Activate_AllComponentOfType<Projector>(false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Deactivate_Type_Effects + "/Deactivate All Visual Effects", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 3)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Deactivate_VisualEffects() => Activate_AllComponentOfType<UnityEngine.VFX.VisualEffect>(false);
        #endregion

        #region Lights
        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Deactivate_Type_Light + "/Deactivate All Lights", false, HierarchyDesigner_Shared_MenuItems.LayerEight + 1)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Deactivate_Lights() => Activate_AllComponentOfType<Light>(false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Deactivate_Type_Light + "/Deactivate All Directional Lights", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 1)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Deactivate_DirectionalLights() => Activate_AllComponentOfType<Light>(false, light => light.type == LightType.Directional);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Deactivate_Type_Light + "/Deactivate All Point Lights", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 1)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Deactivate_PointLights() => Activate_AllComponentOfType<Light>(false, light => light.type == LightType.Point);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Deactivate_Type_Light + "/Deactivate All Spot Lights", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 1)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Deactivate_SpotLights() => Activate_AllComponentOfType<Light>(false, light => light.type == LightType.Spot);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Deactivate_Type_Light + "/Deactivate All Rectangle Area Lights", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 2)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Deactivate_RectangleAreaLights() => Activate_AllComponentOfType<Light>(false, light => light.type == LightType.Rectangle);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Deactivate_Type_Light + "/Deactivate All Disc Area Lights", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 3)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Deactivate_DiscAreaLights() => Activate_AllComponentOfType<Light>(false, light => light.type == LightType.Disc);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Deactivate_Type_Light + "/Deactivate All Reflection Probes", false, HierarchyDesigner_Shared_MenuItems.LayerTen + 3)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Deactivate_ReflectionProbes() => Activate_AllComponentOfType<ReflectionProbe>(false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Deactivate_Type_Light + "/Deactivate All Light Probe Groups", false, HierarchyDesigner_Shared_MenuItems.LayerTen + 4)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Deactivate_LightProbeGroups() => Activate_AllComponentOfType<LightProbeGroup>(false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Deactivate_Type_Light + "/Deactivate All Light Probe Proxy Volumes", false, HierarchyDesigner_Shared_MenuItems.LayerTen + 4)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Deactivate_LightProbeProxyVolumes() => Activate_AllComponentOfType<LightProbeProxyVolume>(false);
        #endregion

        #region Video
        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Deactivate_Type_Video + "/Deactivate All Video Players", false, HierarchyDesigner_Shared_MenuItems.LayerEight + 1)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Deactivate_VideoPlayers() => Activate_AllComponentOfType<UnityEngine.Video.VideoPlayer>(false);
        #endregion

        #region UI Toolkit
        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Deactivate_Type_UIToolkit + "/Deactivate All UI Documents", false, HierarchyDesigner_Shared_MenuItems.LayerEight + 2)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Deactivate_UIDocuments() => Activate_AllComponentOfType<UnityEngine.UIElements.UIDocument>(false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Deactivate_Type_UIToolkit + "/Deactivate All Panel Event Handlers", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 2)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Deactivate_PanelEventHandlers() => Activate_AllComponentOfType<UnityEngine.UIElements.PanelEventHandler>(false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Deactivate_Type_UIToolkit + "/Deactivate All Panel Raycasters", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 2)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Deactivate_PanelRaycasters() => Activate_AllComponentOfType<UnityEngine.UIElements.PanelRaycaster>(false);
        #endregion

        #region Cameras
        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Deactivate_Type + "/Deactivate All Cameras", false, HierarchyDesigner_Shared_MenuItems.LayerEight + 3)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Deactivate_Cameras() => Activate_AllComponentOfType<Camera>(false);
        #endregion

        #region UI
        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Deactivate_Type_UI + "/Deactivate All Images", false, HierarchyDesigner_Shared_MenuItems.LayerEight + 3)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Deactivate_Images() => Activate_AllComponentOfType<Image>(false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Deactivate_Type_UI + "/Deactivate All Texts - TextMeshPro", false, HierarchyDesigner_Shared_MenuItems.LayerEight + 3)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Deactivate_TextMeshPro() => Activate_AllTMPComponentIfAvailable<TMPro.TMP_Text>(false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Deactivate_Type_UI + "/Deactivate All Raw Images", false, HierarchyDesigner_Shared_MenuItems.LayerEight + 4)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Deactivate_RawImages() => Activate_AllComponentOfType<RawImage>(false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Deactivate_Type_UI + "/Deactivate All Toggles", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 4)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Deactivate_Toggles() => Activate_AllComponentOfType<Toggle>(false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Deactivate_Type_UI + "/Deactivate All Sliders", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 5)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Deactivate_Sliders() => Activate_AllComponentOfType<Slider>(false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Deactivate_Type_UI + "/Deactivate All Scrollbars", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 6)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Deactivate_Scrollbars() => Activate_AllComponentOfType<Scrollbar>(false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Deactivate_Type_UI + "/Deactivate All Scroll Views", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 7)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Deactivate_ScrollViews() => Activate_AllComponentOfType<ScrollRect>(false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Deactivate_Type_UI + "/Deactivate All Dropdowns - TextMeshPro", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 8)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Deactivate_DropdownTextMeshPro() => Activate_AllTMPComponentIfAvailable<TMPro.TMP_Dropdown>(false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Deactivate_Type_UI + "/Deactivate All Input Fields - TextMeshPro", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 8)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Deactivate_InputFieldTextMeshPro() => Activate_AllTMPComponentIfAvailable<TMPro.TMP_InputField>(false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Deactivate_Type_UI + "/Deactivate All Canvases", false, HierarchyDesigner_Shared_MenuItems.LayerTen + 8)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Deactivate_Canvases() => Activate_AllComponentOfType<Canvas>(false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Deactivate_Type_UI + "/Deactivate All Event Systems", false, HierarchyDesigner_Shared_MenuItems.LayerTen + 8)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Deactivate_EventSystems() => Activate_AllComponentOfType<UnityEngine.EventSystems.EventSystem>(false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Deactivate_Type_UI_Legacy + "/Deactivate All Texts", false, HierarchyDesigner_Shared_MenuItems.LayerEleven + 8)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Deactivate_Texts() => Activate_AllComponentOfType<Text>(false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Deactivate_Type_UI_Legacy + "/Deactivate All Buttons", false, HierarchyDesigner_Shared_MenuItems.LayerEleven + 9)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Deactivate_Buttons() => Activate_AllComponentOfType<Button>(false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Deactivate_Type_UI_Legacy + "/Deactivate All Dropdowns", false, HierarchyDesigner_Shared_MenuItems.LayerEleven + 9)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Deactivate_Dropdowns() => Activate_AllComponentOfType<Dropdown>(false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Deactivate_Type_UI_Legacy + "/Deactivate All Input Fields", false, HierarchyDesigner_Shared_MenuItems.LayerEleven + 9)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Deactivate_InputFields() => Activate_AllComponentOfType<InputField>(false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Deactivate_Type_UI + "/Deactivate All Masks", false, HierarchyDesigner_Shared_MenuItems.LayerTwelve + 9)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Deactivate_Masks() => Activate_AllComponentOfType<Mask>(false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Deactivate_Type_UI + "/Deactivate All Rect Masks 2D", false, HierarchyDesigner_Shared_MenuItems.LayerTwelve + 9)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Deactivate_RectMasks2D() => Activate_AllComponentOfType<RectMask2D>(false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Deactivate_Type_UI + "/Deactivate All Selectables", false, HierarchyDesigner_Shared_MenuItems.LayerTwelve + 9)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Deactivate_Selectables() => Activate_AllComponentOfType<Selectable>(false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Deactivate_Type_UI + "/Deactivate All Toggle Groups", false, HierarchyDesigner_Shared_MenuItems.LayerTwelve + 9)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Deactivate_ToggleGroups() => Activate_AllComponentOfType<ToggleGroup>(false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Deactivate_Type_UI_Effects + "/Deactivate All Outlines", false, HierarchyDesigner_Shared_MenuItems.LayerThirteen + 9)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Deactivate_Outlines() => Activate_AllComponentOfType<Outline>(false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Deactivate_Type_UI_Effects + "/Deactivate All Positions As UV1", false, HierarchyDesigner_Shared_MenuItems.LayerThirteen + 9)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Deactivate_PositionsAsUV1() => Activate_AllComponentOfType<PositionAsUV1>(false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Deactivate_Type_UI_Effects + "/Deactivate All Shadows", false, HierarchyDesigner_Shared_MenuItems.LayerThirteen + 9)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Activate)]
        public static void MenuItem_Deactivate_Shadows() => Activate_AllComponentOfType<Shadow>(false);
        #endregion
        #endregion
        #endregion

        #region Count
        #region General
        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Count_General + "/Count Selected GameObjects", false, HierarchyDesigner_Shared_MenuItems.LayerSeven)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Count)]
        public static void MenuItem_Count_SelectedGameObjects() => Count_SelectedGameObjects();

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Count_General + "/Count All GameObjects", false, HierarchyDesigner_Shared_MenuItems.LayerEight)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Count)]
        public static void MenuItem_Count_GameObjects() => Count_AllGameObjects();

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Count_General + "/Count All Parent GameObjects", false, HierarchyDesigner_Shared_MenuItems.LayerEight)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Count)]
        public static void MenuItem_Count_ParentGameObjects() => Count_AllParentGameObjects();

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Count_General + "/Count All Empty GameObjects", false, HierarchyDesigner_Shared_MenuItems.LayerNine)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Count)]
        public static void MenuItem_Count_EmptyGameObjects() => Count_AllEmptyGameObjects();

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Count_General + "/Count All Locked GameObjects", false, HierarchyDesigner_Shared_MenuItems.LayerNine)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Count)]
        public static void MenuItem_Count_LockedGameObjects() => Count_AllLockedGameObjects();

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Count_General + "/Count All Active GameObjects", false, HierarchyDesigner_Shared_MenuItems.LayerTen)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Count)]
        public static void MenuItem_Count_ActiveGameObjects() => Count_AllActiveGameObjects();

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Count_General + "/Count All Inactive GameObjects", false, HierarchyDesigner_Shared_MenuItems.LayerTen)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Count)]
        public static void MenuItem_Count_InactiveGameObjects() => Count_AllInactiveGameObjects();

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Count_General + "/Count All Folders", false, HierarchyDesigner_Shared_MenuItems.LayerEleven)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Count)]
        public static void MenuItem_Count_Folders() => Count_AllFolders();
        
        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Count_General + "/Count All Separators", false, HierarchyDesigner_Shared_MenuItems.LayerEleven)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Count)]
        public static void MenuItem_Count_Separators() => Count_AllSeparators();
        #endregion

        #region Types (Count)
        #region 2D Objects
        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Count_Type_2D + "/Count All Sprites", false, HierarchyDesigner_Shared_MenuItems.LayerEight)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Count)]
        public static void MenuItem_Count_Sprites() => Count_AllComponentOfType<SpriteRenderer>("Sprites");

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Count_Type_2D + "/Count All Sprite Masks", false, HierarchyDesigner_Shared_MenuItems.LayerEight + 1)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Count)]
        public static void MenuItem_Count_SpriteMasks() => Count_AllComponentOfType<SpriteMask>("Sprite Masks");

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Count_Type_2D_Sprites + "/Count All 9-Sliced Sprites", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 1)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Count)]
        public static void MenuItem_Count_9SlicedSprites() => Count_All2DSpritesByType("9-Sliced");

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Count_Type_2D_Sprites + "/Count All Capsule Sprites", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 1)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Count)]
        public static void MenuItem_Count_CapsuleSprites() => Count_All2DSpritesByType("Capsule");

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Count_Type_2D_Sprites + "/Count All Circle Sprites", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 1)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Count)]
        public static void MenuItem_Count_CircleSprites() => Count_All2DSpritesByType("Circle");

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Count_Type_2D_Sprites + "/Count All Hexagon Flat-Top Sprites", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 1)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Count)]
        public static void MenuItem_Count_HexagonFlatTopSprites() => Count_All2DSpritesByType("Hexagon Flat-Top");

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Count_Type_2D_Sprites + "/Count All Hexagon Pointed-Top Sprites", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 1)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Count)]
        public static void MenuItem_Count_HexagonPointedTopSprites() => Count_All2DSpritesByType("Hexagon Pointed-Top");

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Count_Type_2D_Sprites + "/Count All Isometric Diamond Sprites", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 1)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Count)]
        public static void MenuItem_Count_IsometricDiamondSprites() => Count_All2DSpritesByType("Isometric Diamond");

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Count_Type_2D_Sprites + "/Count All Square Sprites", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 1)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Count)]
        public static void MenuItem_Count_SquareSprites() => Count_All2DSpritesByType("Square");

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Count_Type_2D_Sprites + "/Count All Triangle Sprites", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 1)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Count)]
        public static void MenuItem_Count_TriangleSprites() => Count_All2DSpritesByType("Triangle");

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Count_Type_2D_Physics + "/Count All Dynamic Sprites", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 2)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Count)]
        public static void MenuItem_Count_PhysicsDynamicSprites() => Count_AllPhysicsDynamicSprites();

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Count_Type_2D_Physics + "/Count All Static Sprites", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 2)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Count)]
        public static void MenuItem_Count_PhysicsStaticSprites() => Count_AllPhysicsStaticSprites();
        #endregion

        #region 3D Objects
        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Count_Type_3D + "/Count All Mesh Filters", false, HierarchyDesigner_Shared_MenuItems.LayerEight)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Count)]
        public static void MenuItem_Count_MeshFilters() => Count_AllComponentOfType<MeshFilter>("Mesh Filters");

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Count_Type_3D + "/Count All Mesh Renderers", false, HierarchyDesigner_Shared_MenuItems.LayerNine)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Count)]
        public static void MenuItem_Count_MeshRenderers() => Count_AllComponentOfType<MeshRenderer>("Mesh Renderers");

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Count_Type_3D + "/Count All Skinned Mesh Renderer", false, HierarchyDesigner_Shared_MenuItems.LayerNine)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Count)]
        public static void MenuItem_Count_SkinnedMeshRenderers() => Count_AllComponentOfType<SkinnedMeshRenderer>("Skinned Mesh Renderers");

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Count_Type_3D + "/Count All Cubes", false, HierarchyDesigner_Shared_MenuItems.LayerNine)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Count)]
        public static void MenuItem_Count_CubesObjects() => Count_All3DObjectsByType("Cube");

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Count_Type_3D + "/Count All Spheres", false, HierarchyDesigner_Shared_MenuItems.LayerNine)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Count)]
        public static void MenuItem_Count_SpheresObjects() => Count_All3DObjectsByType("Sphere");

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Count_Type_3D + "/Count All Capsules", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 1)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Count)]
        public static void MenuItem_Count_CapsulesObjects() => Count_All3DObjectsByType("Capsule");

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Count_Type_3D + "/Count All Cylinders", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 1)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Count)]
        public static void MenuItem_Count_CylindersObjects() => Count_All3DObjectsByType("Cylinder");

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Count_Type_3D + "/Count All Planes", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 1)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Count)]
        public static void MenuItem_Count_PlanesObjects() => Count_All3DObjectsByType("Plane");

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Count_Type_3D + "/Count All Quads", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 2)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Count)]
        public static void MenuItem_Count_QuadsObjects() => Count_All3DObjectsByType("Quad");

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Count_Type_3D + "/Count All Texts - TextMeshPro", false, HierarchyDesigner_Shared_MenuItems.LayerTen + 2)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Count)]
        public static void MenuItem_Count_TextMeshProObjects() => Count_All3DObjectsByType("TextMeshPro Mesh");

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Count_Type_3D_Legacy + "/Count All Text Meshes", false, HierarchyDesigner_Shared_MenuItems.LayerTen + 3)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Count)]
        public static void MenuItem_Count_TextMeshesObjects() => Count_AllComponentOfType<TextMesh>("Text Meshes");

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Count_Type_3D + "/Count All Terrains", false, HierarchyDesigner_Shared_MenuItems.LayerEleven + 3)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Count)]
        public static void MenuItem_Count_TerrainsObjects() => Count_AllComponentOfType<Terrain>("Terrains");

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Count_Type_3D + "/Count All Trees", false, HierarchyDesigner_Shared_MenuItems.LayerEleven + 3)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Count)]
        public static void MenuItem_Count_TreesObjects() => Count_AllComponentOfType<Tree>("Trees");

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Count_Type_3D + "/Count All Wind Zones", false, HierarchyDesigner_Shared_MenuItems.LayerEleven + 3)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Count)]  
        public static void MenuItem_Count_WindZonesObjects() => Count_AllComponentOfType<WindZone>("Wind Zones");
        #endregion

        #region Audio
        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Count_Type_Audio + "/Count All Audio Sources", false, HierarchyDesigner_Shared_MenuItems.LayerEight + 1)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Count)]
        public static void MenuItem_CountAllAudioSources() => Count_AllComponentOfType<AudioSource>("Audio Sources");

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Count_Type_Audio + "/Count All Audio Reverb Zones", false, HierarchyDesigner_Shared_MenuItems.LayerEight + 2)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Count)]
        public static void MenuItem_Count_AudioReverbZones() => Count_AllComponentOfType<AudioReverbZone>("Audio Reverb Zones");

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Count_Type_Audio + "/Count All Audio Chorus Filters", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 2)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Count)]
        public static void MenuItem_Count_AudioChorusFilters() => Count_AllComponentOfType<AudioChorusFilter>("Audio Chorus Filters");

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Count_Type_Audio + "/Count All Audio Distortion Filters", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 2)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Count)]
        public static void MenuItem_Count_AudioDistortionFilters() => Count_AllComponentOfType<AudioDistortionFilter>("Audio Distortion Filters");

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Count_Type_Audio + "/Count All Audio Echo Filters", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 2)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Count)]
        public static void MenuItem_Count_AudioEchoFilters() => Count_AllComponentOfType<AudioEchoFilter>("Audio Echo Filters");

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Count_Type_Audio + "/Count All Audio High Pass Filters", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 2)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Count)]
        public static void MenuItem_Count_AudioHighPassFilters() => Count_AllComponentOfType<AudioHighPassFilter>("Audio High Pass Filters");

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Count_Type_Audio + "/Count All Audio Listeners", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 2)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Count)]
        public static void MenuItem_Count_AudioListeners() => Count_AllComponentOfType<AudioListener>("Audio Listeners");

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Count_Type_Audio + "/Count All Audio Low Pass Filters", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 2)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Count)]
        public static void MenuItem_Count_AudioLowPassFilters() => Count_AllComponentOfType<AudioLowPassFilter>("Audio Low Pass Filters");

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Count_Type_Audio + "/Count All Audio Reverb Filters", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 2)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Count)]
        public static void MenuItem_Count_AudioReverbFilters() => Count_AllComponentOfType<AudioReverbFilter>("Audio Reverb Filters");
        #endregion

        #region Effects
        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Count_Type_Effects + "/Count All Particle Systems", false, HierarchyDesigner_Shared_MenuItems.LayerEight + 1)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Count)]
        public static void MenuItem_Count_ParticleSystems() => Count_AllComponentOfType<ParticleSystem>("Particle Systems");

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Count_Type_Effects + "/Count All Particle System Force Fields", false, HierarchyDesigner_Shared_MenuItems.LayerEight + 2)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Count)]
        public static void MenuItem_Count_ParticleSystemForceFields() => Count_AllComponentOfType<ParticleSystemForceField>("Particle System Force Fields");

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Count_Type_Effects + "/Count All Trail Renderers", false, HierarchyDesigner_Shared_MenuItems.LayerEight + 2)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Count)]
        public static void MenuItem_Count_TrailRenderers() => Count_AllComponentOfType<TrailRenderer>("Trail Renderers");

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Count_Type_Effects + "/Count All Line Renderers", false, HierarchyDesigner_Shared_MenuItems.LayerEight + 3)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Count)]
        public static void MenuItem_Count_LineRenderers() => Count_AllComponentOfType<LineRenderer>("Line Renderers");

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Count_Type_Effects + "/Count All Halos", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 3)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Count)]
        public static void MenuItem_Count_Halos() => Count_AllHalos();

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Count_Type_Effects + "/Count All Lens Flares", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 3)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Count)]
        public static void MenuItem_Count_LensFlares() => Count_AllComponentOfType<LensFlare>("Lens Flares");

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Count_Type_Effects + "/Count All Projectors", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 3)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Count)]
        public static void MenuItem_Count_Projectors() => Count_AllComponentOfType<Projector>("Projectors");

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Count_Type_Effects + "/Count All Visual Effects", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 3)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Count)]
        public static void MenuItem_Count_VisualEffects() => Count_AllComponentOfType<UnityEngine.VFX.VisualEffect>("Visual Effects");
        #endregion

        #region Lights
        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Count_Type_Light + "/Count All Lights", false, HierarchyDesigner_Shared_MenuItems.LayerEight + 1)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Count)]
        public static void MenuItem_Count_Lights() => Count_AllComponentOfType<Light>("Lights");

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Count_Type_Light + "/Count All Directional Lights", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 1)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Count)]
        public static void MenuItem_Count_DirectionalLights() => Count_AllComponentOfType<Light>("Directional Lights", light => light.type == LightType.Directional);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Count_Type_Light + "/Count All Point Lights", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 1)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Count)]
        public static void MenuItem_Count_PointLights() => Count_AllComponentOfType<Light>("Point Lights", light => light.type == LightType.Point);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Count_Type_Light + "/Count All Spot Lights", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 1)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Count)]
        public static void MenuItem_Count_SpotLights() => Count_AllComponentOfType<Light>("Spot Lights", light => light.type == LightType.Spot);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Count_Type_Light + "/Count All Rectangle Area Lights", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 2)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Count)]
        public static void MenuItem_Count_RectangleAreaLights() => Count_AllComponentOfType<Light>("Rectangle Area Lights", light => light.type == LightType.Rectangle);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Count_Type_Light + "/Count All Disc Area Lights", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 3)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Count)]
        public static void MenuItem_Count_DiscAreaLights() => Count_AllComponentOfType<Light>("Disc Area Lights", light => light.type == LightType.Disc);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Count_Type_Light + "/Count All Reflection Probes", false, HierarchyDesigner_Shared_MenuItems.LayerTen + 3)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Count)]
        public static void MenuItem_Count_ReflectionProbes() => Count_AllComponentOfType<ReflectionProbe>("Reflection Probes");

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Count_Type_Light + "/Count All Light Probe Groups", false, HierarchyDesigner_Shared_MenuItems.LayerTen + 4)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Count)]
        public static void MenuItem_Count_LightProbeGroups() => Count_AllComponentOfType<LightProbeGroup>("Light Probe Groups");

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Count_Type_Light + "/Count All Light Probe Proxy Volumes", false, HierarchyDesigner_Shared_MenuItems.LayerTen + 4)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Count)]
        public static void MenuItem_Count_LightProbeProxyVolumes() => Count_AllComponentOfType<LightProbeProxyVolume>("Light Probe Proxy Volumes");
        #endregion

        #region Video
        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Count_Type_Video + "/Count All Video Players", false, HierarchyDesigner_Shared_MenuItems.LayerEight + 1)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Count)]
        public static void MenuItem_Count_VideoPlayers() => Count_AllComponentOfType<UnityEngine.Video.VideoPlayer>("Video Players");
        #endregion

        #region UI Toolkit
        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Count_Type_UIToolkit + "/Count All UI Documents", false, HierarchyDesigner_Shared_MenuItems.LayerEight + 2)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Count)]
        public static void MenuItem_Count_UIDocuments() => Count_AllComponentOfType<UnityEngine.UIElements.UIDocument>("UI Documents");

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Count_Type_UIToolkit + "/Count All Panel Event Handlers", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 2)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Count)]
        public static void MenuItem_Count_PanelEventHandlers() => Count_AllComponentOfType<UnityEngine.UIElements.PanelEventHandler>("Panel Event Handlers");

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Count_Type_UIToolkit + "/Count All Panel Raycasters", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 2)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Count)]
        public static void MenuItem_Count_PanelRaycasters() => Count_AllComponentOfType<UnityEngine.UIElements.PanelRaycaster>("Panel Raycasters");
        #endregion

        #region Cameras
        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Count_Type + "/Count All Cameras", false, HierarchyDesigner_Shared_MenuItems.LayerEight + 3)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Count)]
        public static void MenuItem_Count_Cameras() => Count_AllComponentOfType<Camera>("Cameras");
        #endregion

        #region UI
        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Count_Type_UI + "/Count All Images", false, HierarchyDesigner_Shared_MenuItems.LayerEight + 3)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Count)]
        public static void MenuItem_Count_Images() => Count_AllComponentOfType<Image>("Images");

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Count_Type_UI + "/Count All Texts - TextMeshPro", false, HierarchyDesigner_Shared_MenuItems.LayerEight + 3)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Count)]
        public static void MenuItem_Count_TextMeshPro() => Count_AllTMPComponentIfAvailable<TMPro.TMP_Text>("Text - TextMeshPro");

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Count_Type_UI + "/Count All Raw Images", false, HierarchyDesigner_Shared_MenuItems.LayerEight + 4)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Count)]
        public static void MenuItem_Count_RawImages() => Count_AllComponentOfType<RawImage>("Raw Images");

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Count_Type_UI + "/Count All Toggles", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 4)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Count)]
        public static void MenuItem_Count_Toggles() => Count_AllComponentOfType<Toggle>("Toggles");

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Count_Type_UI + "/Count All Sliders", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 5)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Count)]
        public static void MenuItem_Count_Sliders() => Count_AllComponentOfType<Slider>("Sliders");

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Count_Type_UI + "/Count All Scrollbars", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 6)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Count)]
        public static void MenuItem_Count_Scrollbars() => Count_AllComponentOfType<Scrollbar>("Scrollbars");

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Count_Type_UI + "/Count All Scroll Views", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 7)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Count)]
        public static void MenuItem_Count_ScrollViews() => Count_AllComponentOfType<ScrollRect>("Scroll Views");

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Count_Type_UI + "/Count All Dropdowns - TextMeshPro", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 8)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Count)]
        public static void MenuItem_Count_DropdownTextMeshPro() => Count_AllTMPComponentIfAvailable<TMPro.TMP_Dropdown>("Dropdowns - TextMeshPro");

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Count_Type_UI + "/Count All Input Fields - TextMeshPro", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 8)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Count)]
        public static void MenuItem_Count_InputFieldTextMeshPro() => Count_AllTMPComponentIfAvailable<TMPro.TMP_InputField>("Input Fields - TextMeshPro");

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Count_Type_UI + "/Count All Canvases", false, HierarchyDesigner_Shared_MenuItems.LayerTen + 8)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Count)]
        public static void MenuItem_Count_Canvases() => Count_AllComponentOfType<Canvas>("Canvases");

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Count_Type_UI + "/Count All Event Systems", false, HierarchyDesigner_Shared_MenuItems.LayerTen + 8)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Count)]
        public static void MenuItem_Count_EventSystems() => Count_AllComponentOfType<UnityEngine.EventSystems.EventSystem>("Event Systems");

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Count_Type_UI_Legacy + "/Count All Texts", false, HierarchyDesigner_Shared_MenuItems.LayerEleven + 8)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Count)]
        public static void MenuItem_Count_Texts() => Count_AllComponentOfType<Text>("Texts");

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Count_Type_UI_Legacy + "/Count All Buttons", false, HierarchyDesigner_Shared_MenuItems.LayerEleven + 9)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Count)]
        public static void MenuItem_Count_Buttons() => Count_AllComponentOfType<Button>("Buttons");

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Count_Type_UI_Legacy + "/Count All Dropdowns", false, HierarchyDesigner_Shared_MenuItems.LayerEleven + 9)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Count)]
        public static void MenuItem_Count_Dropdowns() => Count_AllComponentOfType<Dropdown>("Dropdowns");

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Count_Type_UI_Legacy + "/Count All Input Fields", false, HierarchyDesigner_Shared_MenuItems.LayerEleven + 9)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Count)]
        public static void MenuItem_Count_InputFields() => Count_AllComponentOfType<InputField>("Input Fields");

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Count_Type_UI + "/Count All Masks", false, HierarchyDesigner_Shared_MenuItems.LayerTwelve + 9)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Count)]
        public static void MenuItem_Count_Masks() => Count_AllComponentOfType<Mask>("Masks");

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Count_Type_UI + "/Count All Rect Masks 2D", false, HierarchyDesigner_Shared_MenuItems.LayerTwelve + 9)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Count)]
        public static void MenuItem_Count_RectMasks2D() => Count_AllComponentOfType<RectMask2D>("Rect Masks 2D");

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Count_Type_UI + "/Count All Selectables", false, HierarchyDesigner_Shared_MenuItems.LayerTwelve + 9)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Count)]
        public static void MenuItem_Count_Selectables() => Count_AllComponentOfType<Selectable>("Selectables");

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Count_Type_UI + "/Count All Toggle Groups", false, HierarchyDesigner_Shared_MenuItems.LayerTwelve + 9)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Count)]
        public static void MenuItem_Count_ToggleGroups() => Count_AllComponentOfType<ToggleGroup>("Toggle Groups");

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Count_Type_UI_Effects + "/Count All Outlines", false, HierarchyDesigner_Shared_MenuItems.LayerThirteen + 9)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Count)]
        public static void MenuItem_Count_lOutlines() => Count_AllComponentOfType<Outline>("Outlines");

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Count_Type_UI_Effects + "/Count All Positions As UV1", false, HierarchyDesigner_Shared_MenuItems.LayerThirteen + 9)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Count)]
        public static void MenuItem_Count_PositionsAsUV1() => Count_AllComponentOfType<PositionAsUV1>("Positions As UV1");

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Count_Type_UI_Effects + "/Count All Shadows", false, HierarchyDesigner_Shared_MenuItems.LayerThirteen + 9)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Count)]
        public static void MenuItem_Count_Shadows() => Count_AllComponentOfType<Shadow>("Shadows");
        #endregion
        #endregion
        #endregion

        #region Lock
        #region General
        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Lock_General + "/Lock Selected GameObjects", false, HierarchyDesigner_Shared_MenuItems.LayerSeven)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Lock_SelectedGameObjects() => Lock_SelectedGameObjects(true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Lock_General + "/Lock All GameObjects", false, HierarchyDesigner_Shared_MenuItems.LayerEight)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Lock_GameObjects() => Lock_AllGameObjects(true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Lock_General + "/Lock All Parent GameObjects", false, HierarchyDesigner_Shared_MenuItems.LayerEight)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Lock_ParentGameObjects() => Lock_AllParentGameObjects(true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Lock_General + "/Lock All Empty GameObjects", false, HierarchyDesigner_Shared_MenuItems.LayerNine)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Lock_EmptyGameObjects() => Lock_AllEmptyGameObjects(true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Lock_General + "/Lock All Active GameObjects", false, HierarchyDesigner_Shared_MenuItems.LayerTen)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Lock_ActiveGameObjects() => Lock_AllActiveGameObjects(true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Lock_General + "/Lock All Inactive GameObjects", false, HierarchyDesigner_Shared_MenuItems.LayerTen)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Lock_InactiveGameObjects() => Lock_AllInactiveGameObjects(true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Lock_General + "/Lock All Folders", false, HierarchyDesigner_Shared_MenuItems.LayerEleven)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Lock_Folders() => Lock_AllFolders(true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Lock_General + "/Unlock Selected GameObjects", false, HierarchyDesigner_Shared_MenuItems.LayerTwelve)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Unlock_SelectedGameObjects() => Lock_SelectedGameObjects(false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Lock_General + "/Unlock All GameObjects", false, HierarchyDesigner_Shared_MenuItems.LayerThirteen)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Unlock_GameObjects() => Lock_AllGameObjects(false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Lock_General + "/Unlock All Parent GameObjects", false, HierarchyDesigner_Shared_MenuItems.LayerThirteen)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Unlock_ParentGameObjects() => Lock_AllParentGameObjects(false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Lock_General + "/Unlock All Empty GameObjects", false, HierarchyDesigner_Shared_MenuItems.LayerFourteen)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Unlock_EmptyGameObjects() => Lock_AllEmptyGameObjects(false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Lock_General + "/Unlock All Active GameObjects", false, HierarchyDesigner_Shared_MenuItems.LayerFifteen)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Unlock_ActiveGameObjects() => Lock_AllActiveGameObjects(false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Lock_General + "/Unlock All Inactive GameObjects", false, HierarchyDesigner_Shared_MenuItems.LayerFifteen)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Unlock_InactiveGameObjects() => Lock_AllInactiveGameObjects(false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Lock_General + "/Unlock All Folders", false, HierarchyDesigner_Shared_MenuItems.LayerSixteen)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Unlock_Folders() => Lock_AllFolders(false);
        #endregion

        #region Types (Lock)
        #region 2D Objects
        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Lock_Type_2D + "/Lock All Sprites", false, HierarchyDesigner_Shared_MenuItems.LayerEight)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Lock_Sprites() => Lock_AllComponentOfType<SpriteRenderer>(true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Lock_Type_2D + "/Lock All Sprite Masks", false, HierarchyDesigner_Shared_MenuItems.LayerEight + 1)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Lock_SpriteMasks() => Lock_AllComponentOfType<SpriteMask>(true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Lock_Type_2D_Sprites + "/Lock All 9-Sliced Sprites", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 1)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Lock_9SlicedSprites() => Lock_All2DSpritesByType("9-Sliced", true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Lock_Type_2D_Sprites + "/Lock All Capsule Sprites", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 1)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Lock_CapsuleSprites() => Lock_All2DSpritesByType("Capsule", true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Lock_Type_2D_Sprites + "/Lock All Circle Sprites", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 1)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Lock_CircleSprites() => Lock_All2DSpritesByType("Circle", true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Lock_Type_2D_Sprites + "/Lock All Hexagon Flat-Top Sprites", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 1)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Lock_HexagonFlatTopSprites() => Lock_All2DSpritesByType("Hexagon Flat-Top", true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Lock_Type_2D_Sprites + "/Lock All Hexagon Pointed-Top Sprites", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 1)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Lock_HexagonPointedTopSprites() => Lock_All2DSpritesByType("Hexagon Pointed-Top", true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Lock_Type_2D_Sprites + "/Lock All Isometric Diamond Sprites", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 1)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Lock_IsometricDiamondSprites() => Lock_All2DSpritesByType("Isometric Diamond", true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Lock_Type_2D_Sprites + "/Lock All Square Sprites", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 1)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Lock_SquareSprites() => Lock_All2DSpritesByType("Square", true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Lock_Type_2D_Sprites + "/Lock All Triangle Sprites", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 1)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Lock_TriangleSprites() => Lock_All2DSpritesByType("Triangle", true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Lock_Type_2D_Physics + "/Lock All Dynamic Sprites", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 2)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Lock_PhysicsDynamicSprites() => Lock_AllPhysicsDynamicSprites(true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Lock_Type_2D_Physics + "/Lock All Static Sprites", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 2)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Lock_PhysicsStaticSprites() => Lock_AllPhysicsStaticSprites(true);
        #endregion

        #region 3D Objects
        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Lock_Type_3D + "/Lock All Mesh Filters", false, HierarchyDesigner_Shared_MenuItems.LayerEight)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Lock_MeshFilters() => Lock_AllComponentOfType<MeshFilter>(true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Lock_Type_3D + "/Lock All Mesh Renderers", false, HierarchyDesigner_Shared_MenuItems.LayerEight)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Lock_MeshRenderers() => Lock_AllComponentOfType<MeshRenderer>(true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Lock_Type_3D + "/Lock All Skinned Mesh Renderer", false, HierarchyDesigner_Shared_MenuItems.LayerEight)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Lock_SkinnedMeshRenderers() => Lock_AllComponentOfType<SkinnedMeshRenderer>(true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Lock_Type_3D + "/Lock All Cubes", false, HierarchyDesigner_Shared_MenuItems.LayerNine)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Lock_CubesObjects() => Lock_All3DObjectsByType("Cube", true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Lock_Type_3D + "/Lock All Spheres", false, HierarchyDesigner_Shared_MenuItems.LayerNine)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Lock_SpheresObjects() => Lock_All3DObjectsByType("Sphere", true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Lock_Type_3D + "/Lock All Capsules", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 1)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Lock_CapsulesObjects() => Lock_All3DObjectsByType("Capsule", true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Lock_Type_3D + "/Lock All Cylinders", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 1)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Lock_CylindersObjects() => Lock_All3DObjectsByType("Cylinder", true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Lock_Type_3D + "/Lock All Planes", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 1)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Lock_PlanesObjects() => Lock_All3DObjectsByType("Plane", true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Lock_Type_3D + "/Lock All Quads", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 2)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Lock_QuadsObjects() => Lock_All3DObjectsByType("Quad", true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Lock_Type_3D + "/Lock All Texts - TextMeshPro", false, HierarchyDesigner_Shared_MenuItems.LayerTen + 2)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Lock_TextMeshProObjects() => Lock_All3DObjectsByType("TextMeshPro Mesh", true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Lock_Type_3D_Legacy + "/Lock All Text Meshes", false, HierarchyDesigner_Shared_MenuItems.LayerTen + 3)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Lock_TextMeshesObjects() => Lock_AllComponentOfType<TextMesh>(true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Lock_Type_3D + "/Lock All Terrains", false, HierarchyDesigner_Shared_MenuItems.LayerEleven + 3)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Lock_TerrainsObjects() => Lock_AllComponentOfType<Terrain>(true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Lock_Type_3D + "/Lock All Trees", false, HierarchyDesigner_Shared_MenuItems.LayerEleven + 3)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Lock_TreesObjects() => Lock_AllComponentOfType<Tree>(true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Lock_Type_3D + "/Lock All Wind Zones", false, HierarchyDesigner_Shared_MenuItems.LayerEleven + 3)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Lock_WindZonesObjects() => Lock_AllComponentOfType<WindZone>(true);
        #endregion

        #region Audio
        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Lock_Type_Audio + "/Lock All Audio Sources", false, HierarchyDesigner_Shared_MenuItems.LayerEight + 1)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Lock_AudioSources() => Lock_AllComponentOfType<AudioSource>(true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Lock_Type_Audio + "/Lock All Audio Reverb Zones", false, HierarchyDesigner_Shared_MenuItems.LayerEight + 2)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Lock_AudioReverbZones() => Lock_AllComponentOfType<AudioReverbZone>(true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Lock_Type_Audio + "/Lock All Audio Chorus Filters", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 2)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Lock_AudioChorusFilters() => Lock_AllComponentOfType<AudioChorusFilter>(true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Lock_Type_Audio + "/Lock All Audio Distortion Filters", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 2)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Lock_AudioDistortionFilters() => Lock_AllComponentOfType<AudioDistortionFilter>(true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Lock_Type_Audio + "/Lock All Audio Echo Filters", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 2)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Lock_AudioEchoFilters() => Lock_AllComponentOfType<AudioEchoFilter>(true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Lock_Type_Audio + "/Lock All Audio High Pass Filters", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 2)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Lock_AudioHighPassFilters() => Lock_AllComponentOfType<AudioHighPassFilter>(true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Lock_Type_Audio + "/Lock All Audio Listeners", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 2)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Lock_AudioListeners() => Lock_AllComponentOfType<AudioListener>(true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Lock_Type_Audio + "/Lock All Audio Low Pass Filters", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 2)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Lock_AudioLowPassFilters() => Lock_AllComponentOfType<AudioLowPassFilter>(true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Lock_Type_Audio + "/Lock All Audio Reverb Filters", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 2)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Lock_AudioReverbFilters() => Lock_AllComponentOfType<AudioReverbFilter>(true);
        #endregion

        #region Effects
        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Lock_Type_Effects + "/Lock All Particle Systems", false, HierarchyDesigner_Shared_MenuItems.LayerEight + 1)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Lock_ParticleSystems() => Lock_AllComponentOfType<ParticleSystem>(true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Lock_Type_Effects + "/Lock All Particle System Force Fields", false, HierarchyDesigner_Shared_MenuItems.LayerEight + 2)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Lock_ParticleSystemForceFields() => Lock_AllComponentOfType<ParticleSystemForceField>(true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Lock_Type_Effects + "/Lock All Trail Renderers", false, HierarchyDesigner_Shared_MenuItems.LayerEight + 2)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Lock_TrailRenderers() => Lock_AllComponentOfType<TrailRenderer>(true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Lock_Type_Effects + "/Lock All Line Renderers", false, HierarchyDesigner_Shared_MenuItems.LayerEight + 3)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Lock_LineRenderers() => Lock_AllComponentOfType<LineRenderer>(true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Lock_Type_Effects + "/Lock All Halos", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 3)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Lock_Halos() => Lock_AllHalos(true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Lock_Type_Effects + "/Lock All Lens Flares", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 3)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Lock_LensFlares() => Lock_AllComponentOfType<LensFlare>(true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Lock_Type_Effects + "/Lock All Projectors", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 3)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Lock_Projectors() => Lock_AllComponentOfType<Projector>(true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Lock_Type_Effects + "/Lock All Visual Effects", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 3)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Lock_VisualEffects() => Lock_AllComponentOfType<UnityEngine.VFX.VisualEffect>(true);
        #endregion

        #region Lights
        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Lock_Type_Light + "/Lock All Lights", false, HierarchyDesigner_Shared_MenuItems.LayerEight + 1)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Lock_Lights() => Lock_AllComponentOfType<Light>(true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Lock_Type_Light + "/Lock All Directional Lights", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 1)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Lock_DirectionalLights() => Lock_AllComponentOfType<Light>(true, light => light.type == LightType.Directional);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Lock_Type_Light + "/Lock All Point Lights", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 1)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Lock_PointLights() => Lock_AllComponentOfType<Light>(true, light => light.type == LightType.Point);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Lock_Type_Light + "/Lock All Spot Lights", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 1)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Lock_SpotLights() => Lock_AllComponentOfType<Light>(true, light => light.type == LightType.Spot);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Lock_Type_Light + "/Lock All Rectangle Area Lights", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 2)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Lock_RectangleAreaLights() => Lock_AllComponentOfType<Light>(true, light => light.type == LightType.Rectangle);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Lock_Type_Light + "/Lock All Disc Area Lights", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 3)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Lock_DiscAreaLights() => Lock_AllComponentOfType<Light>(true, light => light.type == LightType.Disc);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Lock_Type_Light + "/Lock All Reflection Probes", false, HierarchyDesigner_Shared_MenuItems.LayerTen + 3)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Lock_ReflectionProbes() => Lock_AllComponentOfType<ReflectionProbe>(true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Lock_Type_Light + "/Lock All Light Probe Groups", false, HierarchyDesigner_Shared_MenuItems.LayerTen + 4)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Lock_LightProbeGroups() => Lock_AllComponentOfType<LightProbeGroup>(true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Lock_Type_Light + "/Lock All Light Probe Proxy Volumes", false, HierarchyDesigner_Shared_MenuItems.LayerTen + 4)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Lock_LightProbeProxyVolumes() => Lock_AllComponentOfType<LightProbeProxyVolume>(true);
        #endregion

        #region Video
        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Lock_Type_Video + "/Lock All Video Players", false, HierarchyDesigner_Shared_MenuItems.LayerEight + 1)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Lock_VideoPlayers() => Lock_AllComponentOfType<UnityEngine.Video.VideoPlayer>(true);
        #endregion

        #region UI Toolkit
        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Lock_Type_UIToolkit + "/Lock All UI Documents", false, HierarchyDesigner_Shared_MenuItems.LayerEight + 2)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Lock_UIDocuments() => Lock_AllComponentOfType<UnityEngine.UIElements.UIDocument>(true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Lock_Type_UIToolkit + "/Lock All Panel Event Handlers", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 2)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Lock_PanelEventHandlers() => Lock_AllComponentOfType<UnityEngine.UIElements.PanelEventHandler>(true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Lock_Type_UIToolkit + "/Lock All Panel Raycasters", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 2)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Lock_PanelRaycasters() => Lock_AllComponentOfType<UnityEngine.UIElements.PanelRaycaster>(true);
        #endregion

        #region Cameras
        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Lock_Type + "/Lock All Cameras", false, HierarchyDesigner_Shared_MenuItems.LayerEight + 3)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Lock_Cameras() => Lock_AllComponentOfType<Camera>(true);
        #endregion

        #region UI
        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Lock_Type_UI + "/Lock All Images", false, HierarchyDesigner_Shared_MenuItems.LayerEight + 3)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Lock_Images() => Lock_AllComponentOfType<Image>(true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Lock_Type_UI + "/Lock All Texts - TextMeshPro", false, HierarchyDesigner_Shared_MenuItems.LayerEight + 3)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Lock_TextMeshPro() => Lock_AllTMPComponentIfAvailable<TMPro.TMP_Text>(true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Lock_Type_UI + "/Lock All Raw Images", false, HierarchyDesigner_Shared_MenuItems.LayerEight + 4)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Lock_RawImages() => Lock_AllComponentOfType<RawImage>(true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Lock_Type_UI + "/Lock All Toggles", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 4)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Lock_Toggles() => Lock_AllComponentOfType<Toggle>(true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Lock_Type_UI + "/Lock All Sliders", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 5)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Lock_Sliders() => Lock_AllComponentOfType<Slider>(true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Lock_Type_UI + "/Lock All Scrollbars", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 6)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Lock_Scrollbars() => Lock_AllComponentOfType<Scrollbar>(true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Lock_Type_UI + "/Lock All Scroll Views", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 7)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Lock_ScrollViews() => Lock_AllComponentOfType<ScrollRect>(true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Lock_Type_UI + "/Lock All Dropdowns - TextMeshPro", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 8)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Lock_DropdownTextMeshPro() => Lock_AllTMPComponentIfAvailable<TMPro.TMP_Dropdown>(true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Lock_Type_UI + "/Lock All Input Fields - TextMeshPro", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 8)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Lock_InputFieldTextMeshPro() => Lock_AllTMPComponentIfAvailable<TMPro.TMP_InputField>(true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Lock_Type_UI + "/Lock All Canvases", false, HierarchyDesigner_Shared_MenuItems.LayerTen + 8)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Lock_Canvases() => Lock_AllComponentOfType<Canvas>(true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Lock_Type_UI + "/Lock All Event Systems", false, HierarchyDesigner_Shared_MenuItems.LayerTen + 8)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Lock_EventSystems() => Lock_AllComponentOfType<UnityEngine.EventSystems.EventSystem>(true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Lock_Type_UI_Legacy + "/Lock All Texts", false, HierarchyDesigner_Shared_MenuItems.LayerEleven + 8)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Lock_Texts() => Lock_AllComponentOfType<Text>(true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Lock_Type_UI_Legacy + "/Lock All Buttons", false, HierarchyDesigner_Shared_MenuItems.LayerEleven + 9)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Lock_Buttons() => Lock_AllComponentOfType<Button>(true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Lock_Type_UI_Legacy + "/Lock All Dropdowns", false, HierarchyDesigner_Shared_MenuItems.LayerEleven + 9)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Lock_Dropdowns() => Lock_AllComponentOfType<Dropdown>(true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Lock_Type_UI_Legacy + "/Lock All Input Fields", false, HierarchyDesigner_Shared_MenuItems.LayerEleven + 9)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Lock_InputFields() => Lock_AllComponentOfType<InputField>(true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Lock_Type_UI + "/Lock All Masks", false, HierarchyDesigner_Shared_MenuItems.LayerTwelve + 9)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Lock_Masks() => Lock_AllComponentOfType<Mask>(true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Lock_Type_UI + "/Lock All Rect Masks 2D", false, HierarchyDesigner_Shared_MenuItems.LayerTwelve + 9)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Lock_RectMasks2D() => Lock_AllComponentOfType<RectMask2D>(true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Lock_Type_UI + "/Lock All Selectables", false, HierarchyDesigner_Shared_MenuItems.LayerTwelve + 9)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Lock_Selectables() => Lock_AllComponentOfType<Selectable>(true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Lock_Type_UI + "/Lock All Toggle Groups", false, HierarchyDesigner_Shared_MenuItems.LayerTwelve + 9)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Lock_ToggleGroups() => Lock_AllComponentOfType<ToggleGroup>(true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Lock_Type_UI_Effects + "/Lock All Outlines", false, HierarchyDesigner_Shared_MenuItems.LayerThirteen + 9)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Lock_Outlines() => Lock_AllComponentOfType<Outline>(true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Lock_Type_UI_Effects + "/Lock All Positions As UV1", false, HierarchyDesigner_Shared_MenuItems.LayerThirteen + 9)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Lock_PositionsAsUV1() => Lock_AllComponentOfType<PositionAsUV1>(true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Lock_Type_UI_Effects + "/Lock All Shadows", false, HierarchyDesigner_Shared_MenuItems.LayerThirteen + 9)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Lock_Shadows() => Lock_AllComponentOfType<Shadow>(true);
        #endregion
        #endregion

        #region Types (Unlock)
        #region 2D Objects
        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Unlock_Type_2D + "/Unlock All Sprites", false, HierarchyDesigner_Shared_MenuItems.LayerEight)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_UnlockAllSprites() => Lock_AllComponentOfType<SpriteRenderer>(false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Unlock_Type_2D + "/Unlock All Sprite Masks", false, HierarchyDesigner_Shared_MenuItems.LayerEight + 1)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_UnlockAllSpriteMasks() => Lock_AllComponentOfType<SpriteMask>(false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Unlock_Type_2D_Sprites + "/Unlock All 9-Sliced Sprites", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 1)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_UnlockAll9SlicedSprites() => Lock_All2DSpritesByType("9-Sliced", false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Unlock_Type_2D_Sprites + "/Unlock All Capsule Sprites", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 1)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_UnlockAllCapsuleSprites() => Lock_All2DSpritesByType("Capsule", false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Unlock_Type_2D_Sprites + "/Unlock All Circle Sprites", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 1)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_UnlockAllCircleSprites() => Lock_All2DSpritesByType("Circle", false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Unlock_Type_2D_Sprites + "/Unlock All Hexagon Flat-Top Sprites", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 1)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_UnlockAllHexagonFlatTopSprites() => Lock_All2DSpritesByType("Hexagon Flat-Top", false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Unlock_Type_2D_Sprites + "/Unlock All Hexagon Pointed-Top Sprites", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 1)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_UnlockAllHexagonPointedTopSprites() => Lock_All2DSpritesByType("Hexagon Pointed-Top", false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Unlock_Type_2D_Sprites + "/Unlock All Isometric Diamond Sprites", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 1)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_UnlockAllIsometricDiamondSprites() => Lock_All2DSpritesByType("Isometric Diamond", false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Unlock_Type_2D_Sprites + "/Unlock All Square Sprites", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 1)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_UnlockAllSquareSprites() => Lock_All2DSpritesByType("Square", false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Unlock_Type_2D_Sprites + "/Unlock All Triangle Sprites", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 1)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_UnlockAllTriangleSprites() => Lock_All2DSpritesByType("Triangle", false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Unlock_Type_2D_Physics + "/Unlock All Dynamic Sprites", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 2)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_UnlockAllPhysicsDynamicSprites() => Lock_AllPhysicsDynamicSprites(false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Unlock_Type_2D_Physics + "/Unlock All Static Sprites", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 2)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_UnlockAllPhysicsStaticSprites() => Lock_AllPhysicsStaticSprites(false);
        #endregion

        #region 3D Objects
        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Unlock_Type_3D + "/Unlock All Mesh Filters", false, HierarchyDesigner_Shared_MenuItems.LayerEight)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Unlock_MeshFilters() => Lock_AllComponentOfType<MeshFilter>(false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Unlock_Type_3D + "/Unlock All Mesh Renderers", false, HierarchyDesigner_Shared_MenuItems.LayerEight)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Unlock__UnlockAllMeshRenderers() => Lock_AllComponentOfType<MeshRenderer>(false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Unlock_Type_3D + "/Unlock All Skinned Mesh Renderer", false, HierarchyDesigner_Shared_MenuItems.LayerEight)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Unlock_SkinnedMeshRenderers() => Lock_AllComponentOfType<SkinnedMeshRenderer>(false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Unlock_Type_3D + "/Unlock All Cubes", false, HierarchyDesigner_Shared_MenuItems.LayerNine)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Unlock_CubesObjects() => Lock_All3DObjectsByType("Cube", false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Unlock_Type_3D + "/Unlock All Spheres", false, HierarchyDesigner_Shared_MenuItems.LayerNine)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Unlock_SpheresObjects() => Lock_All3DObjectsByType("Sphere", false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Unlock_Type_3D + "/Unlock All Capsules", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 1)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Unlock_CapsulesObjects() => Lock_All3DObjectsByType("Capsule", false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Unlock_Type_3D + "/Unlock All Cylinders", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 1)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Unlock_CylindersObjects() => Lock_All3DObjectsByType("Cylinder", false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Unlock_Type_3D + "/Unlock All Planes", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 1)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Unlock_PlanesObjects() => Lock_All3DObjectsByType("Plane", false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Unlock_Type_3D + "/Unlock All Quads", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 2)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Unlock_QuadsObjects() => Lock_All3DObjectsByType("Quad", false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Unlock_Type_3D + "/Unlock All Texts - TextMeshPro", false, HierarchyDesigner_Shared_MenuItems.LayerTen + 2)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Unlock_TextMeshProObjects() => Lock_All3DObjectsByType("TextMeshPro Mesh", false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Unlock_Type_3D_Legacy + "/Unlock All Text Meshes", false, HierarchyDesigner_Shared_MenuItems.LayerTen + 3)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Unlock_TextMeshesObjects() => Lock_AllComponentOfType<TextMesh>(false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Unlock_Type_3D + "/Unlock All Terrains", false, HierarchyDesigner_Shared_MenuItems.LayerEleven + 3)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Unlock_TerrainsObjects() => Lock_AllComponentOfType<Terrain>(false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Unlock_Type_3D + "/Unlock All Trees", false, HierarchyDesigner_Shared_MenuItems.LayerEleven + 3)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Unlock_TreesObjects() => Lock_AllComponentOfType<Tree>(false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Unlock_Type_3D + "/Unlock All Wind Zones", false, HierarchyDesigner_Shared_MenuItems.LayerEleven + 3)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Unlock_WindZonesObjects() => Lock_AllComponentOfType<WindZone>(false);
        #endregion

        #region Audio
        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Unlock_Type_Audio + "/Unlock All Audio Sources", false, HierarchyDesigner_Shared_MenuItems.LayerEight + 1)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Unlock_AudioSources() => Lock_AllComponentOfType<AudioSource>(false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Unlock_Type_Audio + "/Unlock All Audio Reverb Zones", false, HierarchyDesigner_Shared_MenuItems.LayerEight + 2)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Unlock_AudioReverbZones() => Lock_AllComponentOfType<AudioReverbZone>(false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Unlock_Type_Audio + "/Unlock All Audio Chorus Filters", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 2)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Unlock_AudioChorusFilters() => Lock_AllComponentOfType<AudioChorusFilter>(false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Unlock_Type_Audio + "/Unlock All Audio Distortion Filters", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 2)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Unlock_AudioDistortionFilters() => Lock_AllComponentOfType<AudioDistortionFilter>(false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Unlock_Type_Audio + "/Unlock All Audio Echo Filters", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 2)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Unlock_AudioEchoFilters() => Lock_AllComponentOfType<AudioEchoFilter>(false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Unlock_Type_Audio + "/Unlock All Audio High Pass Filters", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 2)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Unlock_AudioHighPassFilters() => Lock_AllComponentOfType<AudioHighPassFilter>(false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Unlock_Type_Audio + "/Unlock All Audio Listeners", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 2)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Unlock_AudioListeners() => Lock_AllComponentOfType<AudioListener>(false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Unlock_Type_Audio + "/Unlock All Audio Low Pass Filters", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 2)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Unlock_AudioLowPassFilters() => Lock_AllComponentOfType<AudioLowPassFilter>(false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Unlock_Type_Audio + "/Unlock All Audio Reverb Filters", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 2)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Unlock_AudioReverbFilters() => Lock_AllComponentOfType<AudioReverbFilter>(false);
        #endregion

        #region Effects
        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Unlock_Type_Effects + "/Unlock All Particle Systems", false, HierarchyDesigner_Shared_MenuItems.LayerEight + 1)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Unlock_ParticleSystems() => Lock_AllComponentOfType<ParticleSystem>(false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Unlock_Type_Effects + "/Unlock All Particle System Force Fields", false, HierarchyDesigner_Shared_MenuItems.LayerEight + 2)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Unlock_ParticleSystemForceFields() => Lock_AllComponentOfType<ParticleSystemForceField>(false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Unlock_Type_Effects + "/Unlock All Trail Renderers", false, HierarchyDesigner_Shared_MenuItems.LayerEight + 2)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Unlock_TrailRenderers() => Lock_AllComponentOfType<TrailRenderer>(false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Unlock_Type_Effects + "/Unlock All Line Renderers", false, HierarchyDesigner_Shared_MenuItems.LayerEight + 3)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Unlock_LineRenderers() => Lock_AllComponentOfType<LineRenderer>(false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Unlock_Type_Effects + "/Unlock All Halos", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 3)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Unlock_Halos() => Lock_AllHalos(true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Unlock_Type_Effects + "/Unlock All Lens Flares", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 3)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Unlock_LensFlares() => Lock_AllComponentOfType<LensFlare>(false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Unlock_Type_Effects + "/Unlock All Projectors", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 3)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Unlock_Projectors() => Lock_AllComponentOfType<Projector>(false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Unlock_Type_Effects + "/Unlock All Visual Effects", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 3)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Unlock_VisualEffects() => Lock_AllComponentOfType<UnityEngine.VFX.VisualEffect>(true);
        #endregion

        #region Lights
        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Unlock_Type_Light + "/Unlock All Lights", false, HierarchyDesigner_Shared_MenuItems.LayerEight + 1)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Unlock_Lights() => Lock_AllComponentOfType<Light>(false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Unlock_Type_Light + "/Unlock All Directional Lights", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 1)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Unlock_DirectionalLights() => Lock_AllComponentOfType<Light>(false, light => light.type == LightType.Directional);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Unlock_Type_Light + "/Unlock All Point Lights", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 1)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Unlock_PointLights() => Lock_AllComponentOfType<Light>(false, light => light.type == LightType.Point);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Unlock_Type_Light + "/Unlock All Spot Lights", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 1)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Unlock_SpotLights() => Lock_AllComponentOfType<Light>(false, light => light.type == LightType.Spot);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Unlock_Type_Light + "/Unlock All Rectangle Area Lights", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 2)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Unlock_RectangleAreaLights() => Lock_AllComponentOfType<Light>(false, light => light.type == LightType.Rectangle);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Unlock_Type_Light + "/Unlock All Disc Area Lights", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 3)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Unlock_DiscAreaLights() => Lock_AllComponentOfType<Light>(false, light => light.type == LightType.Disc);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Unlock_Type_Light + "/Unlock All Reflection Probes", false, HierarchyDesigner_Shared_MenuItems.LayerTen + 3)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Unlock_ReflectionProbes() => Lock_AllComponentOfType<ReflectionProbe>(false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Unlock_Type_Light + "/Unlock All Light Probe Groups", false, HierarchyDesigner_Shared_MenuItems.LayerTen + 4)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Unlock_LightProbeGroups() => Lock_AllComponentOfType<LightProbeGroup>(false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Unlock_Type_Light + "/Unlock All Light Probe Proxy Volumes", false, HierarchyDesigner_Shared_MenuItems.LayerTen + 4)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Unlock_LightProbeProxyVolumes() => Lock_AllComponentOfType<LightProbeProxyVolume>(false);
        #endregion

        #region Video
        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Unlock_Type_Video + "/Unlock All Video Players", false, HierarchyDesigner_Shared_MenuItems.LayerEight + 1)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Unlock_VideoPlayers() => Lock_AllComponentOfType<UnityEngine.Video.VideoPlayer>(false);
        #endregion

        #region UI Toolkit
        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Unlock_Type_UIToolkit + "/Unlock All UI Documents", false, HierarchyDesigner_Shared_MenuItems.LayerEight + 2)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Unlock_UIDocuments() => Lock_AllComponentOfType<UnityEngine.UIElements.UIDocument>(false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Unlock_Type_UIToolkit + "/Unlock All Panel Event Handlers", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 2)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Unlock_PanelEventHandlers() => Lock_AllComponentOfType<UnityEngine.UIElements.PanelEventHandler>(false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Unlock_Type_UIToolkit + "/Unlock All Panel Raycasters", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 2)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Unlock_PanelRaycasters() => Lock_AllComponentOfType<UnityEngine.UIElements.PanelRaycaster>(false);
        #endregion

        #region Cameras
        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Unlock_Type + "/Unlock All Cameras", false, HierarchyDesigner_Shared_MenuItems.LayerEight + 3)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Unlock_AllCameras() => Lock_AllComponentOfType<Camera>(false);
        #endregion

        #region UI
        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Unlock_Type_UI + "/Unlock All Images", false, HierarchyDesigner_Shared_MenuItems.LayerEight + 3)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Unlock_Images() => Lock_AllComponentOfType<Image>(false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Unlock_Type_UI + "/Unlock All Texts - TextMeshPro", false, HierarchyDesigner_Shared_MenuItems.LayerEight + 3)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Unlock_TextMeshPro() => Lock_AllTMPComponentIfAvailable<TMPro.TMP_Text>(false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Unlock_Type_UI + "/Unlock All Raw Images", false, HierarchyDesigner_Shared_MenuItems.LayerEight + 4)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Unlock_RawImages() => Lock_AllComponentOfType<RawImage>(false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Unlock_Type_UI + "/Unlock All Toggles", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 4)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Unlock_Toggles() => Lock_AllComponentOfType<Toggle>(false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Unlock_Type_UI + "/Unlock All Sliders", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 5)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Unlock_Sliders() => Lock_AllComponentOfType<Slider>(false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Unlock_Type_UI + "/Unlock All Scrollbars", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 6)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Unlock_Scrollbars() => Lock_AllComponentOfType<Scrollbar>(false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Unlock_Type_UI + "/Unlock All Scroll Views", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 7)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Unlock_ScrollViews() => Lock_AllComponentOfType<ScrollRect>(false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Unlock_Type_UI + "/Unlock All Dropdowns - TextMeshPro", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 8)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Unlock_DropdownTextMeshPro() => Lock_AllTMPComponentIfAvailable<TMPro.TMP_Dropdown>(false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Unlock_Type_UI + "/Unlock All Input Fields - TextMeshPro", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 8)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Unlock_InputFieldTextMeshPro() => Lock_AllTMPComponentIfAvailable<TMPro.TMP_InputField>(false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Unlock_Type_UI + "/Unlock All Canvases", false, HierarchyDesigner_Shared_MenuItems.LayerTen + 8)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Unlock_Canvases() => Lock_AllComponentOfType<Canvas>(false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Unlock_Type_UI + "/Unlock All Event Systems", false, HierarchyDesigner_Shared_MenuItems.LayerTen + 8)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Unlock_EventSystems() => Lock_AllComponentOfType<UnityEngine.EventSystems.EventSystem>(false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Unlock_Type_UI_Legacy + "/Unlock All Texts", false, HierarchyDesigner_Shared_MenuItems.LayerEleven + 8)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Unlock_Texts() => Lock_AllComponentOfType<Text>(false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Unlock_Type_UI_Legacy + "/Unlock All Buttons", false, HierarchyDesigner_Shared_MenuItems.LayerEleven + 9)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Unlock_Buttons() => Lock_AllComponentOfType<Button>(false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Unlock_Type_UI_Legacy + "/Unlock All Dropdowns", false, HierarchyDesigner_Shared_MenuItems.LayerEleven + 9)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Unlock_Dropdowns() => Lock_AllComponentOfType<Dropdown>(false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Unlock_Type_UI_Legacy + "/Unlock All Input Fields", false, HierarchyDesigner_Shared_MenuItems.LayerEleven + 9)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Unlock_InputFields() => Lock_AllComponentOfType<InputField>(false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Unlock_Type_UI + "/Unlock All Masks", false, HierarchyDesigner_Shared_MenuItems.LayerTwelve + 9)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Unlock_Masks() => Lock_AllComponentOfType<Mask>(false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Unlock_Type_UI + "/Unlock All Rect Masks 2D", false, HierarchyDesigner_Shared_MenuItems.LayerTwelve + 9)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Unlock_RectMasks2D() => Lock_AllComponentOfType<RectMask2D>(false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Unlock_Type_UI + "/Unlock All Selectables", false, HierarchyDesigner_Shared_MenuItems.LayerTwelve + 9)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Unlock_Selectables() => Lock_AllComponentOfType<Selectable>(false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Unlock_Type_UI + "/Unlock All Toggle Groups", false, HierarchyDesigner_Shared_MenuItems.LayerTwelve + 9)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Unlock_ToggleGroups() => Lock_AllComponentOfType<ToggleGroup>(false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Unlock_Type_UI_Effects + "/Unlock All Outlines", false, HierarchyDesigner_Shared_MenuItems.LayerThirteen + 9)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Unlock_Outlines() => Lock_AllComponentOfType<Outline>(false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Unlock_Type_UI_Effects + "/Unlock All Positions As UV1", false, HierarchyDesigner_Shared_MenuItems.LayerThirteen + 9)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Unlock_PositionsAsUV1() => Lock_AllComponentOfType<PositionAsUV1>(false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Unlock_Type_UI_Effects + "/Unlock All Shadows", false, HierarchyDesigner_Shared_MenuItems.LayerThirteen + 9)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Lock)]
        public static void MenuItem_Unlock_Shadows() => Lock_AllComponentOfType<Shadow>(false);
        #endregion
        #endregion
        #endregion

        #region Rename
        #region General
        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Rename + "/Rename Selected GameObjects", false, HierarchyDesigner_Shared_MenuItems.LayerSeven)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Rename)]
        public static void MenuItem_Rename_SelectedGameObjectst() => Rename_SelectedGameObjects("rename", false);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Rename + "/Rename Selected GameObjects with Auto-Indexing", false, HierarchyDesigner_Shared_MenuItems.LayerSeven)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Rename)]
        public static void MenuItem_Rename_SelectedGameObjectsWithAutoIndex() => Rename_SelectedGameObjects("rename with automatic indexing", true);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Rename + "/Open Rename Tool Window", false, HierarchyDesigner_Shared_MenuItems.LayerEight)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Rename)]
        public static void MenuItem_Rename_OpenWindow() => Rename_OpenRenameToolWindow();
        #endregion
        #endregion

        #region Select
        #region General
        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Select_General + "/Select All GameObjects", false, HierarchyDesigner_Shared_MenuItems.LayerSeven)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Select)]
        public static void MenuItem_Select_GameObjects() => Select_AllGameObjects();

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Select_General + "/Select All Parent GameObjects", false, HierarchyDesigner_Shared_MenuItems.LayerSeven + 1)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Select)]
        public static void MenuItem_Select_ParentGameObjects() => Select_AllParentGameObjects();

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Select_General + "/Select All Empty GameObjects", false, HierarchyDesigner_Shared_MenuItems.LayerEight + 1)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Select)]
        public static void MenuItem_Select_EmptyGameObjects() => Select_AllEmptyGameObjects();

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Select_General + "/Select All Locked GameObjects", false, HierarchyDesigner_Shared_MenuItems.LayerEight + 1)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Select)]
        public static void MenuItem_Select_LockedGameObjects() => Select_AllLockedGameObjects();

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Select_General + "/Select All Active GameObjects", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 1)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Select)]
        public static void MenuItem_Select_ActiveGameObjects() => Select_AllActiveGameObjects();

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Select_General + "/Select All Inactive GameObjects", false, HierarchyDesigner_Shared_MenuItems.LayerNine + 1)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Select)]
        public static void MenuItem_Select_InactiveGameObjects() => Select_AllInactiveGameObjects();

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Select_General + "/Select All Folders", false, HierarchyDesigner_Shared_MenuItems.LayerTen + 1)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Select)]
        public static void MenuItem_Select_Folders() => Select_AllFolders();

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Select_General + "/Select All Separators", false, HierarchyDesigner_Shared_MenuItems.LayerTen + 1)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Select)]
        public static void MenuItem_Select_Separators() => Select_AllSeparators();
        #endregion

        #region Types (Select)
        #region 2D Objects
        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Select_Type_2D + "/Select All Sprites", false, HierarchyDesigner_Shared_MenuItems.LayerEleven + 1)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Select)]
        public static void MenuItem_Select_Sprites() => Select_AllComponentOfType<SpriteRenderer>();

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Select_Type_2D + "/Select All Sprite Masks", false, HierarchyDesigner_Shared_MenuItems.LayerEleven + 2)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Select)]
        public static void MenuItem_Select_SpriteMasks() => Select_AllComponentOfType<SpriteMask>();

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Select_Type_2D_Sprites + "/Select All 9-Sliced Sprites", false, HierarchyDesigner_Shared_MenuItems.LayerTwelve + 2)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Select)]
        public static void MenuItem_Select_9SlicedSprites() => Select_All2DSpritesByType("9-Sliced");

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Select_Type_2D_Sprites + "/Select All Capsule Sprites", false, HierarchyDesigner_Shared_MenuItems.LayerTwelve + 2)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Select)]
        public static void MenuItem_Select_CapsuleSprites() => Select_All2DSpritesByType("Capsule");

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Select_Type_2D_Sprites + "/Select All Circle Sprites", false, HierarchyDesigner_Shared_MenuItems.LayerTwelve + 2)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Select)]
        public static void MenuItem_Select_CircleSprites() => Select_All2DSpritesByType("Circle");

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Select_Type_2D_Sprites + "/Select All Hexagon Flat-Top Sprites", false, HierarchyDesigner_Shared_MenuItems.LayerTwelve + 2)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Select)]
        public static void MenuItem_Select_HexagonFlatTopSprites() => Select_All2DSpritesByType("Hexagon Flat-Top");

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Select_Type_2D_Sprites + "/Select All Hexagon Pointed-Top Sprites", false, HierarchyDesigner_Shared_MenuItems.LayerTwelve + 2)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Select)]
        public static void MenuItem_Select_HexagonPointedTopSprites() => Select_All2DSpritesByType("Hexagon Pointed-Top");

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Select_Type_2D_Sprites + "/Select All Isometric Diamond Sprites", false, HierarchyDesigner_Shared_MenuItems.LayerTwelve + 2)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Select)]
        public static void MenuItem_Select_IsometricDiamondSprites() => Select_All2DSpritesByType("Isometric Diamond");

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Select_Type_2D_Sprites + "/Select All Square Sprites", false, HierarchyDesigner_Shared_MenuItems.LayerTwelve + 2)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Select)]
        public static void MenuItem_Select_SquareSprites() => Select_All2DSpritesByType("Square");

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Select_Type_2D_Sprites + "/Select All Triangle Sprites", false, HierarchyDesigner_Shared_MenuItems.LayerTwelve + 2)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Select)]
        public static void MenuItem_Select_TriangleSprites() => Select_All2DSpritesByType("Triangle");

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Select_Type_2D_Physics + "/Select All Dynamic Sprites", false, HierarchyDesigner_Shared_MenuItems.LayerTwelve + 3)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Select)]
        public static void MenuItem_Select_PhysicsDynamicSprites() => Select_AllPhysicsDynamicSprites();

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Select_Type_2D_Physics + "/Select All Static Sprites", false, HierarchyDesigner_Shared_MenuItems.LayerTwelve + 3)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Select)]
        public static void MenuItem_Select_PhysicsStaticSprites() => Select_AllPhysicsStaticSprites();
        #endregion

        #region 3D Objects
        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Select_Type_3D + "/Select All Mesh Filters", false, HierarchyDesigner_Shared_MenuItems.LayerEleven + 1)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Select)]
        public static void MenuItem_Select_MeshFilters() => Select_AllComponentOfType<MeshFilter>();

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Select_Type_3D + "/Select All Mesh Renderers", false, HierarchyDesigner_Shared_MenuItems.LayerEleven + 1)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Select)]
        public static void MenuItem_Select_MeshRenderers() => Select_AllComponentOfType<MeshRenderer>();

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Select_Type_3D + "/Select All Skinned Mesh Renderer", false, HierarchyDesigner_Shared_MenuItems.LayerEleven + 1)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Select)]
        public static void MenuItem_Select_SkinnedMeshRenderers() => Select_AllComponentOfType<SkinnedMeshRenderer>();

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Select_Type_3D + "/Select All Cubes", false, HierarchyDesigner_Shared_MenuItems.LayerTwelve + 1)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Select)]
        public static void MenuItem_Select_CubesObjects() => Select_All3DObjectsByType("Cube");

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Select_Type_3D + "/Select All Spheres", false, HierarchyDesigner_Shared_MenuItems.LayerTwelve + 1)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Select)]
        public static void MenuItem_Select_SpheresObjects() => Select_All3DObjectsByType("Sphere");

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Select_Type_3D + "/Select All Capsules", false, HierarchyDesigner_Shared_MenuItems.LayerTwelve + 2)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Select)]
        public static void MenuItem_Select_CapsulesObjects() => Select_All3DObjectsByType("Capsule");

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Select_Type_3D + "/Select All Cylinders", false, HierarchyDesigner_Shared_MenuItems.LayerTwelve + 2)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Select)]
        public static void MenuItem_Select_CylindersObjects() => Select_All3DObjectsByType("Cylinder");

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Select_Type_3D + "/Select All Planes", false, HierarchyDesigner_Shared_MenuItems.LayerTwelve + 2)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Select)]
        public static void MenuItem_Select_PlanesObjects() => Select_All3DObjectsByType("Plane");

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Select_Type_3D + "/Select All Quads", false, HierarchyDesigner_Shared_MenuItems.LayerTwelve + 3)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Select)]
        public static void MenuItem_Select_QuadsObjects() => Select_All3DObjectsByType("Quad");

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Select_Type_3D + "/Select All Texts - TextMeshPro", false, HierarchyDesigner_Shared_MenuItems.LayerThirteen + 3)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Select)]
        public static void MenuItem_Select_TextMeshProObjects() => Select_All3DObjectsByType("TextMeshPro Mesh");

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Select_Type_3D_Legacy + "/Select All Text Meshes", false, HierarchyDesigner_Shared_MenuItems.LayerThirteen + 4)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Select)]
        public static void MenuItem_Select_TextMeshesObjects() => Select_AllComponentOfType<TextMesh>();

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Select_Type_3D + "/Select All Terrains", false, HierarchyDesigner_Shared_MenuItems.LayerFourteen + 4)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Select)]
        public static void MenuItem_Select_TerrainsObjects() => Select_AllComponentOfType<Terrain>();

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Select_Type_3D + "/Select All Trees", false, HierarchyDesigner_Shared_MenuItems.LayerFourteen + 4)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Select)]
        public static void MenuItem_Select_TreesObjects() => Select_AllComponentOfType<Tree>();

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Select_Type_3D + "/Select All Wind Zones", false, HierarchyDesigner_Shared_MenuItems.LayerFourteen + 4)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Select)]
        public static void MenuItem_Select_WindZonesObjects() => Select_AllComponentOfType<WindZone>();
        #endregion

        #region Audio
        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Select_Type_Audio + "/Select All Audio Sources", false, HierarchyDesigner_Shared_MenuItems.LayerEleven + 2)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Select)]
        public static void MenuItem_Select_AudioSources() => Select_AllComponentOfType<AudioSource>();

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Select_Type_Audio + "/Select All Audio Reverb Zones", false, HierarchyDesigner_Shared_MenuItems.LayerEleven + 3)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Select)]
        public static void MenuItem_Select_AudioReverbZones() => Select_AllComponentOfType<AudioReverbZone>();

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Select_Type_Audio + "/Select All Audio Chorus Filters", false, HierarchyDesigner_Shared_MenuItems.LayerTwelve + 3)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Select)]
        public static void MenuItem_Select_AudioChorusFilters() => Select_AllComponentOfType<AudioChorusFilter>();

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Select_Type_Audio + "/Select All Audio Distortion Filters", false, HierarchyDesigner_Shared_MenuItems.LayerTwelve + 3)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Select)]
        public static void MenuItem_Select_AudioDistortionFilters() => Select_AllComponentOfType<AudioDistortionFilter>();

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Select_Type_Audio + "/Select All Audio Echo Filters", false, HierarchyDesigner_Shared_MenuItems.LayerTwelve + 3)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Select)]
        public static void MenuItem_Select_AudioEchoFilters() => Select_AllComponentOfType<AudioEchoFilter>();

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Select_Type_Audio + "/Select All Audio High Pass Filters", false, HierarchyDesigner_Shared_MenuItems.LayerTwelve + 3)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Select)]
        public static void MenuItem_Select_AudioHighPassFilters() => Select_AllComponentOfType<AudioHighPassFilter>();

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Select_Type_Audio + "/Select All Audio Listeners", false, HierarchyDesigner_Shared_MenuItems.LayerTwelve + 3)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Select)]
        public static void MenuItem_Select_AudioListeners() => Select_AllComponentOfType<AudioListener>();

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Select_Type_Audio + "/Select All Audio Low Pass Filters", false, HierarchyDesigner_Shared_MenuItems.LayerTwelve + 3)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Select)]
        public static void MenuItem_Select_AudioLowPassFilters() => Select_AllComponentOfType<AudioLowPassFilter>();

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Select_Type_Audio + "/Select All Audio Reverb Filters", false, HierarchyDesigner_Shared_MenuItems.LayerTwelve + 3)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Select)]
        public static void MenuItem_Select_AudioReverbFilters() => Select_AllComponentOfType<AudioReverbFilter>();
        #endregion

        #region Effects
        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Select_Type_Effects + "/Select All Particle Systems", false, HierarchyDesigner_Shared_MenuItems.LayerEleven + 2)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Select)]
        public static void MenuItem_Select_ParticleSystems() => Select_AllComponentOfType<ParticleSystem>();

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Select_Type_Effects + "/Select All Particle System Force Fields", false, HierarchyDesigner_Shared_MenuItems.LayerEleven + 3)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Select)]
        public static void MenuItem_Select_ParticleSystemForceFields() => Select_AllComponentOfType<ParticleSystemForceField>();

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Select_Type_Effects + "/Select All Trail Renderers", false, HierarchyDesigner_Shared_MenuItems.LayerEleven + 3)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Select)]
        public static void MenuItem_Select_TrailRenderers() => Select_AllComponentOfType<TrailRenderer>();

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Select_Type_Effects + "/Select All Line Renderers", false, HierarchyDesigner_Shared_MenuItems.LayerEleven + 4)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Select)]
        public static void MenuItem_Select_LineRenderers() => Select_AllComponentOfType<LineRenderer>();

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Select_Type_Effects + "/Select All Halos", false, HierarchyDesigner_Shared_MenuItems.LayerTwelve + 4)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Select)]
        public static void MenuItem_Select_Halos() => Select_AllHalos();

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Select_Type_Effects + "/Select All Lens Flares", false, HierarchyDesigner_Shared_MenuItems.LayerTwelve + 4)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Select)]
        public static void MenuItem_Select_LensFlares() => Select_AllComponentOfType<LensFlare>();

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Select_Type_Effects + "/Select All Projectors", false, HierarchyDesigner_Shared_MenuItems.LayerTwelve + 4)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Select)]
        public static void MenuItem_Select_AllProjectors() => Select_AllComponentOfType<Projector>();

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Select_Type_Effects + "/Select All Visual Effects", false, HierarchyDesigner_Shared_MenuItems.LayerTwelve + 4)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Select)]
        public static void MenuItem_Select_VisualEffects() => Select_AllComponentOfType<UnityEngine.VFX.VisualEffect>();
        #endregion

        #region Lights
        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Select_Type_Light + "/Select All Lights", false, HierarchyDesigner_Shared_MenuItems.LayerEleven + 2)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Select)]
        public static void MenuItem_Select_Lights() => Select_AllComponentOfType<Light>();

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Select_Type_Light + "/Select All Directional Lights", false, HierarchyDesigner_Shared_MenuItems.LayerTwelve + 2)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Select)]
        public static void MenuItem_Select_DirectionalLights() => Select_AllComponentOfType<Light>(light => light.type == LightType.Directional);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Select_Type_Light + "/Select All Point Lights", false, HierarchyDesigner_Shared_MenuItems.LayerTwelve + 2)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Select)]
        public static void MenuItem_Select_PointLights() => Select_AllComponentOfType<Light>(light => light.type == LightType.Point);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Select_Type_Light + "/Select All Spot Lights", false, HierarchyDesigner_Shared_MenuItems.LayerTwelve + 2)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Select)]
        public static void MenuItem_Select_SpotLights() => Select_AllComponentOfType<Light>(light => light.type == LightType.Spot);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Select_Type_Light + "/Select All Rectangle Area Lights", false, HierarchyDesigner_Shared_MenuItems.LayerTwelve + 3)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Select)]
        public static void MenuItem_Select_RectangleAreaLights() => Select_AllComponentOfType<Light>(light => light.type == LightType.Rectangle);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Select_Type_Light + "/Select All Disc Area Lights", false, HierarchyDesigner_Shared_MenuItems.LayerTwelve + 4)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Select)]
        public static void MenuItem_Select_DiscAreaLights() => Select_AllComponentOfType<Light>(light => light.type == LightType.Disc);

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Select_Type_Light + "/Select All Reflection Probes", false, HierarchyDesigner_Shared_MenuItems.LayerThirteen + 4)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Select)]
        public static void MenuItem_Select_ReflectionProbes() => Select_AllComponentOfType<ReflectionProbe>();

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Select_Type_Light + "/Select All Light Probe Groups", false, HierarchyDesigner_Shared_MenuItems.LayerThirteen + 5)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Select)]
        public static void MenuItem_Select_LightProbeGroups() => Select_AllComponentOfType<LightProbeGroup>();

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Select_Type_Light + "/Select All Light Probe Proxy Volumes", false, HierarchyDesigner_Shared_MenuItems.LayerThirteen + 5)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Select)]
        public static void MenuItem_Select_LightProbeProxyVolumes() => Select_AllComponentOfType<LightProbeProxyVolume>();
        #endregion

        #region Video
        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Select_Type_Video + "/Select All Video Players", false, HierarchyDesigner_Shared_MenuItems.LayerEleven + 2)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Select)]
        public static void MenuItem_Select_VideoPlayers() => Select_AllComponentOfType<UnityEngine.Video.VideoPlayer>();
        #endregion

        #region UI Toolkit
        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Select_Type_UIToolkit + "/Select All UI Documents", false, HierarchyDesigner_Shared_MenuItems.LayerEleven + 3)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Select)]
        public static void MenuItem_Select_UIDocuments() => Select_AllComponentOfType<UnityEngine.UIElements.UIDocument>();

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Select_Type_UIToolkit + "/Select All Panel Event Handlers", false, HierarchyDesigner_Shared_MenuItems.LayerTwelve + 3)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Select)]
        public static void MenuItem_Select_PanelEventHandlers() => Select_AllComponentOfType<UnityEngine.UIElements.PanelEventHandler>();

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Select_Type_UIToolkit + "/Select All Panel Raycasters", false, HierarchyDesigner_Shared_MenuItems.LayerTwelve + 3)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Select)]
        public static void MenuItem_Select_PanelRaycasters() => Select_AllComponentOfType<UnityEngine.UIElements.PanelRaycaster>();
        #endregion

        #region Cameras
        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Select_Type + "/Select All Cameras", false, HierarchyDesigner_Shared_MenuItems.LayerEleven + 4)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Select)]
        public static void MenuItem_Select_Cameras() => Select_AllComponentOfType<Camera>();
        #endregion

        #region UI
        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Select_Type_UI + "/Select All Images", false, HierarchyDesigner_Shared_MenuItems.LayerEleven + 4)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Select)]
        public static void MenuItem_Select_Images() => Select_AllComponentOfType<Image>();

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Select_Type_UI + "/Select All Texts - TextMeshPro", false, HierarchyDesigner_Shared_MenuItems.LayerEleven + 4)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Select)]
        public static void MenuItem_Select_TextMeshPro() => Select_AllTMPComponentIfAvailable<TMPro.TMP_Text>();

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Select_Type_UI + "/Select All Raw Images", false, HierarchyDesigner_Shared_MenuItems.LayerEleven + 5)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Select)]
        public static void MenuItem_Select_RawImages() => Select_AllComponentOfType<RawImage>();

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Select_Type_UI + "/Select All Toggles", false, HierarchyDesigner_Shared_MenuItems.LayerTwelve + 5)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Select)]
        public static void MenuItem_Select_Toggles() => Select_AllComponentOfType<Toggle>();

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Select_Type_UI + "/Select All Sliders", false, HierarchyDesigner_Shared_MenuItems.LayerTwelve + 6)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Select)]
        public static void MenuItem_Select_Sliders() => Select_AllComponentOfType<Slider>();

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Select_Type_UI + "/Select All Scrollbars", false, HierarchyDesigner_Shared_MenuItems.LayerTwelve + 7)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Select)]
        public static void MenuItem_Select_Scrollbars() => Select_AllComponentOfType<Scrollbar>();

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Select_Type_UI + "/Select All Scroll Views", false, HierarchyDesigner_Shared_MenuItems.LayerTwelve + 8)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Select)]
        public static void MenuItem_Select_ScrollViews() => Select_AllComponentOfType<ScrollRect>();

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Select_Type_UI + "/Select All Dropdowns - TextMeshPro", false, HierarchyDesigner_Shared_MenuItems.LayerTwelve + 9)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Select)]
        public static void MenuItem_Select_DropdownTextMeshPro() => Select_AllTMPComponentIfAvailable<TMPro.TMP_Dropdown>();

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Select_Type_UI + "/Select All Input Fields - TextMeshPro", false, HierarchyDesigner_Shared_MenuItems.LayerTwelve + 9)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Select)]
        public static void MenuItem_Select_InputFieldTextMeshPro() => Select_AllTMPComponentIfAvailable<TMPro.TMP_InputField>();

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Select_Type_UI + "/Select All Canvases", false, HierarchyDesigner_Shared_MenuItems.LayerThirteen + 9)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Select)]
        public static void MenuItem_Select_Canvases() => Select_AllComponentOfType<Canvas>();

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Select_Type_UI + "/Select All Event Systems", false, HierarchyDesigner_Shared_MenuItems.LayerThirteen + 9)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Select)]
        public static void MenuItem_Select_EventSystems() => Select_AllComponentOfType<UnityEngine.EventSystems.EventSystem>();

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Select_Type_UI_Legacy + "/Select All Texts", false, HierarchyDesigner_Shared_MenuItems.LayerFourteen + 9)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Select)]
        public static void MenuItem_Select_Texts() => Select_AllComponentOfType<Text>();

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Select_Type_UI_Legacy + "/Select All Buttons", false, HierarchyDesigner_Shared_MenuItems.LayerFourteen + 10)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Select)]
        public static void MenuItem_Select_Buttons() => Select_AllComponentOfType<Button>();

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Select_Type_UI_Legacy + "/Select All Dropdowns", false, HierarchyDesigner_Shared_MenuItems.LayerFourteen + 10)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Select)]
        public static void MenuItem_Select_Dropdowns() => Select_AllComponentOfType<Dropdown>();

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Select_Type_UI_Legacy + "/Select All Input Fields", false, HierarchyDesigner_Shared_MenuItems.LayerFourteen + 10)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Select)]
        public static void MenuItem_Select_InputFields() => Select_AllComponentOfType<InputField>();

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Select_Type_UI + "/Select All Masks", false, HierarchyDesigner_Shared_MenuItems.LayerFifteen + 10)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Select)]
        public static void MenuItem_Select_Masks() => Select_AllComponentOfType<Mask>();

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Select_Type_UI + "/Select All Rect Masks 2D", false, HierarchyDesigner_Shared_MenuItems.LayerFifteen + 10)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Select)]
        public static void MenuItem_Select_RectMasks2D() => Select_AllComponentOfType<RectMask2D>();

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Select_Type_UI + "/Select All Selectables", false, HierarchyDesigner_Shared_MenuItems.LayerFifteen + 10)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Select)]
        public static void MenuItem_Select_Selectables() => Select_AllComponentOfType<Selectable>();

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Select_Type_UI + "/Select All Toggle Groups", false, HierarchyDesigner_Shared_MenuItems.LayerFifteen + 10)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Select)]
        public static void MenuItem_Select_ToggleGroups() => Select_AllComponentOfType<ToggleGroup>();

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Select_Type_UI_Effects + "/Select All Outlines", false, HierarchyDesigner_Shared_MenuItems.LayerSixteen + 10)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Select)]
        public static void MenuItem_Select_Outlines() => Select_AllComponentOfType<Outline>();

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Select_Type_UI_Effects + "/Select All Positions As UV1", false, HierarchyDesigner_Shared_MenuItems.LayerSixteen + 10)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Select)]
        public static void MenuItem_Select_PositionsAsUV1() => Select_AllComponentOfType<PositionAsUV1>();

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Select_Type_UI_Effects + "/Select All Shadows", false, HierarchyDesigner_Shared_MenuItems.LayerSixteen + 10)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Select)]
        public static void MenuItem_Select_Shadows() => Select_AllComponentOfType<Shadow>();
        #endregion
        #endregion
        #endregion

        #region Sort
        #region General
        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Sort + "/Sort Alphabetically Ascending", false, HierarchyDesigner_Shared_MenuItems.LayerSeven)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Sort)]
        public static void MenuItem_Sort_AlphabeticallyAscending() => Sort_GameObjectChildren(AlphanumericComparison, "sort its children alphabetically ascending");

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Sort + "/Sort Alphabetically Descending", false, HierarchyDesigner_Shared_MenuItems.LayerSeven)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Sort)]
        public static void MenuItem_Sort_AlphabeticallyDescending() => Sort_GameObjectChildren((a, b) => -AlphanumericComparison(a, b), "sort its children alphabetically descending");

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Sort + "/Sort Components Amount Ascending", false, HierarchyDesigner_Shared_MenuItems.LayerEight)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Sort)]
        public static void MenuItem_Sort_ComponentsAmountAscending() => Sort_GameObjectChildren((a, b) => a.GetComponents<Component>().Length.CompareTo(b.GetComponents<Component>().Length), "sort its children by components amount ascending");

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Sort + "/Sort Components Amount Descending", false, HierarchyDesigner_Shared_MenuItems.LayerEight)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Sort)]
        public static void MenuItem_Sort_ComponentsAmountDescending() => Sort_GameObjectChildren((a, b) => b.GetComponents<Component>().Length.CompareTo(a.GetComponents<Component>().Length), "sort its children by components amount descending");

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Sort + "/Sort Length Ascending", false, HierarchyDesigner_Shared_MenuItems.LayerNine)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Sort)]
        public static void MenuItem_Sort_LengthAscending() => Sort_GameObjectChildren((a, b) => a.name.Length.CompareTo(b.name.Length), "sort its children by length ascending");

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Sort + "/Sort Length Descending", false, HierarchyDesigner_Shared_MenuItems.LayerNine)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Sort)]
        public static void MenuItem_Sort_LengthDescending() => Sort_GameObjectChildren((a, b) => b.name.Length.CompareTo(a.name.Length), "sort its children by length descending");

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Sort + "/Sort Tag Alphabetically Ascending", false, HierarchyDesigner_Shared_MenuItems.LayerTen)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Sort)]
        public static void MenuItem_Sort_TagAlphabeticallyAscending() => Sort_GameObjectChildrenByTag(true, "sort its children by tag ascending");

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Sort + "/Sort Tag Alphabetically Descending", false, HierarchyDesigner_Shared_MenuItems.LayerTen)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Sort)]
        public static void MenuItem_Sort_TagAlphabeticallyDescending() => Sort_GameObjectChildrenByTag(false, "sort its children by tag descending");

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Sort + "/Sort Tag List Order Ascending", false, HierarchyDesigner_Shared_MenuItems.LayerEleven)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Sort)]
        public static void MenuItem_Sort_TagListOrderAscending() => Sort_GameObjectChildrenByTagListOrder(true, "sort its children by tag list order ascending");

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Sort + "/Sort Tag List Order Descending", false, HierarchyDesigner_Shared_MenuItems.LayerEleven)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Sort)]
        public static void MenuItem_Sort_TagListOrderDescending() => Sort_GameObjectChildrenByTagListOrder(false, "sort its children by tag list order descending");

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Sort + "/Sort Layer Alphabetically Ascending", false, HierarchyDesigner_Shared_MenuItems.LayerTwelve)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Sort)]
        public static void MenuItem_Sort_LayerAlphabeticallyAscending() => Sort_GameObjectChildrenByLayer(true, "sort its children by layer alphabetically ascending");

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Sort + "/Sort Layer Alphabetically Descending", false, HierarchyDesigner_Shared_MenuItems.LayerTwelve)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Sort)]
        public static void MenuItem_Sort_LayerAlphabeticallyDescending() => Sort_GameObjectChildrenByLayer(false, "sort its children by layer alphabetically descending");

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Sort + "/Sort Layer List Order Ascending", false, HierarchyDesigner_Shared_MenuItems.LayerThirteen)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Sort)]
        public static void MenuItem_Sort_LayerListOrderAscending() => Sort_GameObjectChildrenByLayerListOrder(true, "sort its children by layer list order ascending");

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Sort + "/Sort Layer List Order Descending", false, HierarchyDesigner_Shared_MenuItems.LayerThirteen)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Sort)]
        public static void MenuItem_Sort_LayerListOrderDescending() => Sort_GameObjectChildrenByLayerListOrder(false, "sort its children by layer list order descending");

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Section_Sort + "/Sort Randomly", false, HierarchyDesigner_Shared_MenuItems.LayerFourteen)]
        [HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools.Sort)]
        public static void MenuItem_Sort_Randomly() => Sort_GameObjectChildrenRandomly("sort its children randomly");
        #endregion
        #endregion
        #endregion

        #region Methods
        #region General Operations
        private static IEnumerable<GameObject> GetAllGameObjectsInActiveScene()
        {
            GameObject[] rootObjects = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
            List<GameObject> allGameObjects = new List<GameObject>();

            Stack<GameObject> stack = new Stack<GameObject>(rootObjects);
            while (stack.Count > 0)
            {
                GameObject current = stack.Pop();
                allGameObjects.Add(current);
                foreach (Transform child in current.transform) { stack.Push(child.gameObject); }
            }
            return allGameObjects;
        }

        private static bool IsGameObjectActive(GameObject gameObject)
        {
            if (gameObject == null) { return false; }
            return gameObject.activeSelf;
        }

        private static bool IsGameObjectParent(GameObject gameObject)
        {
            return gameObject.transform.childCount > 0;
        }

        private static bool IsGameObjectEmpty(GameObject gameObject)
        {
            Component[] components = gameObject.GetComponents<Component>();
            return components.Length == 1 && (components[0] is Transform || components[0] is RectTransform);
        }

        public static bool IsGameObjectLocked(GameObject gameObject)
        {
            if (gameObject == null) { return false; }
            return (gameObject.hideFlags & HideFlags.NotEditable) == HideFlags.NotEditable;
        }

        private static bool IsGameObjectFolder(GameObject gameObject)
        {
            return gameObject.GetComponent<HierarchyDesignerFolder>();
        }

        private static bool IsGameObjectSeparator(GameObject gameObject)
        {
            return gameObject.tag == separatorTag && gameObject.name.StartsWith(separatorPrefix);
        }

        private static bool ShouldIncludeFolderAndSeparator(GameObject gameObject, bool addFolderToCount, bool addSeparatorToCount)
        {
            if (!addFolderToCount && IsGameObjectFolder(gameObject)) { return false; }
            if (!addSeparatorToCount && IsGameObjectSeparator(gameObject)) { return false; }
            return true;
        }
        #endregion

        #region Activate Category
        #region Operations
        private static void SetActiveState(GameObject gameObject, bool isActive)
        {
            if (IsGameObjectSeparator(gameObject)) return;
            Undo.RegisterCompleteObjectUndo(gameObject, $"{(isActive ? "Activate" : "Deactivate")} GameObject");

            if (gameObject.activeSelf != isActive)
            {
                gameObject.SetActive(isActive);
                EditorUtility.SetDirty(gameObject);
            }
        }
        #endregion

        #region Menu Items
        private static void Activate_SelectedGameObjects(bool activeState)
        {
            GameObject[] selectedGameObjects = Selection.gameObjects;
            if (selectedGameObjects.Length < 1) { Debug.Log($"No GameObject(s) selected. Please select at least one GameObject to {(activeState ? "activate" : "deactivate")}."); return; }

            foreach (GameObject gameObject in selectedGameObjects)
            {
                if (IsGameObjectActive(gameObject) != activeState) { SetActiveState(gameObject, activeState); }
            }
        }

        private static void Activate_AllGameObjects(bool activeState)
        {
            foreach (GameObject gameObject in GetAllGameObjectsInActiveScene())
            {
                SetActiveState(gameObject, activeState);
            }
        }

        private static void Activate_AllParentGameObjects(bool activeState)
        {
            foreach (GameObject gameObject in GetAllGameObjectsInActiveScene())
            {
                if (IsGameObjectParent(gameObject)) { SetActiveState(gameObject, activeState); }
            }
        }

        private static void Activate_AllEmptyGameObjects(bool activeState)
        {
            foreach (GameObject gameObject in GetAllGameObjectsInActiveScene())
            {
                if (IsGameObjectEmpty(gameObject)) { SetActiveState(gameObject, activeState); }
            }
        }

        private static void Activate_AllLockedGameObjects(bool activeState)
        {
            foreach (GameObject gameObject in GetAllGameObjectsInActiveScene())
            {
                if (IsGameObjectLocked(gameObject)) { SetActiveState(gameObject, activeState); }
            }
        }

        private static void Activate_AllFolders(bool activeState)
        {
            foreach (GameObject gameObject in GetAllGameObjectsInActiveScene())
            {
                if (IsGameObjectFolder(gameObject)) { SetActiveState(gameObject, activeState); }
            }
        }

        private static void Activate_AllComponentOfType<T>(bool isActive) where T : Component
        {
            IEnumerable<GameObject> allGameObjects = GetAllGameObjectsInActiveScene();
            foreach (GameObject gameObject in allGameObjects)
            {
                if (gameObject.GetComponent<T>() != null)
                {
                    SetActiveState(gameObject, isActive);
                }
            }
        }

        private static void Activate_All2DSpritesByType(string spriteType, bool isActive)
        {
            IEnumerable<GameObject> allGameObjects = GetAllGameObjectsInActiveScene();
            foreach (GameObject gameObject in allGameObjects)
            {
                SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
                if (spriteRenderer != null && spriteRenderer.sprite != null && spriteRenderer.sprite.name.Contains(spriteType))
                {
                    SetActiveState(gameObject, isActive);
                }
            }
        }

        private static void Activate_AllPhysicsDynamicSprites(bool isActive)
        {
            IEnumerable<GameObject> allGameObjects = GetAllGameObjectsInActiveScene();
            foreach (GameObject gameObject in allGameObjects)
            {
                SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
                Rigidbody2D rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
                if (spriteRenderer != null && rigidbody2D != null && rigidbody2D.bodyType == RigidbodyType2D.Dynamic)
                {
                    SetActiveState(gameObject, isActive);
                }
            }
        }

        private static void Activate_AllPhysicsStaticSprites(bool isActive)
        {
            IEnumerable<GameObject> allGameObjects = GetAllGameObjectsInActiveScene();
            foreach (GameObject gameObject in allGameObjects)
            {
                SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
                Rigidbody2D rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
                if (spriteRenderer != null && rigidbody2D != null && (rigidbody2D.bodyType == RigidbodyType2D.Static || rigidbody2D.bodyType == RigidbodyType2D.Kinematic))
                {
                    SetActiveState(gameObject, isActive);
                }
            }
        }

        private static void Activate_All3DObjectsByType(string objectType, bool isActive)
        {
            IEnumerable<GameObject> allGameObjects = GetAllGameObjectsInActiveScene();
            foreach (GameObject gameObject in allGameObjects)
            {
                MeshFilter meshFilter = gameObject.GetComponent<MeshFilter>();
                if (meshFilter != null && meshFilter.sharedMesh != null && meshFilter.sharedMesh.name.Contains(objectType))
                {
                    SetActiveState(gameObject, isActive);
                }
            }
        }

        private static void Activate_AllHalos(bool isActive)
        {
            IEnumerable<GameObject> allGameObjects = GetAllGameObjectsInActiveScene();
            foreach (GameObject gameObject in allGameObjects)
            {
                Behaviour halo = (Behaviour)gameObject.GetComponent("Halo");
                if (halo != null)
                {
                    SetActiveState(gameObject, isActive);
                }
            }
        }

        private static void Activate_AllComponentOfType<T>(bool isActive, Func<T, bool> predicate = null) where T : Component
        {
            IEnumerable<GameObject> allGameObjects = GetAllGameObjectsInActiveScene();
            foreach (GameObject gameObject in allGameObjects)
            {
                T component = gameObject.GetComponent<T>();
                if (component != null && (predicate == null || predicate(component)))
                {
                    SetActiveState(gameObject, isActive);
                }
            }
        }

        private static void Activate_AllTMPComponentIfAvailable<T>(bool isActive) where T : Component
        {
            if (IsTMPAvailable())
            {
                Activate_AllComponentOfType<T>(isActive);
            }
            else
            {
                EditorUtility.DisplayDialog("TMP Not Found", "TMP wasn't found in the project, make sure you have it enabled.", "OK");
            }
        }
        #endregion
        #endregion

        #region Count Category
        #region Operations
        private static void CountGameObjects(string description, Func<GameObject, bool> predicate, bool includeFolders = false, bool includeSeparators = false)
        {
            IEnumerable<GameObject> allGameObjects = GetAllGameObjectsInActiveScene();
            int count = 0;
            string gameObjectNames = "";
            bool addFolderToCount = includeFolders || !HierarchyDesigner_Configurable_AdvancedSettings.ExcludeFoldersFromCountSelectToolCalculations;
            bool addSeparatorToCount = includeSeparators || !HierarchyDesigner_Configurable_AdvancedSettings.ExcludeSeparatorsFromCountSelectToolCalculations;

            foreach (GameObject gameObject in allGameObjects)
            {
                if (!ShouldIncludeFolderAndSeparator(gameObject, addFolderToCount, addSeparatorToCount)) { continue; }
                if (predicate(gameObject))
                {
                    count++;
                    gameObjectNames += gameObject.name + ", ";
                }
            }
            gameObjectNames = count == 0 ? "none" : gameObjectNames.TrimEnd(',', ' ');
            Debug.Log($"Total <color=#73FF7A>{description}</color> in the scene: <b>{count}</b>\n<i>All {description} found:</i> <b>{gameObjectNames}</b>.\n");
        }
        #endregion

        #region Menu Items
        private static void Count_SelectedGameObjects()
        {
            GameObject[] selectedGameObjects = Selection.gameObjects;
            if (selectedGameObjects.Length < 1) { Debug.Log($"No GameObject(s) selected. Please select at least one GameObject to count."); return; }

            int count = selectedGameObjects.Length;
            string selectedGameObjectNames = "";
            for (int i = 0; i < selectedGameObjects.Length; i++)
            {
                selectedGameObjectNames += selectedGameObjects[i].name;
                if (i < selectedGameObjects.Length - 1)
                {
                    selectedGameObjectNames += ", ";
                }
            }
            Debug.Log($"Total <color=#73FF7A>Selected GameObjects</color> in the scene: <b>{count}</b>\n<i>All <Selected GameObjects> found:</i> <b>{selectedGameObjectNames}</b>.\n");
        }

        private static void Count_AllGameObjects()
        {
            CountGameObjects("GameObjects", gameObject => true);
        }

        private static void Count_AllParentGameObjects()
        {
            CountGameObjects("Parent GameObjects", IsGameObjectParent);
        }

        private static void Count_AllEmptyGameObjects()
        {
            CountGameObjects("Empty GameObjects", IsGameObjectEmpty);
        }

        private static void Count_AllLockedGameObjects()
        {
            CountGameObjects("Locked GameObjects", IsGameObjectLocked);
        }

        private static void Count_AllActiveGameObjects()
        {
            CountGameObjects("Active GameObjects", IsGameObjectActive);
        }

        private static void Count_AllInactiveGameObjects()
        {
            CountGameObjects("Inactive GameObjects", gameObject => !IsGameObjectActive(gameObject));
        }

        private static void Count_AllFolders()
        {
            CountGameObjects("Folders", IsGameObjectFolder, includeFolders: true);
        }

        private static void Count_AllSeparators()
        {
            CountGameObjects("Separators", IsGameObjectSeparator, includeSeparators: true);
        }

        private static void Count_AllComponentOfType<T>(string componentName) where T : Component
        {
            CountGameObjects($"{componentName}", gameObject => gameObject.GetComponent<T>() != null);
        }

        private static void Count_All2DSpritesByType(string spriteType)
        {
            CountGameObjects($"2D {spriteType} sprites", gameObject =>
            {
                SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
                return spriteRenderer != null && spriteRenderer.sprite != null && spriteRenderer.sprite.name.Contains(spriteType);
            });
        }

        private static void Count_AllPhysicsDynamicSprites()
        {
            CountGameObjects("2D Dynamic physics sprites", gameObject =>
            {
                SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
                Rigidbody2D rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
                return spriteRenderer != null && rigidbody2D != null && rigidbody2D.bodyType == RigidbodyType2D.Dynamic;
            });
        }

        private static void Count_AllPhysicsStaticSprites()
        {
            CountGameObjects("2D Static physics sprites", gameObject =>
            {
                SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
                Rigidbody2D rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
                return spriteRenderer != null && rigidbody2D != null && (rigidbody2D.bodyType == RigidbodyType2D.Static || rigidbody2D.bodyType == RigidbodyType2D.Kinematic);
            });
        }

        private static void Count_All3DObjectsByType(string objectType)
        {
            CountGameObjects($"3D {objectType} objects", gameObject =>
            {
                MeshFilter meshFilter = gameObject.GetComponent<MeshFilter>();
                return meshFilter != null && meshFilter.sharedMesh != null && meshFilter.sharedMesh.name.Contains(objectType);
            });
        }

        private static void Count_AllHalos()
        {
            CountGameObjects("Halos", gameObject =>
            {
                Behaviour halo = (Behaviour)gameObject.GetComponent("Halo");
                return halo != null;
            });
        }

        private static void Count_AllComponentOfType<T>(string componentName, Func<T, bool> predicate = null) where T : Component
        {
            CountGameObjects($"{componentName}", gameObject =>
            {
                T component = gameObject.GetComponent<T>();
                return component != null && (predicate == null || predicate(component));
            });
        }

        private static void Count_AllTMPComponentIfAvailable<T>(string componentName) where T : Component
        {
            if (IsTMPAvailable())
            {
                Count_AllComponentOfType<T>(componentName);
            }
            else
            {
                EditorUtility.DisplayDialog("TMP Not Found", "TMP wasn't found in the project, make sure you have it enabled.", "OK");
            }
        }
        #endregion
        #endregion

        #region Lock Category
        #region Public
        public static void LockGameObject(GameObject gameObject, bool isLocked)
        {
            SetLockState(gameObject, !isLocked);
        }

        public static void SetLockState(GameObject gameObject, bool editable)
        {
            if (IsGameObjectSeparator(gameObject)) return;
            Undo.RegisterCompleteObjectUndo(gameObject, $"{(editable ? "Unlock" : "Lock")} GameObject");
            HideFlags newFlags = editable ? HideFlags.None : HideFlags.NotEditable;
            if (gameObject.hideFlags != newFlags)
            {
                gameObject.hideFlags = newFlags;
                EditorUtility.SetDirty(gameObject);
            }

            foreach (Component component in gameObject.GetComponents<Component>())
            {
                if (component && component.hideFlags != newFlags)
                {
                    component.hideFlags = newFlags;
                    EditorUtility.SetDirty(component);
                }
            }

            if (editable) { SceneVisibilityManager.instance.EnablePicking(gameObject, true); }
            else { SceneVisibilityManager.instance.DisablePicking(gameObject, true); }

            EditorWindow[] allEditorWindows = Resources.FindObjectsOfTypeAll<EditorWindow>();
            foreach (EditorWindow inspector in allEditorWindows)
            {
                if (inspector.GetType().Name == inspectorWindow)
                {
                    inspector.Repaint();
                }
            }
        }
        #endregion

        #region Menu Items
        private static void Lock_SelectedGameObjects(bool lockState)
        {
            GameObject[] selectedGameObjects = Selection.gameObjects;
            if (selectedGameObjects.Length < 1) { Debug.Log($"No GameObject(s) selected. Please select at least one GameObject to {(lockState ? "lock" : "unlock")}."); return; }
            
            foreach (GameObject gameObject in selectedGameObjects)
            {
                if (IsGameObjectLocked(gameObject) != lockState) { SetLockState(gameObject, !lockState); }
            }
        }

        private static void Lock_AllGameObjects(bool lockState)
        {
            foreach (GameObject gameObject in GetAllGameObjectsInActiveScene()) 
            { 
                SetLockState(gameObject, !lockState); 
            }
        }

        private static void Lock_AllParentGameObjects(bool lockState)
        {
            foreach (GameObject gameObject in GetAllGameObjectsInActiveScene()) 
            { 
                if (IsGameObjectParent(gameObject)) { SetLockState(gameObject, !lockState); }
            }
        }

        private static void Lock_AllEmptyGameObjects(bool lockState)
        {
            foreach (GameObject gameObject in GetAllGameObjectsInActiveScene())
            {
                if (IsGameObjectEmpty(gameObject)) { SetLockState(gameObject, !lockState); }
            }
        }

        private static void Lock_AllActiveGameObjects(bool lockState)
        {
            foreach (GameObject gameObject in GetAllGameObjectsInActiveScene())
            {
                if (IsGameObjectActive(gameObject)) { SetLockState(gameObject, !lockState); }
            }
        }

        private static void Lock_AllInactiveGameObjects(bool lockState)
        {
            foreach (GameObject gameObject in GetAllGameObjectsInActiveScene())
            {
                if (!IsGameObjectActive(gameObject)) { SetLockState(gameObject, !lockState); }
            }
        }

        private static void Lock_AllFolders(bool lockState)
        {
            foreach (GameObject gameObject in GetAllGameObjectsInActiveScene())
            {
                if (IsGameObjectFolder(gameObject)) { SetLockState(gameObject, !lockState); }
            }
        }

        private static void Lock_AllComponentOfType<T>(bool isLocked) where T : Component
        {
            IEnumerable<GameObject> allGameObjects = GetAllGameObjectsInActiveScene();
            foreach (GameObject gameObject in allGameObjects)
            {
                if (gameObject.GetComponent<T>() != null)
                {
                    LockGameObject(gameObject, isLocked);
                }
            }
        }

        private static void Lock_All2DSpritesByType(string spriteType, bool isLocked)
        {
            IEnumerable<GameObject> allGameObjects = GetAllGameObjectsInActiveScene();
            foreach (GameObject gameObject in allGameObjects)
            {
                SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
                if (spriteRenderer != null && spriteRenderer.sprite != null && spriteRenderer.sprite.name.Contains(spriteType))
                {
                    LockGameObject(gameObject, isLocked);
                }
            }
        }

        private static void Lock_AllPhysicsDynamicSprites(bool isLocked)
        {
            IEnumerable<GameObject> allGameObjects = GetAllGameObjectsInActiveScene();
            foreach (GameObject gameObject in allGameObjects)
            {
                SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
                Rigidbody2D rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
                if (spriteRenderer != null && rigidbody2D != null && rigidbody2D.bodyType == RigidbodyType2D.Dynamic)
                {
                    LockGameObject(gameObject, isLocked);
                }
            }
        }

        private static void Lock_AllPhysicsStaticSprites(bool isLocked)
        {
            IEnumerable<GameObject> allGameObjects = GetAllGameObjectsInActiveScene();
            foreach (GameObject gameObject in allGameObjects)
            {
                SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
                Rigidbody2D rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
                if (spriteRenderer != null && rigidbody2D != null && (rigidbody2D.bodyType == RigidbodyType2D.Static || rigidbody2D.bodyType == RigidbodyType2D.Kinematic))
                {
                    LockGameObject(gameObject, isLocked);
                }
            }
        }

        private static void Lock_All3DObjectsByType(string objectType, bool isLocked)
        {
            IEnumerable<GameObject> allGameObjects = GetAllGameObjectsInActiveScene();
            foreach (GameObject gameObject in allGameObjects)
            {
                MeshFilter meshFilter = gameObject.GetComponent<MeshFilter>();
                if (meshFilter != null && meshFilter.sharedMesh != null && meshFilter.sharedMesh.name.Contains(objectType))
                {
                    LockGameObject(gameObject, isLocked);
                }
            }
        }

        private static void Lock_AllHalos(bool isLocked)
        {
            IEnumerable<GameObject> allGameObjects = GetAllGameObjectsInActiveScene();
            foreach (GameObject gameObject in allGameObjects)
            {
                Behaviour halo = (Behaviour)gameObject.GetComponent("Halo");
                if (halo != null)
                {
                    LockGameObject(gameObject, isLocked);
                }
            }
        }

        private static void Lock_AllComponentOfType<T>(bool isLocked, Func<T, bool> predicate = null) where T : Component
        {
            IEnumerable<GameObject> allGameObjects = GetAllGameObjectsInActiveScene();
            foreach (GameObject gameObject in allGameObjects)
            {
                T component = gameObject.GetComponent<T>();
                if (component != null && (predicate == null || predicate(component)))
                {
                    LockGameObject(gameObject, isLocked);
                }
            }
        }

        private static void Lock_AllTMPComponentIfAvailable<T>(bool isLocked) where T : Component
        {
            if (IsTMPAvailable())
            {
                Lock_AllComponentOfType<T>(isLocked);
            }
            else
            {
                EditorUtility.DisplayDialog("TMP Not Found", "TMP wasn't found in the project, make sure you have it enabled.", "OK");
            }
        }
        #endregion
        #endregion

        #region Rename Category
        #region Menu Items
        private static void Rename_SelectedGameObjects(string sortingActionDescription, bool autoIndex)
        {
            List<GameObject> selectedGameObjects = new List<GameObject>(Selection.gameObjects);
            if (selectedGameObjects.Count < 1) { Debug.Log($"No GameObject(s) selected. Please select at least one GameObject to {sortingActionDescription}."); return; }

            HierarchyDesigner_Window_RenameTool.OpenWindow(selectedGameObjects, autoIndex, 0);
        }

        private static void Rename_OpenRenameToolWindow()
        {
            HierarchyDesigner_Window_RenameTool.OpenWindow(null, true, 0);
        }
        #endregion
        #endregion

        #region Select Category
        #region Operations
        private static void SelectGameObjects(string description, Func<GameObject, bool> predicate, bool includeFolders = false, bool includeSeparators = false)
        {
            IEnumerable<GameObject> allGameObjects = GetAllGameObjectsInActiveScene();
            List<GameObject> selectedGameObjects = new List<GameObject>();
            bool addFolderToSelect = includeFolders || !HierarchyDesigner_Configurable_AdvancedSettings.ExcludeFoldersFromCountSelectToolCalculations;
            bool addSeparatorToSelect = includeSeparators || !HierarchyDesigner_Configurable_AdvancedSettings.ExcludeSeparatorsFromCountSelectToolCalculations;

            foreach (GameObject gameObject in allGameObjects)
            {
                if (!ShouldIncludeFolderAndSeparator(gameObject, addFolderToSelect, addSeparatorToSelect)) { continue; }
                if (predicate(gameObject))
                {
                    selectedGameObjects.Add(gameObject);
                }
            }

            SelectOrDisplayMessage(selectedGameObjects, description);
        }

        private static void SelectOrDisplayMessage(List<GameObject> gameObjects, string message)
        {
            if (gameObjects.Count > 0)
            {
                FocusHierarchyWindow();
                Selection.objects = gameObjects.ToArray();
            }
            else
            {
                Debug.Log($"No {message} found in the current scene.");
            }
        }

        private static void FocusHierarchyWindow()
        {
            EditorApplication.ExecuteMenuItem("Window/General/Hierarchy");
        }
        #endregion

        #region Menu Items
        private static void Select_AllGameObjects()
        {
            SelectGameObjects("GameObjects", gameObject => true);
        }

        private static void Select_AllParentGameObjects()
        {
            SelectGameObjects("Parent GameObjects", IsGameObjectParent);
        }

        private static void Select_AllEmptyGameObjects()
        {
            SelectGameObjects("Empty GameObjects", IsGameObjectEmpty);
        }

        private static void Select_AllLockedGameObjects()
        {
            SelectGameObjects("Locked GameObjects", IsGameObjectLocked);
        }

        private static void Select_AllActiveGameObjects()
        {
            SelectGameObjects("Active GameObjects", IsGameObjectActive);
        }

        private static void Select_AllInactiveGameObjects()
        {
            SelectGameObjects("Inactive GameObjects", gameObject => !IsGameObjectActive(gameObject));
        }

        private static void Select_AllFolders()
        {
            SelectGameObjects("Folders", IsGameObjectFolder, includeFolders: true);
        }

        private static void Select_AllSeparators()
        {
            SelectGameObjects("Separators", IsGameObjectSeparator, includeSeparators: true);
        }

        private static void Select_AllComponentOfType<T>() where T : Component
        {
            SelectGameObjects(typeof(T).Name, gameObject => gameObject.GetComponent<T>() != null);
        }

        private static void Select_All2DSpritesByType(string spriteType)
        {
            SelectGameObjects(spriteType, gameObject =>
            {
                SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
                return spriteRenderer != null && spriteRenderer.sprite != null && spriteRenderer.sprite.name.Contains(spriteType);
            });
        }

        private static void Select_AllPhysicsDynamicSprites()
        {
            SelectGameObjects("Dynamic Sprites", gameObject =>
            {
                SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
                Rigidbody2D rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
                return spriteRenderer != null && rigidbody2D != null && rigidbody2D.bodyType == RigidbodyType2D.Dynamic;
            });
        }

        private static void Select_AllPhysicsStaticSprites()
        {
            SelectGameObjects("Static Sprites", gameObject =>
            {
                SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
                Rigidbody2D rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
                return spriteRenderer != null && rigidbody2D != null && (rigidbody2D.bodyType == RigidbodyType2D.Static || rigidbody2D.bodyType == RigidbodyType2D.Kinematic);
            });
        }

        private static void Select_All3DObjectsByType(string objectType)
        {
            SelectGameObjects(objectType, gameObject =>
            {
                MeshFilter meshFilter = gameObject.GetComponent<MeshFilter>();
                return meshFilter != null && meshFilter.sharedMesh != null && meshFilter.sharedMesh.name.Contains(objectType);
            });
        }

        private static void Select_AllHalos()
        {
            SelectGameObjects("Halos", gameObject =>
            {
                Behaviour halo = (Behaviour)gameObject.GetComponent("Halo");
                return halo != null;
            });
        }

        private static void Select_AllComponentOfType<T>(Func<T, bool> predicate) where T : Component
        {
            SelectGameObjects(typeof(T).Name, gameObject =>
            {
                T component = gameObject.GetComponent<T>();
                return component != null && predicate(component);
            });
        }

        private static void Select_AllTMPComponentIfAvailable<T>() where T : Component
        {
            if (IsTMPAvailable())
            {
                Select_AllComponentOfType<T>();
            }
            else
            {
                EditorUtility.DisplayDialog("TMP Not Found", "TMP wasn't found in the project, make sure you have it enabled.", "OK");
            }
        }
        #endregion
        #endregion

        #region Sort Category
        #region Operations
        private static int AlphanumericComparison(GameObject x, GameObject y)
        {
            string xName = x.name;
            string yName = y.name;

            string[] xParts = System.Text.RegularExpressions.Regex.Split(xName.Replace(" ", ""), "([0-9]+)");
            string[] yParts = System.Text.RegularExpressions.Regex.Split(yName.Replace(" ", ""), "([0-9]+)");

            for (int i = 0; i < Math.Min(xParts.Length, yParts.Length); i++)
            {
                if (int.TryParse(xParts[i], out int xPartNum) && int.TryParse(yParts[i], out int yPartNum))
                {
                    if (xPartNum != yPartNum) return xPartNum.CompareTo(yPartNum);
                }
                else
                {
                    int compareResult = xParts[i].CompareTo(yParts[i]);
                    if (compareResult != 0) return compareResult;
                }
            }
            return xParts.Length.CompareTo(yParts.Length);
        }
        #endregion

        #region Menu Items
        private static void Sort_GameObjectChildren(Comparison<GameObject> comparison, string sortingActionDescription)
        {
            GameObject[] selectedGameObjects = Selection.gameObjects;
            if (selectedGameObjects.Length == 0) { Debug.Log($"No Parent GameObjects selected. Please select at least one Parent GameObject to {sortingActionDescription}."); return; }

            foreach (GameObject selectedGameObject in selectedGameObjects)
            {
                Undo.RegisterCompleteObjectUndo(selectedGameObject.transform, sortingActionDescription);

                List<GameObject> children = new List<GameObject>();
                foreach (Transform child in selectedGameObject.transform)
                {
                    children.Add(child.gameObject);
                }

                children.Sort(comparison);
                for (int i = 0; i < children.Count; i++)
                {
                    children[i].transform.SetSiblingIndex(i);
                }
            }
        }
        
        private static void Sort_GameObjectChildrenByTag(bool ascending, string sortingActionDescription)
        {
            Sort_GameObjectChildren((x, y) =>
            {
                if (ascending) 
                { 
                    return string.Compare(x.tag, y.tag, StringComparison.Ordinal); 
                }
                else 
                { 
                    return string.Compare(y.tag, x.tag, StringComparison.Ordinal); 
                }
            }, sortingActionDescription);
        }

        private static void Sort_GameObjectChildrenByTagListOrder(bool ascending, string sortingActionDescription)
        {
            List<string> allTags = new List<string>(UnityEditorInternal.InternalEditorUtility.tags);
            string[] predefinedOrder = new string[] { "Untagged", "Respawn", "Finish", "EditorOnly", "MainCamera", "Player", "GameController" };
            allTags.Sort((x, y) =>
            {
                int indexX = Array.IndexOf(predefinedOrder, x);
                int indexY = Array.IndexOf(predefinedOrder, y);
                indexX = indexX == -1 ? int.MaxValue : indexX;
                indexY = indexY == -1 ? int.MaxValue : indexY;
                return indexX.CompareTo(indexY);
            });

            Sort_GameObjectChildren((x, y) =>
            {
                int indexX = allTags.IndexOf(x.tag);
                int indexY = allTags.IndexOf(y.tag);
                return ascending ? indexX.CompareTo(indexY) : indexY.CompareTo(indexX);
            }, sortingActionDescription);
        }

        private static void Sort_GameObjectChildrenByLayer(bool ascending, string sortingActionDescription)
        {
            Sort_GameObjectChildren((x, y) =>
            {
                string layerX = LayerMask.LayerToName(x.layer);
                string layerY = LayerMask.LayerToName(y.layer);
                return ascending ? string.Compare(layerX, layerY, StringComparison.Ordinal) : string.Compare(layerY, layerX, StringComparison.Ordinal);
            }, sortingActionDescription);
        }

        private static void Sort_GameObjectChildrenByLayerListOrder(bool ascending, string sortingActionDescription)
        {
            List<string> allLayers = new List<string>();
            for (int i = 0; i < 32; i++)
            {
                string layerName = LayerMask.LayerToName(i);
                if (!string.IsNullOrEmpty(layerName))
                {
                    allLayers.Add(layerName);
                }
            }

            Sort_GameObjectChildren((x, y) =>
            {
                int indexX = allLayers.IndexOf(LayerMask.LayerToName(x.layer));
                int indexY = allLayers.IndexOf(LayerMask.LayerToName(y.layer));
                return ascending ? indexX.CompareTo(indexY) : indexY.CompareTo(indexX);
            }, sortingActionDescription);
        }

        private static void Sort_GameObjectChildrenRandomly(string sortingActionDescription)
        {
            GameObject[] selectedGameObjects = Selection.gameObjects;
            if (selectedGameObjects.Length == 0)
            {
                Debug.Log($"No Parent GameObjects selected. Please select at least one Parent GameObject to {sortingActionDescription}.");
                return;
            }

            foreach (GameObject selectedGameObject in selectedGameObjects)
            {
                Undo.RegisterCompleteObjectUndo(selectedGameObject.transform, sortingActionDescription);

                List<GameObject> children = new List<GameObject>();
                foreach (Transform child in selectedGameObject.transform)
                {
                    children.Add(child.gameObject);
                }

                System.Random rng = new System.Random();
                int n = children.Count;
                while (n > 1)
                {
                    n--;
                    int k = rng.Next(n + 1);
                    GameObject value = children[k];
                    children[k] = children[n];
                    children[n] = value;
                }

                for (int i = 0; i < children.Count; i++)
                {
                    children[i].transform.SetSiblingIndex(i);
                }
            }
        }
        #endregion
        #endregion
        #endregion
    }
}
#endif