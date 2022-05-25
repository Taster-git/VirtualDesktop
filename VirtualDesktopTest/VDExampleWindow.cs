using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace VirtualDesktopTest
{
    public class NewWindow : Form
    {
    }

    public partial class VDExampleWindow : Form
    {
        public VDExampleWindow()
        {
            InitializeComponent();
        }
        private VirtualDesktopManager vdm;
        private void VDExampleWindow_Load(object sender, EventArgs e)
        {
            //Create IVirtualDesktopManager on load
            vdm = new VirtualDesktopManager();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            //Show details on click
            MessageBox.Show("Virtual Desktop ID: " + vdm.GetWindowDesktopId(Handle).ToString("X") + Environment.NewLine +
                "IsCurrentVirtualDesktop: " + vdm.IsWindowOnCurrentVirtualDesktop(Handle).ToString()
                );
        }
        //Timer tick to check if the window is on the current virtual desktop and change it otherwise
        //A timer does not have to be used, but something has to trigger the check
        //If the window was active before the vd change, it would trigger 
        //the deactivated and lost focus events when the vd changes
        //The timer always gets triggered which makes the example hopefully less confusing
        private void VDCheckTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (!vdm.IsWindowOnCurrentVirtualDesktop(Handle))
                {
                    using (NewWindow nw = new NewWindow())
                    {
                        nw.Show(null);
                        vdm.MoveWindowToDesktop(Handle, vdm.GetWindowDesktopId(nw.Handle));
                    }
                }
            }
            catch
            {
                //This will fail due to race conditions as currently written on occassion
            }
        }


    }
}
