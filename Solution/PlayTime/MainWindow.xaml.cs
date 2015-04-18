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
    class ImageHighlight
    {
        Rectangle mRec = new Rectangle();
        DoubleCollection mStrokeDashArray = new DoubleCollection() { 5, 5 };
        const double mStrokeThickness = 3;
        Color mStrokeColor = Colors.Blue;
        Image2 mImage;

        public Rectangle Rectangle
        {
            get { return mRec; }
        }

        public ImageHighlight(Image2 image)
        {
            mImage = image;
            mRec.Width = mImage.Width;
            mRec.Height = mImage.Height;
            mRec.Stroke = new SolidColorBrush(mStrokeColor);
            mRec.StrokeThickness = mStrokeThickness;
            mRec.StrokeDashArray = mStrokeDashArray;
        }

    }

    class Image2
    {
        string mFilePath;
        BitmapImage mBMP;//the actual image in memory
        Image mControl;//the image control to be placed on a canvas
        ImageHighlight mHighlightRec;
        bool mIsHighlighted = false;
        double mLeft, mTop;

        /// <summary>
        /// Path to image file.
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Height of image.
        /// </summary>
        public double Height { get; private set; }

        /// <summary>
        /// Width of image.
        /// </summary>
        public double Width { get; private set; }

        /// <summary>
        /// Image control for placing on a canvas.
        /// </summary>
        public Image ImageControl
        {
            get { return mControl; }
        }

        /// <summary>
        /// Left position of image.
        /// i.e. The minimum x value of image.
        /// </summary>
        public double Left
        {
            get { return mLeft; }
            set
            {
                mLeft = value;
                Canvas.SetLeft(mControl, mLeft);
            }
        }

        /// <summary>
        /// Top position of image.
        /// i.e. The minimum y value of image.
        /// </summary>
        public double Top
        {
            get { return mTop; }
            set
            {
                mTop = value;
                Canvas.SetTop(mControl, mTop);
            }
        }

        /// <summary>
        /// Rectangle shape for highlighting this image.
        /// </summary>
        public Rectangle HighlightRec
        {
            get { return mHighlightRec.Rectangle; }
        }

        /// <summary>
        /// MyImage constructor
        /// </summary>
        /// <param name="path">Path to image file.</param>
        public Image2(string path)
        {
            mFilePath = path;
            mBMP = new BitmapImage(new Uri(mFilePath));
            Width = mBMP.Width;
            Height = mBMP.Height;
            InitControl();
            mHighlightRec = new ImageHighlight(this);
            mControl.MouseLeftButtonUp += OnLeftButtonUp;


        }

        private void OnLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Canvas canvas = mControl.Parent as Canvas;
            if (canvas != null)
            {
                HighlightIt(canvas);
            }
        }

        public void HighlightIt(Canvas canvas)
        {
            if (!mIsHighlighted)
            {
                //insert highlight rectangle before this control in canvas children
                int i = canvas.Children.IndexOf(this.mControl);
                Canvas.SetLeft(mHighlightRec.Rectangle, Left);
                canvas.Children.Insert(i + 1, mHighlightRec.Rectangle);
                mIsHighlighted = true;
            }
            else
            {
                canvas.Children.Remove(mHighlightRec.Rectangle);
                mIsHighlighted = false;
            }

        }

        void InitControl()
        {
            mControl = new Image();
            mControl.Source = mBMP;
            mControl.Stretch = Stretch.None;
        }

    }

    class SpriteSheet
    {
        List<Image2> mSpritesList = new List<Image2>();
        string mBasePath;
        bool mIsNormalized;
        Canvas mCanvasControl;
        XmlDocument mAtlasDoc = new XmlDocument();
        XmlElement mRootNode;
        //XmlElement mGroupsNode;

        public double Width { get; set; }
        public double Height { get; set; }



        public SpriteSheet(Canvas canvas, string basePath, bool normalize)
        {
            mBasePath = basePath;
            mIsNormalized = normalize;
            InitAtlasDoc();
            mCanvasControl = canvas;            
        }

        
        public void AddSprite(string relativePath)
        {
            Image2 img = new Image2(mBasePath + relativePath);
            GetNextImagePosition(img);
            mCanvasControl.Width += img.Width;
            mCanvasControl.Height += img.Height;
            mCanvasControl.Children.Add(img.ImageControl);

            //XmlElement spriteNode = mAtlasDoc.CreateElement("sprite");
            //XmlAttribute att = mAtlasDoc.CreateAttribute("id");
            //att.Value = id;
            //spriteNode.SetAttributeNode(att);

            //att = mAtlasDoc.CreateAttribute("x");
            //att.Value = x.ToString();
            //spriteNode.SetAttributeNode(att);

            //att = mAtlasDoc.CreateAttribute("y");
            //att.Value = y.ToString();
            //spriteNode.SetAttributeNode(att);

            //att = mAtlasDoc.CreateAttribute("width");
            //att.Value = width.ToString();
            //spriteNode.SetAttributeNode(att);

            //att = mAtlasDoc.CreateAttribute("height");
            //att.Value = height.ToString();
            //spriteNode.SetAttributeNode(att);
             

            mSpritesList.Add(img);
        }

        void GetNextImagePosition(Image2 newImage)
        {
            if (mSpritesList.Count == 0) return;
            
            Image2 lastImage = mSpritesList[mSpritesList.Count - 1];
            //check if need new row
            if(lastImage.Left + lastImage.Width + newImage.Width > Width)
            {
                newImage.Left = 0;
                newImage.Top = GetHighestYInRow();
            }
            else
            {
                newImage.Left = lastImage.Left + lastImage.Width;
                newImage.Top = lastImage.Top;
            }
        }

        double GetHighestYInRow()
        {
            double result = 0;
            foreach(Image2 img in mSpritesList)
            {
                if (img.Top + img.Height > result) result = img.Top + img.Height;
            }
            return result;
        }

        private void InitAtlasDoc()
        {
            mRootNode = mAtlasDoc.CreateElement("SpriteSheet");
            mAtlasDoc.AppendChild(mRootNode);
            XmlAttribute att = mAtlasDoc.CreateAttribute("filePath");
            att.Value = mBasePath;
            mRootNode.SetAttributeNode(att);

            att = mAtlasDoc.CreateAttribute("width");
            att.Value = Width.ToString();
            mRootNode.SetAttributeNode(att);

            att = mAtlasDoc.CreateAttribute("height");
            att.Value = Height.ToString();
            mRootNode.SetAttributeNode(att);

            att = mAtlasDoc.CreateAttribute("page");
            att.Value = "1";
            mRootNode.SetAttributeNode(att);

            //att = mAtlasDoc.CreateAttribute("totalPages");
            //att.Value = mPageCount.ToString();
            //mRootNode.SetAttributeNode(att);

            att = mAtlasDoc.CreateAttribute("isNormalized");
            att.Value = mIsNormalized.ToString();
            mRootNode.SetAttributeNode(att);

           // mGroupsNode = mAtlasDoc.CreateElement("groups");
            //mRootNode.AppendChild(mGroupsNode);

            XmlElement groupNode = mAtlasDoc.CreateElement("group"); ;
           // groupNode.SetAttribute("name", "group0");
            mRootNode.AppendChild(groupNode);
        }
    }


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
            Vector startPos = new Vector(0, 0);

            Uri baseURI = new Uri(AppDomain.CurrentDomain.BaseDirectory + @"../../resources/");

            SpriteSheet sheet = new SpriteSheet(canvasControl, AppDomain.CurrentDomain.BaseDirectory + @"../../resources/", false);
            sheet.Width = 1024;
            sheet.Height = 768;
            sheet.AddSprite("images/lobo.jpg");
            sheet.AddSprite("images/usa.png");
            //Image2 image1 = new Image2(AppDomain.CurrentDomain.BaseDirectory + @"../../resources/" + "images/lobo.jpg");
            //image1.Left = 0;
            //image1.Top = 0;
            //BitmapImage image2 = new BitmapImage(new Uri(baseURI, "images/usa.png"));
           // Image2 image2 = new Image2(AppDomain.CurrentDomain.BaseDirectory + @"../../resources/" + "images/usa.png");
            //Canvas.SetLeft(image1.ImageControl, image1.Left);

            //startPos.X += image1.Width + 10;
            //image2.Left = startPos.X;

            //canvasControl.Width = image1.Width + image2.Width + 20;
            //canvasControl.Height = image1.Height + image2.Height + 10;


            //canvasControl.Children.Add(image1.ImageControl);
           // canvasControl.Children.Add(image2.ImageControl);

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

            // UpdateTreeView();

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
            /*
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
             */
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

        //private void UpdateTreeView()
        //{
        //    XmlNode groupsNode = mRootNode.FirstChild;
        //    TreeViewItem root = new TreeViewItem();
        //    root.Header = "SpriteSheet";
        //    int counter = 0;
        //    foreach (XmlNode groupNode in groupsNode.ChildNodes)
        //    {
        //        TreeViewItem item = new TreeViewItem();
        //        item.Header = groupNode.Attributes.GetNamedItem("name").Value;
        //        //item.ContextMenu = mGroupNameContextMenu;
        //        item.Name = "group" + counter.ToString();
        //        counter++;
        //        item.MouseRightButtonDown += GroupRenameClick;
        //        foreach (XmlElement spriteElement in groupNode.ChildNodes)
        //        {
        //            TreeViewItem spriteId = new TreeViewItem();
        //            spriteId.Header = spriteElement.Attributes.GetNamedItem("id").Value;
        //            item.Items.Add(spriteId);
        //        }

        //        item.ExpandSubtree();

        //        root.Items.Add(item);
        //        root.ExpandSubtree();
        //    }
        //    treeView.Items.Add(root);


        //}


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
            //treeView.Items.Add("i'm new");
        }
    }
}
