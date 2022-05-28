using System;
using System.Drawing;
using System.Windows.Forms;

namespace NobuFaceTransition
{

    // PictureBox系
    public partial class NTF_Form : Form
    {
        const int PBCOLS = 20;
        const int PBROWS = 10;
        PictureBox[,] pbMap = new PictureBox[PBROWS, PBCOLS];
        Label[,] lbMap = new Label[PBROWS, PBCOLS];

        Bitmap bmBlank;
        Bitmap bmUnlink;

        int BMPHeight = 80;
        int BMPWidth = 64;
        int GapPixelWidth = 6; // BMP同士の隙間(横)
        int GapPixelHeight = 14; // BMP同士の隙間(縦)
        int iStartRow = 1; // BMPは1オリジン

        void InitPictureBoxes()
        {
            LoadBlankBmpFromEmbeddedResources();
            LoadBmpFromEmbeddedResources();
            this.SuspendLayout();

            for (int row = 0; row < PBROWS; row++)
            {
                for (int col = 0; col < PBCOLS; col++)
                {
                    pbMap[row, col] = new PictureBox();
                    pbMap[row, col].Left = col * BMPWidth + GapPixelWidth * col + GapPixelWidth + 20;

                    // 1行目と2行目は間隔をあける
                    if (col >= 1) { pbMap[row, col].Left = pbMap[row, col].Left + 10; }

                    pbMap[row, col].Top = row * BMPHeight + GapPixelHeight * row + GapPixelHeight + 10;
                    pbMap[row, col].Width = BMPWidth;
                    pbMap[row, col].Height = BMPHeight;
                    pbMap[row, col].BackgroundImage = bmBlank;
                    this.Controls.Add(pbMap[row, col]);

                    pbMap[row, col].MouseClick += new MouseEventHandler(pictureBox_OnMouseClick);
                    // pbMap[row, col].MouseWheel += new MouseEventHandler(Object_MouseWheel);

                    lbMap[row, col] = new Label();
                    lbMap[row, col].Font = new Font("MS Mincho", 9);
                    lbMap[row, col].Left = pbMap[row, col].Left;
                    lbMap[row, col].Top = pbMap[row, col].Top + BMPHeight;
                    lbMap[row, col].Width = BMPWidth;
                    lbMap[row, col].Height = 12;
                    lbMap[row, col].Text = "";
                    // lbMap[row, col].MouseWheel += new MouseEventHandler(Object_MouseWheel);

                    this.Controls.Add(lbMap[row, col]);
                }
            }
            this.ResumeLayout();

            // とりあえず、最初の初期化は、nobu11pk
            UpdatePictureBoxes(strSeriasList[0]);
        }

        void ClearPictureBox(int row, int col)
        {
            // そのシンボル名のビットマップを新たなイメージとする
            pbMap[row, col].BackgroundImage = bmBlank;
            pbMap[row, col].Image = bmBlank;
            lbMap[row, col].Text = "";
        }
        void UpdatePictureBoxes(String serias)
        {
            this.SuspendLayout();
            for (int row = 0; row < PBROWS; row++)
            {
                int ix = row + iStartRow;
                for (int col = 0; col < PBCOLS; col++)
                {
                    // もう有効ではない。
                    if (ix >= lstDictionary[serias].Count)
                    {
                        ClearPictureBox(row, col);
                        continue;
                    }

                    // とあるシリーズのシンボルListの〇番目のシンボル名
                    string simbol = lstDictionary[serias][ix];

                    // 一番左の列
                    if (col == 0)
                    {
                        // そのシンボル名のビットマップを新たなイメージとする
                        pbMap[row, col].BackgroundImage = bmpDictionary[simbol];
                        string shortname = simbol.Substring(4, 9); // nobu11pk_0001.png ⇒ 11pk_0001

                        lbMap[row, col].Text = shortname;

                    }
                    else
                    {
                        if (!dicFaceTransition.ContainsKey(simbol))
                        {
                            ClearPictureBox(row, col);
                            continue;
                        }

                        // 該当シンボルを基点として、合致した他画像のリスト(厳密には、KeyValuePair型のリスト)
                        var list = dicFaceTransition[simbol];
                        if (list.Count >= col)
                        {

                            string dst_simbol = list[col - 1].Key;
                            // KeyValuのKey(=合致するシンボル名)に対応する、ビットマップが登録されているならば…
                            if (bmpDictionary.ContainsKey(dst_simbol))
                            {
                                // そのシンボル名のビットマップを新たなイメージとする
                                pbMap[row, col].BackgroundImage = bmpDictionary[dst_simbol];

                                // まずフロントはクリア
                                pbMap[row, col].Image = bmBlank;

                                // ダメリストの元が登録されてるか？
                                if (dicDameTransition.ContainsKey(simbol))
                                {
                                    var dameList = dicDameTransition[simbol];
                                    if (dameList.Contains(dst_simbol))
                                    {
                                        pbMap[row, col].Image = bmUnlink;
                                    }
                                }

                                string shortname = dst_simbol.Substring(4, 9); // nobu11pk_0001.png ⇒ 11pk_0001
                                lbMap[row, col].Text = shortname;
                            }
                        }
                        else
                        {
                            ClearPictureBox(row, col);
                            continue;
                        }
                    }
                }
            }
            this.ResumeLayout();
        }

        // PictureBoxをクリックしたら、その行の一番左との関係をUnLinkする。
        void pictureBox_OnMouseClick(Object sender, MouseEventArgs e)
        {
            PictureBox clickPB = (PictureBox)sender;

            String dame_dst = (String)clickPB.BackgroundImage.Tag;
            if (dame_dst == "bmBlank")
            {
                return;
            }

            String dame_org = "";

            // 同じ参照を探す
            for (int row = 0; row < PBROWS; row++)
            {
                for (int col = 0; col < PBCOLS; col++)
                {
                    if (pbMap[row, col] == clickPB)
                    {
                        // 一番左なら何もしない
                        if (col == 0) { return; }

                        // 一番左が元ネタ
                        dame_org = (String)pbMap[row, 0].BackgroundImage.Tag;
                    }
                }
            }


            var result = MessageBox.Show(dame_org + "=>" + dame_dst + "のリンクを切断しますか？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            if (result == DialogResult.OK)
            {
                // 追加で
                System.IO.StreamWriter sw = new System.IO.StreamWriter("UnLink.txt", true);

                // hogehoge.txtに書き込まれている行末に、追加で書き出す
                sw.Write(dame_org + "=>" + dame_dst + "\n");
                sw.Write(dame_dst + "=>" + dame_org + "\n");

                // 閉じる (オブジェクトの破棄)
                sw.Close();

                // ダメリンクとして登録
                RegistDameInDictinary(dame_org, dame_dst);
                RegistDameInDictinary(dame_dst, dame_org);

            }

            // リンク変えたので描画更新
            int tbix = tabControl.SelectedIndex;
            string serias = strSeriasList[tbix];
            UpdatePictureBoxes(serias);

        }


    }
}