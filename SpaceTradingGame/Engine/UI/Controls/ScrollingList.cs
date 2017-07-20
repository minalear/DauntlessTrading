using System;
using System.Drawing;
using System.Collections.Generic;
using OpenTK.Input;
using OpenTK.Graphics;

namespace SpaceTradingGame.Engine.UI.Controls
{
    public class ScrollingList : Control
    {
        public ScrollingList(Control parent, int x, int y, int width, int height)
            : base(parent)
        {
            this.position = new Point(x, y);
            this.size = new Point(width, height);

            this.objectList = new List<ListItem>();
        }

        public override void DrawStep()
        {
            GraphicConsole.SetColor(Color.Transparent, this.fillColor);
            GraphicConsole.Draw.Rect(this.Position.X, this.Position.Y, this.Size.X, this.Size.Y, ' ', true);

            if (!scroll)
            {
                for (int i = 0; i < this.objectList.Count; i++)
                {
                    this.setConsoleColors(i);
                    GraphicConsole.SetCursor(this.Position.X, this.Position.Y + i);

                    this.writeLine(this.objectList[i].ListText);
                }
            }
            else
            {
                //Scroll Bar Rail
                GraphicConsole.SetColor(this.scrollRailColor, this.fillColor);
                GraphicConsole.Draw.Line(this.Position.X + this.Size.X, this.Position.Y, this.Position.X + this.Size.X, this.Position.Y + this.Size.Y - 1, this.scrollRail);

                //Scroll Bar
                GraphicConsole.SetColor(this.scrollBarColor, this.fillColor);
                GraphicConsole.SetCursor(this.Position.X + this.Size.X, (int)(this.scrollValue / 100f * this.Size.Y) + this.Position.Y);
                GraphicConsole.Write(this.scrollBar);

                int line = (int)(this.scrollValue / 100f * (this.objectList.Count - this.Size.Y + 1));
                if (line < 0)
                    line = 0;

                for (int y = 0; y < this.Size.Y; y++)
                {
                    if (line < objectList.Count)
                    {
                        this.setConsoleColors(line);
                        GraphicConsole.SetCursor(this.Position.X, this.Position.Y + y);
                        this.writeLine(this.objectList[line].ListText);
                    }
                    line++;
                }
            }

            base.DrawStep();
        }
        public override void UpdateFrame(GameTime gameTime)
        {
            base.UpdateFrame(gameTime);
        }
        public override void MouseLeave()
        {
            this.hoverIndex = -1;
            InterfaceManager.DrawStep();

            base.MouseLeave();
        }
        public override void MouseMove()
        {
            int index = getIndexOfClick(InterfaceManager.CurrentCursorPosition);
            if (index >= 0 && index < this.objectList.Count)
            {
                this.hoverIndex = index;
                InterfaceManager.DrawStep();

                this.onHover();
            }
            else
            {
                this.hoverIndex = -1;
                InterfaceManager.DrawStep();
            }

            base.MouseMove();
        }
        public override void MouseUp(MouseButtonEventArgs e)
        {
            int index = getIndexOfClick(InterfaceManager.CurrentCursorPosition);
            if (index >= 0 && index < this.objectList.Count)
            {
                this.selectedIndex = index;
                InterfaceManager.DrawStep();

                this.onSelect();
            }
            else
            {
                this.selectedIndex = -1;

                this.Deselected?.Invoke(this);

                InterfaceManager.DrawStep();
            }

            base.MouseUp(e);
        }
        public override void MouseWheel(MouseWheelEventArgs e)
        {
            if (this.scroll)
            {
                if (e.Delta != 0)
                {
                    this.scrollValue -= (e.Delta < 0) ? -1 : 1;

                    if (this.scrollValue < 0f)
                        this.scrollValue = 0f;
                    else if (this.scrollValue >= 100f)
                        this.scrollValue = 99f;

                    InterfaceManager.DrawStep();
                }
            }

            base.MouseWheel(e);
        }

        public void SetList<T>(IList<T> newList) where T:ListItem
        {
            this.objectList.Clear();

            for (int i = 0; i < newList.Count; i++)
                this.objectList.Add(newList[i]);
            this.setupList();

            if (this.selectedIndex >= this.Items.Count)
                this.ClearSelection();
        }
        public void ClearList()
        {
            this.Items.Clear();
            this.ClearSelection();
        }
        public void ClearSelection()
        {
            this.selectedIndex = -1;
            InterfaceManager.DrawStep();
        }
        public void SetSelection(int index)
        {
            if (index >= 0 && index < this.objectList.Count)
            {
                this.selectedIndex = index;
                this.onSelect();
            }
            else
                this.ClearSelection();

            InterfaceManager.DrawStep();
        }
        public void SetSelection(string item)
        {
            for (int i = 0; i < this.objectList.Count; i++)
            {
                if (this.objectList[i] == item)
                {
                    this.selectedIndex = i;
                    InterfaceManager.DrawStep();

                    this.onSelect();

                    break;
                }
            }

            this.selectedIndex = -1;
            InterfaceManager.DrawStep();
        }
        public ListItem GetSelection()
        {
            return (selectedIndex != -1) ? this.objectList[this.selectedIndex] : null;
        }
        public void RemoveItem(string item)
        {
            for (int i = 0; i < this.Items.Count; i++)
            {
                if (this.Items[i].ListText == item)
                {
                    this.Items.RemoveAt(i);
                    break;
                }
            }
            this.ClearSelection();
        }
        public void RemoveItem(int index)
        {
            if (index < this.Items.Count)
            {
                this.Items.RemoveAt(index);

                if (this.selectedIndex >= this.Items.Count)
                    this.ClearSelection();
            }
        }
        public void AddItem(ListItem item)
        {
            this.Items.Add(item);
        }

        protected void onHover()
        {
            this.Hover?.Invoke(this, this.hoverIndex);
        }
        protected void onSelect()
        {
            this.Selected?.Invoke(this, this.selectedIndex);
        }

        private void setupList()
        {
            if (this.objectList.Count > this.Size.Y)
                scroll = true;
            else
                scroll = false;

            this.scrollValue = 0f;
        }
        private int getIndexOfClick(Point point)
        {
            int index = -1;
            if (this.scroll)
            {
                int line = (int)(this.scrollValue / 100f * (this.objectList.Count - this.Size.Y + 1));

                index = point.Y - this.Position.Y;
                index += line;
            }
            else
            {
                index = point.Y - this.Position.Y;
            }

            return index;
        }
        private void writeLine(string line)
        {
            if (!string.IsNullOrEmpty(line))
            {
                //Ensure text doesn't go past the size of the list
                int i = 0;
                for (i = 0; i < line.Length && i < this.Size.X; i++)
                    GraphicConsole.Write(line[i]);
                for (; i < this.Size.X; i++)
                    GraphicConsole.Write(' ');
            }
            else
            {
                for (int i = 0; i < this.Size.X; i++)
                    GraphicConsole.Write(' ');
            }
        }
        private void setConsoleColors(int index)
        {
            if (index == this.selectedIndex)
                GraphicConsole.SetColor(this.selectedTextColor, this.selectedFillColor);
            else if (index == this.hoverIndex)
                GraphicConsole.SetColor(this.hoverTextColor, this.hoverFillColor);
            else
                GraphicConsole.SetColor(this.objectList[index].TextColor, this.fillColor);
        }

        private List<ListItem> objectList;
        private Color4 textColor = Color4.White;
        private Color4 fillColor = Color4.Black;
        private Color4 selectedTextColor = Color4.Black;
        private Color4 selectedFillColor = Color4.White;
        private Color4 hoverTextColor = Color4.White;
        private Color4 hoverFillColor = new Color4(170, 181, 187, 255);
        private Color4 scrollRailColor = Color4.Gray;
        private Color4 scrollBarColor = Color4.LightGray;

        private bool scroll = false;
        private int selectedIndex = -1;
        private int hoverIndex = -1;
        private char scrollRail = '║';
        private char scrollBar = '▓';

        private float scrollValue = 0f;

        public event ItemHovered Hover;
        public event ItemSelected Selected;
        public event ItemDeselected Deselected;
        public delegate void ItemHovered(object sender, int index);
        public delegate void ItemSelected(object sender, int index);
        public delegate void ItemDeselected(object sender);

        #region Properties
        public List<ListItem> Items { get { return this.objectList; } set { this.SetList(value); } }
        public Color4 TextColor { get { return this.textColor; } set { this.textColor = value; } }
        public Color4 FillColor { get { return this.fillColor; } set { this.fillColor = value; } }
        public Color4 SelectedTextColor { get { return this.selectedTextColor; } set { this.selectedTextColor = value; } }
        public Color4 SelectedFillColor { get { return this.selectedFillColor; } set { this.selectedFillColor = value; } }
        public Color4 HoverTextColor { get { return this.hoverTextColor; } set { this.hoverTextColor = value; } }
        public Color4 HoverFillColor { get { return this.hoverFillColor; } set { this.hoverFillColor = value; } }
        public Color4 ScrollRailColor { get { return this.scrollRailColor; } set { this.scrollRailColor = value; } }
        public Color4 ScrollBarColor { get { return this.scrollBarColor; } set { this.scrollBarColor = value; } }
        public bool HasSelection { get { return (this.selectedIndex != -1); } }
        public int SelectedIndex { get { return this.selectedIndex; } set { this.SetSelection(value); } }
        #endregion
    }
    public class ListItem
    {
        public virtual Color4 TextColor { get; set; }
        public virtual string ListText { get; set; }

        public ListItem()
        {
            this.ListText = " ";
            this.TextColor = Color4.White;
        }
        public ListItem(string text)
        {
            this.ListText = text;
            this.TextColor = Color4.White;
        }

        public static implicit operator string(ListItem item)
        {
            return item.ListText;
        }
        public static implicit operator ListItem(string item)
        {
            return new ListItem() { ListText = item, TextColor = Color.White };
        }
    }
}
