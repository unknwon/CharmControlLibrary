#region 文档说明
/* *****************************************************************************************************
 * 文档作者：无闻
 * 创建日期：2013年03月04日
 * 文档用途：CharmSkinPanel - 皮肤面板控件
 * -----------------------------------------------------------------------------------------------------
 * 修改记录：
 * 
 * -----------------------------------------------------------------------------------------------------
 * 参考文献：
 * 
 * *****************************************************************************************************/
#endregion

#region 命名空间引用
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using CharmControlLibrary.Properties;

#endregion

namespace CharmControlLibrary
{
    #region 自定义事件
    /// <summary>
    /// 图像皮肤换肤事件
    /// </summary>
    /// <param name="imageSelectedIndex">图像方格现行选中项索引</param>
    public delegate void ImageSkinChangedEventHandler(int imageSelectedIndex);
    #endregion

    /// <summary>
    /// 表示 CharmControlLibrary.CharmSkinPanel 皮肤面板控件
    /// </summary>
    public sealed class CharmSkinPanel : PictureBox
    {
        #region 字段
        // 指示是否显示色调皮肤面板
        private bool mIsShowColorPanel;
        // 图像资源文件路径
        private string mImageResourcePath;
        // 分页标签现行选中项索引
        private int mTabSelectedIndex;
        // 分页标签高亮项索引
        private int mTabHighlightIndex;
        // 图像方格现行选中项索引
        private int mImageSelectedIndex;
        // 图像方格高亮项索引
        private int mImageHighlightIndex;
        #endregion

        #region 属性
        /// <summary>
        /// 获取或设置一个值，该值指示是否显示色调皮肤面板
        /// </summary>
        public bool IsShowColorPanel
        {
            get { return mIsShowColorPanel; }
            set { mIsShowColorPanel = value; }
        }

        /// <summary>
        /// 获取或设置控件的图像资源文件路径（结尾不需要"\"，图像资源文件必须以.png结尾）
        /// </summary>
        public string ImageResourcePath
        {
            get { return mImageResourcePath; }
            set { mImageResourcePath = value; }
        }
        #endregion

        #region  重载事件
        /// <summary>
        /// 引发 Paint 事件
        /// </summary>
        /// <param name="e">包含事件数据的 PaintEventArgs</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            // 判断用户是否设置图像资源文件路径
            if (mImageResourcePath == null)
                throw new ArgumentNullException("CharmControlLibrary.CharmSkinPanel：未指定图像资源文件路径.\n" +
                    "Name:" + Name);

            // 获取绘制对象
            Graphics g = e.Graphics;

            // 绘制横线
            g.DrawLine(Pens.LightGray, new Point(3, 38), new Point(219, 38));

            // 逐项判断选中与高亮情况
            #region 图片皮肤面板
            if (mTabSelectedIndex == 0)
            {
                // 现行选中项，绘制选中背景
                g.DrawImage(Resources.tab_pushed_bkg, new Rectangle(5, 9, 40, 30));
                // 绘制选中图像
                g.DrawImage(Resources.tabshading_pushed, new Rectangle(5, 9, 40, 30));

                // * 绘制图像皮肤内容 *
                // 绘制阴影底图
                g.DrawImage(Resources.shading_bkg, new Rectangle(8, 43, 208, 133));
                // 绘制图像方格
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        int index = i * 3 + j;   // 缩略图索引
                        Point imgPosition = new Point(9 + j * 68 + j, 44 + i * 43 + i); // 缩略图起始绘制坐标
                        // 判断文件是否存在
                        if (File.Exists(mImageResourcePath + "\\" + Convert.ToString(index) + ".png"))
                        {
                            Image imgTemp = Image.FromFile(mImageResourcePath + "\\" + Convert.ToString(index) + ".png");
                            // 绘制缩略图
                            g.DrawImage(imgTemp, new Rectangle(imgPosition, new Size(68, 43)));
                            imgTemp.Dispose();
                            // 判断是否为选中项
                            if (mImageSelectedIndex == index)
                                g.DrawImage(Resources.pic_check, new Rectangle(imgPosition, new Size(68, 43)));

                            // 判断是否为高亮项
                            if (mImageHighlightIndex == index)
                                g.DrawImage(Resources.pic_shading_highlight, new Rectangle(imgPosition, new Size(68, 43)));
                        }
                    }
                }

                // 绘制添加图标
                // 判断是否为高亮项
                if (mImageHighlightIndex == 8)
                    g.DrawImage(Resources.pic_add_highlight, new Rectangle(new Point(9 + 2 * 68 + 2, 44 + 2 * 43 + 2), new Size(68, 43)));
                else
                    g.DrawImage(Resources.pic_add_normal, new Rectangle(new Point(9 + 2 * 68 + 2, 44 + 2 * 43 + 2), new Size(68, 43)));
            }
            else if (mTabHighlightIndex == 0)   // 高亮的情况
                g.DrawImage(Resources.tabshading_highlight, new Rectangle(5, 9, 40, 30));
            else // 普通情况
                g.DrawImage(Resources.tabshading_normal, new Rectangle(5, 9, 40, 30));
            #endregion

            if (!mIsShowColorPanel) return;
            #region 色调皮肤面板
            if (mTabSelectedIndex == 1)
            {
                // 现行选中项，绘制选中背景
                g.DrawImage(Resources.tab_pushed_bkg, new Rectangle(45, 9, 40, 30));
                // 绘制选中图像
                g.DrawImage(Resources.tabcolor_pushed, new Rectangle(45, 9, 40, 30));

                // * 绘制色调皮肤内容 *
            }
            else if (mTabHighlightIndex == 1)   // 高亮的情况
                g.DrawImage(Resources.tabcolor_highlight, new Rectangle(45, 9, 40, 30));
            else // 普通情况
                g.DrawImage(Resources.tabcolor_normal, new Rectangle(45, 9, 40, 30));
            #endregion
        }

        /// <summary>
        /// 引发 MouseClick 事件
        /// </summary>
        /// <param name="e">包含事件数据的 MouseEventArgs</param>
        protected override void OnMouseClick(MouseEventArgs e)
        {
            // 指示是否需要重绘
            bool isRedraw = false;

            // 判断分页标签区
            for (int i = 0; i < 2; i++)
                if (mTabSelectedIndex != i)
                {
                    // 不是选中项
                    if ((new Rectangle(5 + i * 40, 9, 40, 30)).Contains(e.Location))
                    {
                        // 在工作区矩形内
                        mTabSelectedIndex = i;
                        isRedraw = true;
                    }
                }

            // 判断图像皮肤区域
            if (mTabSelectedIndex == 0 && e.X > 9 && e.X < 215 && e.Y > 45 && e.Y < 176)
            {
                // 判断鼠标所在的选区
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        // 判断是否在当前选区内
                        if (e.X > (9 + j * 68 + j) && e.X < (77 + j * 68 + j) && e.Y > (44 + i * 43 + i) && e.Y < (87 + i * 43 + i))
                        {
                            int index = i * 3 + j;   // 缩略图索引
                            string skinFilePath;

                            // 判断是否为添加本地图片
                            if (index == 8)
                            {
                                // 用户选择自定义背景图片资源
                                // 打开文件选择对话框
                                OpenFileDialog openFileDialog = new OpenFileDialog();
                                openFileDialog.Filter = "本地图片资源(*.jpg,*.gif,*.bmp,*.png)|*.jpg;*.gif;*.bmp;*.png";
                                // 判断用户是否选择文件
                                if (openFileDialog.ShowDialog() == DialogResult.OK)
                                {
                                    // 生成缩略图
                                    Bitmap imgTemp = new Bitmap(Image.FromFile(openFileDialog.FileName), new Size(68, 43));
                                    skinFilePath = mImageResourcePath + "\\" + Convert.ToString(index - 1) + ".png";
                                    // 如果已存在用户自定义图片，则先删除
                                    if (File.Exists(skinFilePath))
                                        File.Delete(skinFilePath);
                                    imgTemp.Save(skinFilePath);
                                    imgTemp.Dispose();
                                    skinFilePath = mImageResourcePath + "\\bkg_" + Convert.ToString(index - 1) + ".png";
                                    // 如果已存在用户自定义图片，则先删除
                                    if (File.Exists(skinFilePath))
                                        File.Delete(skinFilePath);
                                    // 移动文件
                                    File.Copy(openFileDialog.FileName, skinFilePath);

                                    // 如果当前用的是用户自定义图片，则将选中项重置，以便进行重新加载
                                    if (mImageSelectedIndex == 7)
                                        mImageSelectedIndex = -1;
                                }
                                openFileDialog.Dispose();
                            }
                            else
                            {
                                mImageSelectedIndex = index;
                                mImageHighlightIndex = -1;
                                Visible = false;
                                // 触发图像皮肤换肤事件
                                if (ImageSkinChanged != null)
                                    ImageSkinChanged(mImageSelectedIndex);
                            }

                            // 重绘控件
                            Invalidate();
                        }
                    }
                }
            }

            // 判断是否需要重绘
            if (isRedraw)
                Invalidate();

            // 触发基类事件
            base.OnMouseClick(e);
        }

        /// <summary>
        /// 引发 MouseMove 事件
        /// </summary>
        /// <param name="e">包含事件数据的 MouseEventArgs</param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            // 指示是否需要重绘
            bool isRedraw = false;

            // 判断分页标签区
            for (int i = 0; i < 2; i++)
                if (mTabSelectedIndex != i)
                {
                    // 不是选中项
                    if ((new Rectangle(5 + i * 40, 9, 40, 30)).Contains(e.Location))
                    {
                        // 在工作区矩形内
                        if (mTabHighlightIndex != i)
                        {
                            mTabHighlightIndex = i;
                            isRedraw = true;
                        }
                    }
                    else if (mTabHighlightIndex == i)
                    {
                        mTabHighlightIndex = -1;
                        isRedraw = true;
                    }
                }

            // 判断图像皮肤区域
            if (mTabSelectedIndex == 0 && e.X > 9 && e.X < 215 && e.Y > 45 && e.Y < 176)
            {
                // 判断鼠标所在选区
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        // 判断鼠标是否在当前选区内
                        if (e.X > (9 + j * 68 + j) && e.X < (77 + j * 68 + j) && e.Y > (44 + i * 43 + i) && e.Y < (87 + i * 43 + i))
                        {
                            int index = i * 3 + j;   // 缩略图索引
                            // 判断自定义背景图片是否存在
                            if (index == 8 || File.Exists(mImageResourcePath + "\\" + Convert.ToString(index) + ".png"))
                            {
                                // 自定义背景图片资源存在则判断是否需要进行特效重绘
                                if (mImageHighlightIndex != (index))
                                {
                                    mImageHighlightIndex = index;
                                    isRedraw = true;
                                }
                            }
                        }
                    }
                }
            }
            else if (mImageHighlightIndex != -1)
            {
                mImageHighlightIndex = -1;
                isRedraw = true;
            }

            // 判断是否需要重绘
            if (isRedraw)
                Invalidate();

            // 触发基类事件
            base.OnMouseMove(e);
        }
        #endregion

        #region 自定义事件
        /// <summary>
        /// 当图像皮肤换肤时发生
        /// </summary>
        public event ImageSkinChangedEventHandler ImageSkinChanged;
        #endregion

        #region 构造方法
        /// <summary>
        /// 用默认设置初始化 CharmSkinPanel 类的新实例
        /// </summary>
        public CharmSkinPanel()
            : base()
        {
            // * 设置双缓冲模式 *
            SetStyle(ControlStyles.AllPaintingInWmPaint | //不擦除背景 ,减少闪烁
                               ControlStyles.OptimizedDoubleBuffer | //双缓冲
                               ControlStyles.UserPaint, //使用自定义的重绘事件,减少闪烁
                               true);
            UpdateStyles();

            // * 初始化属性 *
            BackColor = Color.Transparent;
            BackgroundImage = Resources.skinpanel_bkg;
            Size = BackgroundImage.Size;
            Visible = false;
            mTabSelectedIndex = 0;
            mTabHighlightIndex = -1;
            mImageSelectedIndex = -1;
            mImageHighlightIndex = -1;
        }
        #endregion
    }
}
