using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Text;

namespace Compression
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        public MainWindow()
        {
            InitializeComponent();

        }

        string text = "";
        string nextChar = "";
        int pointer = 0, length = 0, printTag = 0, LastTime = 0, counter = 0, test = 0, startIndx = 1;
        int numTags = 0, maxLength = 0, maxPointer = 0;

        // compression
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string curChar = "", CompChar = "";
            text = screen.Text;
            screen.Text = "0 0 " + text[0] + "\n"; // first char
            for (int indxTxt = 1; indxTxt < text.Length; indxTxt++)
            {

                CompChar += text[indxTxt];

                counter = 0;
                if (printTag == 1)  // it's true to print tag
                    LastTime = 2;

                curChar = "";
                for (int indxCheck = 0; indxCheck < indxTxt; indxCheck++)
                {

                    while (curChar.Length != CompChar.Length && indxCheck < startIndx - 1) // make length of current char and compare char equalized  
                    {
                        curChar = curChar + text[counter];
                        counter++;
                        indxCheck = counter - 1;
                    }

                    if (indxCheck < startIndx)
                    {
                        curChar = curChar + text[indxCheck];
                        if (curChar.Length > 1) // descard first char and take the nxt char
                        {
                            string delete = "";
                            for (int j = 1; j < curChar.Length; j++)
                                delete += curChar[j];

                            curChar = delete;
                        }
                    }

                    if (curChar == CompChar && indxCheck < startIndx && (indxTxt + 1 < text.Length || curChar.Length == 1)) // when have matching
                    {

                        length = curChar.Length; // length tag
                        pointer = indxTxt - indxCheck; // pointer tag
                        if (indxTxt + 1 < text.Length)
                            nextChar = text[indxTxt + 1] + ""; // nextChar tag
                        else
                            nextChar = "null";
                        printTag = 1; //  true to print tag
                        test = indxTxt; // that mean have matching so take next char
                    }
                    else if (indxCheck + 1 == indxTxt && curChar != CompChar && printTag != 1) // first time for this char
                    {

                        printTag = 1;
                        length = pointer = 0;
                        nextChar = CompChar[0] + "";

                    }



                    if (indxCheck + 1 == indxTxt && curChar != CompChar && LastTime == 2 && test != indxTxt)
                    {
                        indxTxt--;     // go back one char when have no matching   

                        LastTime = 1;
                    }

                }
                if (printTag == 1 && LastTime == 1 /*print tag*/|| indxTxt + 1 == text.Length - 1 || indxTxt + 1 == text.Length || test != indxTxt) // prevent expection
                {

                    if (maxLength < length)
                        maxLength = length;
                    if (maxPointer < pointer)
                        maxPointer = pointer;
                    numTags++;
                    screen.Text += pointer + " " + length + " " + nextChar + "\n";
                    // jump one char when length of tag != 0 to start after next char   
                    if (length > 0)
                        indxTxt++;
                    // prevent counter (indxCheck) to take char that string of counter (indxTxt) have it
                    startIndx = indxTxt + 1;

                    // defult variable
                    length = pointer = 0;
                    nextChar = "";
                    CompChar = curChar = "";
                    printTag = 0;
                    LastTime = 0;
                    test = 0;
                }

            }
            // to calculate compressed size
            int bit1 = 0, bit2 = 0, maxBit = 0, check = 0;
            original.Text = text.Length * 7 + "";
            while (bit1 == 0 && bit2 == 0)  // how many of bits 
            {
                if (maxLength <= maxBit)
                    bit1 = check;
                if (maxPointer <= maxBit)
                    bit2 = check;

                check++;
                maxBit += maxBit + 1;
            }
            Compressed.Text = numTags * (bit1 + bit2 + 7) + "";
            double ratioRes = 0;
            ratioRes = ((double)(numTags * (bit1 + bit2 + 7) / (double)(text.Length * 7))) * 100;
            ratio.Text = (int)ratioRes + " %";

            // defult variable
            pointer = length = printTag = LastTime = counter = numTags = 0;
            startIndx = 1;
            nextChar = "";
            maxLength = maxPointer = 0;


        }

        // Decompression
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

            string[] CompRslt = screen.Text.Split();
            text = screen.Text;
            screen.Text = "";
            for (int i = 0; i < text.Length; i += 3)
            {
                if (CompRslt[i].Length == 0)
                    break;
                // get pointer
                pointer = int.Parse(CompRslt[i]);
                // get length
                length = int.Parse(CompRslt[i + 1]);
                // get char
                nextChar = CompRslt[i + 2];

                string box = "";


                for (int indxTxt = screen.Text.Length; length > 0; length--, indxTxt++)
                    if (indxTxt - pointer >= 0 && indxTxt - pointer < screen.Text.Length)
                        box += screen.Text[indxTxt - pointer];
                
                   if (nextChar != "null")
                        screen.Text += box + nextChar;
                    else
                        screen.Text += box;

                    length = pointer = 0;
                    nextChar = "";

                
            }
            // defult variable 
            pointer = length = 0;
            nextChar = "";

        }

    }
}
