using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ShapesLibrary;

namespace Gallery
{
    public partial class ucImageTableView : UserControl
    {
        public event EventHandler<GalleryItemEventArgs> HoveredItemChanged;

        public IFileItem SelectedItem { get => selectedIndex >= 0 ? dataSource[selectedIndex] : null; }

        [DefaultValue(150)]
        public int ItemMaxWidth { get; set; } = 150; 


        protected float slideRatio = 540f / 960f;   //TODO: ?

        protected int NumberOfColumns;
        protected Size itemSize = new Size();

        protected int selectedIndex = -1;
        protected int hoveredIndex = -1;

        private  IReadOnlyList<IFileItem> dataSource = new List<IFileItem>();

        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IEnumerable<IFileItem> DataSource
        {
            get { return dataSource; }

            set
            {
                dataSource = value?.ToList()?? new List<IFileItem>();

                selectedIndex = dataSource.Count > 0 ? 0 : -1;
                CalcLayout();
                Invalidate();
            }
        }

        public ucImageTableView()
        {
            InitializeComponent();
        }

        public void CalcLayout()
        {
            if (!DesignMode)
            {
                if (ItemMaxWidth <= 0)
                {
                    ItemMaxWidth = 120;
                }

                NumberOfColumns = Width / ItemMaxWidth;

                if (NumberOfColumns <= 0)
                {
                    NumberOfColumns = 1;
                }

                itemSize.Width = this.Width / NumberOfColumns;
                itemSize.Height = (int)(itemSize.Width * slideRatio);

                int h = (Math.Max(1, dataSource.Count) + (NumberOfColumns - 1)) / NumberOfColumns * itemSize.Height;
                if (this.Height != h)
                {
                    this.Height = h;
                    Invalidate();
                }
            }
        }

        protected void OnHoveredItemChanged(int idx)
        {
            Invalidate(GetItemRect(hoveredIndex));
            hoveredIndex = idx;
            Invalidate(GetItemRect(hoveredIndex));
            IFileItem item = null;
            if (idx >= 0 && idx < dataSource.Count)
            {
                item = dataSource[idx];
            }

            HoveredItemChanged?.Invoke(this, new GalleryItemEventArgs(item));
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            CalcLayout();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            OnHoveredItemChanged(-1);
        }
        
        protected override void OnMouseDown(MouseEventArgs e)
        {
            Invalidate(GetItemRect(selectedIndex));
            selectedIndex = GetItemIndex(e.Location);
            Invalidate(GetItemRect(selectedIndex));

            base.OnMouseDown(e);
        }

        protected Rectangle GetItemRect(int idx)
        {
            int row = idx / NumberOfColumns;
            int col = idx % NumberOfColumns;
            Rectangle r = new Rectangle();
            r.Location = new Point(col * itemSize.Width, row * itemSize.Height);
            r.Size = itemSize;
            return r;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (e.Button != System.Windows.Forms.MouseButtons.None)
            {
                OnHoveredItemChanged(-1);
                return;
            }

            int idx = GetItemIndex(e.Location);

            if (idx == -1)
            {
                OnHoveredItemChanged(-1);
                return;
            }

            if (hoveredIndex != idx)
            {
                OnHoveredItemChanged(idx);
            }
        }
       
        protected int GetItemIndex(Point loc)
        {
            int dx = this.Width / NumberOfColumns;
            int dy = (int)(dx * slideRatio);

            int row = loc.Y / dy;
            int col = loc.X / dx;

            int idx = row * NumberOfColumns + col;

            if (idx >= dataSource.Count)
                idx = -1;

            return idx;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            Rectangle r = new Rectangle(new Point(0, 0), itemSize);
            for (int idx = 0; idx < dataSource.Count; idx++)
            {
                int row = idx / NumberOfColumns;
                int col = idx % NumberOfColumns;
                r.Location = new Point(col * itemSize.Width, row * itemSize.Height);

                if (g.ClipBounds.IntersectsWith(r))
                {
                    Rectangle rb = r;
                    rb.Inflate(-2, -2);
                    g.DrawImage(dataSource[idx].Image, rb);

                    Size ms = new Size(16, 16);
                    Rectangle rm = new Rectangle(
                        new Point(rb.Right - ms.Width-1, rb.Bottom - ms.Height-1),
                        ms
                        );
                    g.DrawImage(imageCollection2.Images[(int)dataSource[idx].Type], rm);
                    

                    rb = r;
                    rb.Inflate(-2, -2);
                    g.DrawRectangle(Pens.LightGray, rb);
                }
                if (idx == selectedIndex)
                {
                    Rectangle rb = r;
                    rb.Inflate(-1, -1);
                    using (Pen p = new Pen(Color.RoyalBlue, 2))
                    {
                        g.DrawRectangle(p, rb);
                    }
                }
                else if (idx == hoveredIndex)
                {
                    Rectangle rb = r;
                    rb.Inflate(-1, -1);
                    using (Pen p = new Pen(Color.DimGray, 2))
                    {
                        g.DrawRectangle(p, rb);
                    }
                }

            }
        }

    }
    
    public class GalleryItemEventArgs : EventArgs
    {
        public IFileItem Item { get; protected set; }
        public GalleryItemEventArgs(IFileItem item)
        {
            Item = item;
        }
    }
    
}
