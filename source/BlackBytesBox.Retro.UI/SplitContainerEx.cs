using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace BlackBytesBox.Retro.UI
{
    /// <summary>
    /// A SplitContainer variant that remembers and restores its size and Panel1/Panel2 width when collapsing/expanding,
    /// and raises an event whenever its overall width changes.
    /// </summary>
    public class SplitContainerEx : SplitContainer
    {
        private int _origWidth;
        private int _origSplitterDistance;
        private bool _isPanel2Collapsed;
        private bool _isPanel1Collapsed;
        private bool _suspendUpdate;
        private int _lastNotifiedWidth;

        /// <summary>
        /// Occurs when the overall width of the SplitContainerEx changes.
        /// </summary>
        public event EventHandler<ContainerWidthChangedEventArgs> ContainerWidthChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="SplitContainerEx"/> class.
        /// </summary>
        public SplitContainerEx()
        {
            this.SplitterMoved += OnSplitterMoved;
            this.HandleCreated += (s, e) =>
            {
                _origWidth = this.Width;
                _origSplitterDistance = this.SplitterDistance;
            };
            this.IsSplitterFixed = true;
            this.Panel1MinSize = 0; // Allow Panel1 to collapse to zero width
            this.Panel2MinSize = 0; // Allow Panel2 to collapse to zero width
            this.SplitterWidth = 1; // Start with no visible splitter
        }

        /// <summary>
        /// Collapses Panel2 to zero width, shrinks total control width to exactly Panel1 width (splitter hidden).
        /// </summary>
        public void CollapsePanel2()
        {
            if (_isPanel2Collapsed) return;
            _suspendUpdate = true;

            // Store original dimensions
            _origWidth = this.Width;
            _origSplitterDistance = this.SplitterDistance;

            // Collapse panel2 & lock splitter
            Panel2Collapsed = true;
            IsSplitterFixed = true;

            // Shrink control to Panel1 width (splitter is removed)
            this.Width = _origSplitterDistance;

            _isPanel2Collapsed = true;
            _suspendUpdate = false;

            NotifyContainerWidthChanged();
        }

        /// <summary>
        /// Collapses Panel1 to zero width, shrinks total control width to exactly Panel2 width (splitter hidden).
        /// </summary>
        public void CollapsePanel1()
        {
            if (_isPanel1Collapsed) return;
            _suspendUpdate = true;

            // Store original dimensions
            _origWidth = this.Width;
            _origSplitterDistance = this.SplitterDistance;

            // Collapse panel1 & lock splitter
            Panel1Collapsed = true;
            IsSplitterFixed = true;

            // Compute Panel2 width (orig total minus Panel1 and splitter)
            int panel2Width = _origWidth - _origSplitterDistance - this.SplitterWidth;
            // Shrink control to Panel2 width (splitter is removed)
            this.Width = panel2Width;

            _isPanel1Collapsed = true;
            _suspendUpdate = false;

            NotifyContainerWidthChanged();
        }

        /// <summary>
        /// Expands Panel2 to its previous width, restores total control width, and unlocks the splitter.
        /// </summary>
        public void ExpandPanel2()
        {
            if (!_isPanel2Collapsed) return;
            _suspendUpdate = true;

            // Restore panel2 and splitter
            Panel2Collapsed = false;
            IsSplitterFixed = true;

            // Restore sizes
            this.Width = _origWidth;
            this.SplitterDistance = _origSplitterDistance;

            _isPanel2Collapsed = false;
            _suspendUpdate = false;

            NotifyContainerWidthChanged();
        }

        /// <summary>
        /// Expands Panel1 to its previous width, restores total control width, and unlocks the splitter.
        /// </summary>
        public void ExpandPanel1()
        {
            if (!_isPanel1Collapsed) return;
            _suspendUpdate = true;

            // Restore panel1 and splitter
            Panel1Collapsed = false;
            IsSplitterFixed = true;

            // Restore sizes
            this.Width = _origWidth;
            this.SplitterDistance = _origSplitterDistance;

            _isPanel1Collapsed = false;
            _suspendUpdate = false;

            NotifyContainerWidthChanged();
        }

        /// <summary>
        /// Toggles the collapsed/expanded state of Panel2.
        /// </summary>
        public void TogglePanel2()
        {
            if (_isPanel2Collapsed) ExpandPanel2();
            else CollapsePanel2();
        }

        /// <summary>
        /// Toggles the collapsed/expanded state of Panel1.
        /// </summary>
        public void TogglePanel1()
        {
            if (_isPanel1Collapsed) ExpandPanel1();
            else CollapsePanel1();
        }

        private void OnSplitterMoved(object sender, SplitterEventArgs e)
        {
            if (_suspendUpdate || _isPanel2Collapsed || _isPanel1Collapsed) return;

            // Update stored dimensions
            _origSplitterDistance = this.SplitterDistance;
            _origWidth = this.Width;
        }

        /// <summary>
        /// Raises ContainerWidthChanged with the *actual* visible width.
        /// </summary>
        private void NotifyContainerWidthChanged()
        {
            int reportedWidth;

            if (_isPanel2Collapsed)
                reportedWidth = this.Panel1.Width;
            else if (_isPanel1Collapsed)
                reportedWidth = this.Panel2.Width;
            else
                reportedWidth = this._origWidth;

            // only fire if it really changed
            if (reportedWidth != _lastNotifiedWidth)
            {
                _lastNotifiedWidth = reportedWidth;
                ContainerWidthChanged?.Invoke(
                    this,
                    new ContainerWidthChangedEventArgs(reportedWidth)
                );
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
        }
    }

    /// <summary>
    /// Provides data for the ContainerWidthChanged event.
    /// </summary>
    public class ContainerWidthChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the new width of the container.
        /// </summary>
        public int NewWidth { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContainerWidthChangedEventArgs"/> class.
        /// </summary>
        /// <param name="newWidth">The new width of the container.</param>
        public ContainerWidthChangedEventArgs(int newWidth)
        {
            NewWidth = newWidth;
        }
    }
}