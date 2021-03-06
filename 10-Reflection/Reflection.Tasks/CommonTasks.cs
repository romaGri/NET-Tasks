﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace Reflection.Tasks
{
    public static class CommonTasks
    {

        /// <summary>
        /// Returns the lists of public and obsolete classes for specified assembly.
        /// Pleasde take attention: classes (not interfaces, not structs)
        /// </summary>
        /// <param name="assemblyName">name of assembly</param>
        /// <returns>List of public but obsolete classes</returns>
        public static IEnumerable<string> GetPublicObsoleteClasses(string assemblyName) {
            // TODO : Implement GetPublicObsoleteClasses method
            // Type type = Type.GetType(assemblyName);
            var assembly = Assembly.Load(assemblyName);

            return assembly.GetTypes()
                .Where(x => x.IsClass && x.IsPublic && x.GetCustomAttributes(typeof(ObsoleteAttribute), true).Any())
            .Select(x => x.Name);



            // throw new NotImplementedException();
        }

        /// <summary>
        /// Returns the value for required property path
        /// </summary>
        /// <example>
        ///  1) 
        ///  string value = instance.GetPropertyValue("Property1")
        ///  The result should be equal to invoking statically
        ///  string value = instance.Property1;
        ///  2) 
        ///  string name = instance.GetPropertyValue("Property1.Property2.FirstName")
        ///  The result should be equal to invoking statically
        ///  string name = instance.Property1.Property2.FirstName;
        /// </example>
        /// <typeparam name="T">property type</typeparam>
        /// <param name="obj">source object to get property from</param>
        /// <param name="propertyPath">dot-separated property path</param>
        /// <returns>property value of obj for required propertyPath</returns>
        public static T GetPropertyValue<T>(this object obj, string propertyPath) {
            // TODO : Implement GetPropertyValue method
            var prop = propertyPath.Split('.');
            foreach (var p in prop)
            {
                obj = obj.GetType().GetProperty(p).GetValue(obj, null);
            }
            return (T)obj; 
        }
        


        /// <summary>
        /// Assign the value to the required property path
        /// </summary>
        /// <example>
        ///  1)
        ///  instance.SetPropertyValue("Property1", value);
        ///  The result should be equal to invoking statically
        ///  instance.Property1 = value;
        ///  2)
        ///  instance.SetPropertyValue("Property1.Property2.FirstName", value);
        ///  The result should be equal to invoking statically
        ///  instance.Property1.Property2.FirstName = value;
        /// </example>
        /// <param name="obj">source object to set property to</param>
        /// <param name="propertyPath">dot-separated property path</param>
        /// <param name="value">assigned value</param>
        public static void SetPropertyValue(this object obj, string propertyPath, object value) {
            // TODO : Implement SetPropertyValue method
            var property = propertyPath.Split('.');
            var newObj = property.Length == 1 ? obj : GetPropertyValue<object>(obj, String.Join(".", property.Take(property.Length - 1)));
            newObj.GetType().
                BaseType.InvokeMember(property.Last(), BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, newObj, new object[] { value });

        }


    }
}
