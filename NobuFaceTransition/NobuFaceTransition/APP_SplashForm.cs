using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace NobuFaceTransition
{
    class APP_SplashForm : Form
    {
        //Splashフォーム
        private static APP_SplashForm _form = null;
        //メインフォーム
        private static Form _mainForm = null;
        //Splashを表示するスレッド
        private static System.Threading.Thread _thread = null;
        //lock用のオブジェクト
        private static readonly object syncObject = new object();
        //Splashが表示されるまで待機するための待機ハンドル
        private static System.Threading.ManualResetEvent splashShownEvent = null;

        /// <summary>
        /// Splashフォーム
        /// </summary>
        public static APP_SplashForm Form
        {
            get { return _form; }
        }

        /// <summary>
        /// Splashフォームを表示する
        /// </summary>
        /// <param name="mainForm">メインフォーム</param>
        public static void ShowSplash(Form mainForm)
        {
            lock (syncObject)
            {
                if (_form != null || _thread != null)
                {
                    return;
                }

                //待機ハンドルの作成
                splashShownEvent = new System.Threading.ManualResetEvent(false);

                _mainForm = mainForm;

                //スレッドの作成
                _thread = new System.Threading.Thread(new System.Threading.ThreadStart(StartThread));
                _thread.Name = "SplashForm";
                _thread.IsBackground = true;
                _thread.SetApartmentState(System.Threading.ApartmentState.STA);

                //スレッドの開始
                _thread.Start();
            }
        }

        /// <summary>
        /// Splashフォームを表示する
        /// </summary>
        public static void ShowSplash()
        {
            ShowSplash(null);
        }

        /// <summary>
        /// Splashフォームを消す
        /// </summary>
        public static void CloseSplash()
        {
            lock (syncObject)
            {
                if (_thread == null)
                {
                    return;
                }

                //Splashが表示されるまで待機する
                if (splashShownEvent != null)
                {
                    splashShownEvent.WaitOne();
                    splashShownEvent.Close();
                    splashShownEvent = null;
                }

                //Splashフォームを閉じる
                //Invokeが必要か調べる
                if (_form != null)
                {
                    if (_form.InvokeRequired)
                    {
                        _form.Invoke(new MethodInvoker(CloseSplashForm));
                    }
                    else
                    {
                        CloseSplashForm();
                    }
                }

                //メインフォームをアクティブにする
                if (_mainForm != null)
                {
                    if (_mainForm.InvokeRequired)
                    {
                        _mainForm.Invoke(new MethodInvoker(ActivateMainForm));
                    }
                    else
                    {
                        ActivateMainForm();
                    }
                }

                _form = null;
                _thread = null;
                _mainForm = null;
            }
        }

        private static void LoadLoadingImage() {
            // このプログラムのアセンブリ
            System.Reflection.Assembly asm = System.Reflection.Assembly.GetExecutingAssembly();
            System.Resources.ResourceManager rm = new System.Resources.ResourceManager(  asm.GetName().Name + ".NobuFaceTransitionRes", asm);
            Bitmap bmp = (Bitmap)rm.GetObject("LoadingImage");
            _form.BackgroundImage = bmp;

        }

        //スレッドで開始するメソッド
        private static void StartThread()
        {
            //Splashフォームを作成
            _form = new APP_SplashForm();
            _form.FormBorderStyle = FormBorderStyle.FixedSingle;

            _form.Width = 480;
            _form.Height = 266;
            _form.StartPosition = FormStartPosition.CenterScreen;
            _form.ControlBox = false;

            Label btn = new Label();
            btn.Text = "ロード中です...";
            btn.ForeColor = Color.White;
            btn.BackColor = Color.Transparent;
            btn.Width = 80;
            btn.Height = 20;
            btn.Left = _form.Width - btn.Width;
            btn.Top = _form.Height - btn.Height;

            _form.Controls.Add(btn);

            LoadLoadingImage();

            //Splashが表示されるまでCloseSplashメソッドをブロックする
            _form.Activated += new EventHandler(_form_Activated);

            //Splashフォームを表示する
            Application.Run(_form);
        }

        //SplashのCloseメソッドを呼び出す
        private static void CloseSplashForm()
        {
            if (!_form.IsDisposed)
            {
                _form.Close();
            }
        }

        //メインフォームのActivateメソッドを呼び出す
        private static void ActivateMainForm()
        {
            if (!_mainForm.IsDisposed)
            {
                _mainForm.Activate();
            }
        }

        //Splashフォームがクリックされた時
        private static void _form_Click(object sender, EventArgs e)
        {
            //Splashフォームを閉じる
            CloseSplash();
        }

        //Splashフォームが表示された時
        private static void _form_Activated(object sender, EventArgs e)
        {
            _form.Activated -= new EventHandler(_form_Activated);

            //CloseSplashメソッドの待機を解除
            if (splashShownEvent != null)
            {
                splashShownEvent.Set();
            }
        }
    }
}
