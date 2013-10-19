#region 文档说明
/* *****************************************************************************************************
 * 文档作者：无闻
 * 创建日期：2012年10月20日
 * 文档用途：CharmMenu - 菜单控件
 * -----------------------------------------------------------------------------------------------------
 * 修改记录：
 * 2013-02-22：针对CSBox界面标准2.0进行升级改造
 * -----------------------------------------------------------------------------------------------------
 * 参考文献：
 * 
 * *****************************************************************************************************/
#endregion

#region 命名空间引用
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

using CharmCommonMethod;
using CharmControlLibrary.Properties;

#endregion

namespace CharmControlLibrary
{
    #region 枚举类型
    /// <summary>
    /// 菜单类型：QQ、QQ软件管家
    /// </summary>
    public enum MenuType
    {
        /// <summary>
        /// 菜单类型：QQ
        /// </summary>
        QQ,
        /// <summary>
        /// 菜单类型：QQ软件管家
        /// </summary>
        QQSoftMgr
    }

    /// <summary>
    /// 菜单项类型：文本项、分割线
    /// </summary>
    public enum MenuItemType
    {
        /// <summary>
        /// 菜单项类型：文本项
        /// </summary>
        TextItem,
        /// <summary>
        /// 菜单项类型：分割线
        /// </summary>
        Spliter
    }
    #endregion

    #region 自定义事件
    /// <summary>
    /// 菜单事件处理
    /// </summary>
    /// <param name="clickIndex">菜单单击项索引</param>
    public delegate void MenuEventHandler(int clickIndex);
    #endregion

    /// <summary>
    /// 表示 CharmControlLibrary.CharmMenu 菜单控件
    /// </summary>
    public partial class CharmMenu : Form
    {
        #region 结构
        // 菜单项：菜单项类型、菜单图标、标题文本
        private struct MenuItem
        {
            public MenuItemType MenuItemType;  // 菜单项类型
            public Image Icon;                              // 菜单图标
            public string Text;                               // 标题文本
        }
        #endregion

        #region 字段
        // 菜单类型
        private MenuType mMenuType;
        // 菜单项集合
        private List<MenuItem> mItems;
        // 自定义的背景图像资源
        private Image mCustomizeBackgourndImage;
        // 菜单现行选中项索引
        private int mSelectedIndex;
        #endregion

        #region 属性
        /// <summary>
        /// 获取或设置菜单类型
        /// </summary>
        public MenuType MenuType
        {
            get { return mMenuType; }
            set { mMenuType = value; }
        }

        /// <summary>
        /// 获取或设置自定义的背景图像资源
        /// </summary>
        public Image CustomizeBackgourndImage
        {
            get { return mCustomizeBackgourndImage; }
            set { mCustomizeBackgourndImage = value; }
        }
        #endregion

        #region 窗体事件
        /// <summary>
        /// 用默认设置初始化 CharmMenu 类的新实例
        /// </summary>
        public CharmMenu()
        {
            InitializeComponent();
            // 设置窗体阴影特效
            APIOperation.FormBorderShadow(Handle);
            // * 设置双缓冲模式 *
            SetStyle(ControlStyles.AllPaintingInWmPaint | //不擦除背景 ,减少闪烁
                               ControlStyles.OptimizedDoubleBuffer | //双缓冲
                               ControlStyles.UserPaint, //使用自定义的重绘事件,减少闪烁
                               true);
            UpdateStyles();

            // * 初始化属性 *
            TopMost = true;
            ShowInTaskbar = false;
            FormBorderStyle = FormBorderStyle.None;

            mItems = new List<MenuItem>();
            mSelectedIndex = -1;
        }

        // 窗体失去焦点事件，用于隐藏窗体
        private void CharmMenu_Deactivate(object sender, EventArgs e)
        {
            Visible = false;
        }

        // 窗体可见性改变事件，用于重新设置窗体属性
        private void CharmMenu_VisibleChanged(object sender, EventArgs e)
        {
            // 判断窗体是否变为可见
            if (Visible)
            {
                // 窗体长度及宽度
                int formWidth = 0;
                int formHeight = 0;

                // 根据菜单项数组长度决定窗体属性
                Graphics g = CreateGraphics();
                SizeF sf;
                MenuItem menuItem;

                // 轮询每个菜单项，确定窗体需要的长度和宽度
                for (int i = 0; i < mItems.Count; i++)
                {
                    menuItem = mItems[i];
                    // 判断菜单项类型
                    if (menuItem.MenuItemType == MenuItemType.Spliter)
                    {
                        // 判断当前菜单类型是否支持分割线
                        if (mMenuType != MenuType.QQ)
                            throw new ArgumentException(
                                "CharmControlLibrary.CharmMenu：当前菜单类型不支持分割线.\n" +
                                "Name: " + Name);
                        formHeight += 3;    // 分割线
                    }
                    else
                    {
                        // 文本项
                        formHeight += 22;
                        // 计算绘制文本所需的像素
                        sf = g.MeasureString(menuItem.Text, Font);
                        // 判断当前项文本所需像素是否大于之前项
                        if ((int)sf.Width > formWidth)
                            formWidth = (int)sf.Width;
                    }
                }

                Bitmap imgTemp = null;
                // 设置窗体长度及宽度
                Width = formWidth + 28 + 18;
                Height = formHeight + 4;

                // 根据菜单类型设置窗体背景
                switch (mMenuType)
                {
                    case MenuType.QQ:
                        imgTemp = new Bitmap(Resources.menu_bkg_qq);
                        imgTemp = ImageOperation.ResizeImageWithoutBorder(
                                            imgTemp, 29, 2, 2, 2, new Size(Width, Height));
                        break;
                    case MenuType.QQSoftMgr:
                        imgTemp = new Bitmap(Resources.menu_bkg_qqsoftmgr);
                        imgTemp = ImageOperation.ResizeImageWithoutBorder(
                                  imgTemp, 29, 2, 2, 2, new Size(Width, Height));
                        break;
                }
                BackgroundImage = new Bitmap(imgTemp);

                // 释放系统资源
                g.Dispose();
                imgTemp.Dispose();

                // 设置窗体显示的位置
                Rectangle rect = Screen.GetWorkingArea(this);
                // 判断是否有足够空间在光标右侧弹出菜单
                if ((rect.Width - MousePosition.X) > Width)
                    Left = MousePosition.X;     // 有足够空间
                else
                    Left = MousePosition.X - Width;   // 没有足够空间

                // 判断是否有足够空间在光标下方弹出菜单
                if ((rect.Height - MousePosition.Y) > Height)
                    Top = MousePosition.Y;     // 有足够空间
                else
                    Top = MousePosition.Y - Height;   // 没有足够空间
            }
        }

        // 窗体鼠标移动事件
        private void CharmMenu_MouseMove(object sender, MouseEventArgs e)
        {
            // 过程中需要用到的变量
            MenuItem menuItem;
            int formHeight = 0;
            int pointY = MousePosition.Y - Top;

            // 轮询每个菜单项，确定窗体需要的长度和宽度
            for (int i = 0; i < mItems.Count; i++)
            {
                menuItem = mItems[i];
                // 判断菜单项类型
                if (menuItem.MenuItemType == MenuItemType.Spliter)
                {
                    // 分割线
                    // 判断当前菜单类型是否支持分割线
                    if (mMenuType != MenuType.QQ)
                        throw new ArgumentException(
                            "CharmControlLibrary.CharmMenu：当前菜单类型不支持分割线.\n" +
                            "Name: " + Name);
                    formHeight += 3;
                }
                else
                {
                    // 文本项
                    // 判断是否为选择项
                    if (mSelectedIndex != i && pointY > formHeight && pointY < (formHeight + 22))
                    {
                        mSelectedIndex = i;
                        Invalidate();
                        break;
                    }
                    formHeight += 22;
                }
            }
        }

        // 窗体鼠标离开事件
        private void CharmMenu_MouseLeave(object sender, EventArgs e)
        {
            mSelectedIndex = -1;
            Invalidate();
        }

        // 窗体鼠标单击事件
        private void CharmMenu_MouseClick(object sender, MouseEventArgs e)
        {
            // 过程中需要用到的变量
            MenuItem menuItem;
            int formHeight = 0;

            // 轮询每个菜单项，确定窗体需要的长度和宽度
            for (int i = 0; i < mItems.Count; i++)
            {
                menuItem = mItems[i];
                // 判断菜单项类型
                if (menuItem.MenuItemType == MenuItemType.Spliter)
                {
                    // 分割线
                    // 判断当前菜单类型是否支持分割线
                    if (mMenuType != MenuType.QQ)
                        throw new ArgumentException(
                            "CharmControlLibrary.CharmMenu：当前菜单类型不支持分割线.\n" +
                            "Name: " + Name);
                    formHeight += 3;
                }
                else
                {
                    // 文本项
                    // 判断是否为选择项
                    if (e.Y > formHeight && e.Y < (formHeight + 22))
                    {
                        if (MenuClick != null)
                            MenuClick(i);
                        Visible = false;
                        mSelectedIndex = -1;
                        return;
                    }
                    formHeight += 22;
                }
            }
        }
        #endregion

        #region  重载事件
        /// <summary>
        /// 引发 Paint 事件
        /// </summary>
        /// <param name="e">包含事件数据的 PaintEventArgs</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            // 获取绘制对象
            Graphics g = e.Graphics;

            // 过程中需要用到的变量
            MenuItem menuItem;
            int formHeight = 0;

            // 轮询每个菜单项，确定窗体需要的长度和宽度
            for (int i = 0; i < mItems.Count; i++)
            {
                menuItem = mItems[i];
                // 判断菜单项类型
                if (menuItem.MenuItemType == MenuItemType.Spliter)
                {
                    // 分割线
                    // 判断当前菜单类型是否支持分割线
                    if (mMenuType != MenuType.QQ)
                        throw new ArgumentException(
                            "CharmControlLibrary.CharmMenu：当前菜单类型不支持分割线.\n" +
                            "Name: " + Name);
                    // 绘制分割线
                    g.DrawImage(Resources.menu_spliter,
                        new Rectangle(new Point(32, formHeight + 1), new Size(Width - 39, 2)));
                    formHeight += 3;
                }
                else
                {
                    // 文本项
                    // 判断是否为选择项，是则需要绘制背景
                    if (mSelectedIndex == i)
                    {
                        Bitmap imgTemp = Resources.mun_select_bkg;
                        imgTemp = ImageOperation.ResizeImageWithoutBorder(
                            imgTemp, 3, 1, 3, 1, new Size(Width - 2, 21));
                        g.DrawImage(imgTemp, new Rectangle(3, formHeight + 2, Width - 5, 21));
                        // 绘制菜单项文本
                        g.DrawString(menuItem.Text, Font, Brushes.White, new Point(32, formHeight + 3));
                    }
                    else
                        g.DrawString(menuItem.Text, Font, Brushes.Black, new Point(32, formHeight + 3));

                    // 判断是否需要绘制图标
                    if (menuItem.Icon != null)
                        g.DrawImage(menuItem.Icon,
                            new Rectangle(new Point(14 - menuItem.Icon.Width / 2, formHeight + 12 - menuItem.Icon.Height / 2),
                            menuItem.Icon.Size));

                    formHeight += 22;
                }
            }
        }
        #endregion

        #region 自定义事件
        /// <summary>
        /// 当菜单项被单击时发生
        /// </summary>
        public event MenuEventHandler MenuClick;
        #endregion

        #region 方法
        /// <summary>
        /// 增加菜单项：只指定标题文本
        /// </summary>
        /// <param name="itemText">标题文本</param>
        public void AddItem(string itemText)
        {
            AddItem(itemText, MenuItemType.TextItem);
        }

        /// <summary>
        /// 增加菜单项：指定标题文本和菜单项类型
        /// </summary>
        /// <param name="itemText">标题文本</param>
        /// <param name="itemType">菜单项类型</param>
        public virtual void AddItem(string itemText, MenuItemType itemType)
        {
            AddItem(itemText, itemType, null);
        }

        /// <summary>
        /// 增加菜单项：指定标题文本、菜单项类型和菜单图标
        /// </summary>
        /// <param name="itemText">标题文本</param>
        /// <param name="itemType">菜单项类型</param>
        /// <param name="itemIcon">菜单图标</param>
        public virtual void AddItem(string itemText, MenuItemType itemType, Image itemIcon)
        {
            MenuItem menuItem = new MenuItem();
            menuItem.Text = itemText;
            menuItem.MenuItemType = itemType;
            menuItem.Icon = itemIcon;
            // 将菜单项加入到集合中
            mItems.Add(menuItem);
        }
        #endregion
    }
}
