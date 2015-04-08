using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;

namespace PlayTime
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static XmlDocument atlasDoc = new XmlDocument();
        private static XmlElement rootNode;
        private const string SPRITE_SHEET_FILE_PATH = "foo.png";
        private const int SPRITE_SHEET_WIDTH = 900;
        private const int SPRITE_SHEET_HEIGHT = 600;
        private static int mPageCount = 1;
        private static bool mNormalizeUV = false;


        public MainWindow()
        {
            InitializeComponent();
            InitAtlasDoc();
            XmlNode groupsNode = rootNode.SelectSingleNode("groups");
            if (groupsNode != null)
            {
                XmlElement groupNode = atlasDoc.CreateElement("group");
                groupsNode.AppendChild(groupNode);
                groupNode.AppendChild(CreateSpriteNode("0", 0, 0, 25, 25));
                groupNode.AppendChild(CreateSpriteNode("1", 0, 0, 25, 25));
                groupNode.AppendChild(CreateSpriteNode("2", 0, 0, 25, 25));
            }
        }

        private static void InitAtlasDoc()
        {
            rootNode = atlasDoc.CreateElement("SpriteSheet");
            atlasDoc.AppendChild(rootNode);
            XmlAttribute att = atlasDoc.CreateAttribute("filePath");
            att.Value = SPRITE_SHEET_FILE_PATH;
            rootNode.SetAttributeNode(att);

            att = atlasDoc.CreateAttribute("width");
            att.Value = SPRITE_SHEET_WIDTH.ToString();
            rootNode.SetAttributeNode(att);

            att = atlasDoc.CreateAttribute("height");
            att.Value = SPRITE_SHEET_HEIGHT.ToString();
            rootNode.SetAttributeNode(att);

            att = atlasDoc.CreateAttribute("page");
            att.Value = "1";
            rootNode.SetAttributeNode(att);

            att = atlasDoc.CreateAttribute("totalPages");
            att.Value = mPageCount.ToString();
            rootNode.SetAttributeNode(att);

            att = atlasDoc.CreateAttribute("isNormalized");
            att.Value = "false";
            rootNode.SetAttributeNode(att);

            XmlElement node = atlasDoc.CreateElement("groups");
            rootNode.AppendChild(node);
        }

        private static XmlElement CreateSpriteNode(string id, int x, int y, int width, int height)
        {
            XmlElement spriteNode = atlasDoc.CreateElement("sprite");
            XmlAttribute att = atlasDoc.CreateAttribute("id");
            att.Value = id;
            spriteNode.SetAttributeNode(att);
            
            att = atlasDoc.CreateAttribute("x");
            att.Value = x.ToString();
            spriteNode.SetAttributeNode(att);

            att = atlasDoc.CreateAttribute("y");
            att.Value = y.ToString();
            spriteNode.SetAttributeNode(att);

            att = atlasDoc.CreateAttribute("width");
            att.Value = width.ToString();
            spriteNode.SetAttributeNode(att);

            att = atlasDoc.CreateAttribute("height");
            att.Value = height.ToString();
            spriteNode.SetAttributeNode(att);

            return spriteNode;
        }

        private void ProcessFileOpenClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = true;
            dialog.DefaultExt = ".png";
            dialog.Filter = "Image Files|*.png;*.bmp;*.jpg";

            //show the box
            System.Windows.Forms.DialogResult result = dialog.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                string[] fileNames = dialog.FileNames;
                string text = "";

                for (int i = 0; i < fileNames.Length; i++)
                {
                    text += fileNames[i] + "\n";
                }
                txtBox.Text = text;
            }
        }

        private void mWindow_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                mWindow.Close();
            }
        }

        private void btn_Click(object sender, RoutedEventArgs e)
        {
            treeView.Items.Add("i'm new");
        }
    }
}
