﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeRoutine.Routine.BuildYourOwnRoutine.Extension
{
    public abstract class ExtensionComponent
    {
        /// <summary>
        /// Extensions that owns this component
        /// </summary>
        public String Owner { get; protected set; }

        /// <summary>
        /// Name of this component. This should be unique.
        /// </summary>
        public String Name { get; protected set; }

        /// <summary>
        /// Used to initalize the instance's values, especially when loading a profile.
        /// </summary>
        /// <param name="Parameters"></param>
        public abstract void Initialise(Dictionary<String, Object> Parameters);

        /// <summary>
        /// Called when configuring this component. Should draw all necessary configuration and save it in the parameters dictionary.
        /// </summary>
        /// <param name="Parameters"></param>
        /// <returns>Not currently used. Recommended to return true.</returns>
        public abstract bool CreateConfigurationMenu(ExtensionParameter extensionParameter, ref Dictionary<String, Object> Parameters);

        public static String InitialiseParameterString(String parameterName, String defaultValue, ref Dictionary<String, Object> Parameters)
        {
            return Parameters.TryGetValue(parameterName, out object value) ? (string)value : defaultValue;
        }

        public static Boolean InitialiseParameterBoolean(String parameterName, Boolean defaultValue, ref Dictionary<String, Object> Parameters)
        {
            return Parameters.TryGetValue(parameterName, out object value) ? Boolean.Parse((string)value) : defaultValue;
        }

        public static Int32 InitialiseParameterInt32(String parameterName, Int32 defaultValue, ref Dictionary<String, Object> Parameters)
        {
            return Parameters.TryGetValue(parameterName, out object value) ? Int32.Parse((string)value) : defaultValue;
        }
    }
}
