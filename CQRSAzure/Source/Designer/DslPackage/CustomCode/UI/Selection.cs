using Microsoft.VisualStudio;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using VsShell = Microsoft.VisualStudio.Shell.Interop;

namespace CQRSAzure.CQRSdsl.Dsl
{
    /// <summary>
    /// General VS selection representation.
    /// </summary>
    [CLSCompliant(false)]
    public class Selection
    {
        #region Private data, factory method

        private IServiceProvider m_serviceProvider;
        private VsShell.ISelectionContainer m_selectionContainer;
        private List<object> m_selectedObjects;

        [SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands")]
        public static T Create<T>(IServiceProvider serviceProvider, VsShell.ISelectionContainer selectionContainer)
            where T : Selection, new()
        {
            if (serviceProvider == null) throw new ArgumentNullException("serviceProvider");

            T selection = new T();
            selection.m_serviceProvider = serviceProvider;

            // If no selection container provided, initialize it with the current selection in VS
            if (selectionContainer == null)
            {
                VsShell.IVsMonitorSelection monitorService = serviceProvider.GetService(typeof(VsShell.IVsMonitorSelection)) as VsShell.IVsMonitorSelection;
                Debug.Assert(monitorService != null);
                if (monitorService != null)
                {
                    IntPtr ppHier, ppSC;
                    uint pitemid;
                    VsShell.IVsMultiItemSelect ppMIS;
                    ErrorHandler.ThrowOnFailure(monitorService.GetCurrentSelection(out ppHier, out pitemid, out ppMIS, out ppSC));

                    if (ppSC != IntPtr.Zero)
                        selectionContainer = Marshal.GetObjectForIUnknown(ppSC) as VsShell.ISelectionContainer;
                }
            }
            selection.m_selectionContainer = selectionContainer;

            return selection;
        }

        #endregion

        #region Selection properties

        public IServiceProvider ServiceProvider
        {
            [DebuggerStepThrough]
            get { return m_serviceProvider; }
        }

        public VsShell.ISelectionContainer SelectionContainer
        {
            [DebuggerStepThrough]
            get { return m_selectionContainer; }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public IList<object> SelectedObjects
        {
            [DebuggerStepThrough]
            get
            {
                if (m_selectedObjects == null)
                {
                    m_selectedObjects = new List<object>();
                    if (m_selectionContainer != null)
                    {
                        uint count;
                        m_selectionContainer.CountObjects(2 /*selected*/, out count);
                        if (count > 0)
                        {
                            object[] result = new object[count];
                            m_selectionContainer.GetObjects(2 /*selected*/, count, result);
                            m_selectedObjects.AddRange(result);
                        }
                    }
                }
                return m_selectedObjects;
            }
        }

        #endregion

        #region Filtering methods

        public int Count<T>() where T : class
        {
            return Count<T>(null);
        }

        public int Count<T>(Predicate<T> predicate) where T : class
        {
            int result = 0;
            for (int i = 0; i < this.SelectedObjects.Count; i++)
            {
                T obj = this.SelectedObjects[i] as T;
                if (obj != null && (predicate == null || predicate(obj)))
                    result++;
            }
            return result;
        }

        public T FindFirst<T>() where T : class
        {
            return FindFirst<T>(null);
        }

        public T FindFirst<T>(Predicate<T> predicate) where T : class
        {
            for (int i = 0; i < this.SelectedObjects.Count; i++)
            {
                T obj = this.SelectedObjects[i] as T;
                if (obj != null && (predicate == null || predicate(obj)))
                    return obj;
            }
            return null;
        }

        public bool Exists<T>() where T : class
        {
            return FindFirst<T>(null) != null;
        }

        public bool Exists<T>(Predicate<T> predicate) where T : class
        {
            return FindFirst<T>(predicate) != null;
        }

        public IEnumerable<T> Filter<T>() where T : class
        {
            return Filter<T>(null);
        }

        public IEnumerable<T> Filter<T>(Predicate<T> predicate) where T : class
        {
            for (int i = 0; i < this.SelectedObjects.Count; i++)
            {
                T obj = this.SelectedObjects[i] as T;
                if (obj != null && (predicate == null || predicate(obj)))
                {
                    yield return obj;
                }
            }
        }

        #endregion
    }
}