using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Book.ApplicationData
{
    internal class AppFrame
    {
        public static MainWindow MWindow { get; set; }
        public static Frame MainFrame { get; set; }

        private static Stack<string> _titles = new Stack<string>();

        // Методы навигации
        public static void Navigate(Page page, string title)
        {
            MainFrame.Navigate(page);
            MWindow.Title = title;
            _titles.Push(title);
        }
        // Методы навигации
        public static void GoBack()
        {
            MainFrame.GoBack();
            _titles.Pop();
            MWindow.Title = _titles.Peek();
        }
    }
}
