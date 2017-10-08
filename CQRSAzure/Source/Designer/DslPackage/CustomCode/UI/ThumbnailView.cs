//------------------------------------------------------------------------------
// <copyright file="ThumbnailView.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Reflection;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Diagnostics;
using System.ComponentModel.Design;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Shell;

namespace CQRSAzure.CQRSdsl.Dsl
{
    #region Pan/Zoom Tool Window

    [Feature(Name = "Pan & Zoom", Container = typeof(DesignerContext),
		Description = "Provides a pan/zoom tool window for faster navigation on the diagram.")]
	internal sealed class PanZoomWindow : AddInToolWindow<PanZoomPanel>
    {
		private bool m_needsRefresh;
		private DiagramDocView m_currentDocView;
		private TransactionCommitHandler m_transactionHandler;

        public PanZoomWindow(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            m_transactionHandler = new TransactionCommitHandler(OnTransactionCommited);
			// updates to the thumbnail image happen on idle
            Application.Idle += new EventHandler(OnApplicationIdle);
        }

        protected override void OnDocumentWindowChanged(ModelingDocView oldView, ModelingDocView newView)
        {
            PanZoomPanel panel = this.Window as PanZoomPanel;
            if (panel != null)
            {
				// detach from previous document view
				if (m_currentDocView != null)
                {
					UnsubscribeFromEvents(m_currentDocView);
					m_currentDocView = null;
                }

                DiagramDocView docView = newView as DiagramDocView;
                if (docView != null && docView.CurrentDesigner != null)
                {
					// attach to the new document view
					SubscribeToEvents(docView);
					m_currentDocView = docView;

					// make sure thumbnail image is updated
                    panel.InvalidateImage(docView.CurrentDesigner.DiagramClientView);
                }
                else
                {
					// document is not recognized - erase the image
                    panel.InvalidateImage(null);
                }
            }
        }

		protected override void Dispose(bool disposing)
		{
            try
            {
                if (disposing)
                {
                    if (m_currentDocView != null)
                    {
                        UnsubscribeFromEvents(m_currentDocView);
                        m_currentDocView = null;
                    }
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
		}

		private void SubscribeToEvents(DiagramDocView docView)
		{
			Debug.Assert(docView != null);
			if (docView != null)
			{
				if (docView.CurrentDesigner != null)
				{
					DiagramClientView diagramClientView = docView.CurrentDesigner.DiagramClientView;
					Debug.Assert(diagramClientView != null);
					if (diagramClientView != null)
					{
						// Watch scroll and zoom changes so that view bounds are updated
						// when user scrolls/zooms the diagram via usual means.
						diagramClientView.ScrollPositionChanged += OnScrollPositionChanged;
						diagramClientView.ZoomChanged += OnZoomChanged;

						m_needsRefresh = true;
					}
				}

				if (docView.CurrentDiagram != null)
				{
					Store store = docView.CurrentDiagram.Store;
					Debug.Assert(store != null && IsValidModelStore(store) );
					if (store != null && IsValidModelStore(store))
					{
						// Watch any transactions so that image itself is updated when changes
						// are made to the diagram.
						store.EventManagerDirectory.TransactionCommitted.Add(m_transactionHandler);
					}
				}
			}
		}

        private bool IsValidModelStore(Store store)
        {
            if (store.ShuttingDown )
            {
                return false;
            }
            if (store.DemandLoading)
            {
                return false;
            }
            return true;
        }

        private void UnsubscribeFromEvents(DiagramDocView docView)
		{
			Debug.Assert(docView != null);
			if (docView != null)
			{
				if (docView.CurrentDesigner != null)
				{
					DiagramClientView diagramClientView = docView.CurrentDesigner.DiagramClientView;
					Debug.Assert(diagramClientView != null);
					if (diagramClientView != null)
					{
						diagramClientView.ScrollPositionChanged -= OnScrollPositionChanged;
						diagramClientView.ZoomChanged -= OnZoomChanged;
					}
				}

				if (docView.CurrentDiagram != null)
				{
					Store store = docView.CurrentDiagram.Store;
					Debug.Assert(store != null && IsValidModelStore(store));
					if (store != null && IsValidModelStore(store))
					{
						store.EventManagerDirectory.TransactionCommitted.Remove(m_transactionHandler);
					}
				}
			}
		}

        private void OnApplicationIdle(object sender, EventArgs e)
        {
            if (m_needsRefresh && m_currentDocView != null)
            {
                m_needsRefresh = false;
				PanZoomPanel panel = this.Window as PanZoomPanel;
				Debug.Assert(panel != null);
				if (panel != null)
				{
					// Invalidate the current image if there were changes during last idle period.
					panel.InvalidateImage();
				}
            }
        }

        private void OnZoomChanged(object sender, DiagramEventArgs e)
        {
            m_needsRefresh = true;
        }

        private void OnScrollPositionChanged(object sender, DiagramEventArgs e)
        {
            m_needsRefresh = true;
        }

        private void OnTransactionCommited()
        {
            m_needsRefresh = true;
        }
    }

    [Command(Name = "Pan && &Zoom Window", Container = typeof(DesignerContext),
		Description = "Adds a command to View | Other Windows main menu to display pan/zoom tool window.")]
    [CommandPlacement(CommandBarId.ViewOtherWindowsMenu)]
	internal sealed class ViewPanZoomWindowCommand : CustomCommand<Selection>
    {
        public ViewPanZoomWindowCommand(CommandID commandId) : base(commandId) { }

        protected override void OnInvoke()
        {
            IFeatureContext featureContext = this.ServiceProvider.GetService<IFeatureContext>();
            Debug.Assert(featureContext != null);
            if (featureContext != null)
            {
                PanZoomWindow toolWindow = featureContext.CreateFeature<PanZoomWindow>();
                Debug.Assert(toolWindow != null);
                if (toolWindow != null)
                {
                    toolWindow.Show();
                }
            }
        }
    }

    #endregion

    #region Thumbnail View form and feature

    [Feature(Name = "Thumbnail View Feature", Container = typeof(DiagramContext),
		Description = "Adds a thumbnail view control at the bottom-right corner of all designer windows.")]
    internal sealed class ThumbnailViewFeature : Feature
    {
        private Panel m_scrollPanel;

        public override void Associate(MetaFeature metaFeature, ITypedServiceProvider serviceProvider)
        {
            if (metaFeature == null) throw new ArgumentNullException("metaFeature");
            if (serviceProvider == null) throw new ArgumentNullException("serviceProvider");

            base.Associate(metaFeature, serviceProvider);

            Debug.Assert(m_scrollPanel == null);
            if (m_scrollPanel == null)
            {
                DiagramContext diagramContext = serviceProvider.GetService<IFeatureContext>() as DiagramContext;
                Debug.Assert(diagramContext != null && diagramContext.DiagramView != null);
				if (diagramContext != null && diagramContext.DiagramView != null)
                {
					// Create and show a thumbnail form.
                    DiagramView diagramView = diagramContext.DiagramView;
                    m_scrollPanel = new Panel();
                    m_scrollPanel.BackgroundImage = new Bitmap(typeof(ThumbnailViewForm), @"ThumbnailView.bmp");
                    m_scrollPanel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
                    m_scrollPanel.BackgroundImageLayout = ImageLayout.Center;
                    diagramView.Invoke(new EventHandler(delegate(object sender, EventArgs e)
                    {
                        diagramView.Controls.Add(m_scrollPanel);
                    }));
                    m_scrollPanel.Width = diagramView.Controls[1].Width;
                    m_scrollPanel.Height = diagramView.Controls[2].Height;
                    m_scrollPanel.Left = diagramView.Width - m_scrollPanel.Width;
                    m_scrollPanel.Top = diagramView.Height - m_scrollPanel.Height;
                    m_scrollPanel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
                    m_scrollPanel.BringToFront();
                    m_scrollPanel.MouseDown += delegate(object sender, MouseEventArgs args)
                    {
						// Right-click on the thumbnail icon brings pan/zoom tool window.
						Debug.Assert(args != null);
                        if (args != null && args.Button == MouseButtons.Right)
                        {
                            IFeatureContext featureContext = this.ServiceProvider.GetService<IFeatureContext>();
                            Debug.Assert(featureContext != null);
                            if (featureContext != null)
                            {
                                PanZoomWindow toolWindow = featureContext.CreateFeature<PanZoomWindow>();
                                Debug.Assert(toolWindow != null);
                                if (toolWindow != null)
                                {
                                    toolWindow.Show();
                                }
                            }
                        }
                        else
                        {
							// Any other click on thumbnail icon displays the thumbnail form.
                            new ThumbnailViewForm(m_scrollPanel, diagramView.DiagramClientView).ShowDialog();
                        }
                    };
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    if (m_scrollPanel != null)
                    {
                        if (!m_scrollPanel.IsDisposed)
                            m_scrollPanel.Dispose();
                        m_scrollPanel = null;
                    }
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }
    }

	/// <summary>
	/// A thumbnail form class to host a pan/zoom control.
	/// </summary>
	internal sealed class ThumbnailViewForm : Form
	{
		// width/height of the window.
		private const int ViewSize = 180;
		// control itself
		private PanZoomPanel m_panZoomPanel;

		internal ThumbnailViewForm(Control baseControl, DiagramClientView diagramClientView)
		{
			if (baseControl == null) throw new ArgumentNullException("baseControl");
			if (diagramClientView == null) throw new ArgumentNullException("diagramClientView");

			// Initialize the form.
            this.TopMost = true;
            this.ShowInTaskbar = false;
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.Manual;

			// Position form so that its center lines up with the center of thumbnail control
			// at designer's bottom-right corner.
            Point location = baseControl.PointToScreen(new Point(baseControl.Width / 2, baseControl.Height / 2));
            location.Offset(-ViewSize / 2, -ViewSize / 2);
            this.Bounds = new Rectangle(location.X, location.Y, ViewSize, ViewSize);

			// Make sure thumbnail form fits the screen and doesn't go below or off the right
			// edge of the screen.
			Rectangle screenBounds = Screen.FromControl(diagramClientView).WorkingArea;
			if (this.Right > screenBounds.Right)
                this.Left = screenBounds.Right - this.Width;
            if (this.Bottom > screenBounds.Bottom)
                this.Top = screenBounds.Bottom - this.Height;

			// Initialize a panel to host pan/zoom control.
			Panel panel1 = new Panel();
			panel1.Dock = DockStyle.Fill;
			panel1.BorderStyle = BorderStyle.FixedSingle;
			this.Controls.Add(panel1);

			// Initialize and dock pan/zoom control on the panel.
			m_panZoomPanel = new PanZoomPanel();
			m_panZoomPanel.Dock = DockStyle.Fill;
			panel1.Controls.Add(m_panZoomPanel);
			m_panZoomPanel.InvalidateImage(diagramClientView);

			Cursor.Hide();
		}

		protected override CreateParams CreateParams
		{
            [DebuggerStepThrough]
			get
			{
				// Give this form a nice shadow.
				CreateParams createParams = base.CreateParams;
				Debug.Assert(createParams != null);
				createParams.ClassStyle |= 0x00020000;
				return createParams;
			}
		}

		protected override void OnLoad(EventArgs e)
		{
			Debug.Assert(e != null);
			Debug.Assert(m_panZoomPanel != null);

			Point initialMousePos = Cursor.Position;
			m_panZoomPanel.MouseUp += delegate(object sender, MouseEventArgs args)
			{
				// When mouse is released, the form should go away.
				Cursor.Position = initialMousePos;
				this.Close();
				Cursor.Show();
			};

			// We automatically start moving diagram view on load.
			m_panZoomPanel.StartMove();
		}
	}

    #endregion

    /// <summary>
    /// Diagram thumbnail control to be used in pan/zoom window and thumbnail view features.
    /// </summary>
    internal sealed class PanZoomPanel : Control
    {
        #region Private fields, types

        private DiagramClientView m_diagramClientView;
        private Bitmap m_diagramImage;
        private double m_imageScale;

		private enum MouseMode
		{
			None,
			Move,
		}

		private MouseMode m_mouseMode = MouseMode.None;

        #endregion

        #region Properties

		private DiagramClientView DiagramClientView
        {
            [DebuggerStepThrough]
            get { return m_diagramClientView; }
        }

        private Bitmap DiagramImage
        {
			[DebuggerStepThrough]
			get { return m_diagramImage; }
        }

		private new bool Enabled
		{
			[DebuggerStepThrough]
			get { return m_diagramClientView != null && m_diagramImage != null; }
		}

        private Diagram Diagram
        {
			[DebuggerStepThrough]
			get { return m_diagramClientView != null ? m_diagramClientView.Diagram : null; }
        }

        private Size MaximumImageSize
        {
            [DebuggerStepThrough]
            get
            {
                Size size = this.Size;
                Point location = MinimalImageOffset;
                size.Width -= location.X * 2;
                if (size.Width < 0)
                {
                    size.Width = 0;
                }
                size.Height -= location.Y * 2;
                if (size.Height < 0)
                {
                    size.Height = 0;
                }
                return size;
            }
        }

        private static Point MinimalImageOffset
        {
            [DebuggerStepThrough]
            get { return new Point(5, 5); }
        }

        private Size ImageSize
        {
            [DebuggerStepThrough]
            get
			{
				Debug.Assert(m_diagramImage != null);
				return m_diagramImage != null ? m_diagramImage.Size : Size.Empty;
			}
            set
            {
                if (m_diagramImage != null)
                {
					if (m_diagramImage.Size != value)
					{
						m_diagramImage.Dispose();
						m_diagramImage = null;
					}
					else
					{
						return;
					}
                }
                if (m_diagramImage == null)
                {
                    m_diagramImage = new Bitmap(value.Width, value.Height);
                }
            }
        }

        private Point ImageLocation
        {
            [DebuggerStepThrough]
            get
            {
                Point location = MinimalImageOffset;
                Size maxSize = this.MaximumImageSize;
                Size realSize = this.ImageSize;
                location.Offset((maxSize.Width - realSize.Width) / 2, (maxSize.Height - realSize.Height) / 2);
                return location;
            }
        }

		private Rectangle ImageViewBounds
        {
            [DebuggerStepThrough]
            get
            {
				Debug.Assert(this.Enabled);
				if (this.Enabled)
				{
					RectangleD viewBounds = this.DiagramClientView.ViewBounds;
					Rectangle imageViewBounds = new Rectangle(DiagramToImage(viewBounds.Location), DiagramToImage(viewBounds.Size));
					imageViewBounds.Offset(this.ImageLocation);
					return imageViewBounds;
				}
				else
				{
					return Rectangle.Empty;
				}
            }
        }

        #endregion

        #region Coordinates translation

        private Point DiagramToImage(PointD worldPoint)
        {
			Debug.Assert(this.Enabled);
			if (this.Enabled)
			{
				Size ds = this.DiagramClientView.WorldToDevice(new SizeD(worldPoint.X, worldPoint.Y));
				return new Point((int)(ds.Width * m_imageScale), (int)(ds.Height * m_imageScale));
			}
			else
			{
				return Point.Empty;
			}
        }

        private Size DiagramToImage(SizeD worldSize)
        {
			Debug.Assert(this.Enabled);
			if (this.Enabled)
			{
				Debug.Assert(this.Enabled);
				Size ds = this.DiagramClientView.WorldToDevice(worldSize);
				return new Size((int)(ds.Width * m_imageScale), (int)(ds.Height * m_imageScale));
			}
			else
			{
				return Size.Empty;
			}
        }

        private PointD ImageToDiagram(Point imagePoint)
        {
			Debug.Assert(this.Enabled);
			if (this.Enabled)
			{
				SizeD s = this.DiagramClientView.DeviceToWorld(new Size(
					(int)(imagePoint.X / m_imageScale),
					(int)(imagePoint.Y / m_imageScale)));
				return new PointD(s.Width, s.Height);
			}
			else
			{
				return PointD.Empty;
			}
        }

		#endregion

		#region Diagram view control

		private void SetViewLocation(PointD viewLocation)
        {
			Debug.Assert(this.Enabled);
			if (this.Enabled)
			{
				this.Invalidate(Rectangle.Inflate(this.ImageViewBounds, 2, 2));

				double scrollUnitLength = (double)typeof(DiagramClientView).GetProperty("ScrollUnitLength", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(this.DiagramClientView, new object[0]);

				this.DiagramClientView.HorizontalScrollPosition = (int)(viewLocation.X / scrollUnitLength);
				this.DiagramClientView.VerticalScrollPosition = (int)(viewLocation.Y / scrollUnitLength);

				this.DiagramClientView.Invalidate();

				this.Invalidate(Rectangle.Inflate(this.ImageViewBounds, 2, 2));
			}
        }

        #endregion

        #region Thumbnail image

		internal void InvalidateImage()
		{
			this.InvalidateImage(this.DiagramClientView);
		}

		internal void InvalidateImage(DiagramClientView diagramClientView)
		{
			m_diagramClientView = diagramClientView;

			if (m_diagramClientView != null && this.Size.Width > 0 && this.Size.Height > 0)
			{
				SizeD diagramSize = this.Diagram.Size;
				Size deviceDiagramSize = this.DiagramClientView.WorldToDevice(diagramSize);
				Size maxImageSize = this.MaximumImageSize;

				m_imageScale = Math.Min(
					(double)maxImageSize.Width / deviceDiagramSize.Width,
					(double)maxImageSize.Height / deviceDiagramSize.Height);

				this.ImageSize = new Size(
					(int)(deviceDiagramSize.Width * m_imageScale),
					(int)(deviceDiagramSize.Height * m_imageScale));

				using (Graphics g = Graphics.FromImage(this.DiagramImage))
				{
					g.Clear(Color.White);

					MethodInfo drawMethod = typeof(Diagram).GetMethod("DrawDiagram", BindingFlags.NonPublic | BindingFlags.Instance);
					drawMethod.Invoke(Diagram, new object[] {
						g,
						new Rectangle(0, 0, ImageSize.Width, ImageSize.Height), // fit the image
						new PointD(0, 0), // from origin
						(float)(m_imageScale * DiagramClientView.ZoomFactor), // fit the whole diagram
						null // don't need selection etc
					});
				}
			}
			this.Invalidate();
		}

        #endregion

        #region Event overrides

        protected override void OnHandleDestroyed(EventArgs e)
        {
            if (m_diagramImage != null)
            {
                m_diagramImage.Dispose();
                m_diagramImage = null;
            }
			m_diagramClientView = null;
			base.OnHandleCreated(e);
        }

        protected override void OnResize(EventArgs e)
        {
			if (this.Enabled)
			{
				InvalidateImage();
			}
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            if (this.Enabled)
            {
                Graphics graphics = pevent.Graphics;
                Rectangle clientRect = this.ClientRectangle;

                Point imageLocation = this.ImageLocation;
                Size imageSize = this.ImageSize;

                graphics.SetClip(new Rectangle(imageLocation, imageSize), CombineMode.Exclude);
				Brush diagramBackgroundBrush = Diagram.StyleSet.GetBrush(DiagramBrushes.DiagramBackground);
				Debug.Assert(diagramBackgroundBrush != null);
				graphics.FillRectangle(diagramBackgroundBrush, clientRect);
                graphics.ResetClip();

                graphics.DrawImage(this.DiagramImage, imageLocation.X, imageLocation.Y, imageSize.Width, imageSize.Height);

                Pen zoomLassoPen = Diagram.StyleSet.GetPen(DiagramPens.ZoomLasso);
                Debug.Assert(zoomLassoPen != null);
                graphics.DrawRectangle(zoomLassoPen, this.ImageViewBounds);
            }
            else
            {
				pevent.Graphics.Clear(SystemColors.Control);
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
			if (!this.Enabled) return;

            switch (m_mouseMode)
            {
				case MouseMode.None:
				{
					this.Cursor = Cursors.SizeAll;
					break;
				}
                case MouseMode.Move:
                {
                    Point p = e.Location;
					Point imageLocation = this.ImageLocation;
					p.Offset(-imageLocation.X, -imageLocation.Y);
					Rectangle imageBounds = this.ImageViewBounds;
					p.Offset(-imageBounds.Width / 2, -imageBounds.Height / 2);
                    SetViewLocation(ImageToDiagram(p));
                    break;
                }
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
			if (!this.Enabled) return;

            if (m_mouseMode == MouseMode.None)
            {
                m_mouseMode = MouseMode.Move;
                this.Capture = true;
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
			if (!this.Enabled) return;

            this.Capture = false;
            m_mouseMode = MouseMode.None;
			base.OnMouseUp(e);
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
			if (!this.Enabled) return;

            Point p = e.Location;
            p.Offset(-ImageLocation.X, -ImageLocation.Y);
            p.Offset(-ImageViewBounds.Width / 2, -ImageViewBounds.Height / 2);
            SetViewLocation(ImageToDiagram(p));
        }

		internal void StartMove()
		{
			Debug.Assert(this.Enabled);
			if (!this.Enabled) return;

			Debug.Assert(m_mouseMode == MouseMode.None);
			if (m_mouseMode == MouseMode.None)
			{
				this.Capture = true;
				Rectangle viewRect = this.ImageViewBounds;
				viewRect.Offset(this.Location);
				Cursor.Position = PointToScreen(new Point(viewRect.Left + viewRect.Width / 2, viewRect.Top + viewRect.Height / 2));
				m_mouseMode = MouseMode.Move;
			}
		}

        #endregion
    }
}
