using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace NobuFaceTransition
{


    // 顔データの流用情報テキストを読み込んで、辞書を形成する
    public partial class NTF_Form : Form
    {
        // Stringがキー。値は、リスト。各リストの型は「文字列がキー、整数が値」。
        static Dictionary<String, List<KeyValuePair<String, int>>> dicFaceTransition = new Dictionary<String, List<KeyValuePair<String, int>>>();

        void InitKaoTransitionDic()
        {
            String line = "";

            //Regexオブジェクトを作成
            System.Text.RegularExpressions.Regex r = new System.Text.RegularExpressions.Regex(@"^(nobu.+?\.png)\=\>(nobu.+?\.png)");

            System.IO.StreamReader file = new System.IO.StreamReader("Link.txt");
            while ((line = file.ReadLine()) != null)
            {
                line = line.ToLower();
                Match m = r.Match(line);
                if (m.Success)
                {
                    string org_simbol = (string)m.Groups[1].Value;
                    string dst_simbol = (string)m.Groups[2].Value;
                    RegistFaceInDictinary(org_simbol, dst_simbol); // 正規の順番を登録
                    RegistFaceInDictinary(dst_simbol, org_simbol); // 逆側も登録
                }
            }

            // シリーズ準でソート
            foreach (var hash in dicFaceTransition)
            {
                var list = hash.Value;
                list.Sort((a, b) => a.Key.CompareTo(b.Key));
            }

            file.Close();
        }

        void RegistFaceInDictinary(string org_simbol, string dst_simbol)
        {
            if (org_simbol.Length > 1 && dst_simbol.Length > 1)
            {
                KeyValuePair<String, int> elem = new KeyValuePair<string, int>(dst_simbol, 1);
                if (!dicFaceTransition.ContainsKey(org_simbol))
                {
                    dicFaceTransition[org_simbol] = new List<KeyValuePair<String, int>>();
                }
                var list = dicFaceTransition[org_simbol];
                if (!list.Contains(elem))
                {
                    dicFaceTransition[org_simbol].Add(elem);
                }

            }
        }
    }

}