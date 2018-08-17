namespace Gallery
{
    partial class ucImageTableView
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucImageTableView));
            this.imageCollection2 = new DevExpress.Utils.ImageCollection();
            ((System.ComponentModel.ISupportInitialize)(this.imageCollection2)).BeginInit();
            this.SuspendLayout();
            // 
            // imageCollection2
            // 
            this.imageCollection2.ImageSize = new System.Drawing.Size(32, 32);
            this.imageCollection2.ImageStream = ((DevExpress.Utils.ImageCollectionStreamer)(resources.GetObject("imageCollection2.ImageStream")));
            this.imageCollection2.InsertGalleryImage("barcode_16x16.png", "office2013/content/barcode_16x16.png", DevExpress.Images.ImageResourceCache.Default.GetImage("office2013/content/barcode_16x16.png"), 0);
            this.imageCollection2.Images.SetKeyName(0, "barcode_16x16.png");
            this.imageCollection2.InsertGalleryImage("shape_16x16.png", "images/toolbox%20items/shape_16x16.png", DevExpress.Images.ImageResourceCache.Default.GetImage("images/toolbox%20items/shape_16x16.png"), 1);
            this.imageCollection2.Images.SetKeyName(1, "shape_16x16.png");
            this.imageCollection2.InsertGalleryImage("stackedbar_16x16.png", "office2013/chart/stackedbar_16x16.png", DevExpress.Images.ImageResourceCache.Default.GetImage("office2013/chart/stackedbar_16x16.png"), 2);
            this.imageCollection2.Images.SetKeyName(2, "stackedbar_16x16.png");
            this.imageCollection2.InsertGalleryImage("inlinesizelegend_16x16.png", "images/maps/inlinesizelegend_16x16.png", DevExpress.Images.ImageResourceCache.Default.GetImage("images/maps/inlinesizelegend_16x16.png"), 3);
            this.imageCollection2.Images.SetKeyName(3, "inlinesizelegend_16x16.png");
            // 
            // ucGalleryItemsList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "ucGalleryItemsList";
            ((System.ComponentModel.ISupportInitialize)(this.imageCollection2)).EndInit();
            this.ResumeLayout(false);

        }


        #endregion

        private DevExpress.Utils.ImageCollection imageCollection2;
    }
}
