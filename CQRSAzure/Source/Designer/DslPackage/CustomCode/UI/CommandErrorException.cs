using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Windows.Forms;

namespace CQRSAzure.CQRSdsl.Dsl
{
    #region CommandErrorException

    /// <summary>
    /// This exception is thrown by command methods when command can't execute as expected.
    /// The message of this exception is shown to the user in a message dialog box with the icon provided.
    /// </summary>
    [Serializable]
    public class CommandErrorException : Exception
    {
        private MessageBoxIcon m_icon = MessageBoxIcon.Warning;

        /// <summary>
        /// Gets icon to be displayed to the user on the message box dialog.
        /// </summary>
        public MessageBoxIcon Icon
        {
            [DebuggerStepThrough]
            get { return m_icon; }
        }

        public CommandErrorException(MessageBoxIcon icon, string message)
            : base(message)
        {
            m_icon = icon;
        }

        public CommandErrorException() { }
        public CommandErrorException(string message) : base(message) { }
        public CommandErrorException(string message, Exception inner) : base(message, inner) { }
        protected CommandErrorException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        [SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands")]
        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null) throw new ArgumentNullException("info");

            base.GetObjectData(info, context);
            info.AddValue("icon", m_icon);
        }
    }

    #endregion
}