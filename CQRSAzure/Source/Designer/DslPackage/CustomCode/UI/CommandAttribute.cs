using System;
using System.ComponentModel.Design;
using System.Diagnostics;

namespace CQRSAzure.CQRSdsl.Dsl
{
    /// <summary>
    /// Applying this attribute on a class makes that class a command feature.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class CommandAttribute : FeatureAttribute
    {
        /// <summary>
        /// The type of corresponding MetaFeature class (MetaCommand).
        /// </summary>
        public override Type MetaFeature
        {
            [DebuggerStepThrough]
            get { return typeof(MenuCommand); }
        }
    }
}