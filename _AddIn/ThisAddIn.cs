using PowerPoint = Microsoft.Office.Interop.PowerPoint;

namespace PPTShapesLibraryAddIn
{
    public partial class ThisAddIn
    {
        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {
            Application.PresentationBeforeClose += Application_PresentationBeforeClose;

            //DateTime dt0 = DateTime.Now;
            //int max = 1000000000;
            //double val = 0.1;
            //for(int i=0; i< max; i++)
            //{
            //    val += 0.2;
            //}
            //DateTime dt1 = DateTime.Now;

            //decimal val2 = 0.1m;
            //for (int i = 0; i < max; i++)
            //{
            //    val2 += 0.2m;
            //}
            //DateTime dt2 = DateTime.Now;

            //double t1 = (dt1 - dt0).TotalMilliseconds;
            //double t2 = (dt2 - dt1).TotalMilliseconds;
        }

        void Application_PresentationBeforeClose(PowerPoint.Presentation Pres, ref bool Cancel)
        {
            if (ribbon != null && Pres.Windows.Count>0)
            {
                ribbon.CloseTaskPane();
            }
        }

        private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
        {
            Application.PresentationBeforeClose -= Application_PresentationBeforeClose;
        }

        Ribbon ribbon = null;
        protected override Microsoft.Office.Core.IRibbonExtensibility CreateRibbonExtensibilityObject()
        {
            ribbon = new Ribbon();
            return ribbon ;
        }

        #region VSTO generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup()
        {
            this.Startup += new System.EventHandler(ThisAddIn_Startup);
            this.Shutdown += new System.EventHandler(ThisAddIn_Shutdown);
        }
        
        #endregion


    }
}
