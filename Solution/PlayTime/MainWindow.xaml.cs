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
                groupNode.SetAttribute("name", "idle");
                AddSprite(groupNode, "0", 0, 0, 25, 25);
                AddSprite(groupNode, "1", 28, 28, 25, 25);
                AddSprite(groupNode, "2", 55, 55, 25, 25);
                groupsNode.AppendChild(groupNode);

                groupNode = atlasDoc.CreateElement("group");
                groupNode.SetAttribute("name", "walk");
                AddSprite(groupNode, "0", 0, 0, 25, 25);
                AddSprite(groupNode, "1", 28, 28, 25, 25);
                AddSprite(groupNode, "2", 55, 55, 25, 25);
                groupsNode.AppendChild(groupNode);
            }

            UpdateTreeView();
        }


        /// <summary>
        /// creates s sprite child node and appends it to the given group node.
        /// </summary>
        /// <param name="groupNode">Group to append the new sprite child node to.</param>
        /// <param name="id">Id string for the new sprite node.</param>
        /// <param name="x">Minimum x coordinate for new sprite's bounding box on sprite sheet.</param>
        /// <param name="y">Minimum y coordinate for new sprite's bounding box on sprite sheet.</param>
        /// <param name="width">Width of new sprite's bounding box.</param>
        /// <param name="height">Width of new sprite's bounding box.</param>
        private void AddSprite(XmlNode groupNode, string id, int x, int y, int width, int height)
        {
            groupNode.AppendChild(CreateSpriteNode(id, x, y, width, height));
        }

        private void UpdateTreeView()
        {
            XmlNode groupsNode = rootNode.FirstChild;
            
            foreach(XmlNode groupNode in groupsNode.ChildNodes)
            {
                TreeViewItem item = new TreeViewItem();
                item.Header = groupNode.Attributes.GetNamedItem("name").Value;

                foreach(XmlElement spriteElement in groupNode.ChildNodes)
                {
                    TreeViewItem spriteId = new TreeViewItem();
                    spriteId.Header = spriteElement.Attributes.GetNamedItem("id").Value;
                    item.Items.Add(spriteId);
                }

                item.ExpandSubtree();

                treeView.Items.Add(item);
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
