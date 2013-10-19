#region 文档说明
/* ******************************************************************************************************
 * 文档作者：无闻
 * 创建日期：2012年10月28日
 * 文档用途：可伸缩二级分类列表自绘目录i列表
 * ------------------------------------------------------------------------------------------------------
 * 修改记录：
 * 2013-02-12（无闻）：
 *  - 改造成二级分类式可伸缩列表框
 *  - 正式植入 CharmControlLibrary，并更名为 CharmCatalogueList
 *  2013-03-18：针对CSBox界面标准2.0进行升级改造
 * ------------------------------------------------------------------------------------------------------
 * 参考文献：
 * 
 * ******************************************************************************************************/
#endregion

#region 命名空间引用
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using CharmControlLibrary.Properties;

#endregion

namespace CharmControlLibrary
{
    /// <summary>
    /// 表示 CharmControlLibrary.CharmCatalogueList 列表框
    /// </summary>
    public partial class CharmCatalogueList : ListBox
    {
        #region 字段
        // 鼠标现行选中项
        private CatalogListBoxItem mMouseItem;
        // 列表项集合
        private CatalogListBoxItemCollection mItems;
        // 隐藏列表项集合
        private List<CatalogListBoxItem> mHideItems;
        // 按钮高亮项索引（针对某一表项而言）
        private int buttonHighlightIndex;
        // 分类名称
        private string mClassName;
        #endregion

        #region 属性
        /// <summary>
        /// 获取或设置当前选中项分类名称
        /// </summary>
        public string ClassName
        {
            get { return mClassName; }
            set { mClassName = value; }
        }

        internal ObjectCollection OldItems
        {
            get { return base.Items; }
        }

        /// <summary>
        /// 获取 CharmCatalogueList 的项
        /// </summary>
        [Localizable(true)]
        [MergableProperty(false)]
        [DesignerSerializationVisibility(
            DesignerSerializationVisibility.Content)]
        public new CatalogListBoxItemCollection Items
        {
            get { return mItems; }
        }
        #endregion

        #region 重载事件
        /// <summary>
        /// 引发 Paint 事件
        /// </summary>
        /// <param name="e">包含事件数据的 PaintEventArgs</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            // 获取绘制对象
            Graphics g = e.Graphics;

            // 循环绘制每项
            for (int i = 0; i < Items.Count; i++)
            {
                // 获取表项工作区矩形
                Rectangle itemRect = GetItemRectangle(i);
                // 获取当前绘制的表项
                CatalogListBoxItem item = Items[i];
                // 绘制表项分割线
                g.DrawLine(Pens.LightGray, new Point(0, itemRect.Top), new Point(Width, itemRect.Top));
                // 绘制表项背景
                g.DrawImage(Resources.Catalog_normal, new Rectangle(new Point(0, itemRect.Y), new Size(189, 33)));
                // 第1项需要绘制特殊背景
                if (i == 0)
                    g.DrawImage(Resources.Catalog_First, new Rectangle(new Point(0, itemRect.Y), new Size(189, 33))); ;

                // 判断该表项是否为分类
                if (item.IsClass)
                {
                    // 绘制展开/收缩图标
                    if (item.IsExpand)
                        g.DrawImage(Resources.catalog_class_expand, new Rectangle(new Point(itemRect.Left + 8, itemRect.Y + 11), new Size(12, 12)));
                    else
                        g.DrawImage(Resources.catalog_class_collapse, new Rectangle(new Point(itemRect.Left + 8, itemRect.Y + 11), new Size(12, 12))); ;
                    // 绘制表项标题文本
                    g.DrawString(item.Text, new Font("微软雅黑", 10, FontStyle.Bold), Brushes.DarkBlue, itemRect.Left + 25, itemRect.Top + 7);
                }
                else
                {
                    // 判断是否为选中项
                    if (SelectedIndex == i)
                    {
                        // 绘制现行选中项
                        g.DrawImage(Resources.Catalog_pushed, new Rectangle(new Point(0, itemRect.Y), new Size(189, 33)));
                        if (item.StatusImages[2] != null)
                        {
                            g.DrawImage(item.StatusImages[2], new Rectangle(new Point(25, itemRect.Y + 7), item.StatusImages[0].Size));
                            g.DrawString(item.Text, new Font("微软雅黑", 9, FontStyle.Bold), Brushes.White, itemRect.Left + 55, itemRect.Top + 7);
                        }
                        else
                            g.DrawString(item.Text, new Font("微软雅黑", 9, FontStyle.Bold), Brushes.White, itemRect.Left + 50, itemRect.Top + 7);
                    }
                    else
                    {
                        // 非现行选中项
                        // 判断是否为高亮项，是则绘制高亮背景
                        if (mMouseItem == Items[i])
                        {
                            g.DrawImage(Resources.Catalog_hover, new Rectangle(new Point(0, itemRect.Y), new Size(189, 33)));
                            if (item.StatusImages[1] != null)
                            {
                                g.DrawImage(item.StatusImages[1], new Rectangle(new Point(25, itemRect.Y + 7), item.StatusImages[0].Size));
                                g.DrawString(item.Text, new Font("微软雅黑", 9), Brushes.Black, itemRect.Left + 55, itemRect.Top + 7);
                            }
                            else
                                g.DrawString(item.Text, new Font("微软雅黑", 9), Brushes.Black, itemRect.Left + 50, itemRect.Top + 7);
                        }
                        else if (item.StatusImages[0] != null)
                        {
                            g.DrawImage(item.StatusImages[0], new Rectangle(new Point(25, itemRect.Y + 7), item.StatusImages[0].Size));
                            g.DrawString(item.Text, new Font("微软雅黑", 9), Brushes.Black, itemRect.Left + 55, itemRect.Top + 7);
                        }
                        else
                            g.DrawString(item.Text, new Font("微软雅黑", 9), Brushes.Black, itemRect.Left + 50, itemRect.Top + 7);
                    }
                }

            }
            //base.OnPaint(e);
        }

        /// <summary>
        /// 测量表项事件
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMeasureItem(MeasureItemEventArgs e)
        {
            //base.OnMeasureItem(e);
            try
            {
                e.ItemHeight = 33;
            }
            catch { }
        }

        /// <summary>
        /// 引发 MouseMove 事件
        /// </summary>
        /// <param name="e">包含事件数据的 MouseEventArgs</param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            bool isChange = false;  // 是否改变标识符
            base.OnMouseMove(e);
            for (int i = 0; i < Items.Count; i++)
            {
                Rectangle bounds = GetItemRectangle(i);
                if (bounds.Contains(e.X, e.Y))
                {
                    if (e.X > 445 && e.X < 545 && e.Y > (27 + bounds.Y) && e.Y < (57 + bounds.Y))
                    {
                        if (buttonHighlightIndex != i)
                        {
                            buttonHighlightIndex = i;
                            isChange = true;
                        }
                    }
                    else if (e.X > 330 && e.X < 410 && e.Y > (45 + bounds.Y) && e.Y < (65 + bounds.Y))
                    {
                        if (Cursor != Cursors.Hand)
                            Cursor = Cursors.Hand;
                    }
                    else
                    {
                        if (Cursor != Cursors.Arrow)
                            Cursor = Cursors.Arrow;

                        if (buttonHighlightIndex == i)
                        {
                            buttonHighlightIndex = -1;
                            isChange = true;
                        }
                    }

                    if (Items[i] != mMouseItem)
                    {
                        mMouseItem = Items[i];
                        isChange = true;
                    }
                }

                if (isChange)
                    break;
            }

            if (isChange)
            {
                Invalidate();
            }
        }

        /// <summary>
        /// 选中项被改变事件
        /// </summary>
        /// <param name="e"></param>
        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            // 判断不为主分类标签才执行事件
            if (!Items[SelectedIndex].IsClass)
            {
                mMouseItem = Items[SelectedIndex];
                mClassName = mMouseItem.Text;
                base.OnSelectedIndexChanged(e);
            }
            else
            {
                // 分类表项被单击
                // 获取分类展开标识、分类ID
                bool isExpand = Items[SelectedIndex].IsExpand;
                int classID = Items[SelectedIndex].ClassID;
                // 判断当前展开状态
                if (isExpand)
                {
                    // 当前为展开状态，需要闭合
                    for (int i = Items.Count - 1; i >= 0; i--)
                    {
                        if (Items[i].ClassID == classID && !Items[i].IsClass)
                        {
                            mHideItems.Insert(0, Items[i]);
                            Items.RemoveAt(i);
                        }
                    }
                    // 设置展开状态为假
                    Items[SelectedIndex].IsExpand = false;
                }
                else
                {
                    // 当前为闭合状态，需要展开
                    for (int i = mHideItems.Count - 1; i >= 0; i--)
                    {
                        if ((mHideItems[i]).ClassID == classID)
                        {
                            Items.Insert(SelectedIndex + 1,mHideItems[i]);
                            mHideItems.RemoveAt(i);
                        }
                    }
                    // 设置展开状态为真
                    Items[SelectedIndex].IsExpand = true;
                }
            }
            // 重绘列表
            Invalidate();
        }
        #endregion

        #region 构造方法
        /// <summary>
        /// 初始化 CharmCatalogueList 类的新实例
        /// </summary>
        public CharmCatalogueList()
            : base()
        {
            mItems = new CatalogListBoxItemCollection(this);
            mHideItems = new List<CatalogListBoxItem>();
            base.DrawMode = DrawMode.OwnerDrawVariable;
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);// 双缓冲
            SetStyle(ControlStyles.ResizeRedraw, true);//调整大小时重绘
            SetStyle(ControlStyles.AllPaintingInWmPaint, true); // 禁止擦除背景.
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);// 双缓冲            
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);

            // 初始化属性
            buttonHighlightIndex = -1;
        }
        #endregion
    }

    /// <summary>
    /// 表示 CharmControlLibrary.CatalogListBoxItem 列表项
    /// </summary>
    [Serializable]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class CatalogListBoxItem : IDisposable
    {
        #region 字段
        // 表项标题文本
        private string mText;
        // 指示表项是否为分类     
        private bool mIsClass;
        // 所属分类ID（非分类表项专用）
        private int mClassID;
        // 指示分类下级是否展开（分类表项专用）
        private bool mIsExpand;
        // 表项状态图标（非分类表项专用）
        private Image[] mStatusImages;
        #endregion

        #region 属性
        /// <summary>
        /// 获取或设置表项标题文本
        /// </summary>
        public string Text
        {
            get { return mText; }
            set { mText = value; }
        }

        /// <summary>
        /// 获取或设置一个值，该值指示该项是否为分类
        /// </summary>
        public bool IsClass
        {
            get { return mIsClass; }
            set { mIsClass = value; }
        }

        /// <summary>
        /// 获取或设置所属分类ID（非分类表项专用）
        /// </summary>
        public int ClassID
        {
            get { return mClassID; }
            set { mClassID = value; }
        }

        /// <summary>
        /// 获取或设置一个值，该值指示该项下级表项是否展开（分类表项专用）
        /// </summary>
        public bool IsExpand
        {
            get { return mIsExpand; }
            set { mIsExpand = value; }
        }

        /// <summary>
        /// 获取或设置表项状态图标（非分类表项专用）
        /// </summary>
        public Image[] StatusImages
        {
            get { return mStatusImages; }
            set { mStatusImages = value; }
        }
        #endregion

        #region 构造方法
        /// <summary>
        /// 初始化 CatalogListBoxItem 类的新实例
        /// </summary>
        public CatalogListBoxItem()
        {

        }

        /// <summary>
        /// 初始化 CatalogListBoxItem 类的新实例
        /// </summary>
        /// <param name="text">表项标题文本</param>
        /// <param name="classID">所属分类ID</param>
        public CatalogListBoxItem(
            string text,
            int classID)
        {
            mText = text;
            mIsClass = false;
            mClassID = classID;
            IsExpand = false;
            mStatusImages = new Image[3];
        }

        /// <summary>
        /// 表示用于 CatalogueList 的表项
        /// </summary>
        /// <param name="text">表项标题文本</param>
        /// <param name="classID">所属分类ID</param>
        /// <param name="isClass">是否为分类</param>
        public CatalogListBoxItem(
            string text,
            int classID,
            bool isClass)
        {
            mText = text;
            mIsClass = isClass;
            mClassID = classID;
            IsExpand = false;
            mStatusImages = new Image[3];
        }
        #endregion

        #region 方法
        /// <summary>
        /// 释放由 Component 使用的所有资源
        /// </summary>
        public void Dispose()
        {
            mStatusImages = null;
        }
        #endregion
    }

    /// <summary>
    /// 表示 CharmControlLibrary.CatalogListBoxItemCollection 列表项集合
    /// </summary>
    [ListBindable(false)]
    public class CatalogListBoxItemCollection : IList, ICollection, IEnumerable
    {
        #region 字段
        private CharmCatalogueList _owner;
        #endregion

        #region 属性
        internal CharmCatalogueList Owner
        {
            get { return _owner; }
        }

        /// <summary>
        /// 获取或设置集合中指定索引处的项
        /// </summary>
        /// <param name="index">集合中要获取或设置项的索引</param>
        /// <returns></returns>
        public CatalogListBoxItem this[int index]
        {
            get { return Owner.OldItems[index] as CatalogListBoxItem; }
            set { Owner.OldItems[index] = value; }
        }

        /// <summary>
        /// 获取集合中项的数目
        /// </summary>
        public int Count
        {
            get { return Owner.OldItems.Count; }
        }

        /// <summary>
        /// 获取一个值，该值指示集合是否为只读
        /// </summary>
        public bool IsReadOnly
        {
            get { return Owner.OldItems.IsReadOnly; }
        }

        #endregion

        #region 方法
        /// <summary>
        /// 表示 CharmCatalogueList 的列表项集合
        /// </summary>
        /// <param name="owner"></param>
        public CatalogListBoxItemCollection(CharmCatalogueList owner)
        {
            _owner = owner;
        }

        /// <summary>
        /// 向 CharmCatalogueList 的项列表添加项
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int Add(CatalogListBoxItem item)
        {
            if (item == null)
            {
                throw new ArgumentException("item");
            }
            return Owner.OldItems.Add(item);
        }

        /// <summary>
        /// 确定指定的项是否位于集合内
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(CatalogListBoxItem item)
        {
            return Owner.OldItems.Contains(item);
        }

        /// <summary>
        /// 从集合中移除所有项
        /// </summary>
        public void Clear()
        {
            Owner.OldItems.Clear();
        }

        /// <summary>
        /// 返回指定项在集合中的索引
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int IndexOf(CatalogListBoxItem item)
        {
            return Owner.OldItems.IndexOf(item);
        }

        /// <summary>
        /// 将整个集合复制到现有的数组集合中，从该数组的指定位置开始复制
        /// </summary>
        /// <param name="destination"></param>
        /// <param name="arrayIndex"></param>
        public void CopyTo(CatalogListBoxItem[] destination, int arrayIndex)
        {
            Owner.OldItems.CopyTo(destination, arrayIndex);
        }

        /// <summary>
        /// 将项插入 CharmCatalogueList 的指定索引处
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
        public void Insert(int index, CatalogListBoxItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            Owner.OldItems.Insert(index, item);
        }

        /// <summary>
        /// 从集合中移除指定的对象
        /// </summary>
        /// <param name="item"></param>
        public void Remove(CatalogListBoxItem item)
        {
            Owner.OldItems.Remove(item);
        }

        /// <summary>
        /// 移除集合中指定索引处的项
        /// </summary>
        /// <param name="index"></param>
        public void RemoveAt(int index)
        {
            Owner.OldItems.RemoveAt(index);
        }

        #endregion

        #region 成员接口
        /// <summary>
        /// 返回一个枚举数，将使用该枚举数循环访问集合
        /// </summary>
        /// <returns></returns>
        public IEnumerator GetEnumerator()
        {
            return Owner.OldItems.GetEnumerator();
        }

        int IList.Add(object value)
        {
            if (!(value is CatalogListBoxItem))
            {
                throw new ArgumentException();
            }
            return Add(value as CatalogListBoxItem);
        }

        bool IList.Contains(object value)
        {
            return Contains(value as CatalogListBoxItem);
        }

        void IList.Clear()
        {
            Clear();
        }

        bool ICollection.IsSynchronized
        {
            get { return false; }
        }

        object ICollection.SyncRoot
        {
            get { return this; }
        }

        int IList.IndexOf(object value)
        {
            return IndexOf(value as CatalogListBoxItem);
        }

        void ICollection.CopyTo(Array array, int index)
        {
            CopyTo((CatalogListBoxItem[])array, index);
        }

        int ICollection.Count
        {
            get { return Count; }
        }

        bool IList.IsFixedSize
        {
            get { return false; }
        }

        bool IList.IsReadOnly
        {
            get { return IsReadOnly; }
        }

        void IList.Remove(object value)
        {
            Remove(value as CatalogListBoxItem);
        }

        void IList.RemoveAt(int index)
        {
            RemoveAt(index);
        }

        object IList.this[int index]
        {
            get
            {
                return this[index];
            }
            set
            {
                if (!(value is CatalogListBoxItem))
                {
                    throw new ArgumentException();
                }
                this[index] = value as CatalogListBoxItem;
            }
        }

        void IList.Insert(int index, object value)
        {
            if (!(value is CatalogListBoxItem))
            {
                throw new ArgumentException();
            }
            Insert(index, value as CatalogListBoxItem);
        }

        #endregion
    }
}
