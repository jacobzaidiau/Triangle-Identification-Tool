/**
	Client Application that identifies a triangle type based on the lengths of the 3 sides.
	
	Name: Jacob Zaidi
    Email: jacob@zaidi.nz
    Website: https://jacob.zaidi.nz/
    Mobile: +64 22 084 6961
	GitHub: https://github.io/jacobzaidi/
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using CefSharp;
using CefSharp.WinForms;

namespace TriangleClient
{
    public partial class FormTriangleTool : Form
    {
        int pageCount = 0;
        
        int currentPage = 1;
        bool isPageEmpty = true;

        ToolReference.TriangleTool obj = new ToolReference.TriangleTool();
        public FormTriangleTool()
        {
            InitializeComponent();
            pageCount = GetPageCount();
            DisplayPage(currentPage);
            Cef.EnableHighDPISupport();
            Cef.Initialize(new CefSettings());
        }

        public void MenuItemAbout_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "Triangle Identification Tool is a client application developed " +
                "by Jacob Zaidi in C# using Visual Studio 2017.\n\n" +
                "If the number of significant figures exceeds 15 and the length is less than 7.9 x 10^28, " +
                "the Decimal Format will be used instead to avoid rounding errors.", 
                "About Triangle Identification Tool");
        }
        
        public void MenuItemExit_Click(object sender, EventArgs e) { Application.Exit(); }
        private void Form1_Load(object sender, EventArgs e) { }
        public void ButtonCheck_Click(object sender, EventArgs e)
        {
            List<bool> triangleIdentification;
            try { triangleIdentification = obj.Check(textBoxSideOne.Text, textBoxSideTwo.Text, textBoxSideThree.Text).ToList(); }
            catch (System.Net.WebException)
            {
                MessageBox.Show("In order to use this feature, please run the WebService Application.", "Error");
                return;
            }
            if (triangleIdentification.Count == 4)
            {
                checkBoxEquilateral.Checked = triangleIdentification.ElementAt(0);
                checkBoxIsosceles.Checked = triangleIdentification.ElementAt(1);
                checkBoxScalene.Checked = triangleIdentification.ElementAt(2);
                checkBoxRightAngle.Checked = triangleIdentification.ElementAt(3);
                pageCount = GetPageCount();
                currentPage = 1;
                DisplayPage(currentPage);
            }
            else
            {
                try { MessageBox.Show(obj.Error(textBoxSideOne.Text, textBoxSideTwo.Text, textBoxSideThree.Text), "Error"); }
                catch (System.Net.WebException)
                {
                    MessageBox.Show("The WebService Application appears to be down. Please run the WebService Application and try again.", "Error");
                    return;
                }
            } 
        }

        public void ButtonFirst_Click(object sender, EventArgs e)
        {
            if (isPageEmpty)
            {
                try
                {
                    pageCount = GetPageCount();
                    isPageEmpty = false;
                }
                catch { return; }
            }
            else
            {
                currentPage = 1;
                DisplayPage(currentPage);
            }
        }
        public void ButtonLast_Click(object sender, EventArgs e)
        {
            if (isPageEmpty)
            {
                try
                {
                    pageCount = GetPageCount();
                    isPageEmpty = false;
                }
                catch { return; }
            }
            else
            {
                pageCount = GetPageCount();
                currentPage = pageCount;
                DisplayPage(currentPage);
            }
        }
        public void ButtonPrevious_Click(object sender, EventArgs e)
        {
            if (isPageEmpty)
            {
                try
                {
                    pageCount = GetPageCount();
                    isPageEmpty = false;
                }
                catch { return; }
            }
            else
            {
                pageCount = GetPageCount();
                if (currentPage < 2) { return; }
                currentPage -= 1;
                DisplayPage(currentPage);
            }


        }
        public void ButtonNext_Click(object sender, EventArgs e)
        {
            if (isPageEmpty)
            {
                try
                {
                    pageCount = GetPageCount();
                    isPageEmpty = false;
                }
                catch { return; }
            }
            else
            {
                pageCount = GetPageCount();
                if (currentPage + 1 > pageCount) { return; }
                currentPage += 1;
                DisplayPage(currentPage);
            }
        }

        public void ButtonGo_Click(object sender, EventArgs e)
        {
            try
            {
                int page = int.Parse(textBoxPageNumber.Text);
                if (page > 0 && page <= pageCount)
                {
                    currentPage = page;
                    DisplayPage(currentPage);
                }
            }
            catch { return; }
        }

        public void menuItemTriangleEditor_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<FormTriangleEditor>().Count() == 1)
                Application.OpenForms.OfType<FormTriangleEditor>().First().Close();
            FormTriangleEditor triangleEditor = new FormTriangleEditor();
            triangleEditor.Show();
        }

        public int GetPageCount()
        {
            try
            {
                int pageCount = obj.GetPageCount();
                return pageCount;
            }
            catch { return 0; }
        }

        public void DisplayPage(int page)
        {
            try
            {
                string[] results = obj.GetPage(page);

                textBoxSideOne.Text   = results[0];
                textBoxSideTwo.Text   = results[1];
                textBoxSideThree.Text = results[2];

                checkBoxEquilateral.Checked = Convert.ToBoolean(results[3]);
                checkBoxIsosceles.Checked   = Convert.ToBoolean(results[4]);
                checkBoxScalene.Checked     = Convert.ToBoolean(results[5]);
                checkBoxRightAngle.Checked  = Convert.ToBoolean(results[6]);

                pagesLabel.Text = string.Format("Page {0} of {1}", currentPage, pageCount);
            }
            catch { return; }
        }
    }
}