#region 文档说明
/* ******************************************************************************************************
 * 文档作者：无闻
 * 创建日期：2012年10月20日
 * 文档用途：CSharp宝盒学习计划列表控件
 * ------------------------------------------------------------------------------------------------------
 * 修改记录：
 * 2013-03-13：针对CSBox界面标准2.0进行升级改造
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
using System.ComponentModel;

using CharmCommonMethod;
using CharmControlLibrary.Properties;

#endregion

namespace CharmControlLibrary
{
    /// <summary>
    /// 表示 CharmControlLibrary.CharmStudyPlanList 列表框
    /// </summary>
    public partial class CharmStudyPlanList : ListBox
    {
        #region 字段
        // 鼠标现行选中项
        private StudyPlanListBoxItem mMouseItem;
        // 列表项集合
        private StudyPlanListBoxItemCollection mItems;
        // 按钮高亮项索引（0取消订阅链接标签-1开始学习按钮）
        private int buttonHighlightIndex;
        // 按钮按下项索引
        private int buttonDownIndex;
        #endregion

        #region 属性
        /// <summary>
        /// 获取或设置鼠标现行选中项
        /// </summary>
        public StudyPlanListBoxItem MouseItem
        {
            get { return mMouseItem; }
            set { mMouseItem = value; }
        }

        /// <summary>
        /// 获取列表项集合
        /// </summary>
        [Localizable(true)]
        [MergableProperty(false)]
        [DesignerSerializationVisibility(
            DesignerSerializationVisibility.Content)]
        public new StudyPlanListBoxItemCollection Items
        {
            get { return mItems; }
        }

        /// <summary>
        /// 获取或设置按钮高亮项索引
        /// </summary>
        public int ButtonHighlightInedx
        {
            get { return buttonHighlightIndex; }
            set { buttonHighlightIndex = value; }
        }

        /// <summary>
        /// 获取或设置按钮按下项索引
        /// </summary>
        public int ButtonDownIndex
        {
            get { return buttonDownIndex; }
            set { buttonDownIndex = value; }
        }

        internal ObjectCollection OldItems
        {
            get { return base.Items; }
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
                Rectangle bounds = GetItemRectangle(i);
                StudyPlanListBoxItem item = Items[i];
                // 绘制表项分界线
                Pen p = new Pen(Brushes.LightGray);
                //p.DashStyle = DashStyle.Dash;
                g.DrawLine(p, new Point(0, bounds.Top), new Point(Width, bounds.Top));
                // 绘制表项背景
                if (SelectedIndex != i)
                {
                    Color backColor = Color.FromArgb(20, 216, 211, 211);
                    using (SolidBrush brush = new SolidBrush(backColor))
                    {
                        //g.FillRectangle(new SolidBrush(Color.White),bounds);
                        g.FillRectangle(brush, new Rectangle(bounds.X, bounds.Y + 1, bounds.Width, bounds.Height - 1));
                    }
                }
                else
                    g.FillRectangle(new SolidBrush(Color.FromArgb(250, 255, 252, 217)),
                        new Rectangle(bounds.X, bounds.Y + 1, bounds.Width, bounds.Height - 1));

                // 高亮非选中项
                if (mMouseItem == Items[i] && SelectedIndex != i)
                {
                    Color backColor = Color.FromArgb(200, 192, 224, 248);
                    using (SolidBrush brush = new SolidBrush(backColor))
                    {
                        //g.FillRectangle(new SolidBrush(Color.White),bounds);
                        g.FillRectangle(brush, new Rectangle(bounds.X, bounds.Y + 1, bounds.Width, bounds.Height - 1));
                    }

                }

                // 绘制表项内容
                // 行1
                g.DrawString("教程名称：", new Font("微软雅黑", 9), Brushes.Black, bounds.Left + 5, bounds.Top + 3);
                g.DrawString("教程类别：", new Font("微软雅黑", 9), Brushes.Black, bounds.Left + 250, bounds.Top + 3);
                g.DrawString("教程级别：", new Font("微软雅黑", 9), Brushes.Black, bounds.Left + 400, bounds.Top + 3);

                g.DrawString(item.CourseName, new Font("微软雅黑", 9, FontStyle.Bold), Brushes.Indigo, bounds.Left + 65, bounds.Top + 3);

                // 判断教程标识，转换不同类别
                string configPath = Application.StartupPath + "\\Config\\Config.ini";
                string planType = IniOperation.ReadValue(configPath, "coursetype", item.CourseType);
                g.DrawString(planType, new Font("微软雅黑", 9, FontStyle.Bold), Brushes.Black, bounds.Left + 310, bounds.Top + 3);

                // 判断教程级别，指定不同颜色
                Brush levelBrush = Brushes.Black;
                string planLevel = string.Empty;
                switch (item.CourseLevel)
                {
                    case "1":
                        planLevel = "基础";
                        levelBrush = Brushes.Black;
                        break;
                    case "2":
                        planLevel = "中级";
                        levelBrush = Brushes.BlueViolet;
                        break;
                    case "3":
                        planLevel = "高级";
                        levelBrush = Brushes.MidnightBlue;
                        break;
                    case "4":
                        planLevel = "大师";
                        levelBrush = Brushes.Red;
                        break;
                    case "5":
                        planLevel = "骨灰";
                        levelBrush = Brushes.Purple;
                        break;
                }
                g.DrawString(planLevel, new Font("微软雅黑", 9, FontStyle.Bold), levelBrush, bounds.Left + 460, bounds.Top + 3);
                // 行2
                g.DrawString("起始日期：", new Font("微软雅黑", 9), Brushes.Black, bounds.Left + 5, bounds.Top + 24);
                g.DrawString("最近学习时间：", new Font("微软雅黑", 9), Brushes.Black, bounds.Left + 180, bounds.Top + 24);

                g.DrawString(item.StartDate, new Font("微软雅黑", 9, FontStyle.Bold), Brushes.Black, bounds.Left + 65, bounds.Top + 24);
                g.DrawString(item.LastestStudy, new Font("微软雅黑", 9, FontStyle.Bold), Brushes.Black, bounds.Left + 265, bounds.Top + 24);

                // 行3
                //计算学习进度百分比
                string[] info = item.RelatedResIDs.Split('-');
                double progress = item.StudyProgress / Convert.ToDouble(info[0]);
                if (progress == 0)
                    progress = 0.01;
                g.DrawString("教程学习进度：", new Font("微软雅黑", 9), Brushes.Black, bounds.Left + 5, bounds.Top + 45);
                g.DrawImage(new Bitmap(Resources.timeprogressbg, new Size(200, 18)), 
                    new Point(bounds.Left + 95, bounds.Top + 44));
                g.DrawImage(new Bitmap(Resources.time_progress_fg_lightGreen, new Size((int)(200 * progress), 18)), 
                    new Point(bounds.Left + 95, bounds.Top + 44));
                g.DrawString("取消订阅教程", new Font("微软雅黑", 9), Brushes.Blue, bounds.Left + 330, bounds.Top + 45);

                // 判断按钮状态
                // 按下态
                if (buttonDownIndex == i)
                {
                    g.DrawImage(Resources.btn_big_down,
                        new Rectangle(new Point(bounds.Left + 445, bounds.Top + 27), new Size(100, 30)));
                    g.DrawString("开始学习", new Font("微软雅黑", 10), Brushes.DarkGreen, bounds.Left + 467, bounds.Top + 32);
                }
                else
                {
                    // 高亮态
                    if (buttonHighlightIndex == i)
                        g.DrawImage(Resources.btn_big_hover,
                            new Rectangle(new Point(bounds.Left + 445, bounds.Top + 27), new Size(100, 30)));
                    else
                        g.DrawImage(Resources.btn_big_normal,
                            new Rectangle(new Point(bounds.Left + 445, bounds.Top + 27), new Size(100, 30)));
                    g.DrawString("开始学习", new Font("微软雅黑", 10), Brushes.DarkGreen, bounds.Left + 466, bounds.Top + 31);
                }

                g.DrawLine(p, new Point(0, bounds.Top + 65), new Point(Width, bounds.Top + 65));
                p.Dispose();
            }
            //base.OnPaint(e);
        }

        /// <summary>
        /// 测量表项事件
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMeasureItem(MeasureItemEventArgs e)
        {
            base.OnMeasureItem(e);
            try
            {
                e.ItemHeight = 65;
            }
            catch { }
        }

        /// <summary>
        /// 引发 MouseDown 事件
        /// </summary>
        /// <param name="e">包含事件数据的 MouseEventArgs</param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            // 只响应鼠标左键
            if (e.Button == MouseButtons.Left)
            {
                bool isChange = false;  // 指示是否改变
                for (int i = 0; i < Items.Count; i++)
                {
                    Rectangle bounds = GetItemRectangle(i);
                    if (bounds.Contains(e.X, e.Y))
                    {
                        if (e.X > 445 && e.X < 545 && e.Y > (27 + bounds.Y) && e.Y < (57 + bounds.Y))
                        {
                            if (buttonDownIndex != i)
                            {
                                buttonDownIndex = i;
                                isChange = true;
                            }
                        }
                        else if (buttonDownIndex == i)
                        {
                            buttonDownIndex = -1;
                            isChange = true;
                        }
                    }

                    if (isChange)
                        break;
                }

                if (isChange)
                    Invalidate();
            }
            base.OnMouseDown(e);
        }

        /// <summary>
        /// 引发 MouseMove 事件
        /// </summary>
        /// <param name="e">包含事件数据的 MouseEventArgs</param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            bool isChange = false;  // 指示是否改变
            Rectangle bounds;
            // 轮询表项
            for (int i = 0; i < Items.Count; i++)
            {
                bounds = GetItemRectangle(i);
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
                    else if (e.X > 330 && e.X < 410 && e.Y > (45 + bounds.Y) && e.Y < (64 + bounds.Y))
                    {
                        buttonDownIndex = -1;
                        if (Cursor != Cursors.Hand)
                        {
                            Cursor = Cursors.Hand;
                            if (Items[i] != mMouseItem)
                                mMouseItem = Items[i];
                            break;
                        }
                    }
                    else
                    {
                        buttonDownIndex = -1;
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

            // 判断鼠标指针是否悬浮在空白区域
            if (Items.Count > 0)
            {
                bounds = GetItemRectangle(Items.Count - 1);
                if (e.Y > (65 + bounds.Y))
                {
                    if (Cursor != Cursors.Arrow)
                        Cursor = Cursors.Arrow;
                }

                if (isChange)
                    Invalidate();
            }
            base.OnMouseMove(e);
        }

        /// <summary>
        /// 引发 MouseUp 事件
        /// </summary>
        /// <param name="e">包含事件数据的 MouseEventArgs</param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            // 只响应鼠标左键
            if (e.Button == MouseButtons.Left)
            {
                bool isChange = false;  // 指示是否改变
                for (int i = 0; i < Items.Count; i++)
                {
                    Rectangle bounds = GetItemRectangle(i);
                    if (bounds.Contains(e.X, e.Y))
                    {
                        if (e.X > 330 && e.X < 410 && e.Y > (45 + bounds.Y) && e.Y < (65 + bounds.Y))
                        {
                            Items[i].ClickAreaIndex = 0;
                            isChange = true;
                        }
                        else if (e.X > 445 && e.X < 545 && e.Y > (27 + bounds.Y) && e.Y < (57 + bounds.Y))
                        {
                            Items[i].ClickAreaIndex = 1;
                            isChange = true;
                        }
                        else
                            Items[i].ClickAreaIndex = -1;
                    }
                    else
                        Items[i].ClickAreaIndex = -1;

                    if (isChange)
                        break;
                }

                if (isChange)
                    Invalidate();
            }
            base.OnMouseUp(e);
        }

        /// <summary>
        /// 选中项被改变事件
        /// </summary>
        /// <param name="e"></param>
        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            mMouseItem = Items[SelectedIndex];
            Invalidate();
            base.OnSelectedIndexChanged(e);
        }
        #endregion

        #region 构造方法
        /// <summary>
        /// 初始化 CharmStudyPlanList 类的新实例
        /// </summary>
        public CharmStudyPlanList()
            : base()
        {
            mItems = new StudyPlanListBoxItemCollection(this);
            base.DrawMode = DrawMode.OwnerDrawVariable;
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);// 双缓冲
            SetStyle(ControlStyles.ResizeRedraw, true);//调整大小时重绘
            SetStyle(ControlStyles.AllPaintingInWmPaint, true); // 禁止擦除背景.
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);// 双缓冲            
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);

            // 初始化属性
            buttonHighlightIndex = -1;
            buttonDownIndex = -1;
        }
        #endregion
    }

    /// <summary>
    /// 表示 CharmControlLibrary.StudyPlanListBoxItem 列表项
    /// </summary>
    [Serializable]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class StudyPlanListBoxItem : IDisposable
    {
        #region 字段
        // 学习计划ID
        private string mStudyPlanID;
        // 教程名称
        private string mCourseName;
        // 教程类型
        private string mCourseType;
        // 教程难度级别
        private string mCourseLevel;
        // 开始学习日期
        private string mStartDate;
        // 学习进度
        private int mStudyProgress;
        // 结束学习日期
        private string mEndDate;
        // 最后学习日期
        private string mLatestStudy;
        // 相关资源ID集合
        private string mRelatedResIDs;
        // 单击区域索引（0取消订阅链接标签-1开始学习按钮）
        private int mClickAreaIndex;
        #endregion

        #region 属性
        /// <summary>
        /// 获取或设置学习计划ID
        /// </summary>
        public string StudyPlanID
        {
            get { return mStudyPlanID; }
            set { mStudyPlanID = value; }
        }

        /// <summary>
        /// 获取或设置教程名称
        /// </summary>
        public string CourseName
        {
            get { return mCourseName; }
            set { mCourseName = value; }
        }

        /// <summary>
        /// 获取或设置程类型
        /// </summary>
        public string CourseType
        {
            get { return mCourseType; }
            set { mCourseType = value; }
        }

        /// <summary>
        /// 获取或设置教程难度级别
        /// </summary>
        public string CourseLevel
        {
            get { return mCourseLevel; }
            set { mCourseLevel = value; }
        }

        /// <summary>
        /// 获取或设置开始学习日期
        /// </summary>
        public string StartDate
        {
            get { return mStartDate; }
            set { mStartDate = value; }
        }

        /// <summary>
        /// 获取或设置学习进度
        /// </summary>
        public int StudyProgress
        {
            get { return mStudyProgress; }
            set { mStudyProgress = value; }
        }

        /// <summary>
        /// 获取或设置结束学习日期
        /// </summary>
        public string EndDate
        {
            get { return mEndDate; }
            set { mEndDate = value; }
        }

        /// <summary>
        /// 获取或设置最后学习日期
        /// </summary>
        public string LastestStudy
        {
            get { return mLatestStudy; }
            set { mLatestStudy = value; }
        }

        /// <summary>
        /// 获取或设置相关资源ID集合
        /// </summary>
        public string RelatedResIDs
        {
            get { return mRelatedResIDs; }
            set { mRelatedResIDs = value; }
        }

        /// <summary>
        /// 获取或设置单击区域索引
        /// </summary>
        public int ClickAreaIndex
        {
            get { return mClickAreaIndex; }
            set { mClickAreaIndex = value; }
        }
        #endregion

        #region 构造方法
        /// <summary>
        /// 初始化 StudyPlanListBoxItem 类的新实例
        /// </summary>
        public StudyPlanListBoxItem()
        {

        }

        /// <summary>
        /// 初始化 StudyPlanListBoxItem 类的新实例
        /// </summary>
        /// <param name="studyPlanID">学习计划ID</param>
        /// <param name="courseName">教程名称</param>
        /// <param name="courseType">教程类型</param>
        /// <param name="courseLevel">教程难度级别</param>
        /// <param name="startDate">开始学习日期</param>
        /// <param name="studyProgress">学习进度</param>
        /// <param name="endDate">结束学习日期</param>
        /// <param name="latestStudy">最后学习日期</param>
        /// <param name="relatedResIDs">相关资源ID集合</param>
        public StudyPlanListBoxItem(
            string studyPlanID,
            string courseName,
            string courseType,
            string courseLevel,
            string startDate,
            int studyProgress,
            string endDate,
            string latestStudy,
            string relatedResIDs)
        {
            mStudyPlanID = studyPlanID;
            mCourseName = courseName;
            mCourseType = courseType;
            mCourseLevel = courseLevel;
            mStartDate = startDate;
            mStudyProgress = studyProgress;
            mEndDate = endDate;
            mLatestStudy = latestStudy;
            mRelatedResIDs = relatedResIDs;
            mClickAreaIndex = -1;
        }
        #endregion

        #region 方法
        /// <summary>
        /// 释放由 Component 使用的所有资源
        /// </summary>
        public void Dispose()
        {

        }
        #endregion
    }

    /// <summary>
    /// 表示 CharmControlLibrary.StudyPlanListBoxItemCollection 列表项集合
    /// </summary>
    [ListBindable(false)]
    public class StudyPlanListBoxItemCollection : IList, ICollection, IEnumerable
    {
        #region 字段
        private CharmStudyPlanList _owner;
        #endregion

        #region 属性
        internal CharmStudyPlanList Owner
        {
            get { return _owner; }
        }

        public StudyPlanListBoxItem this[int index]
        {
            get { return Owner.OldItems[index] as StudyPlanListBoxItem; }
            set { Owner.OldItems[index] = value; }
        }

        public int Count
        {
            get { return Owner.OldItems.Count; }
        }

        public bool IsReadOnly
        {
            get { return Owner.OldItems.IsReadOnly; }
        }

        #endregion

        #region 方法
        public StudyPlanListBoxItemCollection(CharmStudyPlanList owner)
        {
            _owner = owner;
        }

        public int Add(StudyPlanListBoxItem item)
        {
            if (item == null)
            {
                throw new ArgumentException("item");
            }
            return Owner.OldItems.Add(item);
        }

        public bool Contains(StudyPlanListBoxItem item)
        {
            return Owner.OldItems.Contains(item);
        }

        public void Clear()
        {
            Owner.OldItems.Clear();
        }

        public int IndexOf(StudyPlanListBoxItem item)
        {
            return Owner.OldItems.IndexOf(item);
        }

        public void CopyTo(StudyPlanListBoxItem[] destination, int arrayIndex)
        {
            Owner.OldItems.CopyTo(destination, arrayIndex);
        }

        public void Insert(int index, StudyPlanListBoxItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            Owner.OldItems.Insert(index, item);
        }

        public void Remove(StudyPlanListBoxItem item)
        {
            Owner.OldItems.Remove(item);
        }

        public void RemoveAt(int index)
        {
            Owner.OldItems.RemoveAt(index);
        }

        #endregion

        #region 成员接口
        public IEnumerator GetEnumerator()
        {
            return Owner.OldItems.GetEnumerator();
        }

        int IList.Add(object value)
        {
            if (!(value is StudyPlanListBoxItem))
            {
                throw new ArgumentException();
            }
            return Add(value as StudyPlanListBoxItem);
        }

        bool IList.Contains(object value)
        {
            return Contains(value as StudyPlanListBoxItem);
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
            return IndexOf(value as StudyPlanListBoxItem);
        }

        void ICollection.CopyTo(Array array, int index)
        {
            CopyTo((StudyPlanListBoxItem[])array, index);
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
            Remove(value as StudyPlanListBoxItem);
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
                if (!(value is StudyPlanListBoxItem))
                {
                    throw new ArgumentException();
                }
                this[index] = value as StudyPlanListBoxItem;
            }
        }

        void IList.Insert(int index, object value)
        {
            if (!(value is StudyPlanListBoxItem))
            {
                throw new ArgumentException();
            }
            Insert(index, value as StudyPlanListBoxItem);
        }

        #endregion
    }
}
