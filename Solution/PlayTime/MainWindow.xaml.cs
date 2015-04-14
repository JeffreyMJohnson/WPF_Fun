using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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
        private XmlDocument atlasDoc = new XmlDocument();
        private XmlElement mRootNode;
        private XmlElement mGroupsNode;
        private ContextMenu mGroupNameContextMenu;
        private const string SPRITE_SHEET_FILE_PATH = "foo.png";
        private const int SPRITE_SHEET_WIDTH = 900;
        private const int SPRITE_SHEET_HEIGHT = 600;
        private int mPageCount = 1;
        private bool mNormalizeUV = false;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("foo");
        }

        public MainWindow()
        {
            InitializeComponent();
            InitAtlasDoc();
            //InitContextMenu();
            //XmlNode groupsNode = rootNode.SelectSingleNode("groups");
            //if (groupsNode != null)
            //{
            //    XmlElement groupNode = atlasDoc.CreateElement("group");
            //    groupNode.SetAttribute("name", "idle");
            //    AddSprite(groupNode, "0", 0, 0, 25, 25);
            //    AddSprite(groupNode, "1", 28, 28, 25, 25);
            //    AddSprite(groupNode, "2", 55, 55, 25, 25);
            //    groupsNode.AppendChild(groupNode);

            //    groupNode = atlasDoc.CreateElement("group");
            //    groupNode.SetAttribute("name", "walk");
            //    AddSprite(groupNode, "0", 0, 0, 25, 25);
            //    AddSprite(groupNode, "1", 28, 28, 25, 25);
            //    AddSprite(groupNode, "2", 55, 55, 25, 25);
            //    groupsNode.AppendChild(groupNode);
            //}

            UpdateTreeView();

           // double dpi = 96;
           // int width = 128;
           // int height = 128;
            
          
           //// byte[] pixelData = new byte[width * height];
           // Color[] pixelData = new Color[width * height];
           // for (int y = 0; y < height; y++)
           // {
           //     int yIndex = y * width;
           //     for (int x = 0; x < width; x++)
           //     {
           //         //pixelData[x + yIndex] = 
           //     }
           // }
           // BitmapSource bmpSource = BitmapSource.Create(width, height, dpi, dpi, PixelFormats.Gray8, null, pixelData, width);

           // imageContainer.Width = width + 10;
           // imageContainer.Height = height + 10;
            
           // imageContainer.Source = bmpSource;

            List<string> filesList = new List<string>() {@"c:\Users\Jeff\Documents\GitHub\2D_Retro_Clone\resources\images\galaxian.png",
                                     @"c:\Users\Jeff\Documents\GitHub\2D_Retro_Clone\resources\images\blue_enemy\blue_enemy_1.png"};

            BitmapImage bmp = new BitmapImage();
            bmp.BeginInit();
            bmp.UriSource = new Uri(@"c:\Users\Jeff\Documents\GitHub\2D_Retro_Clone\resources\images\galaxian.png");
            bmp.EndInit();

            PixelFormat format = bmp.Format;
            int width = (int)bmp.Width;
            int height = (int)bmp.Height;
            int stride = 0;
            if (format == PixelFormats.Bgra32)
            {
                stride = width * 4;
            }

            Byte[] b = new Byte[stride * height];

           

            bmp.CopyPixels(b, stride, 0);

            for (int i = 0; i < b.Length; i++)
            {
                if (b[i] == 0 &&
                    b[++i] == 0 &&
                    b[++i] == 0 &&
                    b[++i] == 255)
                {
                    b[i] = 0;
                }
            }
            //int spriteSheetWidth = (int)(width * .5) + width;
            //int spriteSheetHeight = (int)(height * .5) + height;
            //int spriteSheetStride = spriteSheetWidth * 4;
            ////Byte[] spriteSheetArray = new byte[(int)((stride * height) * .5) + (stride * height)];
            //Byte[] spriteSheetArray = new byte[spriteSheetStride * spriteSheetHeight];
            //for (int i = 0; i < spriteSheetArray.Length; i++)
            //{
            //    spriteSheetArray[i] = 255;
            //}
            //Array.Copy(b, spriteSheetArray, b.Length);

            System.Drawing.Bitmap myBMap = new System.Drawing.Bitmap(filesList[0]);
            myBMap.MakeTransparent();
            BitmapSource b2 = BitmapSource.Create(width, height, bmp.DpiX, bmp.DpiY, format, null, b, stride);
            imageContainer.Source = bmp;
            img2Container.Source = loadBitmap(myBMap);
            txtBlock.Text = bmp.Format.ToString();
        }

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);
        public static BitmapSource loadBitmap(System.Drawing.Bitmap source)
        {
            IntPtr ip = source.GetHbitmap();
            BitmapSource bs = null;
            try
            {
                bs = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(ip,
                   IntPtr.Zero, Int32Rect.Empty,
                   System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
            }
            finally
            {
                DeleteObject(ip);
            }

            return bs;
        }

        private void InitContextMenu()
        {
            InitGroupNameContextMenu();
        }

        private void InitGroupNameContextMenu()
        {
            mGroupNameContextMenu = new ContextMenu();
            MenuItem mi = new MenuItem();
            mi.Header = "_Rename Group";
            mi.Click += GroupRenameClick;
            mGroupNameContextMenu.Items.Add(mi);
        }

        private void GroupRenameClick(object sender, EventArgs e)
        {
            TreeViewItem m = sender as TreeViewItem;
            if (m != null)
            {
                Console.WriteLine(m.Name);

            }
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
            XmlNode groupsNode = mRootNode.FirstChild;
            TreeViewItem root = new TreeViewItem();
            root.Header = "SpriteSheet";
            int counter = 0;
            foreach (XmlNode groupNode in groupsNode.ChildNodes)
            {
                TreeViewItem item = new TreeViewItem();
                item.Header = groupNode.Attributes.GetNamedItem("name").Value;
                //item.ContextMenu = mGroupNameContextMenu;
                item.Name = "group" + counter.ToString();
                counter++;
                item.MouseRightButtonDown += GroupRenameClick;
                foreach (XmlElement spriteElement in groupNode.ChildNodes)
                {
                    TreeViewItem spriteId = new TreeViewItem();
                    spriteId.Header = spriteElement.Attributes.GetNamedItem("id").Value;
                    item.Items.Add(spriteId);
                }

                item.ExpandSubtree();

                root.Items.Add(item);
                root.ExpandSubtree();
            }
            treeView.Items.Add(root);


        }


        private void InitAtlasDoc()
        {
            mRootNode = atlasDoc.CreateElement("SpriteSheet");
            atlasDoc.AppendChild(mRootNode);
            XmlAttribute att = atlasDoc.CreateAttribute("filePath");
            att.Value = SPRITE_SHEET_FILE_PATH;
            mRootNode.SetAttributeNode(att);

            att = atlasDoc.CreateAttribute("width");
            att.Value = SPRITE_SHEET_WIDTH.ToString();
            mRootNode.SetAttributeNode(att);

            att = atlasDoc.CreateAttribute("height");
            att.Value = SPRITE_SHEET_HEIGHT.ToString();
            mRootNode.SetAttributeNode(att);

            att = atlasDoc.CreateAttribute("page");
            att.Value = "1";
            mRootNode.SetAttributeNode(att);

            att = atlasDoc.CreateAttribute("totalPages");
            att.Value = mPageCount.ToString();
            mRootNode.SetAttributeNode(att);

            att = atlasDoc.CreateAttribute("isNormalized");
            att.Value = mNormalizeUV.ToString();
            mRootNode.SetAttributeNode(att);

            mGroupsNode = atlasDoc.CreateElement("groups");
            mRootNode.AppendChild(mGroupsNode);

            XmlElement groupNode = atlasDoc.CreateElement("group"); ;
            groupNode.SetAttribute("name", "group0");
            mGroupsNode.AppendChild(groupNode);
        }

        private XmlElement CreateSpriteNode(string id, int x, int y, int width, int height)
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
            System.Windows.Forms.OpenFileDialog dialog = new System.Windows.Forms.OpenFileDialog();
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
                // txtBox.Text = text;
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
