using System;
using System.Windows.Forms;

namespace NobuFaceTransition
{
    // フォーム系
    public partial class NTF_Form : Form
    {
        static String[] strSeriasList = { "nobu11pk", "nobu12pk", "nobu13pk", "nobu14pk", "nobu14rd", "nobu15pk" };

        public NTF_Form()
        {
            InitializeComponent();

            this.Shown += new EventHandler(NobuFaceTransitionForm_Shown);
            this.MouseWheel += new MouseEventHandler(Object_MouseWheel);
            InitFormLayout();

            InitTabControl();
        }

        void InitFormLayout()
        {
            this.Text = "信長シリーズ 顔グラ素材 流用図鑑";
            this.Width = 1060;
            this.Height = 1012;
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        void NobuFaceTransitionForm_Shown(object senver, EventArgs e)
        {
            this.Refresh();

            //スプラッシュウィンドウを表示
            APP_SplashForm.ShowSplash(this);

            // リンクが間違っていたという情報の辞書の読み込み
            InitDameTransitionDic();

            // 関連辞書の読み取り
            InitKaoTransitionDic();

            InitPictureBoxes();

            InitTrackBar();

            //スプラッシュウィンドウを表示
            APP_SplashForm.CloseSplash();

        }

        void Object_MouseWheel(object sender, MouseEventArgs e)
        {
            var iValue = e.Delta * SystemInformation.MouseWheelScrollLines;
            if (iValue < -3) { iValue = -3; }
            if (iValue > 3) { iValue = 3; }
            TrackBarValue = TrackBarValue - iValue;
        }
    }
}