using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace NaturalSelection.ViewModel
{
    public class GridProperties : DependencyObject
    {

        public static int GetRows(System.Windows.Controls.Grid grid)
        {
            return (int)grid.GetValue(RowsProperty);
        }

        public static void SetRows(System.Windows.Controls.Grid grid, int value)
        {
            grid.SetValue(RowsProperty, value);
        }

        
        public static readonly DependencyProperty RowsProperty =
            DependencyProperty.RegisterAttached("Rows", typeof(int), typeof(Grid), new PropertyMetadata(0, ChangeRows));

        
        private static void ChangeRows(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // Приведение к Grid.
            if (!(d is System.Windows.Controls.Grid grid))
                throw new ArgumentException("Must be a Grid", nameof(d));

            // Приведение к int.
            int rows = (int)e.NewValue;

            // Добавление строк, если их не хватает.
            for (int i = grid.RowDefinitions.Count; i < rows; i++)
                grid.RowDefinitions.Add(new RowDefinition());

            // Удаление строк, если их больше.
            for (int i = grid.RowDefinitions.Count; i > rows;)
                grid.RowDefinitions.RemoveAt(--i);
        }

        
        public static int GetColumns(System.Windows.Controls.Grid grid)
        {
            return (int)grid.GetValue(ColumnsProperty);
        }

        
        public static void SetColumns(DependencyObject obj, int value)
        {
            obj.SetValue(ColumnsProperty, value);
        }

        
        public static readonly DependencyProperty ColumnsProperty =
            DependencyProperty.RegisterAttached("Columns", typeof(int), typeof(Grid), new PropertyMetadata(0, ChangeColumns));

        
        private static void ChangeColumns(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // Приведение к Grid.
            if (!(d is System.Windows.Controls.Grid grid))
                throw new ArgumentException("Должен быть Grid", nameof(d));

            // Приведение к int.
            int columns = (int)e.NewValue;

            // Добавление строк, если их не хватает.
            for (int i = grid.ColumnDefinitions.Count; i < columns; i++)
                grid.ColumnDefinitions.Add(new ColumnDefinition());

            // Удаление строк, если их больше.
            for (int i = grid.ColumnDefinitions.Count; i > columns;)
                grid.ColumnDefinitions.RemoveAt(--i);
        }
    }
}
