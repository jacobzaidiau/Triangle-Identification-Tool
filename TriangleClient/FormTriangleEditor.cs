/**
	Window that opens the MVC website in order to manually edit the Triangle Information.
	
	Name: Jacob Zaidi
    Email: jacob@zaidi.nz
    Website: https://jacob.zaidi.nz/
    Mobile: +64 22 084 6961
	GitHub: https://github.io/jacobzaidi/
*/

using System.Windows.Forms;
using CefSharp.WinForms;

namespace TriangleClient
{
    public partial class FormTriangleEditor : Form
    {
        public FormTriangleEditor()
        {
            ChromiumWebBrowser browser = new ChromiumWebBrowser($"http://localhost:51253");
            this.Controls.Add(browser);
            browser.Dock = DockStyle.Fill;
        }
    }
}