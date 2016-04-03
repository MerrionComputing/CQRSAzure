using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRSAzure.CQRSdsl.CustomCode.Settings
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field , AllowMultiple=false ,Inherited =false) ]
    public sealed class SettingStorageAttribute
        : Attribute 
    {

        /// <summary>
        /// Where to store this setting
        /// </summary>
        public enum StorageTarget
        {
            /// <summary>
            /// Store the setting per solution
            /// </summary>
            SolutionSetting = 0,
            /// <summary>
            /// Store the setting per user
            /// </summary>
            UserSetting = 1
        }

        readonly string m_path;
        /// <summary>
        /// folder the setting is contained in
        /// </summary>
        public string Path
        { get { return m_path; } }

        readonly string m_fieldName;
        public string FieldName
        { get { return m_fieldName; } }

        readonly StorageTarget m_target;
        public StorageTarget Target
        { get { return m_target; } }

        public SettingStorageAttribute(string pathIn, string fieldNameIn, StorageTarget targetIn)
        {
            m_path = pathIn;
            m_fieldName = fieldNameIn;
            m_target = targetIn;
        }

        public static bool IsSettingStorageAttributeSet(System.Reflection.MemberInfo prop, StorageTarget targetIn)
        {
            SettingStorageAttribute[] attr = (SettingStorageAttribute[])prop.GetCustomAttributes(typeof(SettingStorageAttribute),false ) ;
            if (attr != null)
            {
                if (attr.Length > 0)
                {
                    return (attr[0].Target == targetIn );
                }
            }
            return false;
        }

    }
}
