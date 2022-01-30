using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

// Josh Davidson's Custom UI

namespace UI
{
    public class SplitButton : Button
    {
        ContextMenuStrip contextStrip;
        bool isHot;
        bool menuOpen;
        bool alwaysShowMenu = false;

        #region context menu
        [DefaultValue(false)]
        public bool AlwaysShowMenu
        {
            get
            {
                return alwaysShowMenu;
            }
            set
            {
                alwaysShowMenu = value;
            }
        }

        void contextStrip_Opening(object sender, CancelEventArgs e)
        {
            menuOpen = true;
        }

        void contextStrip_Closing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            menuOpen = false;
            setHot(false);
        }

        [DefaultValue(null)]
        public ContextMenuStrip DropDownContextMenu
        {
            get
            {
                return contextStrip;
            }
            set
            {
                if (contextStrip != null) // Remove from old menu
                {
                    contextStrip.Closing -= contextStrip_Closing;
                    contextStrip.Opening -= contextStrip_Opening;
                }

                if (value != null) // Add to new menu
                {
                    value.Closing += contextStrip_Closing;
                    value.Opening += contextStrip_Opening;
                }

                contextStrip = value;
            }
        }

        [Browsable(false)]
        public override ContextMenuStrip ContextMenuStrip
        {
            get
            {
                return DropDownContextMenu;
            }
            set
            {
                DropDownContextMenu = value;
            }
        }

        private void showMenu()
        {
            if (contextStrip != null)
            {
                contextStrip.Show(this, new Point(0, Height), ToolStripDropDownDirection.BelowRight);
            }
        }
        #endregion

        #region appearance
        public Image BtnImage
        {
            get
            {
                return Image;
            }
            set
            {
                Image = value;
                Invalidate();
            }
        }

        private void setHot(bool hot) // False sets not hot only if all focus is lost
        {
            if (menuOpen || Focused || hot) isHot = true;
            else isHot = false;

            if (isHot)
            {
                BtnImage = JRunner.Properties.Resources.arrow_dn_hot;
            }
            else
            {
                BtnImage = JRunner.Properties.Resources.arrow_dn;
            }
        }
        #endregion

        #region overrides
        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            setHot(true);
        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            setHot(false);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            setHot(true);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            setHot(false);
        }

        protected override void OnClick(EventArgs e)
        {
            var clickPos = PointToClient(new Point(MousePosition.X, MousePosition.Y));

            if (clickPos.X >= (Size.Width - Image.Width) || alwaysShowMenu) showMenu();
            else base.OnClick(e);
        }

        protected override bool IsInputKey(Keys key)
        {
            if (key == Keys.Down) return true;
            else return base.IsInputKey(key);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down && isHot)
            {
                showMenu();
            }
            
            base.OnKeyDown(e);
        }
        #endregion
    }
}
