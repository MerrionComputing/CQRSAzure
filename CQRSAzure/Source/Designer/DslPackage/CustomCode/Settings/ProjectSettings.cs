using System.Collections.Generic;
using Microsoft.VisualStudio.Settings;
using Microsoft.VisualStudio.Shell.Settings;
using CQRSAzure.CQRSdsl.Dsl;

namespace CQRSAzure.CQRSdsl.CustomCode.Settings
{
    /// <summary>
    /// Class to read/write the per-prject project for code generation and documentation to the settings store
    /// </summary>
    /// <remarks>
    /// Some settings (such as paths) can be set on a per-user basis whereas some others make sense on a per-project 
    /// basis.  This does not use Microsoft.VisualStudio.Shell.Settings but rather embeds the settings in the DSL model
    /// file
    /// </remarks>
    public class ProjectSettings
    {


        /// <summary>
        /// Populate the target settings class from the CQRS model's internal settings collaection
        /// </summary>
        /// <param name="TargetSettingsClass">
        /// The class we are populating 
        /// </param>
        /// <param name="modelWithSettings">
        /// The model (stored in xml) that contains the settings
        /// </param>
        public void LoadSettings(object TargetSettingsClass, CQRSModel modelWithSettings )
        {
            if (null != TargetSettingsClass)
            {
                if (null != modelWithSettings)
                {

                }
            }
        }

        /// <summary>
        /// Save the project level settings in the settings object passed in into the CQRSModel
        /// </summary>
        /// <param name="TargetSettingsClass">
        /// The class populated with the settings to save
        /// </param>
        /// <param name="modelWithSettings">
        /// The CQRS model to save them into
        /// </param>
        public void SaveSettings(object TargetSettingsClass, CQRSModel modelWithSettings)
        {
            if (null != TargetSettingsClass)
            {
                if (null != modelWithSettings)
                {

                }
            }
        }

    }
}
