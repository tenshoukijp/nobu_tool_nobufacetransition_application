using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace NobuFaceTransition
{


    // ダメなリンクを形成する
    public partial class NTF_Form : Form
    {
        // Stringがキー。値は、リスト。各リストの型は「文字列がキー、整数が値」。
        static Dictionary<String, List<String>> dicDameTransition = new Dictionary<String, List<String>>();

        void InitDameTransitionDic()
        {
            String line = "";

            //Regexオブジェクトを作成
            System.Text.RegularExpressions.Regex r = new System.Text.RegularExpressions.Regex(@"^(nobu.+?\.png)\=\>(nobu.+?\.png)");

            System.IO.StreamReader file = new System.IO.StreamReader(@"UnLink.txt");
            while ((line = file.ReadLine()) != null)
            {
                line = line.ToLower();
                Match m = r.Match(line);
                if (m.Success)
                {
                    string org_simbol = (string)m.Groups[1].Value;
                    string dst_simbol = (string)m.Groups[2].Value;
                    RegistDameInDictinary(org_simbol, dst_simbol); // 正規の順番を登録
                    RegistDameInDictinary(dst_simbol, org_simbol); // 逆側も登録
                }
            }

            file.Close();
        }

        void RegistDameInDictinary(string org_simbol, string dst_simbol)
        {
            if (org_simbol.Length > 1 && dst_simbol.Length > 1)
            {
                String elem = dst_simbol;
                if (!dicDameTransition.ContainsKey(org_simbol))
                {
                    dicDameTransition[org_simbol] = new List<String>();
                }
                var list = dicDameTransition[org_simbol];
                if (!list.Contains(elem))
                {
                    dicDameTransition[org_simbol].Add(elem);
                }

            }
        }
    }




}
