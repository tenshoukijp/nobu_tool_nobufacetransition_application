using System;
using System.Windows.Forms;

namespace NobuFaceTransition
{


    // トラックバー系
    public partial class NTF_Form : Form
    {
        TrackBar trackBar;

        void InitTrackBar()
        {
            trackBar = new TrackBar();
            trackBar.Orientation = Orientation.Vertical;
            trackBar.Top = 20;
            trackBar.Left = 1;
            trackBar.Width = 6;
            trackBar.Height = this.Height - (trackBar.Top * 2) - 40;
            trackBar.TickStyle = TickStyle.None;

            // 最小値、最大値を設定
            trackBar.Minimum = 0;
            trackBar.Maximum = 1000;

            // 描画される目盛りの刻みを設定
            trackBar.TickFrequency = 100;

            // スライダーをキーボードやマウス、
            // PageUp,Downキーで動かした場合の移動量設定
            trackBar.SmallChange = 1;
            trackBar.LargeChange = 10;
            this.Controls.Add(trackBar);

            // 値が変更された際のイベントハンドらーを追加
            trackBar.ValueChanged += new EventHandler(trackBar_ValueChanged);

            // とりあえず、起動状態では、最初のnobu11pk
            UpdateTrackBar(strSeriasList[0]);

            TrackBarValue = 0;
        }

        void trackBar_ValueChanged(object sender, EventArgs e)
        {
            iStartRow = TrackBarValue;
            tabControl_SelectedIndexChanged(null, null);
        }

        // タブページが変わると、トラックバーのMaximumが該当のシリーズの顔グラ個数となる。
        void UpdateTrackBar(string serias)
        {
            int count = lstDictionary[serias].Count;

            // 現在のリストベースでの値をとっておき…
            int value = TrackBarValue;

            // トラックバーのマックスを変えて
            trackBar.Maximum = count + trackBar.Minimum;

            if (value > trackBar.Maximum)
            {
                value = trackBar.Maximum;
            }
            // リストベース⇒トラックバーベースへと代入しなおす
            TrackBarValue = value;

            trackBar.Focus();
            trackBar.Select();
        }

        int TrackBarValue
        {
            get { return trackBar.Maximum - trackBar.Value; }
            set
            {
                int iValue = trackBar.Maximum - value;
                if (iValue > trackBar.Maximum)
                {
                    iValue = trackBar.Maximum;
                }
                else if (iValue < trackBar.Minimum)
                {
                    iValue = trackBar.Minimum;
                }
                trackBar.Value = iValue;
            }
        }

    }
}