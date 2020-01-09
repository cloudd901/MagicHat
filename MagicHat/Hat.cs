using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace RandomTool
{
    public delegate void ToolActionEventHandler(object entry, string[] actionInfo);
    public delegate void ToolStopEventHandler(object entry);

    public partial class Hat : IRandomTool, IDisposable
    {
        public bool IsDisposed { get; private set; } = false;

        public event ToolActionEventHandler ToolActionCall;
        public event ToolStopEventHandler ToolStopCall;

        private readonly PictureBox _ControlHat = new PictureBox()
        { BackColor = Color.Transparent, Location = new Point(0, 0), Size = new Size(0, 0), BorderStyle = BorderStyle.None, Visible = false };

        private ObjectSize ToolSize { get; } = new ObjectSize();
        public ToolProperties ToolProperties { get; set; } = new ToolProperties();
        public List<Entry> EntryList { get; } = new List<Entry>();
        public bool AllowExceptions { get; set; } = true;
        public bool IsBusy { get; private set; } = false;

        public Hat(Control contentForm)
        {
            Control ContentWindow = contentForm ?? throw new NullReferenceException("A valid control must be used.");
            _ControlHat.Parent = ContentWindow;
            ContentWindow.Controls.Add(_ControlHat);
        }

        public void BringToFront()
        {
            _ControlHat.BringToFront();
            _ControlHat.Update();
        }
        public void SendToBack()
        {
            _ControlHat.SendToBack();
            _ControlHat.Update();
        }

        private void SetPictureBoxes()
        {
            _ControlHat.Top = (int)ToolSize.Top;
            _ControlHat.Left = (int)ToolSize.Left;
            _ControlHat.ClientSize = new Size(ToolSize.Diameter + ToolProperties.ShadowLength, ToolSize.Diameter + ToolProperties.ShadowLength);
            _ControlHat.Visible = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (IsDisposed)
            { return; }

            if (disposing)
            {
                EntriesClear();

                ToolProperties.objectImage.Dispose();
                _ControlHat.Dispose();

                ToolSize.initialized = false;
            }

            IsDisposed = true;
        }
    }
}
