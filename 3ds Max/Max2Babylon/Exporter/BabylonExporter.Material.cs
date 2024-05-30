using System;
using System.Linq;
using System.Collections.Generic;
using Autodesk.Max;
using Utilities;
using BabylonExport.Entities;
using System.Drawing;
using System.Runtime.ConstrainedExecution;
using System.Diagnostics;
using Autodesk.Max.Plugins;

namespace BabylonExport.Entities
{
    partial class BabylonMaterial
    {
        public IIGameMaterial maxGameMaterial { get; set; }
    }
}

namespace Max2Babylon
{
    /// <summary>
    /// This is the base class used to decorate IIGameMaterial in order to access properties.
    /// </summary>
    public class MaterialDecorator
    {
        protected IIGameMaterial _node;
        // add temporary cache to optimize the map access.
        IDictionary<string, ITexmap> _mapCaches;


        public MaterialDecorator(IIGameMaterial node)
        {
            _node = node ?? throw new ArgumentNullException(nameof(node));
        }

        public IIPropertyContainer Properties => _node.IPropertyContainer;
        public string Id => _node.MaxMaterial.GetGuid().ToString();
        public string Name => _node.MaterialName;
        public IIGameMaterial Node => _node;

        protected ITexmap _getTexMapWithCache(IIGameMaterial materialNode, string name)
        {
            var materialName = materialNode.MaterialName;
            if (_mapCaches == null)
            {
                _mapCaches = new Dictionary<string, ITexmap>();
                for (int i = 0; i < materialNode.MaxMaterial.NumSubTexmaps; i++)
                {
                    // according to https://help.autodesk.com/view/MAXDEV/2022/ENU/?guid=Max_Developer_Help_what_s_new_whats_new_3dsmax_2022_sdk_localization_html
#if MAX2022 || MAX2023 ||MAX2024
                    var mn = materialNode.MaxMaterial.GetSubTexmapSlotName(i, false); // Non localized, then en-US
#else
                    var mn = materialNode.MaxMaterial.GetSubTexmapSlotName(i); // en-US
#endif
                    _mapCaches.Add(mn, materialNode.MaxMaterial.GetSubTexmap(i));
                }
            }
            if (_mapCaches.TryGetValue(name, out ITexmap texmap))
            {
                return texmap;
            }

            // sometime the name of the material changed - plugin side effect with script material
            // so give a second chance with lowercase to be more resilient
            if (_mapCaches.TryGetValue(name.ToLower(), out texmap))
            {
                return texmap;
            }

            // max 2022 maj introduce a change into the naming of the map.
            // the SDK do not return the name of the map anymore but a display name with camel style and space
            // Here a fix which maintain the old style and transform the name for a second try if failed.
            name = string.Join(" ", name.Split('_').Select(s => char.ToUpper(s[0]) + s.Substring(1)));
            return _mapCaches.TryGetValue(name, out texmap) ? texmap : null;
        }

        protected ITexmap _getTexMap(IIGameMaterial materialNode, string name, bool cache = true)
        {
            if (cache)
            {
                return _getTexMapWithCache(materialNode, name);
            }

            for (int k = 0; k < 2; k++)
            {
                for (int i = 0; i < materialNode.MaxMaterial.NumSubTexmaps; i++)
                {
#if MAX2024
                    // 3dsMax2024+ introduced new localized flag
                    var slotName = materialNode.MaxMaterial.GetSubTexmapSlotName(i, false);
#else
                    var slotName = materialNode.MaxMaterial.GetSubTexmapSlotName(i);
#endif

                    // sometime the name of the material changed - plugin side effect with script material
                    // so give a second chance with lowercase to be more resilient
                    if (slotName == name || slotName == name.ToLower())
                    {
                        return materialNode.MaxMaterial.GetSubTexmap(i);
                    }
                }
                // max 2022 maj introduce a change into the naming of the map.
                // the SDK do not return the name of the map anymore but a display name with camel style and space
                // Here a fix which maintain the old style and transform the name for a second try if failed.
                name = string.Join(" ", name.Split('_').Select(s => char.ToUpper(s[0]) + s.Substring(1)));
            }
            return null;
        }

    }


    /// <summary>
    /// Babylon custom attribute decorator
    /// </summary>
    public class BabylonCustomAttributeDecorator : MaterialDecorator
    {
        public const string UnlitStr = "babylonUnlit";
        public const string BackfaceCullingStr = "babylonBackfaceCulling";
        public const string SepCullingPassStr = "babylonSeparateCullingPass";
        public const string MaxSimultaneousLightsStr = "babylonMaxSimultaneousLights";
        public const string UseFactorsStr = "babylonUseFactors";
        public const string DirectIntensityStr = "babylonDirectIntensity";
        public const string EmissiveIntensityStr = "babylonEmissiveIntensity";
        public const string EnvironmentIntensityStr = "babylonEnvironmentIntensity";
        public const string SpecularIntensityStr = "babylonSpecularIntensity";
        public const string TransparencyModeStr = "babylonTransparencyMode";

        public static IEnumerable<string> ListPrivatePropertyNames()
        {
            yield return UnlitStr;
            yield return BackfaceCullingStr;
            yield return MaxSimultaneousLightsStr;
            yield return UseFactorsStr;
            yield return DirectIntensityStr;
            yield return EmissiveIntensityStr;
            yield return EnvironmentIntensityStr;
            yield return SpecularIntensityStr;
            yield return TransparencyModeStr;
        }

        public BabylonCustomAttributeDecorator(IIGameMaterial node) : base(node)
        {
        }

        public bool IsUnlit => Properties?.GetBoolProperty(UnlitStr, false) ?? false;
        public bool BackFaceCulling => Properties?.GetBoolProperty(BackfaceCullingStr, true) ?? true;
        public bool SeparateCullingPass => Properties?.GetBoolProperty(SepCullingPassStr, false) ?? false;
        public int MaxSimultaneousLights => Properties?.GetIntProperty(MaxSimultaneousLightsStr, 4) ?? 4;
        public bool UseMaxFactor => Properties?.GetBoolProperty(UseFactorsStr, true) ?? true;
        public float DirectIntensity => Properties?.GetFloatProperty(DirectIntensityStr, 1.0f) ?? 1.0f;
        public float EmissiveIntensity => Properties?.GetFloatProperty(EmissiveIntensityStr, 1.0f) ?? 1.0f;
        public float EnvironementIntensity => Properties?.GetFloatProperty(EnvironmentIntensityStr, 1.0f) ?? 1.0f;
        public float SpecularIntensity => Properties?.GetFloatProperty(SpecularIntensityStr, 1.0f) ?? 1.0f;
        public int TransparencyMode => Properties?.GetIntProperty(TransparencyModeStr, 0) ?? 0;
    }

    /// <summary>
    /// The Exporter
    /// </summary>
    partial class BabylonExporter
    {
        const string MaterialCustomBabylonAttributeName = "Babylon Attributes";

        // use as scale for GetSelfIllumColor to convert [0,PI] to [O,1]
        private const float selfIllumScale = (float)(1.0 / Math.PI);

        readonly List<IIGameMaterial> referencedMaterials = new List<IIGameMaterial>();
        Dictionary<ClassIDWrapper, IMaxMaterialExporter> materialExporters;

        private static int STANDARD_MATERIAL_TEXTURE_ID_DIFFUSE = 1;
        private static int STANDARD_MATERIAL_TEXTURE_ID_OPACITY = 6;

        private void ExportMaterial(IIGameMaterial materialNode, BabylonScene babylonScene)
        {
            var name = materialNode.MaterialName;
            var id = materialNode.MaxMaterial.GetGuid().ToString();

            // Check if the material was already exported. The material id is unique.
            if (babylonScene.MaterialsList.FirstOrDefault(m => m.id == id) != null)
            {
                return;
            }

            RaiseMessage("ExportMaterial:" + name, 1);

            // --- prints ---
            #region prints
            {
                RaiseVerbose("materialNode.MaterialClass=" + materialNode.MaterialClass, 2);
                RaiseVerbose("materialNode.NumberOfTextureMaps=" + materialNode.NumberOfTextureMaps, 2);

                Print(materialNode.IPropertyContainer, 2);
                for (int i = 0; i < materialNode.MaxMaterial.NumSubTexmaps; i++)
                {
#if MAX2024
                    // 3dsMax2024+ introduced new localized flag
                    RaiseVerbose("Texture[" + i + "] is named '" + materialNode.MaxMaterial.GetSubTexmapSlotName(i, false) + "'", 2);
#else
                    RaiseVerbose("Texture[" + i + "] is named '" + materialNode.MaxMaterial.GetSubTexmapSlotName(i) + "'", 2);
#endif
                }
            }
            #endregion

            //RaiseMessage("SubMaterialCount:" + materialNode.SubMaterialCount);

            if (materialNode.SubMaterialCount > 0)
            {
                var babylonMultimaterial = new BabylonMultiMaterial { name = name, id = id };

                var guids = new List<string>();

                for (var index = 0; index < materialNode.SubMaterialCount; index++)
                {
                    var subMat = materialNode.GetSubMaterial(index);

                    if (subMat != null)
                    {
                        //.NL.Gestione dei Corona_Layered_Material
                        //TODO Per adesso prendiamo il primo subMateriale e lo usiamo al posto del Layered_Material
                        while (subMat.SubMaterialCount > 0 && isCoronaLayeredMaterial(subMat))
                        {
                            subMat = subMat.GetSubMaterial(0);
                        }

                        if (subMat.SubMaterialCount > 0)
                        {
                            RaiseError("MultiMaterials as inputs to other MultiMaterials are not supported!");
                        }
                        else
                        {
                            guids.Add(subMat.MaxMaterial.GetGuid().ToString());

                            if (!referencedMaterials.Contains(subMat))
                            {
                                referencedMaterials.Add(subMat);
                                ExportMaterial(subMat, babylonScene);
                            }
                        }
                    }
                    else
                    {
                        guids.Add(Guid.Empty.ToString());
                    }
                }

                babylonMultimaterial.materials = guids.ToArray();

                babylonScene.MultiMaterialsList.Add(babylonMultimaterial);
                return;
            }

            // Retreive Attributes container
            IIPropertyContainer attributesContainer = materialNode.IPropertyContainer;

            bool isUnlit = attributesContainer?.GetBoolProperty("babylonUnlit", false) ?? false;

            // check custom exporters first, to allow custom exporters of supported material classes
            materialExporters.TryGetValue(new ClassIDWrapper(materialNode.MaxMaterial.ClassID), out IMaxMaterialExporter materialExporter);

            IStdMat2 stdMat = null;
            IMtl maxMaterial = materialNode.MaxMaterial;

            if (maxMaterial != null && maxMaterial.NumParamBlocks > 0)
            {
                var paramBlock = maxMaterial.GetParamBlock(0);
                if (paramBlock != null && paramBlock.Owner != null)
                {
                    stdMat = maxMaterial.GetParamBlock(0).Owner as IStdMat2;
                }
            }

            RaiseVerbose("maxMaterial: " + (maxMaterial != null), 2);
            RaiseVerbose("stdMat: " + (stdMat != null), 2);
            RaiseVerbose("isBabylonExported: " + isBabylonExported, 2);
            RaiseVerbose("isGltfExported: " + isGltfExported, 2);

            if (isBabylonExported && materialExporter != null && materialExporter is IMaxBabylonMaterialExporter)
            {
                RaiseVerbose("IN babylonMaterialExporter", 2);

                IMaxBabylonMaterialExporter babylonMaterialExporter = materialExporter as IMaxBabylonMaterialExporter;
                BabylonMaterial babylonMaterial = babylonMaterialExporter.ExportBabylonMaterial(materialNode);
                if (babylonMaterial == null)
                {
                    string message = string.Format("Custom Babylon material exporter failed to export | Exporter: '{0}' | Material Name: '{1}' | Material Class: '{2}'",
                        babylonMaterialExporter.GetType().ToString(), materialNode.MaterialName, materialNode.MaterialClass);
                    RaiseWarning(message, 2);
                }
                else
                {
                    RaiseVerbose("1st Case: " + babylonMaterial.name, 2);
                    babylonScene.MaterialsList.Add(babylonMaterial);
                }
            }
            else if (isGltfExported && materialExporter != null && materialExporter is IMaxGLTFMaterialExporter)
            {
                RaiseVerbose("IN BabylonMaterial", 2);

                // add a basic babylon material to the list to forward the max material reference
                var babylonMaterial = new BabylonMaterial(id)
                {
                    maxGameMaterial = materialNode,
                    name = name
                };
                RaiseVerbose("2nd Case: " + babylonMaterial.name, 2);
                babylonScene.MaterialsList.Add(babylonMaterial);
            }
            else if (stdMat != null)
            {
                RaiseVerbose("IN BabylonStandardMaterial", 2);

                var babylonMaterial = new BabylonStandardMaterial(id)
                {
                    maxGameMaterial = materialNode,
                    name = name,
                    isUnlit = isUnlit,
                    diffuse = materialNode.MaxMaterial.GetDiffuse(0, false).ToArray()
                };
                ExportStandardMaterial(materialNode, attributesContainer, stdMat, babylonScene, babylonMaterial);
                RaiseVerbose("Standard Material: " + babylonMaterial.name, 2);
            }
            else if (isPhysicalMaterial(materialNode))
            {
                RaiseVerbose("IN BabylonPBRMetallicRoughnessMaterial", 2);

                var babylonMaterial = new BabylonPBRMetallicRoughnessMaterial(id)
                {
                    maxGameMaterial = materialNode,
                    name = name,
                    isUnlit = isUnlit
                };
                ExportPhysicalMaterial(materialNode, attributesContainer, babylonScene, babylonMaterial);
                RaiseVerbose("Physical Material: " + babylonMaterial.name, 2);
            }
            else if (isPbrMetalRoughMaterial(materialNode))
            {
                ExportPbrMetalRoughMaterial(materialNode, babylonScene);
                RaiseVerbose("Metal Rough Material", 2);
            }
            else if (isPbrSpecGlossMaterial(materialNode))
            {
                ExportPbrSpecGlossMaterial(materialNode, babylonScene);
                RaiseVerbose("Spec Gloss Material", 2);
            }
#if MAX2023 || MAX2024
            else if (isGLTFMaterial(materialNode))
            {
                ExportGLTFMaterial(materialNode, babylonScene);
                RaiseVerbose("GLTF Material", 2);
            }
#endif
            else if (isArnoldMaterial(materialNode))
            {
                RaiseVerbose("IN isArnoldMaterial", 2);

                var babylonMaterial = new BabylonPBRMetallicRoughnessMaterial(id)
                {
                    maxGameMaterial = materialNode,
                    name = name,
                    isUnlit = isUnlit
                };

                ExportArnoldMaterial(materialNode, attributesContainer, babylonScene, babylonMaterial);
                RaiseVerbose("Arnold Material", 2);
            }
            else if (isCoronaPhysicalMaterial(materialNode))
            {
                RaiseVerbose("IN isCoronaPhysicalMaterial", 2);

                var babylonMaterial = new BabylonPBRMetallicRoughnessMaterial(id)
                {
                    maxGameMaterial = materialNode,
                    name = name,
                    isUnlit = isUnlit
                };

                ExportCoronaPhysicalMaterial(materialNode, attributesContainer, babylonScene, babylonMaterial);
                RaiseVerbose("CoronaPhysical Material", 2);
            }
            else if (isCoronaLayeredMaterial(materialNode))
            {
                RaiseVerbose("IN isCoronaLayeredMaterial", 2);

                var babylonMaterial = new BabylonPBRMetallicRoughnessMaterial(id)
                {
                    maxGameMaterial = materialNode,
                    name = name,
                    isUnlit = isUnlit
                };

                ExportCoronaLayeredMaterial(materialNode, attributesContainer, babylonScene, babylonMaterial);
                RaiseVerbose("CoronaLayered Material", 2);
            }

            else if (isCoronaLegacyMaterial(materialNode))
            {
                RaiseVerbose("IN isCoronaLegacyMaterial", 2);
            }
            
            else if (isCoronaLightMaterial(materialNode))
            {
                RaiseVerbose("IN isCoronaLightMaterial", 2);

                var babylonMaterial = new BabylonPBRMetallicRoughnessMaterial(id)
                {
                    maxGameMaterial = materialNode,
                    name = name,
                    isUnlit = isUnlit
                };

                ExportCoronaLightMaterial(materialNode, attributesContainer, babylonScene, babylonMaterial);
                RaiseVerbose("CoronaLight Material", 2);
            }

            else
            {
                // isMaterialExportable check should prevent this to happen
                RaiseError($"Unsupported material type: {materialNode.MaterialClass} {materialNode.MaxMaterial.ClassID.PartA.ToString("X")} {materialNode.MaxMaterial.ClassID.PartB.ToString("X")}", 2);
            }
        }

        private void ExportStandardMaterial(IIGameMaterial materialNode, IIPropertyContainer attributesContainer, IStdMat2 stdMat, BabylonScene babylonScene, BabylonStandardMaterial babylonMaterial)
        {
            bool isTransparencyModeFromBabylonAttributes = false;
            if (attributesContainer != null)
            {
                IIGameProperty babylonTransparencyModeGameProperty = attributesContainer.QueryProperty("babylonTransparencyMode");
                if (babylonTransparencyModeGameProperty != null)
                {
                    babylonMaterial.transparencyMode = babylonTransparencyModeGameProperty.GetIntValue();
                    isTransparencyModeFromBabylonAttributes = true;
                }
            }

            if (isTransparencyModeFromBabylonAttributes == false || babylonMaterial.transparencyMode != 0)
            {
                // The user specified value in 3ds Max is opacity
                // The retreived value here is transparency
                // Convert transparency to opacity
                babylonMaterial.alpha = 1.0f - materialNode.MaxMaterial.GetXParency(0, false);
            }

            babylonMaterial.backFaceCulling = !stdMat.TwoSided;
            babylonMaterial.wireframe = stdMat.Wire;

            var isSelfIllumColor = materialNode.MaxMaterial.GetSelfIllumColorOn(0, false);
            var maxSpecularColor = materialNode.MaxMaterial.GetSpecular(0, false).ToArray();

            if (babylonMaterial.isUnlit == false)
            {
                babylonMaterial.ambient = materialNode.MaxMaterial.GetAmbient(0, false).ToArray();
                babylonMaterial.specular = maxSpecularColor.Multiply(materialNode.MaxMaterial.GetShinStr(0, false));
                babylonMaterial.specularPower = materialNode.MaxMaterial.GetShininess(0, false) * 256;
                babylonMaterial.emissive =
                    isSelfIllumColor
                        ? materialNode.MaxMaterial.GetSelfIllumColor(0, false).Scale(selfIllumScale).ToArray()
                        : materialNode.MaxMaterial.GetDiffuse(0, false).Scale(materialNode.MaxMaterial.GetSelfIllum(0, false)); // compute the pre-multiplied emissive color

                // If Self-Illumination color checkbox is checked
                // Then self-illumination is assumed to be pre-multiplied
                // Otherwise self-illumination needs to be multiplied with diffuse
                // linkEmissiveWithDiffuse attribute tells the Babylon engine to perform such multiplication
                babylonMaterial.linkEmissiveWithDiffuse = !isSelfIllumColor;
                // useEmissiveAsIllumination attribute tells the Babylon engine to use pre-multiplied emissive as illumination
                babylonMaterial.useEmissiveAsIllumination = isSelfIllumColor;

                // Store the emissive value (before multiplication) for gltf
                babylonMaterial.selfIllum = materialNode.MaxMaterial.GetSelfIllum(0, false);
            }

            // Textures

            float[] multiplyColor = null;
            if (exportParameters.exportTextures)
            {
                BabylonFresnelParameters fresnelParameters;
                babylonMaterial.diffuseTexture = ExportDiffuseTexture(stdMat, STANDARD_MATERIAL_TEXTURE_ID_DIFFUSE, out fresnelParameters, babylonScene, out multiplyColor);                // Diffuse
                if (fresnelParameters != null)
                {
                    babylonMaterial.diffuseFresnelParameters = fresnelParameters;
                }
                if (multiplyColor != null)
                {
                    babylonMaterial.diffuse = multiplyColor;
                }
                if ((babylonMaterial.alpha == 1.0f && babylonMaterial.opacityTexture == null) &&
                    babylonMaterial.diffuseTexture != null &&
                    (babylonMaterial.diffuseTexture.originalPath.EndsWith(".tif") || babylonMaterial.diffuseTexture.originalPath.EndsWith(".tiff")) &&
                    babylonMaterial.diffuseTexture.hasAlpha)
                {
                    RaiseWarning($"Diffuse texture named {babylonMaterial.diffuseTexture.originalPath} is a .tif file and its Alpha Source is 'Image Alpha' by default.", 2);
                    RaiseWarning($"If you don't want material to be in BLEND mode, set diffuse texture Alpha Source to 'None (Opaque)'", 2);
                }

                if (isTransparencyModeFromBabylonAttributes == false || babylonMaterial.transparencyMode != 0)
                {
                    // The map is opacity
                    babylonMaterial.opacityTexture = ExportTexture(stdMat, STANDARD_MATERIAL_TEXTURE_ID_OPACITY, out fresnelParameters, babylonScene, false, true);   // Opacity
                }

                if (fresnelParameters != null)
                {
                    babylonMaterial.opacityFresnelParameters = fresnelParameters;
                    if (babylonMaterial.alpha == 1 &&
                        babylonMaterial.opacityTexture == null)
                    {
                        babylonMaterial.alpha = 0;
                    }
                }

                if (babylonMaterial.isUnlit == false)
                {
                    babylonMaterial.ambientTexture = ExportTexture(stdMat, 0, out fresnelParameters, babylonScene);                // Ambient

                    babylonMaterial.specularTexture = ExportSpecularTexture(materialNode, maxSpecularColor, babylonScene);

                    babylonMaterial.emissiveTexture = ExportTexture(stdMat, 5, out fresnelParameters, babylonScene);               // Emissive
                    if (fresnelParameters != null)
                    {
                        babylonMaterial.emissiveFresnelParameters = fresnelParameters;
                        if (babylonMaterial.emissive[0] == 0 &&
                            babylonMaterial.emissive[1] == 0 &&
                            babylonMaterial.emissive[2] == 0 &&
                            babylonMaterial.emissiveTexture == null)
                        {
                            babylonMaterial.emissive = new float[] { 1, 1, 1 };
                        }
                    }

                    babylonMaterial.bumpTexture = ExportTexture(stdMat, 8, out fresnelParameters, babylonScene);                   // Bump
                    babylonMaterial.reflectionTexture = ExportTexture(stdMat, 9, out fresnelParameters, babylonScene, true);       // Reflection
                    if (fresnelParameters != null)
                    {
                        if (babylonMaterial.reflectionTexture == null)
                        {
                            RaiseWarning("Fallout cannot be used with reflection channel without a texture", 2);
                        }
                        else
                        {
                            babylonMaterial.reflectionFresnelParameters = fresnelParameters;
                        }
                    }
                }
            }

            if (isTransparencyModeFromBabylonAttributes == false && (babylonMaterial.alpha != 1.0f || (babylonMaterial.diffuseTexture != null && babylonMaterial.diffuseTexture.hasAlpha) || babylonMaterial.opacityTexture != null))
            {
                babylonMaterial.transparencyMode = (int)BabylonPBRMetallicRoughnessMaterial.TransparencyMode.ALPHABLEND;
            }

            // Constraints
            if (babylonMaterial.diffuseTexture != null && multiplyColor == null)
            {
                babylonMaterial.diffuse = new[] { 1.0f, 1.0f, 1.0f };
            }

            if (babylonMaterial.emissiveTexture != null)
            {
                babylonMaterial.emissive = new float[] { 0, 0, 0 };
            }

            if (babylonMaterial.opacityTexture != null && babylonMaterial.diffuseTexture != null &&
                babylonMaterial.diffuseTexture.name == babylonMaterial.opacityTexture.name &&
                babylonMaterial.diffuseTexture.hasAlpha && !babylonMaterial.opacityTexture.getAlphaFromRGB)
            {
                // This is a alpha testing purpose
                babylonMaterial.opacityTexture = null;
                babylonMaterial.diffuseTexture.hasAlpha = true;
                RaiseWarning("Opacity texture was removed because alpha from diffuse texture can be use instead", 2);
                RaiseWarning("If you do not want this behavior, just set Alpha Source = None on your diffuse texture", 2);
            }


            if (babylonMaterial.transparencyMode == (int)BabylonPBRMetallicRoughnessMaterial.TransparencyMode.ALPHATEST)
            {
                // Set the alphaCutOff value explicitely to avoid different interpretations on different engines
                // Use the glTF default value rather than the babylon one
                babylonMaterial.alphaCutOff = 0.5f;
            }

            // Add babylon attributes
            if (attributesContainer == null)
            {
                AddStandardBabylonAttributes(materialNode.MaterialName, babylonMaterial);
            }

            if (attributesContainer != null)
            {
                RaiseVerbose("Babylon Attributes of " + materialNode.MaterialName, 2);

                // Common attributes
                ExportCommonBabylonAttributes(attributesContainer, babylonMaterial);

                // Special treatment for Unlit
                if (babylonMaterial.isUnlit)
                {
                    if ((babylonMaterial.emissive != null && (babylonMaterial.emissive[0] != 0 || babylonMaterial.emissive[1] != 0 || babylonMaterial.emissive[2] != 0))
                        || (babylonMaterial.emissiveTexture != null)
                        || (babylonMaterial.emissiveFresnelParameters != null))
                    {
                        RaiseWarning("Material is unlit. Emission is discarded and replaced by diffuse.", 2);
                    }
                    // Copy diffuse to emissive
                    babylonMaterial.emissive = babylonMaterial.diffuse;
                    babylonMaterial.emissiveTexture = babylonMaterial.diffuseTexture;
                    babylonMaterial.emissiveFresnelParameters = babylonMaterial.diffuseFresnelParameters;

                    babylonMaterial.disableLighting = true;
                    babylonMaterial.linkEmissiveWithDiffuse = false;
                }
                // Special treatment for "Alpha test" transparency mode
                if (exportParameters.exportTextures && babylonMaterial.transparencyMode == (int)BabylonPBRMetallicRoughnessMaterial.TransparencyMode.ALPHATEST)
                {
                    // Base color and alpha files need to be merged into a single file
                    Color defaultColor = Color.FromArgb((int)(babylonMaterial.diffuse[0] * 255), (int)(babylonMaterial.diffuse[1] * 255), (int)(babylonMaterial.diffuse[2] * 255));
                    ITexmap baseColorTextureMap = GetSubTexmap(stdMat, STANDARD_MATERIAL_TEXTURE_ID_DIFFUSE);
                    ITexmap opacityTextureMap = GetSubTexmap(stdMat, STANDARD_MATERIAL_TEXTURE_ID_OPACITY);
                    multiplyColor = null;
                    babylonMaterial.diffuseTexture = ExportBaseColorAlphaTexture(baseColorTextureMap, opacityTextureMap, babylonMaterial.diffuse, babylonMaterial.alpha, babylonScene, out multiplyColor, true);
                    if (multiplyColor != null)
                    {
                        babylonMaterial.diffuse = multiplyColor;
                    }
                    babylonMaterial.opacityTexture = null;
                    babylonMaterial.alpha = 1.0f;
                }
            }

            // List all babylon material attributes
            // Those attributes are currently stored into the native material
            // They should not be exported as extra attributes
            List<string> excludeAttributes = new List<string>();
            excludeAttributes.Add("babylonUnlit");
            excludeAttributes.Add("babylonMaxSimultaneousLights");
            excludeAttributes.Add("babylonTransparencyMode");

            // Export the custom attributes of this material
            babylonMaterial.metadata = ExportExtraAttributes(materialNode, babylonScene, excludeAttributes);

            babylonScene.MaterialsList.Add(babylonMaterial);
        }


        private void ExportPhysicalMaterial(IIGameMaterial materialNode, IIPropertyContainer attributesContainer, BabylonScene babylonScene, BabylonPBRMetallicRoughnessMaterial babylonMaterial)
        {
            var propertyContainer = materialNode.IPropertyContainer;

            bool isTransparencyModeFromBabylonAttributes = false;
            bool usePbrFactor = false; // this force the exporter to set the metallic or roughness even if the map are set
            if (attributesContainer != null)
            {
                IIGameProperty gameProperty = attributesContainer.QueryProperty("babylonTransparencyMode");
                if (gameProperty != null)
                {
                    babylonMaterial.transparencyMode = gameProperty.GetIntValue();
                    isTransparencyModeFromBabylonAttributes = true;
                    RaiseVerbose(".NL.Attribute TransparencyMode:" + babylonMaterial.transparencyMode, 2);
                }
                gameProperty = attributesContainer.QueryProperty("babylonUseFactors");
                if (gameProperty != null)
                {
                    usePbrFactor = gameProperty.GetBoolValue();
                }
            }
            else
            {
                RaiseVerbose(".NL.attributesContainer is NULL", 2);
            }

            // --- Global ---

            // Alpha
            //.NL.Converte la transparency da positiva a negativa
            if (isTransparencyModeFromBabylonAttributes == false || babylonMaterial.transparencyMode != 0)
            {
                // Convert transparency to opacity
                babylonMaterial.alpha = 1.0f - propertyContainer.GetFloatProperty(17);
                RaiseVerbose(".NL.new alpha value for " + materialNode.MaterialName + " is:" + babylonMaterial.alpha, 2);
            }

            babylonMaterial.baseColor = materialNode.MaxMaterial.GetDiffuse(0, false).ToArray();

            var invertRoughness = propertyContainer.GetBoolProperty(5);
            if (babylonMaterial.isUnlit == false)
            {
                babylonMaterial.metallic = propertyContainer?.GetFloatProperty("metalness", 0.0f) ?? 0.0f;

                babylonMaterial.roughness = propertyContainer?.GetFloatProperty("roughness", 0.0f) ?? 0.0f;
                if (invertRoughness)
                {
                    // Inverse roughness
                    babylonMaterial.roughness = 1 - babylonMaterial.roughness;
                }

                // Self illumination is computed from emission color, luminance, temperature and weight
                babylonMaterial.emissive = materialNode.MaxMaterial.GetSelfIllumColorOn(0, false)
                                                ? materialNode.MaxMaterial.GetSelfIllumColor(0, false).Scale(selfIllumScale).ToArray()
                                                : materialNode.MaxMaterial.GetDiffuse(0, false).Scale(materialNode.MaxMaterial.GetSelfIllum(0, false));
            }
            else
            {
                // Ignore specified roughness and metallic values
                babylonMaterial.metallic = 0;
                babylonMaterial.roughness = 0.9f;
            }

            // --- Textures ---
            float[] multiplyColor = null;
            if (exportParameters.exportTextures)
            {
                // 1 - base color ; 9 - transparency weight
                ITexmap colorTexmap = _getTexMap(materialNode, 1);
                ITexmap alphaTexmap = null;
                if (isTransparencyModeFromBabylonAttributes == false || babylonMaterial.transparencyMode != 0)
                {
                    alphaTexmap = _getTexMap(materialNode, 9);
                }
                babylonMaterial.baseTexture = ExportBaseColorAlphaTexture(colorTexmap, alphaTexmap, babylonMaterial.baseColor, babylonMaterial.alpha, babylonScene, out multiplyColor);
                if (multiplyColor != null)
                {
                    babylonMaterial.baseColor = multiplyColor;
                }

                if (babylonMaterial.isUnlit == false)
                {
                    // Metallic, roughness, ambient occlusion
                    ITexmap metallicTexmap = _getTexMap(materialNode, 5);
                    ITexmap roughnessTexmap = _getTexMap(materialNode, 4);
                    ITexmap ambientOcclusionTexmap = _getTexMap(materialNode, 6); // Use diffuse roughness map as ambient occlusion

                    // Check if MR or ORM textures are already merged
                    bool areTexturesAlreadyMerged = false;
                    if (metallicTexmap != null && roughnessTexmap != null)
                    {
                        string sourcePathMetallic = getSourcePath(metallicTexmap);
                        string sourcePathRoughness = getSourcePath(roughnessTexmap);

                        if (sourcePathMetallic == sourcePathRoughness)
                        {
                            if (ambientOcclusionTexmap != null && exportParameters.mergeAO)
                            {
                                string sourcePathAmbientOcclusion = getSourcePath(ambientOcclusionTexmap);
                                if (sourcePathMetallic == sourcePathAmbientOcclusion)
                                {
                                    // Metallic, roughness and ambient occlusion are already merged
                                    RaiseVerbose("Metallic, roughness and ambient occlusion are already merged", 2);
                                    BabylonTexture ormTexture = ExportTexture(metallicTexmap, babylonScene);
                                    babylonMaterial.metallicRoughnessTexture = ormTexture;
                                    babylonMaterial.occlusionTexture = ormTexture;
                                    areTexturesAlreadyMerged = true;
                                }
                            }
                            else
                            {
                                // Metallic and roughness are already merged
                                RaiseVerbose("Metallic and roughness are already merged", 2);
                                BabylonTexture ormTexture = ExportTexture(metallicTexmap, babylonScene);
                                babylonMaterial.metallicRoughnessTexture = ormTexture;
                                areTexturesAlreadyMerged = true;
                            }
                        }
                    }
                    if (areTexturesAlreadyMerged == false)
                    {
                        if (metallicTexmap != null || roughnessTexmap != null)
                        {
                            // Merge metallic, roughness and ambient occlusion
                            RaiseVerbose("Merge metallic and roughness (and ambient occlusion if `mergeAOwithMR` is enabled)", 2);
                            BabylonTexture ormTexture = ExportORMTexture(exportParameters.mergeAO ? ambientOcclusionTexmap : null, roughnessTexmap, metallicTexmap, babylonMaterial.metallic, babylonMaterial.roughness, babylonScene, invertRoughness);
                            babylonMaterial.metallicRoughnessTexture = ormTexture;

                            if (ambientOcclusionTexmap != null)
                            {
                                if (exportParameters.mergeAO)
                                {
                                    // if the ambient occlusion texture map uses a different set of texture coordinates than
                                    // metallic roughness, create a new instance of the ORM BabylonTexture with the different texture
                                    // coordinate indices
                                    var ambientOcclusionTexture = _getBitmapTex(ambientOcclusionTexmap);
                                    var texCoordIndex = ambientOcclusionTexture.UVGen.MapChannel - 1;
                                    if (texCoordIndex != ormTexture.coordinatesIndex)
                                    {
                                        babylonMaterial.occlusionTexture = new BabylonTexture(ormTexture);
                                        babylonMaterial.occlusionTexture.coordinatesIndex = texCoordIndex;
                                        // Set UVs/texture transform for the ambient occlusion texture
                                        var uvGen = _exportUV(ambientOcclusionTexture.UVGen, babylonMaterial.occlusionTexture);
                                    }
                                    else
                                    {
                                        babylonMaterial.occlusionTexture = ormTexture;
                                    }
                                }
                                else
                                {
                                    babylonMaterial.occlusionTexture = ExportPBRTexture(materialNode, 6, babylonScene);
                                }
                            }
                        }
                        else if (ambientOcclusionTexmap != null)
                        {
                            // Simply export occlusion texture
                            RaiseVerbose("Simply export occlusion texture", 2);
                            babylonMaterial.occlusionTexture = ExportTexture(ambientOcclusionTexmap, babylonScene);
                        }
                    }
                    if (ambientOcclusionTexmap != null && !exportParameters.mergeAO && babylonMaterial.occlusionTexture == null)
                    {
                        RaiseVerbose("Exporting occlusion texture without merging with metallic roughness", 2);
                        babylonMaterial.occlusionTexture = ExportTexture(ambientOcclusionTexmap, babylonScene);
                    }

#if MAX2023 || MAX2024
                    var normalMapAmount = propertyContainer.GetFloatProperty("bump_map_amt");
#else
                    var normalMapAmount = propertyContainer.GetFloatProperty(91);
#endif
                    babylonMaterial.normalTexture = ExportPBRTexture(materialNode, 30, babylonScene, normalMapAmount);

                    babylonMaterial.emissiveTexture = ExportPBRTexture(materialNode, 17, babylonScene);

                    if (babylonMaterial.metallicRoughnessTexture != null && !usePbrFactor)
                    {
                        // Change the factor to zero if combining partial channel to avoid issue (in case of image compression).
                        // ie - if no metallic map, then b MUSt be fully black. However channel of jpeg MAY not beeing fully black 
                        // cause of the compression algorithm. Keeping MetallicFactor to 1 will make visible artifact onto texture. So set to Zero instead.
                        babylonMaterial.metallic = areTexturesAlreadyMerged || metallicTexmap != null || babylonMaterial.metallic != 0 ? 1.0f : 0.0f;
                        babylonMaterial.roughness = areTexturesAlreadyMerged || roughnessTexmap != null || babylonMaterial.roughness != 0 ? 1.0f : 0.0f;
                    }
                }
            }

            // Constraints
            if (babylonMaterial.baseTexture != null && multiplyColor == null)
            {
                babylonMaterial.baseColor = new[] { 1.0f, 1.0f, 1.0f };
            }

            if (isTransparencyModeFromBabylonAttributes == false && (babylonMaterial.alpha != 1.0f || (babylonMaterial.baseTexture != null && babylonMaterial.baseTexture.hasAlpha)))
            {
                babylonMaterial.transparencyMode = (int)BabylonPBRMetallicRoughnessMaterial.TransparencyMode.ALPHABLEND;
            }

            if (babylonMaterial.emissiveTexture != null)
            {
                babylonMaterial.emissive = new[] { 1.0f, 1.0f, 1.0f };
            }

            if (babylonMaterial.transparencyMode == (int)BabylonPBRMetallicRoughnessMaterial.TransparencyMode.ALPHATEST)
            {
                // Set the alphaCutOff value explicitely to avoid different interpretations on different engines
                // Use the glTF default value rather than the babylon one
                babylonMaterial.alphaCutOff = 0.5f;
            }

            //RaiseVerbose(".NL.alphaCutOff " + materialNode.MaterialName + " " + babylonMaterial.alphaCutOff + " Mode:" + babylonMaterial.transparencyMode, 2);

            // Add babylon attributes
            if (attributesContainer == null)
            {
                AddPhysicalBabylonAttributes(materialNode.MaterialName, babylonMaterial);
            }

            if (attributesContainer != null)
            {
                //RaiseVerbose("Babylon Attributes of " + materialNode.MaterialName, 2);

                // Common attributes
                ExportCommonBabylonAttributes(attributesContainer, babylonMaterial);
                babylonMaterial._unlit = babylonMaterial.isUnlit;

                // Backface culling
                bool backFaceCulling = attributesContainer.GetBoolProperty("babylonBackfaceCulling");
                RaiseVerbose("backFaceCulling=" + backFaceCulling, 3);
                babylonMaterial.backFaceCulling = backFaceCulling;
                babylonMaterial.doubleSided = !backFaceCulling;
            }

            // List all babylon material attributes
            // Those attributes are currently stored into the native material
            // They should not be exported as extra attributes
            List<string> excludeAttributes = new List<string>
            {
                "babylonUnlit",
                "babylonBackfaceCulling",
                "babylonMaxSimultaneousLights",
                "babylonTransparencyMode",
                "babylonUseFactors",
                "babylonDirectIntensity",
                "babylonEmissiveIntensity",
                "babylonEnvironmentIntensity",
                "babylonSpecularIntensity"
            };

            // Export the custom attributes of this material
            babylonMaterial.metadata = ExportExtraAttributes(materialNode, babylonScene, excludeAttributes);

            RaiseVerbose("GroupedProperty[" + babylonMaterial.name + "]: " + (babylonMaterial.metadata != null), 2);

            // Export additional info 
            getGroupedProperties(propertyContainer, babylonMaterial);

            babylonScene.MaterialsList.Add(babylonMaterial);
        }


        private void ExportArnoldMaterial(IIGameMaterial materialNode, IIPropertyContainer attributesContainer, BabylonScene babylonScene, BabylonPBRMetallicRoughnessMaterial babylonMaterial)
        {
            var propertyContainer = materialNode.IPropertyContainer;

            bool isTransparencyModeFromBabylonAttributes = false;
            if (attributesContainer != null)
            {
                IIGameProperty babylonTransparencyModeGameProperty = attributesContainer.QueryProperty("babylonTransparencyMode");
                if (babylonTransparencyModeGameProperty != null)
                {
                    babylonMaterial.transparencyMode = babylonTransparencyModeGameProperty.GetIntValue();
                    isTransparencyModeFromBabylonAttributes = true;
                }
            }

            // Alpha
            if (isTransparencyModeFromBabylonAttributes == false || babylonMaterial.transparencyMode != 0)
            {
                // Retreive alpha value from R channel of opacity color
                babylonMaterial.alpha = propertyContainer.GetPoint3Property("opacity")[0];
            }

            // Color: base * weight
            float[] baseColor = propertyContainer.GetPoint3Property(5).ToArray();
            float baseWeight = propertyContainer.GetFloatProperty(2);
            babylonMaterial.baseColor = baseColor.Multiply(baseWeight);

            // Metallic & roughness
            bool invertRoughness = false;
            babylonMaterial.roughness = propertyContainer.GetFloatProperty(17); // specular_roughness
            babylonMaterial.metallic = propertyContainer.GetFloatProperty(29);

            // Emissive: emission_color * emission
            float[] emissionColor = propertyContainer.QueryProperty("emission_color").GetPoint3Property().ToArray();
            float emissionWeight = propertyContainer.QueryProperty("emission").GetFloatValue();
            if (emissionColor != null && emissionWeight > 0f)
            {
                babylonMaterial.emissive = emissionColor.Multiply(emissionWeight);
            }

            // --- Clear Coat ---
            float coatWeight = propertyContainer.GetFloatProperty(75);
            if (coatWeight > 0.0f)
            {
                babylonMaterial.clearCoat.isEnabled = true;
                babylonMaterial.clearCoat.indexOfRefraction = propertyContainer.GetFloatProperty(84);

                ITexmap intensityTexmap = _getTexMap(materialNode, 23);
                ITexmap roughnessTexmap = _getTexMap(materialNode, 25);
                var coatRoughness = propertyContainer.GetFloatProperty(81);
                var coatTexture = exportParameters.exportTextures ? ExportClearCoatTexture(intensityTexmap, roughnessTexmap, coatWeight, coatRoughness, babylonScene, babylonMaterial.name, invertRoughness) : null;
                if (coatTexture != null)
                {
                    babylonMaterial.clearCoat.texture = coatTexture;
                    babylonMaterial.clearCoat.roughness = 1.0f;
                    babylonMaterial.clearCoat.intensity = 1.0f;
                }
                else
                {
                    babylonMaterial.clearCoat.intensity = coatWeight;
                    babylonMaterial.clearCoat.roughness = coatRoughness;
                }

                float[] coatColor = propertyContainer.GetPoint3Property(78).ToArray();
                if (coatColor[0] != 1.0f || coatColor[1] != 1.0f || coatColor[2] != 1.0f)
                {
                    babylonMaterial.clearCoat.isTintEnabled = true;
                    babylonMaterial.clearCoat.tintColor = coatColor;
                }

                babylonMaterial.clearCoat.tintTexture = exportParameters.exportTextures ? ExportPBRTexture(materialNode, 24, babylonScene) : null;
                if (babylonMaterial.clearCoat.tintTexture != null)
                {
                    babylonMaterial.clearCoat.tintColor = new[] { 1.0f, 1.0f, 1.0f };
                    babylonMaterial.clearCoat.isTintEnabled = true;
                }

                // EyeBall deduction...
                babylonMaterial.clearCoat.tintThickness = 0.65f;

                babylonMaterial.clearCoat.bumpTexture = exportParameters.exportTextures ? ExportPBRTexture(materialNode, 27, babylonScene) : null;
            }

            // --- Textures ---

            float[] multiplyColor = null;
            if (exportParameters.exportTextures)
            {
                // 1 - base_color ; 5 - specular_roughness ; 9 - metalness ; 40 - transparent
                ITexmap colorTexmap = _getTexMap(materialNode, 1);
                ITexmap alphaTexmap = null;
                if (isTransparencyModeFromBabylonAttributes == false || babylonMaterial.transparencyMode != 0)
                {
                    alphaTexmap = _getTexMap(materialNode, "opacity");
                }

                babylonMaterial.baseTexture = ExportBaseColorAlphaTexture(colorTexmap, alphaTexmap, babylonMaterial.baseColor, babylonMaterial.alpha, babylonScene, out multiplyColor, true);

                if (multiplyColor != null)
                {
                    babylonMaterial.baseColor = multiplyColor;
                }

                if (babylonMaterial.isUnlit == false)
                {
                    // Metallic, roughness
                    ITexmap metallicTexmap = _getTexMap(materialNode, 9);
                    ITexmap roughnessTexmap = _getTexMap(materialNode, 5);
                    ITexmap ambientOcclusionTexmap = _getTexMap(materialNode, 6); // Use diffuse roughness map as ambient occlusion

                    // Check if MR textures are already merged
                    bool areTexturesAlreadyMerged = false;
                    if (metallicTexmap != null && roughnessTexmap != null)
                    {
                        string sourcePathMetallic = getSourcePath(metallicTexmap);
                        string sourcePathRoughness = getSourcePath(roughnessTexmap);

                        if (sourcePathMetallic == sourcePathRoughness)
                        {
                            // Metallic and roughness are already merged
                            RaiseVerbose("Metallic and roughness are already merged", 2);
                            BabylonTexture ormTexture = ExportTexture(metallicTexmap, babylonScene);
                            babylonMaterial.metallicRoughnessTexture = ormTexture;
                            // The already merged map is assumed to contain Ambient Occlusion in R channel

                            if (ambientOcclusionTexmap != null)
                            {
                                // if the ambient occlusion texture map uses a different set of texture coordinates than
                                // metallic roughness, create a new instance of the ORM BabylonTexture with the different texture
                                // coordinate indices

                                var ambientOcclusionTexture = _getBitmapTex(ambientOcclusionTexmap);
                                var texCoordIndex = ambientOcclusionTexture.UVGen.MapChannel - 1;
                                if (texCoordIndex != ormTexture.coordinatesIndex)
                                {
                                    babylonMaterial.occlusionTexture = new BabylonTexture(ormTexture);
                                    babylonMaterial.occlusionTexture.coordinatesIndex = texCoordIndex;
                                    // Set UVs/texture transform for the ambient occlusion texture
                                    var uvGen = _exportUV(ambientOcclusionTexture.UVGen, babylonMaterial.occlusionTexture);
                                }
                                else
                                {
                                    babylonMaterial.occlusionTexture = ormTexture;
                                }
                            }
                            else
                            {
                                babylonMaterial.occlusionTexture = ormTexture;
                            }
                            areTexturesAlreadyMerged = true;
                        }
                    }
                    if (areTexturesAlreadyMerged == false)
                    {
                        if (metallicTexmap != null || roughnessTexmap != null)
                        {
                            // Merge metallic, roughness
                            RaiseVerbose("Merge metallic and roughness", 2);
                            BabylonTexture ormTexture = ExportORMTexture(null, roughnessTexmap, metallicTexmap, babylonMaterial.metallic, babylonMaterial.roughness, babylonScene, invertRoughness);
                            babylonMaterial.metallicRoughnessTexture = ormTexture;
                        }
                    }

                    var numOfTexMapSlots = materialNode.MaxMaterial.NumSubTexmaps;

                    for (int i = 0; i < numOfTexMapSlots; i++)
                    {

#if MAX2024
                        // 3dsMax2024+ introduced new localized flag
                        if (materialNode.MaxMaterial.GetSubTexmapSlotName(i, false) == "normal")
#else
                        if(materialNode.MaxMaterial.GetSubTexmapSlotName(i) == "normal")
#endif

                        {
                            babylonMaterial.normalTexture = ExportPBRTexture(materialNode, i, babylonScene);
                        }

#if MAX2024
                        // 3dsMax2024+ introduced new localized flag
                        else if (materialNode.MaxMaterial.GetSubTexmapSlotName(i, false) == "emission")
#else
                        else if (materialNode.MaxMaterial.GetSubTexmapSlotName(i) == "emission")
#endif
                        {
                            babylonMaterial.emissiveTexture = ExportPBRTexture(materialNode, i, babylonScene);
                        }
                    }

                    if (babylonMaterial.metallicRoughnessTexture != null)
                    {
                        // Change the factor to zero if combining partial channel to avoid issue (in case of image compression).
                        // ie - if no metallic map, then b MUSt be fully black. However channel of jpeg MAY not beeing fully black 
                        // cause of the compression algorithm. Keeping MetallicFactor to 1 will make visible artifact onto texture. So set to Zero instead.
                        babylonMaterial.metallic = areTexturesAlreadyMerged || metallicTexmap != null || babylonMaterial.metallic != 0 ? 1.0f : 0.0f;
                        babylonMaterial.roughness = areTexturesAlreadyMerged || roughnessTexmap != null || babylonMaterial.roughness != 0 ? 1.0f : 0.0f;
                    }
                }
            }

            // Constraints
            if (babylonMaterial.baseTexture != null)
            {
                if (multiplyColor == null)
                {
                    babylonMaterial.baseColor = new[] { 1.0f, 1.0f, 1.0f };
                }
                babylonMaterial.alpha = 1.0f;
            }

            if (isTransparencyModeFromBabylonAttributes == false && (babylonMaterial.alpha != 1.0f || (babylonMaterial.baseTexture != null && babylonMaterial.baseTexture.hasAlpha)))
            {
                babylonMaterial.transparencyMode = (int)BabylonPBRMetallicRoughnessMaterial.TransparencyMode.ALPHABLEND;
            }


            if (babylonMaterial.transparencyMode == (int)BabylonPBRMetallicRoughnessMaterial.TransparencyMode.ALPHATEST)
            {
                // Set the alphaCutOff value explicitely to avoid different interpretations on different engines
                // Use the glTF default value rather than the babylon one
                babylonMaterial.alphaCutOff = 0.5f;
            }

            // Add babylon attributes
            if (attributesContainer == null)
            {
                AddAiStandardSurfaceBabylonAttributes(materialNode.MaterialName, babylonMaterial);
            }

            if (attributesContainer != null)
            {
                RaiseVerbose("Babylon Attributes of " + materialNode.MaterialName, 2);

                // Common attributes
                ExportCommonBabylonAttributes(attributesContainer, babylonMaterial);
                babylonMaterial._unlit = babylonMaterial.isUnlit;

                // Backface culling
                bool backFaceCulling = attributesContainer.GetBoolProperty("babylonBackfaceCulling");
                RaiseVerbose("backFaceCulling=" + backFaceCulling, 3);
                babylonMaterial.backFaceCulling = backFaceCulling;
                babylonMaterial.doubleSided = !backFaceCulling;
            }

            // List all babylon material attributes
            // Those attributes are currently stored into the native material
            // They should not be exported as extra attributes
            List<string> excludeAttributes = new List<string>();
            excludeAttributes.Add("babylonUnlit");
            excludeAttributes.Add("babylonBackfaceCulling");
            excludeAttributes.Add("babylonMaxSimultaneousLights");
            excludeAttributes.Add("babylonTransparencyMode");

            // Export the custom attributes of this material
            babylonMaterial.metadata = ExportExtraAttributes(materialNode, babylonScene, excludeAttributes);

            if (exportParameters.pbrFull)
            {
                var fullPBR = new BabylonPBRMaterial(babylonMaterial)
                {
                    directIntensity = attributesContainer?.GetIntProperty("babylonDirectIntensity") ?? 1.0f,
                    emissiveIntensity = attributesContainer?.GetIntProperty("babylonEmissiveIntensity") ?? 1.0f,
                    environmentIntensity = attributesContainer?.GetIntProperty("babylonEnvironmentIntensity") ?? 1.0f,
                    specularIntensity = attributesContainer?.GetIntProperty("babylonSpecularIntensity") ?? 1.0f,
                    maxGameMaterial = babylonMaterial.maxGameMaterial
                };
                babylonScene.MaterialsList.Add(fullPBR);
            }
            else
            {
                // Add the material to the scene
                babylonScene.MaterialsList.Add(babylonMaterial);
            }
        }

        /// <summary>
        /// Refer to this script for the conversion logic: https://github.com/jmdvella/3dsmax-scripts/blob/main/JV_CoronaToPhysical.ms
        /// 
        /*
            0 - baseColor - Point3Prop
            1 - baseLevel - FloatProp
            2 - baseTexmap - UnknownProp
            3 - baseTexmapOn - IntProp
            4 - baseMapAmount - FloatProp
            5 - metalnessMode - IntProp
            6 - opacityColor - Point3Prop
            7 - opacityLevel - FloatProp
            8 - opacityTexmap - UnknownProp
            9 - opacityTexmapOn - IntProp
            10 - opacityMapAmount - FloatProp
            11 - opacityCutout - IntProp
            12 - baseRoughness - FloatProp
            13 - baseRoughnessTexmap - UnknownProp
            14 - baseRoughnessTexmapOn - IntProp
            15 - baseRoughnessMapAmount - FloatProp
            16 - baseAnisotropy - FloatProp
            17 - baseAnisotropyTexmap - UnknownProp
            18 - baseAnisotropyTexmapOn - IntProp
            19 - baseAnisotropyMapAmount - FloatProp
            20 - baseAnisoRotation - FloatProp
            21 - baseAnisoRotationTexmap - UnknownProp
            22 - baseAnisoRotationTexmapOn - IntProp
            23 - baseAnisoRotationMapAmount - FloatProp
            24 - baseIor - FloatProp
            25 - baseIorTexmap - UnknownProp
            26 - baseIorTexmapOn - IntProp
            27 - baseIorMapAmount - FloatProp
            28 - refractionAmount - FloatProp
            29 - refractionAmountTexmap - UnknownProp
            30 - refractionAmountTexmapOn - IntProp
            31 - refractionAmountMapAmount - FloatProp
            32 - dispersionEnable - IntProp
            33 - dispersion - FloatProp
            34 - useThinMode - IntProp
            35 - useCaustics - IntProp
            36 - clearcoatAmount - FloatProp
            37 - clearcoatAmountTexmap - UnknownProp
            38 - clearcoatAmountTexmapOn - IntProp
            39 - clearcoatAmountMapAmount - FloatProp
            40 - clearcoatIor - FloatProp
            41 - clearcoatIorTexmap - UnknownProp
            42 - clearcoatIorTexmapOn - IntProp
            43 - clearcoatIorMapAmount - FloatProp
            44 - clearcoatRoughness - FloatProp
            45 - clearcoatRoughnessTexmap - UnknownProp
            46 - clearcoatRoughnessTexmapOn - IntProp
            47 - clearcoatRoughnessMapAmount - FloatProp
            48 - sheenAmount - FloatProp
            49 - sheenAmountTexmap - UnknownProp
            50 - sheenAmountTexmapOn - IntProp
            51 - sheenAmountMapAmount - FloatProp
            52 - sheenColor - Point3Prop
            53 - sheenColorTexmap - UnknownProp
            54 - sheenColorTexmapOn - IntProp
            55 - sheenColorMapAmount - FloatProp
            56 - sheenRoughness - FloatProp
            57 - sheenRoughnessTexmap - UnknownProp
            58 - sheenRoughnessTexmapOn - IntProp
            59 - sheenRoughnessMapAmount - FloatProp
            60 - volumetricAbsorptionColor - Point3Prop
            61 - volumetricAbsorptionTexmap - UnknownProp
            62 - volumetricAbsorptionTexmapOn - IntProp
            63 - volumetricAbsorptionMapAmount - FloatProp
            64 - volumetricScatteringColor - Point3Prop
            65 - volumetricScatteringTexmap - UnknownProp
            66 - volumetricScatteringTexmapOn - IntProp
            67 - volumetricScatteringMapAmount - FloatProp
            68 - attenuationDistance - FloatProp
            69 - scatterDirectionality - FloatProp
            70 - scatterSingleBounce - IntProp
            71 - sssAmount - FloatProp
            72 - sssAmountTexmap - UnknownProp
            73 - sssAmountTexmapOn - IntProp
            74 - sssAmountMapAmount - FloatProp
            75 - sssRadius - FloatProp
            76 - sssRadiusTexmap - UnknownProp
            77 - sssRadiusTexmapOn - IntProp
            78 - sssRadiusMapAmount - FloatProp
            79 - sssScatterColor - Point3Prop
            80 - sssScatterTexmap - UnknownProp
            81 - sssScatterTexmapOn - IntProp
            82 - sssScatterMapAmount - FloatProp
            83 - displacementMinimum - FloatProp
            84 - displacementMaximum - FloatProp
            85 - displacementWaterLevelOn - IntProp
            86 - displacementWaterLevel - FloatProp
            87 - displacementTexmap - UnknownProp
            88 - displacementTexmapOn - IntProp
            89 - selfIllumColor - Point3Prop
            90 - selfIllumLevel - FloatProp
            91 - selfIllumTexmap - UnknownProp
            92 - selfIllumTexmapOn - IntProp
            93 - selfIllumMapAmount - FloatProp
            94 - alphaMode - IntProp
            95 - gBufferOverride - IntProp
            96 - anisotropyOrientationMode - IntProp
            97 - anisotropyOrientationUvwChannel - IntProp
            98 - renderElementPropagation - IntProp
            99 - materialLibraryId - StringProp
            100 - baseBumpTexmap - UnknownProp
            101 - baseBumpTexmapOn - IntProp
            102 - baseBumpMapAmount - FloatProp
            103 - bgOverrideReflectTexmap - UnknownProp
            104 - bgOverrideReflectTexmapOn - IntProp
            105 - bgOverrideRefractTexmap - UnknownProp
            106 - bgOverrideRefractTexmapOn - IntProp
            107 - translucencyFraction - FloatProp
            108 - translucencyFractionTexmap - UnknownProp
            109 - translucencyFractionTexmapOn - IntProp
            110 - translucencyFractionMapAmount - FloatProp
            111 - thinAbsorptionColor - Point3Prop
            112 - thinAbsorptionTexmap - UnknownProp
            113 - thinAbsorptionTexmapOn - IntProp
            114 - thinAbsorptionMapAmount - FloatProp
            115 - clearcoatAbsorptionColor - Point3Prop
            116 - clearcoatAbsorptionTexmap - UnknownProp
            117 - clearcoatAbsorptionTexmapOn - IntProp
            118 - clearcoatAbsorptionMapAmount - FloatProp
            119 - clearcoatBumpTexmap - UnknownProp
            120 - clearcoatBumpTexmapOn - IntProp
            121 - clearcoatBumpMapAmount - FloatProp
            122 - metalnessTexmap - UnknownProp
            123 - metalnessTexmapOn - IntProp
            124 - roughnessMode - IntProp
            125 - preset - IntProp
            126 - edgeColor - Point3Prop
            127 - edgeColorTexmap - UnknownProp
            128 - edgeColorTexmapOn - IntProp
            129 - edgeColorMapAmount - FloatProp
            130 - translucencyColor - Point3Prop
            131 - translucencyColorTexmap - UnknownProp
            132 - translucencyColorTexmapOn - IntProp
            133 - translucencyColorMapAmount - FloatProp
            134 - useComplexIor - IntProp
            135 - complexIorNRed - FloatProp
            136 - complexIorNGreen - FloatProp
            137 - complexIorNBlue - FloatProp
            138 - complexIorKRed - FloatProp
            139 - complexIorKGreen - FloatProp
            140 - complexIorKBlue - FloatProp
            141 - iorMode - IntProp
            142 - baseTail - FloatProp
            143 - baseTailTexmap - UnknownProp
            144 - baseTailTexmapOn - IntProp
            145 - baseTailMapAmount - FloatProp
            146 - normalFilteringMode - IntProp
            147 - enabled - IntProp
            148 - effect - IntProp
            149 - dxStdMat - IntProp
            150 - SaveFXFile - UnknownProp
        */
        /// </summary>
        /// <param name="materialNode"></param>
        /// <param name="attributesContainer"></param>
        /// <param name="babylonScene"></param>
        /// <param name="babylonMaterial"></param>
        private void ExportCoronaPhysicalMaterial(IIGameMaterial materialNode, IIPropertyContainer attributesContainer, BabylonScene babylonScene, BabylonPBRMetallicRoughnessMaterial babylonMaterial)
        {
            var propertyContainer = materialNode.IPropertyContainer;

            for (int i = 0; i < propertyContainer.NumberOfProperties; i++)
            {
                var prop = propertyContainer.GetProperty(i);
                Trace.WriteLine($"{i} - {prop.Name} - {prop.GetType_}: {propertyValueToString(prop)}");
            }

            babylonMaterial.name = materialNode.MaterialName;
            babylonMaterial.baseColor = propertyContainer.GetPoint3Property("baseColor").ToArray();
            babylonMaterial.roughness = propertyContainer.GetFloatProperty("baseRoughness");

            if (exportParameters.pbrFull)
            {
                var fullPBR = new BabylonPBRMaterial(babylonMaterial)
                {
                    directIntensity = attributesContainer?.GetIntProperty("babylonDirectIntensity") ?? 1.0f,
                    emissiveIntensity = attributesContainer?.GetIntProperty("babylonEmissiveIntensity") ?? 1.0f,
                    environmentIntensity = attributesContainer?.GetIntProperty("babylonEnvironmentIntensity") ?? 1.0f,
                    specularIntensity = attributesContainer?.GetIntProperty("babylonSpecularIntensity") ?? 1.0f,
                    maxGameMaterial = babylonMaterial.maxGameMaterial
                };
                babylonScene.MaterialsList.Add(fullPBR);
            }
            else
            {
                // Add the material to the scene
                babylonScene.MaterialsList.Add(babylonMaterial);
            }
        }

        private void ExportCoronaLayeredMaterial(IIGameMaterial materialNode, IIPropertyContainer attributesContainer, BabylonScene babylonScene, BabylonPBRMetallicRoughnessMaterial babylonMaterial)
        {
            var propertyContainer = materialNode.IPropertyContainer;
        }

        /// <summary>
        /*
            0 - intensity - FloatProp
            1 - texmapOn - IntProp
            2 - texmap - UnknownProp
            3 - color - Point3Prop
            4 - affectAlpha - IntProp
            5 - occludeOther - IntProp
            6 - emitLight - IntProp
            7 - visibleRefl - IntProp
            8 - visibleDirect - IntProp
            9 - visibleRefract - IntProp
            10 - opacityTexmap - UnknownProp
            11 - opacityTexmapOn - IntProp
            12 - directionality - FloatProp
            13 - excludeList - UnknownProp
            14 - excludeListIncludeMod - IntProp
            15 - visibleInMasks - IntProp
            16 - shadowcatcherIlluminator - IntProp
            17 - twosidedEmission - IntProp
            18 - legacyMode - IntProp
            19 - displayWire - IntProp
            20 - removedParam - IntProp
            21 - gBufferOverride - IntProp
            22 - nondirectionalFake - IntProp
            23 - visibleCaustics - IntProp
            24 - enabled - IntProp
            25 - effect - IntProp
            26 - dxStdMat - IntProp
            27 - SaveFXFile - UnknownProp
        */
        /// </summary>
        /// <param name="materialNode"></param>
        /// <param name="attributesContainer"></param>
        /// <param name="babylonScene"></param>
        /// <param name="babylonMaterial"></param>
        private void ExportCoronaLightMaterial(IIGameMaterial materialNode, IIPropertyContainer attributesContainer, BabylonScene babylonScene, BabylonPBRMetallicRoughnessMaterial babylonMaterial) 
        {
            var propertyContainer = materialNode.IPropertyContainer;

            babylonMaterial.name = materialNode.MaterialName;
            babylonMaterial.baseColor = propertyContainer.GetPoint3Property("color").ToArray();

            var texmap = propertyContainer.GetProperty(2);

            Trace.WriteLine($"{materialNode.MaterialName}");

            for (int i = 0; i < propertyContainer.NumberOfProperties; i++)
            {
                var prop = propertyContainer.GetProperty(i);
                Trace.WriteLine($"{i} - {prop.Name} - {prop.GetType_}: {propertyValueToString(prop)}");
            }
        }



        public bool isPhysicalMaterial(IIGameMaterial materialNode)
        {
            return ClassIDWrapper.Physical_Material.Equals(materialNode.MaxMaterial.ClassID);
        }

        public bool isDoubleSidedMaterial(IIGameMaterial materialNode)
        {
            return ClassIDWrapper.Double_Sided_Material.Equals(materialNode.MaxMaterial.ClassID);
        }

        public bool isMultiSubObjectMaterial(IIGameMaterial materialNode)
        {
            return ClassIDWrapper.Multi_Sub_Object_Material.Equals(materialNode.MaxMaterial.ClassID);
        }

        public bool isDirectXShaderMaterial(IIGameMaterial materialNode)
        {
            return ClassIDWrapper.DirectX_Shader_Material.Equals(materialNode.MaxMaterial.ClassID);
        }

        public bool isArnoldMaterial(IIGameMaterial materialNode)
        {
            return ClassIDWrapper.Standard_Surface_Material.Equals(materialNode.MaxMaterial.ClassID);
        }

        public bool isShellMaterial(IIGameMaterial materialNode)
        {
            return ClassIDWrapper.Shell_Material.Equals(materialNode.MaxMaterial.ClassID);
        }

        public bool isCoronaPhysicalMaterial(IIGameMaterial materialNode)
        {
            return ClassIDWrapper.Corona_Physical_Material.Equals(materialNode.MaxMaterial.ClassID);
        }

        public bool isCoronaLayeredMaterial(IIGameMaterial materialNode)
        {
            return ClassIDWrapper.Corona_Layered_Material.Equals(materialNode.MaxMaterial.ClassID);
        }

        public bool isCoronaLegacyMaterial(IIGameMaterial materialNode)
        {
            return ClassIDWrapper.Corona_Legacy_Material.Equals(materialNode.MaxMaterial.ClassID);
        }

        public bool isCoronaLightMaterial(IIGameMaterial materialNode)
        {
            return ClassIDWrapper.Corona_Light_Material.Equals(materialNode.MaxMaterial.ClassID);
        }

        /// <summary>
        /// Return null if the material is supported.
        /// Otherwise return the unsupported material (himself or one of its sub-materials)
        /// </summary>
        /// <param name="materialNode"></param>
        /// <returns></returns>
        public IIGameMaterial isMaterialSupported(IIGameMaterial materialNode)
        {
            // Shell material
            if (isShellMaterial(materialNode))
            {
                var bakedMaterial = GetBakedMaterialFromShellMaterial(materialNode);
                if (bakedMaterial == null)
                {
                    return materialNode;
                }
                return isMaterialSupported(bakedMaterial);
            }

            if (materialNode.SubMaterialCount > 0)
            {
                // Check sub materials recursively
                for (int indexSubMaterial = 0; indexSubMaterial < materialNode.SubMaterialCount; indexSubMaterial++)
                {
                    IIGameMaterial subMaterialNode = materialNode.GetSubMaterial(indexSubMaterial);
                    IIGameMaterial unsupportedSubMaterial = isMaterialSupported(subMaterialNode);
                    if (unsupportedSubMaterial != null)
                    {
                        return unsupportedSubMaterial;
                    }
                }

                // Multi/sub-object material
                if (isMultiSubObjectMaterial(materialNode))
                {
                    return null;
                }

                // Double sided material
                if (isDoubleSidedMaterial(materialNode))
                {
                    return null;
                }

                if (isCoronaLayeredMaterial(materialNode))
                {
                    return null;
                }

                if (isCoronaLegacyMaterial(materialNode))
                {
                    return null;
                }
            }
            else
            {
                // Standard material
                IStdMat2 stdMat = null;
                if (materialNode.MaxMaterial != null && materialNode.MaxMaterial.NumParamBlocks > 0)
                {
                    var paramBlock = materialNode.MaxMaterial.GetParamBlock(0);
                    if (paramBlock != null && paramBlock.Owner != null)
                    {
                        stdMat = materialNode.MaxMaterial.GetParamBlock(0).Owner as IStdMat2;
                    }
                }

                if (stdMat != null)
                {
                    return null;
                }

                // Physical materials
                if (isPhysicalMaterial(materialNode) || isPbrMetalRoughMaterial(materialNode) || isPbrSpecGlossMaterial(materialNode)
#if MAX2023 || MAX2024
                    || isGLTFMaterial(materialNode)
#endif
                    )
                {
                    return null;
                }

                // Custom material exporters
                IMaxMaterialExporter materialExporter;
                if (materialExporters.TryGetValue(new ClassIDWrapper(materialNode.MaxMaterial.ClassID), out materialExporter))
                {
                    if (isGltfExported && materialExporter is IMaxGLTFMaterialExporter)
                        return null;
                    else if (isBabylonExported && materialExporter is IMaxBabylonMaterialExporter)
                        return null;
                }

                // Arnold material
                if (isArnoldMaterial(materialNode))
                {
                    return null;
                }

                // DirectX Shader
                if (isDirectXShaderMaterial(materialNode))
                {
                    return isMaterialSupported(GetRenderMaterialFromDirectXShader(materialNode));
                }

                // Corona materials
                if (isCoronaPhysicalMaterial(materialNode) || isCoronaLegacyMaterial(materialNode) || isCoronaLightMaterial(materialNode))
                {
                    return null;
                }
            }
            return materialNode;
        }

        private IIGameMaterial GetRenderMaterialFromDirectXShader(IIGameMaterial materialNode)
        {
            IIGameMaterial renderMaterial = null;

            if (isDirectXShaderMaterial(materialNode))
            {
                var gameScene = Loader.Global.IGameInterface;
                IIGameProperty property = materialNode.IPropertyContainer.GetProperty(35);
                if (property != null)
                {
                    IMtl renderMtl = materialNode.IPropertyContainer.GetProperty(35).MaxParamBlock2.GetMtl(4, 0, 0);
                    if (renderMtl != null)
                    {
                        renderMaterial = gameScene.GetIGameMaterial(renderMtl);
                    }
                }
                else
                {
                    RaiseWarning($"DirectX material property for {materialNode.MaterialName} is null...", 2);
                }

            }

            return renderMaterial;
        }

        private IIGameMaterial GetBakedMaterialFromShellMaterial(IIGameMaterial materialNode)
        {
            if (isShellMaterial(materialNode))
            {
                // Shell Material Parameters
                // Original Material not exported => only for the offline rendering in 3DS Max
                // Baked Material => used for the export
                IMtl bakedMtl = materialNode.IPropertyContainer.GetProperty(1).MaxParamBlock2.GetMtl(3, 0, 0);

                if (bakedMtl != null)
                {
                    Guid guid = bakedMtl.GetGuid();

                    for (int indexSubMaterial = 0; indexSubMaterial < materialNode.SubMaterialCount; indexSubMaterial++)
                    {
                        IIGameMaterial subMaterialNode = materialNode.GetSubMaterial(indexSubMaterial);
                        if (guid.Equals(subMaterialNode.MaxMaterial.GetGuid()))
                        {
                            return subMaterialNode;
                        }
                    }
                }
            }

            return null;
        }

        private void AddStandardBabylonAttributes(string attributesContainer, BabylonStandardMaterial babylonMaterial = null) => AddCustomAttributes(attributesContainer, MaterialScripts.StandardBabylonCAtDef, "STANDARD_MATERIAL_CAT_DEF");

        private void AddPhysicalBabylonAttributes(string attributesContainer, BabylonPBRMetallicRoughnessMaterial babylonMaterial = null) => AddCustomAttributes(attributesContainer, MaterialScripts.PhysicalBabylonCAtDef, "PHYSICAL_MATERIAL_CAT_DEF");

        private void AddAiStandardSurfaceBabylonAttributes(string attributesContainer, BabylonPBRMetallicRoughnessMaterial babylonMaterial = null) => AddCustomAttributes(attributesContainer, MaterialScripts.AIBabylonCAtDef, "ARNOLD_MATERIAL_CAT_DEF");

        private void AddCustomAttributes(string attributesContainer, string cmdCreateBabylonAttributes, string def) =>
#if MAX2022 || MAX2023 || MAX2024
            ManagedServices.MaxscriptSDK.ExecuteMaxscriptCommand(MaterialScripts.AddCustomAttribute(cmdCreateBabylonAttributes, attributesContainer, def), ManagedServices.MaxscriptSDK.ScriptSource.NotSpecified);
#else
        ManagedServices.MaxscriptSDK.ExecuteMaxscriptCommand(MaterialScripts.AddCustomAttribute(cmdCreateBabylonAttributes, attributesContainer, def));
#endif


        private void ExportCommonBabylonAttributes(IIPropertyContainer babylonAttributesContainer, BabylonMaterial babylonMaterial)
        {
            int maxSimultaneousLights = babylonAttributesContainer.GetIntProperty("babylonMaxSimultaneousLights", 4);
            maxSimultaneousLights = Math.Min(100, Math.Max(maxSimultaneousLights, 1));// force to be [1,100], because of 3DSMax Slider issue.
            RaiseVerbose($"maxSimultaneousLights={maxSimultaneousLights}", 3);
            babylonMaterial.maxSimultaneousLights = maxSimultaneousLights;

            bool unlit = babylonAttributesContainer.GetBoolProperty("babylonUnlit");
            RaiseVerbose($"unlit={unlit}", 3);
            babylonMaterial.isUnlit = unlit;
        }
    }
}