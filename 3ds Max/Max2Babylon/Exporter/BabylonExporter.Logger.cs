using Autodesk.Max;
using BabylonExport.Entities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace Max2Babylon
{
    internal partial class BabylonExporter
    {
        public enum LogLevel
        {
            ERROR,
            WARNING,
            MESSAGE,
            VERBOSE
        }

        // TODO - Update log level for release
        //.NL.Cambiato il livello di logging da MESSAGE a VERBOSE
        //public LogLevel logLevel = LogLevel.MESSAGE;
        public LogLevel logLevel = LogLevel.VERBOSE;

        public event Action<int> OnExportProgressChanged;
        public event Action<string, int> OnError;
        public event Action<string, int> OnWarning;
        public event Action<string, Color, int, bool> OnMessage;
        public event Action<string, Color, int, bool> OnVerbose;

        public void ReportProgressChanged(int progress)
        {
            if (OnExportProgressChanged != null)
            {
                OnExportProgressChanged(progress);
            }
        }

        public void ReportProgressChanged(float progress)
        {
            ReportProgressChanged((int)progress);
        }

        public void RaiseError(string error, int rank = 0)
        {
            if (OnError != null && logLevel >= LogLevel.ERROR)
            {
                OnError(error, rank);
            }
        }

        public void RaiseWarning(string warning, int rank = 0)
        {
            if (OnWarning != null && logLevel >= LogLevel.WARNING)
            {
                OnWarning(warning, rank);
            }
        }

        public void RaiseMessage(string message, int rank = 0, bool emphasis = false)
        {
            RaiseMessage(message, Color.Black, rank, emphasis);
        }

        public void RaiseMessage(string message, Color color, int rank = 0, bool emphasis = false)
        {
            if (OnMessage != null && logLevel >= LogLevel.MESSAGE)
            {
                OnMessage(message, color, rank, emphasis);
            }
        }

        public void RaiseVerbose(string message, int rank = 0, bool emphasis = false)
        {
            RaiseVerbose(message, Color.FromArgb(100, 100, 100), rank, emphasis);
        }

        public void RaiseVerbose(string message, Color color, int rank = 0, bool emphasis = false)
        {
            if (OnVerbose != null && logLevel >= LogLevel.VERBOSE)
            {
                OnVerbose(message, color, rank, emphasis);
            }
        }

        public void Print(IIParamBlock2 paramBlock, int logRank)
        {
            RaiseVerbose("paramBlock=" + paramBlock, logRank);
            if (paramBlock != null)
            {
                RaiseVerbose("paramBlock.NumParams=" + paramBlock.NumParams, logRank + 1);
                for (short i = 0; i < paramBlock.NumParams; i++)
                {
                    ParamType2 paramType = paramBlock.GetParameterType(i);

#if MAX2022 || MAX2023 || MAX2024
                    RaiseVerbose("paramBlock.GetLocalName(" + i + ")=" + paramBlock.GetLocalName(i, 0, false) + ", type=" + paramType, logRank + 1);
#else
                    RaiseVerbose("paramBlock.GetLocalName(" + i + ")=" + paramBlock.GetLocalName(i, 0) + ", type=" + paramType, logRank + 1);
#endif
                    switch (paramType)
                    {
                        case ParamType2.String:
                            RaiseVerbose("paramBlock.GetProperty(" + i + ")=" + paramBlock.GetStr(i, 0, 0), logRank + 2);
                            break;
                        case ParamType2.Int:
                            RaiseVerbose("paramBlock.GetProperty(" + i + ")=" + paramBlock.GetInt(i, 0, 0), logRank + 2);
                            break;
                        case ParamType2.Float:
                            RaiseVerbose("paramBlock.GetProperty(" + i + ")=" + paramBlock.GetFloat(i, 0, 0), logRank + 2);
                            break;
                        case ParamType2.Bool2:
                            RaiseVerbose("paramBlock.GetProperty(" + i + ")=" + paramBlock.GetInt(i, 0, 0), logRank + 2);
                            break;
                        case ParamType2.Rgba:
                            RaiseVerbose("paramBlock.GetProperty(" + i + ")=" + paramBlock.GetColor(i, 0, 0), logRank + 2);
                            break;
                        default:
                            RaiseVerbose("Unknown property type", logRank + 2);
                            break;
                    }
                }
            }
        }

        public void Print(IIPropertyContainer propertyContainer, int logRank)
        {
            RaiseVerbose("propertyContainer=" + propertyContainer, logRank);
            if (propertyContainer != null)
            {
                RaiseVerbose("propertyContainer.NumberOfProperties=" + propertyContainer.NumberOfProperties, logRank + 1);
                for (int i = 0; i < propertyContainer.NumberOfProperties; i++)
                {
                    var prop = propertyContainer.GetProperty(i);
                    if (prop != null)
                    {
                        RaiseVerbose("propertyContainer.GetProperty(" + i + ")=" + prop.Name, logRank + 1);
                        switch (prop.GetType_)
                        {
                            case PropType.StringProp:
                                string propertyString = "";
                                RaiseVerbose("prop.GetPropertyValue(ref propertyString, 0)=" + prop.GetPropertyValue(ref propertyString, 0), logRank + 2);
                                RaiseVerbose("propertyString=" + propertyString, logRank + 2);
                                break;
                            case PropType.IntProp:
                                int propertyInt = 0;
                                RaiseVerbose("prop.GetPropertyValue(ref propertyInt, 0)=" + prop.GetPropertyValue(ref propertyInt, 0), logRank + 2);
                                RaiseVerbose("propertyInt=" + propertyInt, logRank + 2);
                                break;
                            case PropType.FloatProp:
                                float propertyFloat = 0;
                                RaiseVerbose("prop.GetPropertyValue(ref propertyFloat, 0, true)=" + prop.GetPropertyValue(ref propertyFloat, 0, true), logRank + 2);
                                RaiseVerbose("propertyFloat=" + propertyFloat, logRank + 2);
                                RaiseVerbose("prop.GetPropertyValue(ref propertyFloat, 0, false)=" + prop.GetPropertyValue(ref propertyFloat, 0, false), logRank + 2);
                                RaiseVerbose("propertyFloat=" + propertyFloat, logRank + 2);
                                break;
                            case PropType.Point3Prop:
                                IPoint3 propertyPoint3 = Loader.Global.Point3.Create(0, 0, 0);
                                RaiseVerbose("prop.GetPropertyValue(ref propertyPoint3, 0)=" + prop.GetPropertyValue(propertyPoint3, 0), logRank + 2);
                                RaiseVerbose("propertyPoint3=" + Point3ToString(propertyPoint3), logRank + 2);
                                break;
                            case PropType.Point4Prop:
                                IPoint4 propertyPoint4 = Loader.Global.Point4.Create(0, 0, 0, 0);
                                RaiseVerbose("prop.GetPropertyValue(ref propertyPoint4, 0)=" + prop.GetPropertyValue(propertyPoint4, 0), logRank + 2);
                                RaiseVerbose("propertyPoint4=" + Point4ToString(propertyPoint4), logRank + 2);
                                break;
                            case PropType.UnknownProp:
                            default:
                                string propv = tryPropType(prop);

                                if (propv != string.Empty)
                                    RaiseVerbose("Unknown property type: " + propv, logRank + 2);
                                break;
                        }
                    }
                    else
                    {
                        RaiseVerbose("propertyContainer.GetProperty(" + i + ") IS NULL", logRank + 1);
                    }
                }
            }
        }


        /// <summary>
        /// VRMUR - Export the groups of values
        /// 
        /// Esporta nei metadati le proprietà valorizzate
        /// </summary>
        /// <param name="propertyContainer"></param>
        /// <param name="babylonMaterial"></param>
        private void getGroupedProperties(IIPropertyContainer propertyContainer, BabylonPBRMetallicRoughnessMaterial babylonMaterial)
        {
            if (propertyContainer == null)
                return;

            if (babylonMaterial.metadata == null)
                babylonMaterial.metadata = new Dictionary<string, object>();

            //Quelle commentate vengono già esportate come pbrMetallicRoughness
            //getGroupedProperty(propertyContainer, babylonMaterial.metadata, 2, null);                         //base_color
            //getGroupedProperty(propertyContainer, babylonMaterial.metadata, 4, new int[] { 5 });              //roughness
            //getGroupedProperty(propertyContainer, babylonMaterial.metadata, 6, null);                         //metalness
            getGroupedProperty(propertyContainer, babylonMaterial.metadata, 3, null);                           //reflectivity
            getGroupedProperty(propertyContainer, babylonMaterial.metadata, 7, null);                           //refl_color
            getGroupedProperty(propertyContainer, babylonMaterial.metadata, 8, null);                           //diff_roughness

            getGroupedProperty(propertyContainer, babylonMaterial.metadata, 9, new int[] { 10, 11, 12 });                           //Brdf mode
            getGroupedProperty(propertyContainer, babylonMaterial.metadata, 13, new int[] { 14, 15, 16 });                          //Asinotropy
            getGroupedProperty(propertyContainer, babylonMaterial.metadata, 17, new int[] { 18, 19, 20, 21, 22, 23, 24, 25, 26 });  //Transparency
            getGroupedProperty(propertyContainer, babylonMaterial.metadata, 28, new int[] { 29, 30, 31, 32 });                      //Subsurface scattering
            getGroupedProperty(propertyContainer, babylonMaterial.metadata, 33, new int[] { 34, 35, 36 });                          //Emission
            getGroupedProperty(propertyContainer, babylonMaterial.metadata, 37, new int[] { 39, 39, 40, 41, 42, 43, 44, 45, 117 }); //Coating
            getGroupedProperty(propertyContainer, babylonMaterial.metadata, 46, new int[] { 47, 48 });                              //Sheen
            getGroupedProperty(propertyContainer, babylonMaterial.metadata, 49, new int[] { 50, 51 });                              //Thin Film
        }

        /// <summary>
        /// VRMUR - Export a group values
        /// 
        /// Esporta tutti i metadati se quello di reiferimento è valorizzato
        /// </summary>
        /// <param name="propertyContainer"></param>
        /// <param name="metadata"></param>
        /// <param name="mainIndex"></param>
        /// <param name="otherIndex"></param>
        public void getGroupedProperty(IIPropertyContainer propertyContainer, Dictionary<string, object> metadata, int mainIndex, int[] otherIndex)
        {
            if (propertyContainer == null)
                return;

            var prop = propertyContainer.GetProperty(mainIndex);

            if (isPropValued(prop))
            {
                var value = getPropertyValue(prop);

                RaiseVerbose("Property group: " + prop.Name + " -> " + value, 2);

                if (!metadata.ContainsKey(prop.Name))
                    metadata.Add(prop.Name, value);

                if (otherIndex != null)
                {
                    foreach (int idx in otherIndex)
                    {
                        prop = propertyContainer.GetProperty(idx);

                        if (prop != null)
                        {
                            value = getPropertyValue(prop);
                            if (!metadata.ContainsKey(prop.Name))
                                metadata.Add(prop.Name, value);
                        }
                    }
                }
            }
        }

        private string tryPropType(IIGameProperty prop) {
            string propV = "";
            bool isV = false;

            try
            {
                IPoint4 pp4 = Loader.Global.Point4.Create(0, 0, 0, 0);
                prop.GetPropertyValue(pp4, 0);
                propV+= " Point4 -> " + Point4ToString(pp4);
                if (pp4.X != 0 || pp4.Y != 0 || pp4.Z != 0 || pp4.W != 0)
                    isV = true;
            }
            catch { }
            try
            {
                IPoint3 pp3 = Loader.Global.Point3.Create(0, 0, 0);
                prop.GetPropertyValue(pp3, 0);
                propV += " Point3 -> " + Point3ToString(pp3);
                if (pp3.X != 0 || pp3.Y != 0 || pp3.Z != 0)
                    isV = true;
            }
            catch { }
            try
            {
                float propertyFloatT = 0;
                float propertyFloatF = 0;
                prop.GetPropertyValue(ref propertyFloatT, 0, true);
                prop.GetPropertyValue(ref propertyFloatF, 0, false);
                propV += " float -> " + propertyFloatT + " : " + propertyFloatF;
                if (propertyFloatF != 0 || propertyFloatT != 0)
                    isV = true;
            }
            catch { }
            try
            {
                int propertyInt = 0;
                prop.GetPropertyValue(ref propertyInt, 0);
                propV += " int -> " + propertyInt;
                if (propertyInt != 0)
                    isV = true;
            }
            catch { }
            try
            {
                string propertyString = "";
                prop.GetPropertyValue(ref propertyString, 0);
                propV += " string -> " +  propertyString;
                if (!String.IsNullOrWhiteSpace(propertyString))
                    isV = true;
            }
            catch { }

            if (isV)
                return propV;

            return string.Empty;
        }

        private bool isPropValued(IIGameProperty prop)
        {
            if (prop == null) 
                return false;

            IPoint4 pp4 = Loader.Global.Point4.Create(0, 0, 0, 0);
            prop.GetPropertyValue(pp4, 0);
            if (pp4.X != 0 || pp4.Y != 0 || pp4.Z != 0 || pp4.W != 0)
                return true;

            IPoint3 pp3 = Loader.Global.Point3.Create(0, 0, 0);
            prop.GetPropertyValue(pp3, 0);
            if (pp3.X != 0 || pp3.Y != 0 || pp3.Z != 0)
                return true;

            float propertyFloatT = 0;
            float propertyFloatF = 0;
            prop.GetPropertyValue(ref propertyFloatT, 0, true);
            prop.GetPropertyValue(ref propertyFloatF, 0, false);
            if (propertyFloatF != 0 || propertyFloatT != 0)
                return true;

            int propertyInt = 0;
            prop.GetPropertyValue(ref propertyInt, 0);
            if (propertyInt != 0)
                return true;

            string propertyString = "";
            prop.GetPropertyValue(ref propertyString, 0);
            if (!String.IsNullOrWhiteSpace(propertyString))
                return true;

            return false;
        }

        public object getPropertyValue(IIGameProperty prop)
        {
            if (prop != null)
            {
                switch (prop.GetType_)
                {
                    case PropType.StringProp:
                        string propertyString = "";
                        prop.GetPropertyValue(ref propertyString, 0);
                        return propertyString;

                    case PropType.IntProp:
                        int propertyInt = 0;
                        prop.GetPropertyValue(ref propertyInt, 0);
                        return propertyInt;

                    case PropType.FloatProp:
                        float propertyFloatT = 0;
                        float propertyFloatF = 0;
                        prop.GetPropertyValue(ref propertyFloatT, 0, true);
                        prop.GetPropertyValue(ref propertyFloatF, 0, false);
                        return new float[2] { propertyFloatT, propertyFloatF };

                    case PropType.Point3Prop:
                        IPoint3 propertyPoint3 = Loader.Global.Point3.Create(0, 0, 0);
                        prop.GetPropertyValue(propertyPoint3, 0);
                        return propertyPoint3.ToArray();

                    case PropType.Point4Prop:
                        IPoint4 propertyPoint4 = Loader.Global.Point4.Create(0, 0, 0, 0);
                        prop.GetPropertyValue(propertyPoint4, 0);
                        return propertyPoint4.ToArray();

                    case PropType.UnknownProp:
                    default:
                        return null;
                }
            }
            else
            {
                return null;
            }
        }

        public string propertyValueToString(IIGameProperty prop)
        {
            if (prop != null)
            {
                switch (prop.GetType_)
                {
                    case PropType.StringProp:
                        string propertyString = "";
                        prop.GetPropertyValue(ref propertyString, 0);
                        return propertyString;

                    case PropType.IntProp:
                        int propertyInt = 0;
                        prop.GetPropertyValue(ref propertyInt, 0);
                        return propertyInt.ToString();

                    case PropType.FloatProp:
                        float propertyFloatT = 0;
                        float propertyFloatF = 0;
                        prop.GetPropertyValue(ref propertyFloatT, 0, true);
                        prop.GetPropertyValue(ref propertyFloatF, 0, false);
                        return $"{propertyFloatT}, {propertyFloatF}";

                    case PropType.Point3Prop:
                        IPoint3 propertyPoint3 = Loader.Global.Point3.Create(0, 0, 0);
                        prop.GetPropertyValue(propertyPoint3, 0);
                        return Point3ToString(propertyPoint3);

                    case PropType.Point4Prop:
                        IPoint4 propertyPoint4 = Loader.Global.Point4.Create(0, 0, 0, 0);
                        prop.GetPropertyValue(propertyPoint4, 0);
                        return Point4ToString(propertyPoint4);

                    case PropType.UnknownProp:
                    default:
                        return null;
                }
            }
            else
            {
                return null;
            }
        }


        // -------------------------
        // --------- Utils ---------
        // -------------------------


        private float[] Point4toFloat(IPoint4 point)
        {
            return new float[4] { point.X, point.Y, point.Z, point.W };
        }


        private string ColorToString(IColor color)
        {
            if (color == null)
            {
                return "";
            }

            return "{ r=" + color.R + ", g=" + color.G + ", b=" + color.B + " }";
        }

        private string Point3ToString(IPoint3 point)
        {
            if (point == null)
            {
                return "";
            }

            return "{ x=" + point.X + ", y=" + point.Y + ", z=" + point.Z + " }";
        }

        private string Point4ToString(IPoint4 point)
        {
            if (point == null)
            {
                return "";
            }

            return "{ x=" + point.X + ", y=" + point.Y + ", z=" + point.Z + ", w=" + point.W + " }";
        }
    }
}
