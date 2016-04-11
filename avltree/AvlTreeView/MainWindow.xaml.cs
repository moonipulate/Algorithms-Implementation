using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AVLTree;
using System.Runtime.InteropServices;
using BinaryTree;

namespace AvlTreeView
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        AVLTree<int> _tree = new AVLTree<int>();
        BinaryTree<int> _binaryTree = new BinaryTree<int>();
        int currentCount = 0;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            RenderAvlTree(_tree);
            RenderBinaryTree(_binaryTree);
        }

        private void RenderAvlTree(AVLTree<int> tree)
        {
            Microsoft.Glee.Drawing.Graph graph = new Microsoft.Glee.Drawing.Graph("");

            AVLTreeNode<int> current = tree.Head;

            RenderAvlTree(current, graph);

            Microsoft.Glee.GraphViewerGdi.GraphRenderer renderer = new Microsoft.Glee.GraphViewerGdi.GraphRenderer(graph);
            renderer.CalculateLayout();
            System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap((int)avlImage.Width, (int)avlImage.Height, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
            renderer.Render(bitmap);

            avlImage.Source = loadBitmap(bitmap);
        }

        private void RenderBinaryTree(BinaryTree<int> tree)
        {
            Microsoft.Glee.Drawing.Graph graph = new Microsoft.Glee.Drawing.Graph("");

            BinaryTreeNode<int> current = tree.Head;

            RenderBinaryTree(null, current, graph);

            Microsoft.Glee.GraphViewerGdi.GraphRenderer renderer = new Microsoft.Glee.GraphViewerGdi.GraphRenderer(graph);
            renderer.CalculateLayout();
            System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap((int)binaryImage.Width, (int)binaryImage.Height, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
            renderer.Render(bitmap);

            binaryImage.Source = loadBitmap(bitmap);
        }

        private void RenderAvlTree(AVLTreeNode<int> node, Microsoft.Glee.Drawing.Graph graph)
        {
            if (node != null)
            {
                RenderAvlTree(node.Left, graph);

                if (node.Parent != null)
                {
                    graph.AddEdge(node.Parent.Value.ToString(), node.Value.ToString());
                }

                RenderAvlTree(node.Right, graph);
            }
        }

        private void RenderBinaryTree(BinaryTreeNode<int> parent, BinaryTreeNode<int> child, Microsoft.Glee.Drawing.Graph graph)
        {
            if (child != null)
            {
                RenderBinaryTree(child, child.Left, graph);

                if (parent != null)
                {
                    graph.AddEdge(parent.Value.ToString(), child.Value.ToString());
                }

                RenderBinaryTree(child, child.Right, graph);
            }
        }


        [DllImport("gdi32")]
        static extern int DeleteObject(IntPtr o);

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

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            string[] values = this.txtAddValue.Text.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string value in values)
            {
                _binaryTree.Add(int.Parse(value.Trim()));
                _tree.Add(int.Parse(value.Trim()));
            }
            RenderAvlTree(_tree);
            RenderBinaryTree(_binaryTree);
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            string[] values = this.txtRemoveValue.Text.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string value in values)
            {
                _binaryTree.Remove(int.Parse(value.Trim()));
                _tree.Remove(int.Parse(value.Trim()));
            }

            RenderAvlTree(_tree);
            RenderBinaryTree(_binaryTree);
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            _tree.Clear();
            RenderAvlTree(_tree);

            _binaryTree.Clear();
            RenderBinaryTree(_binaryTree);
        }

        private void btnBad100_Click(object sender, RoutedEventArgs e)
        {
            _tree.Clear();
            _binaryTree.Clear();

            for (int i = 0; i < 100; i++)
            {
                _tree.Add(i);
                _binaryTree.Add(i);
            }

            RenderAvlTree(_tree);
            RenderBinaryTree(_binaryTree);
        }

        private void btnAdd100_Click(object sender, RoutedEventArgs e)
        {
            Random rng = new Random();
            for (int i = 0; i < 100; i++)
            {
                int next = rng.Next();
                while (_tree.Contains(next))
                {
                    next = rng.Next();
                }

                _tree.Add(next);
                _binaryTree.Add(next);
            }

            RenderAvlTree(_tree);
            RenderBinaryTree(_binaryTree);
        }
    }
}
