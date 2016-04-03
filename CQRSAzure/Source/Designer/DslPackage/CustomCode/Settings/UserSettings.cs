using System.Collections.Generic;
using Microsoft.VisualStudio.Settings;
using Microsoft.VisualStudio.Shell.Settings;
using Microsoft.VisualStudio.Shell;

namespace CQRSAzure.CQRSdsl.CustomCode.Settings
{
    /// <summary>
    /// Class to read/write the per-user settings for code generation and documentation to the settings store
    /// </summary>
    /// <remarks>
    /// Some settings (such as paths) can be set on a per-user basis whereas some others make sense on a per-project 
    /// basis.
    /// </remarks>
    public class UserSettings
    {
        
        public void LoadSettings(object TargetSettingsClass)
        {
            if (null != TargetSettingsClass)
            {
                SettingsManager settingsManager = new ShellSettingsManager(ServiceProvider.GlobalProvider);
                if (null != settingsManager)
                {
                    SettingsStore userSettingsStore = settingsManager.GetReadOnlySettingsStore(SettingsScope.UserSettings);

                    foreach (var prop in TargetSettingsClass.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public))
                    {
                        if (SettingStorageAttribute.IsSettingStorageAttributeSet(prop, SettingStorageAttribute.StorageTarget.UserSetting))
                        {
                            // TODO: Read the setting to the user store
                        }
                    }
                }
            }
        }

        public void SaveSettings(object TargetSettingsClass)
        {
            if (null != TargetSettingsClass)
            {
                SettingsManager settingsManager = new ShellSettingsManager(ServiceProvider.GlobalProvider);
                if (null != settingsManager)
                {
                    WritableSettingsStore userSettingsStore = settingsManager.GetWritableSettingsStore(SettingsScope.UserSettings);

                    foreach (var prop in TargetSettingsClass.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public))
                    {
                        if (SettingStorageAttribute.IsSettingStorageAttributeSet(prop, SettingStorageAttribute.StorageTarget.UserSetting))
                        {
                            // TODO: Write the setting to the user store
                        }
                    }
                }
            }
        }

    }
}
