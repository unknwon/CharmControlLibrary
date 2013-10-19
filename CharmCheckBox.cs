#region 文档说明
/* ****************************************************************************************************
 * 文档作者：无闻
 * 创建日期：2013年2月23日
 * 文档用途：CharmCheckBox - 检查框控件
 * -----------------------------------------------------------------------------------------------------
 * 修改记录：
 * 
 * -----------------------------------------------------------------------------------------------------
 * 参考文献：
 * 
 * *****************************************************************************************************/
#endregion

#region 命名空间引用

using System.Drawing;
using System.Windows.Forms;
using CharmControlLibrary.Properties;

#endregion

namespace CharmControlLibrary
{
    #region 枚举类型
    /// <summary>
    /// 检查框类型：QQ2012
    /// </summary>
    public enum CheckBoxType
    {
        /// <summary>
        /// 检查框类型：QQ2012
        /// </summary>
        QQ2012
    }
    #endregion

    /// <summary>
    /// 表示 CharmControlLibrary.CharmCheckBox 检查框控件
    /// </summary>
    public class CharmCheckBox : CharmControl
    {
        #region 字段
        // 控件的状态图片
        private Image[] mStatusImages;
        #endregion

        #region 属性
        /// <summary>
        /// 获取或设置 CharmControlLibrary.CharmCheckBox 的检查框类型
        /// </summary>
        public override CheckBoxType CheckBoxType
        {
            get { return base.CheckBoxType; }
            set
            {
                // 获取用户设置的检查框类型
                base.CheckBoxType = value;
                // 根据检查框类型决定四态图片
                switch (CheckBoxType)
                {
                    case CheckBoxType.QQ2012:
                        mStatusImages[0] = Resources.checkbox_normal;
                        mStatusImages[1] = Resources.checkbox_hightlight;
                        mStatusImages[2] = Resources.checkbox_tick_normal;
                        mStatusImages[3] = Resources.checkbox_tick_highlight;
                        break;
                }
                // 重新设置控件状态以便绘制背景图像
                ControlStatus = ControlStatus.Normal;
            }
        }

        /// <summary>
        /// 获取或设置控件的状态
        /// </summary>
        public override ControlStatus ControlStatus
        {
            get { return base.ControlStatus; }
            set
            {
                base.ControlStatus = value;
                // 判断检查框是否被选中，并根据控件状态设定背景图像
                if (Checked)
                {
                    if (ControlStatus != ControlStatus.Normal)    // 选中，高亮
                        BackgroundImage = mStatusImages[3];
                    else    // 选中，未高亮
                        BackgroundImage = mStatusImages[2];
                }
                else
                {
                    if (ControlStatus != ControlStatus.Normal)    // 未选中，高亮
                        BackgroundImage = mStatusImages[1];
                    else    // 未选中，未高亮
                        BackgroundImage = mStatusImages[0];
                }
            }
        }

        /// <summary>
        /// 获取或设置控件的状态图片
        /// </summary>
        public Image[] StatusImages
        {
            get { return mStatusImages; }
            set { mStatusImages = value; }
        }

        /// <summary>
        /// 获取或设置与此控件关联的文本
        /// </summary>
        public override string Text
        {
            get { return base.Text; }
            set
            {
                base.Text = value;

                // 根据文本长度重新设置控件尺寸
                using (Graphics g = Graphics.FromImage(mStatusImages[0]))
                {
                    SizeF sf = g.MeasureString(base.Text, base.Font);
                    Size = new Size(22 + (int)sf.Width, 4 + (int)sf.Height);
                }
            }
        }
        #endregion

        #region 重载事件
        /// <summary>
        /// 引发 MouseClick 事件
        /// </summary>
        /// <param name="e">包含事件数据的 MouseEventArgs</param>
        protected override void OnMouseClick(MouseEventArgs e)
        {
            // 修改检查框选中值
            Checked = !Checked;
            // 修改状态是为了让主窗体能重绘该控件
            ControlStatus = ControlStatus.Normal;
            // 触发基类事件
            base.OnMouseClick(e);
        }
        #endregion

        #region 构造方法
        /// <summary>
        /// 初始化 CharmCheckBox 类的新实例
        /// </summary>
        public CharmCheckBox()
            : base()
        {
            // * 初始化属性 *
            ControlType = ControlType.CharmCheckBox;
            mStatusImages = new Image[4];
            CheckBoxType = CheckBoxType.QQ2012;
            Size = new Size(21, 21);
            TextPosition = new Point(22, 4);
        }
        #endregion

        #region 方法
        /// <summary>
        /// 释放由 Component 使用的所有资源
        /// </summary>
        public new void Dispose()
        {
            // * 释放系统资源 *
            mStatusImages = null;

            // 调用基类方法
            base.Dispose();
        }
        #endregion
    }
}
