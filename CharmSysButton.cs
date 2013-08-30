#region 文档说明
/* *****************************************************************************************************
 * 文档作者：无闻
 * 创建日期：2013年2月19日
 * 文档用途：CharmSysButton - 系统按钮控件
 * -----------------------------------------------------------------------------------------------------
 * 修改记录：
 * 2013-02-19（无闻）：针对CSBox界面标准2.0进行升级改造
 * 2013-04-08（无闻）：增加 多参构造方法
 * -----------------------------------------------------------------------------------------------------
 * 参考文献：
 * 
 * *****************************************************************************************************/
#endregion

#region 命名空间引用
using System;
using System.Drawing;
#endregion

namespace CharmControlLibrary
{
    #region 枚举类型
    /// <summary>
    /// 系统按钮类型：皮肤、反馈、主菜单、最小化、最大化、关闭
    /// </summary>
    public enum SysButtonType : int
    {
        /// <summary>
        /// 未指定系统按钮类型
        /// </summary>
        Undefined,
        /// <summary>
        /// 系统按钮类型：皮肤
        /// </summary>
        Skin,
        /// <summary>
        /// 系统按钮类型：反馈
        /// </summary>
        Feedback,
        /// <summary>
        /// 系统按钮类型：主菜单
        /// </summary>
        MainMenu,
        /// <summary>
        /// 系统按钮类型：最小化
        /// </summary>
        Minimum,
        /// <summary>
        /// 系统按钮类型：最大化
        /// </summary>
        Maximum,
        /// <summary>
        /// 系统按钮类型：关闭
        /// </summary>
        Close,
        /// <summary>
        /// 系统按钮类型：关闭 - 金山软件v4
        /// </summary>
        Close_ksv4
    }
    #endregion

    /// <summary>
    /// 表示 CharmControlLibrary.CharmSysButton 系统按钮控件
    /// </summary>
    public class CharmSysButton : CharmControl
    {
        #region 字段
        // 控件的状态图片
        private Image[] mStatusImages;
        #endregion

        #region 属性
        /// <summary>
        /// 获取或设置 CharmControlLibrary.CharmSysButton 的系统按钮类型
        /// </summary>
        public override SysButtonType SysButtonType
        {
            get { return base.SysButtonType; }
            set
            {
                // 获取用户设置的系统按钮类型
                base.SysButtonType = value;
                // 根据系统按钮类型决定三态图片及控件大小
                switch (this.SysButtonType)
                {
                    case SysButtonType.Skin:
                        this.Size = new Size(31, 23);
                        mStatusImages[0] = Properties.Resources.sysbtn_skin_normal;
                        mStatusImages[1] = Properties.Resources.sysbtn_skin_hover;
                        mStatusImages[2] = Properties.Resources.sysbtn_skin_down;
                        break;
                    case SysButtonType.Feedback:
                        this.Size = new Size(31, 23);
                        mStatusImages[0] = Properties.Resources.sysbtn_feedback_normal;
                        mStatusImages[1] = Properties.Resources.sysbtn_feedback_hover;
                        mStatusImages[2] = Properties.Resources.sysbtn_feedback_down;
                        break;
                    case SysButtonType.MainMenu:
                        this.Size = new Size(31, 23);
                        mStatusImages[0] = Properties.Resources.sysbtn_menu_normal;
                        mStatusImages[1] = Properties.Resources.sysbtn_menu_hover;
                        mStatusImages[2] = Properties.Resources.sysbtn_menu_down;
                        break;
                    case SysButtonType.Minimum:
                        this.Size = new Size(31, 23);
                        mStatusImages[0] = Properties.Resources.sysbtn_min_normal;
                        mStatusImages[1] = Properties.Resources.sysbtn_min_hover;
                        mStatusImages[2] = Properties.Resources.sysbtn_min_down;
                        break;
                    case SysButtonType.Maximum:
                        throw new ArgumentException("CharmControlLibrary.CharmSysButton.SysButtonType：暂不支持最大化类型的系统按钮.");
                    //break;
                    case SysButtonType.Close:
                        this.Size = new Size(45, 23);
                        mStatusImages[0] = Properties.Resources.sysbtn_close_normal;
                        mStatusImages[1] = Properties.Resources.sysbtn_close_hover;
                        mStatusImages[2] = Properties.Resources.sysbtn_close_down;
                        break;
                    case SysButtonType.Close_ksv4:
                        this.Size = new Size(43, 21);
                        mStatusImages[0] = Properties.Resources.sysbtn_ksv4_close_normal;
                        mStatusImages[1] = Properties.Resources.sysbtn_ksv4_close_over;
                        mStatusImages[2] = Properties.Resources.sysbtn_ksv4_close_down;
                        break;
                }
                // 重新设置控件状态以便绘制背景图像
                this.ControlStatus = ControlStatus.Normal;
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
                // 根据控件状态设定背景图像
                this.BackgroundImage = mStatusImages[(int)this.ControlStatus];
            }
        }
        #endregion

        #region 构造方法
        /// <summary>
        /// 初始化 CharmSysButton 类的新实例
        /// </summary>
        public CharmSysButton()
            : base()
        {
            // * 初始化属性 *
            this.ControlType = ControlType.CharmSysButton;
            this.mStatusImages = new Image[3];
        }

        /// <summary>
        /// 初始化 CharmSysButton 类的新实例
        /// </summary>
        /// <param name="sysButtonType">系统按钮类型</param>
        /// <param name="location">该控件的左上角相对于其容器的左上角的坐标</param>
        public CharmSysButton(
            SysButtonType sysButtonType, 
            Point location)
            : this()
        {
            // * 初始化属性 *
            this.SysButtonType = sysButtonType;
            this.Location = location;
        }

        /// <summary>
        /// 初始化 CharmSysButton 类的新实例
        /// </summary>
        /// <param name="sysButtonType">系统按钮类型</param>
        /// <param name="location">该控件的左上角相对于其容器的左上角的坐标</param>
        /// <param name="toolTipText">控件的工具提示文本</param>
        public CharmSysButton(
            SysButtonType sysButtonType,
            Point location,
            string toolTipText)
            : this(sysButtonType, location)
        {
            // * 初始化属性 *
            this.ToolTipText = toolTipText;
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
