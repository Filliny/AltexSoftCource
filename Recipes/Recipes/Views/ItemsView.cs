﻿using Recipes.Models;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Recipes.Views
{

    internal interface IItemsView
    {

        //IViewSettings Settings { get; }

        void ShowItems(IList<IListable> selectedList, bool selectable);

    }

    class ItemsView : IItemsView
    {

        private IViewSettings Settings { get; }

        public ItemsView(IViewSettings settings)
        {
            Settings = settings;
        }

        //Display list in chosen number of rows 
        public void ShowItems(IList<IListable> selectedList, bool selectable)
        {
            //Console.SetCursorPosition(1, 4);

            int rowWidth = Console.WindowWidth / (Settings.ListColumns + 1); //calculate row width from current window size

            int rowsInCol =
                (selectedList.Count - (selectedList.Count % Settings.ListColumns)) / (Settings.ListColumns); //get rows for each column

            if (Math.Abs(rowsInCol) < 1) //when there is one recipe in category
                rowsInCol = 1;

            foreach (var item in selectedList)
            {
                int column =
                    (int) Math.Ceiling((decimal) (selectedList.IndexOf(item) / rowsInCol)); //get column from item index
                int row = selectedList.IndexOf(item) - (rowsInCol * column) + Settings.ListStartRow; //get row by index & column
                Console.WriteLine(" ");
                Console.SetCursorPosition(column * rowWidth + Settings.ListRowOffset, row);

                if (item.Active && item.Selected)
                {
                    Console.BackgroundColor = ConsoleColor.Magenta;
                }
                else if(item.Selected)
                {
                    Console.BackgroundColor = ConsoleColor.Red;
                }
                else if (item.Active)
                {
                    
                    Console.BackgroundColor = ConsoleColor.DarkYellow;
                }


                Console.WriteLine(" " + item.Name+"  ");
                Console.BackgroundColor = ConsoleColor.Black;

            }


            int lastLine = Console.WindowHeight - 3;
            Console.SetCursorPosition(0,lastLine);
            string selectKey = "Enter";

            if (selectable)
                selectKey = "Space";
          


            Console.WriteLine($"    Используйте клавиши ↑↓ для навигации, {selectKey} для выбора,\n" +
                              "    Esc для возврата,\n" +
                              "    Insert чтобы создать новый элемент в этой категории.");

            Console.SetCursorPosition(0, 0);
        }

    }

}