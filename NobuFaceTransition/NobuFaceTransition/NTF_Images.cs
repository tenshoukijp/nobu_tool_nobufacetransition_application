using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace NobuFaceTransition
{


    // イメージリソースとシンボル管理
    public partial class NTF_Form : Form
    {
        void LoadBlankBmpFromEmbeddedResources()
        {
            // このプログラムのアセンブリ
            System.Reflection.Assembly asm = System.Reflection.Assembly.GetExecutingAssembly();
            System.Resources.ResourceManager rm = new System.Resources.ResourceManager(asm.GetName().Name + ".NobuFaceTransitionRes", asm);
            bmBlank = (Bitmap)rm.GetObject("BlankImage");
            bmBlank.Tag = "bmBlank";

            bmUnlink = (Bitmap)rm.GetObject("UnlinkImage");
            bmUnlink.Tag = "bmUnlink";
        }


        Dictionary<string, Bitmap> bmpDictionary = new Dictionary<string, Bitmap>();
        Dictionary<string, List<String>> lstDictionary = new Dictionary<string, List<String>>();

        void LoadBmpFromEmbeddedResources()
        {
            // このプログラムのアセンブリ
            System.Reflection.Assembly asm = System.Reflection.Assembly.GetExecutingAssembly();

            // アセンブリに含まれるリソースの名前全部
            string[] resources = asm.GetManifestResourceNames();


            foreach (string resourcename in resources)
            {
                // ****.nobu***.png。数も多いのでこのぐらいの判定で。
                if (resourcename.Contains(".nobu") && resourcename.EndsWith(".png"))
                {
                    {
                        Bitmap bmp = new Bitmap(asm.GetManifestResourceStream(resourcename));
                        string simbol = getSimbolName(resourcename);
                        bmp.Tag = simbol;
                        bmpDictionary[simbol] = bmp;

                        string serias = getSeriasName(resourcename);
                        if (!lstDictionary.ContainsKey(serias))
                        {
                            lstDictionary[serias] = new List<String>();
                        }

                        lstDictionary[serias].Add(simbol);
                    }
                }
            }
        }

        string getSimbolName(string simbol)
        {
            if (simbol.Length > 17)
            {
                return simbol.Substring(simbol.Length - 17, 17); // NobuFaceTransition.images.nobu11pk_0001.png ⇒ nobu11pk_001.png へするため。最後の17文字
            }
            else
            {
                throw new System.NullReferenceException("リソース名が短すぎます。");
            }
        }
        string getSeriasName(string simbol)
        {
            if (simbol.Length > 8)
            {
                return simbol.Substring(simbol.Length - 17, 8); // NobuFaceTransition.images.nobu11pk_0001.png ⇒ nobu11pk_001.png へするため。最後の17文字
            }
            else
            {
                throw new System.NullReferenceException("リソース名が短すぎます。");
            }
        }
    }

}