using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Cryptography.X509Certificates;
using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;

//This class is just to group data together, and will probably be used to map to SQL at the end
public class DataItem
{
    public string Date { get; set; }
    public string Max { get; set; }
    public string Min { get; set; }
    public string Avg { get; set; }
    public string Hdd { get; set; }
    public string Cdd { get; set; }
    public string Prcp { get; set; }
    public string Month { get; set; }

    public override string ToString()
    {
        return "MONTH: " + Month + " DATE: " + Date + " MAX: " + Max + " MIN: " + Min + " AVG: " + Avg + " HDD: " + Hdd + " CDD: " + Cdd + " PRCP: " + Prcp;
    }



}
public static class Program
{
    public static void Main()
    {
        using (PdfDocument document = PdfDocument.Open(@"C:\Users\dixon\Desktop\Devproj\PDFWeather\test\sample.pdf"))
        {

            //SortedDictionary<double, int> countFont = new SortedDictionary<double, int>();
            Dictionary<string, int> dateMonths = new Dictionary<string, int>
            {
                { "January", 31 },
                { "February", 28 },
                { "March", 31 },
                { "April", 30 },
                { "May", 31 },
                { "June", 30 },
                { "July", 31 },
                { "August", 31 },
                { "September", 30},
                { "October", 31 },
                { "November", 30 },
                { "December", 31 },
            };


            ArrayList dataHeadings = new ArrayList();
            ArrayList collumnHeadings = new ArrayList();
            ArrayList entireDoc = new ArrayList();
            List<DataItem> monthOne = new List<DataItem>();
            List<DataItem> monthTwo = new List<DataItem>();
            List<DataItem> monthThree = new List<DataItem>();



            bool startWrite = false;

            foreach (Page page in document.GetPages())
            {
                // Loop through all words on page, if font size matches the heading size and is not already in the list, add to list
                foreach (Word word in page.GetWords())
                {
                    if (word.Letters[0].FontSize == 9 && !dataHeadings.Contains(word))
                    {
                        dataHeadings.Add(word.ToString());
                        // After a table titles has been found, text following will be data, set startWrite to true
                        startWrite = true;
                    }

                    // Now startWrite is true, find data and collumn headings 
                    if (word.Letters[0].FontSize == 7.5 && startWrite)
                    {
                        // Letter width < 5 is /probably/ data
                        if (word.Letters[0].Width < 5)
                        {
                            entireDoc.Add(word);
                            Console.WriteLine(word);
                        }
                        else
                        {
                            if (!collumnHeadings.Contains(word.ToString()))
                            {
                                // Add collumn headings to list, stop writing after MNTH
                                // Lazy work around, needs fixing
                                collumnHeadings.Add(word.ToString());
                                if (word.ToString() == "MNTH:")
                                {
                                    startWrite = false;
                                }
                            }
                        }
                    }
                }
                // Data comes in sets of 7, always in the same format
                int chooseMonth = 0;
                //string monthName = (string)dataHeadings[chooseMonth];
                //int monthValue = dateMonths[monthName];

                

                for (int k = 0; k < entireDoc.Count; k += 8)
                {
                    string monthName = (string)dataHeadings[chooseMonth];
                    int monthValue = dateMonths[monthName];
                    if (chooseMonth == 0)
                    {
                        if (monthOne.Count == monthValue)
                        {
                            chooseMonth++;
                            Console.WriteLine("Month full");
                            Console.WriteLine($"Month {monthName} contains {dateMonths[monthName]}");
                        }
                        else
                        {
                            Console.WriteLine($"Item Added to: {monthName}, contains {monthValue} days");
                            monthOne.Add(new DataItem()
                            {
                                Date = entireDoc[k].ToString(),
                                Max = entireDoc[k + 1].ToString(),
                                Min = entireDoc[k + 2].ToString(),
                                Avg = entireDoc[k + 3].ToString(),
                                Hdd = entireDoc[k + 4].ToString(),
                                Cdd = entireDoc[k + 5].ToString(),
                                Prcp = entireDoc[k + 6].ToString(),
                                Month = dataHeadings[chooseMonth].ToString()
                                // Ignore 7th item, can be added later or tagged on the end of Prcp
                            });
                        }

                    }

                    if (chooseMonth == 1)
                    {
                        if (monthTwo.Count == monthValue)
                        {
                            chooseMonth++;
                        }
                        else
                        {
                            Console.WriteLine($"Item Added to: {monthName}, contains {monthValue} days");

                            monthTwo.Add(new DataItem()
                            {
                                Date = entireDoc[k].ToString(),
                                Max = entireDoc[k + 1].ToString(),
                                Min = entireDoc[k + 2].ToString(),
                                Avg = entireDoc[k + 3].ToString(),
                                Hdd = entireDoc[k + 4].ToString(),
                                Cdd = entireDoc[k + 5].ToString(),
                                Prcp = entireDoc[k + 6].ToString(),
                                Month = dataHeadings[chooseMonth].ToString()
                                // Ignore 7th item, can be added later or tagged on the end of Prcp
                            });
                        }
                    }

                    if (chooseMonth == 2)
                    {
                        if (monthThree.Count == monthValue)
                        {
                            chooseMonth++;
                        }
                        else
                        {
                            Console.WriteLine($"Item Added to: {monthName}, contains {monthValue} days");

                            monthThree.Add(new DataItem()
                            {
                                Date = entireDoc[k].ToString(),
                                Max = entireDoc[k + 1].ToString(),
                                Min = entireDoc[k + 2].ToString(),
                                Avg = entireDoc[k + 3].ToString(),
                                Hdd = entireDoc[k + 4].ToString(),
                                Cdd = entireDoc[k + 5].ToString(),
                                Prcp = entireDoc[k + 6].ToString(),
                                Month = dataHeadings[chooseMonth].ToString()
                                // Ignore 7th item, can be added later or tagged on the end of Prcp
                            });
                        }
                    }


                    chooseMonth++;
                    if (chooseMonth >= 3)
                    {
                        chooseMonth = 0;
                    }
                }
                // TODO: Arrange data in correct order, currently runs across entire rows
                foreach (DataItem item in monthOne)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine(item);
                }

                foreach (DataItem item in monthTwo)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine(item);
                }

                foreach (DataItem item in monthThree)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine(item);
                }
            }
            Console.ForegroundColor = ConsoleColor.White;
        }
        Console.ReadKey();
    }
}