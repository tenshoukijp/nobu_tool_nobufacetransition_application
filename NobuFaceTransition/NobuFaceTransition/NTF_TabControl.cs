using System;
using System.Windows.Forms;

namespace NobuFaceTransition
{

    // タブ系
    public partial class NTF_Form : Form
    {
        static String[] strTabTitleList = { "創世 PK", "革新 PK", "天道 PK", "創造 PK", "創造 RD", "大志 PK" };
        TabControl tabControl;

        void InitTabControl()
        {
            tabControl = new TabControl() { Dock = DockStyle.Top };
            tabControl.Height = 19;
            this.Controls.Add(tabControl);

            for (int i = 0; i < strSeriasList.Length; i++)
            {
                TabPage tabPage = new TabPage()
                {
                    Text = strTabTitleList[i],
                    Height = 17
                };
                tabControl.Controls.Add(tabPage);
            }
            tabControl.SelectedIndexChanged += new EventHandler(tabControl_SelectedIndexChanged);
            this.Controls.Add(tabControl);

        }
        // タブの切り替え
        void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            int tbix = tabControl.SelectedIndex;
            string serias = strSeriasList[tbix];
            UpdatePictureBoxes(serias);
            UpdateTrackBar(serias);
        }
    }

}