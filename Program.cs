using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            string strLine,strLine1;//strLine读skcw.txt，strLine1读水井
            List<string> water = new List<string>();//水井数组
            int index = 0;//计数器
            List <decimal> fipoil = new List<decimal>();
            List<decimal> pressure = new List<decimal>();
            List<decimal> soil = new List<decimal>();
            List<decimal> permx = new List<decimal>();
            List<decimal> dznet = new List<decimal>();
            decimal fAverage, preAverage, sAverage, permAverage, dAverage;
            int count = 0;
            string jh,str;
            int x, y, k;//坐标轴
            try
            {
                FileStream aFile = new FileStream(@"C:\Users\Sun\Desktop\data\0223data\skcw.txt", FileMode.Open);
                StreamReader sr = new StreamReader(aFile);
                FileStream bFile = new FileStream(@"C:\Users\Sun\Desktop\data\0223data\water.txt", FileMode.Open);
                StreamReader sr1 = new StreamReader(bFile);
                FileStream wFile = new FileStream(@"C:\Users\Sun\Desktop\data\0223data\Log_new.txt", FileMode.OpenOrCreate);
                StreamWriter sw = new StreamWriter(wFile);
                string isWater;
                //    //char nextChar = Convert.ToChar(sr.Read());

                //Read data in line by line 一行一行的读取
                strLine1 = sr1.ReadLine();
                while (strLine1 != null)
                {
                    //water[size] = strLine1;
                    //strLine1 = sr1.ReadLine();
                    //size++;
                    water.Add(strLine1);
                    strLine1 = sr1.ReadLine();
                }
                strLine = sr.ReadLine();
                fipoil = fileReader(@"C:\Users\Sun\Desktop\data\0223data\fipoil");
                pressure = fileReader(@"C:\Users\Sun\Desktop\data\0223data\pressure");
                soil = fileReader(@"C:\Users\Sun\Desktop\data\0223data\soil");
                permx = fileReader(@"C:\Users\Sun\Desktop\data\0223data\permx.txt");
                dznet = fileReader(@"C:\Users\Sun\Desktop\data\0223data\dznet.txt");
                index = 0;
                sw.WriteLine("油井        类别          坐标      渗透率      渗透率平均                        厚度          厚度平均          压力         压力平均                饱和度        饱和度平均                    储量         储量平均");
                while (strLine != null)
                //for (int z = 0; z < 15; z++)
                {
                    string[] nums = Regex.Split(strLine, @"\s+");
                    str = nums[0];
                    jh = str.Substring(0, str.Length-1);
                    if (water.Contains(jh))
                    {
                        isWater = "water";
                    }
                    else
                        isWater = "oil";
                    x = Convert.ToInt32(nums[1]);  //获得数字 
                    y = Convert.ToInt32(nums[2]);
                    k = Convert.ToInt32(nums[3]);
                    //Console.WriteLine(x + " " + y + " " + k);
                    count = (k - 1) * 152 * 223 + ((y - 1) * 223 + x) - 1;
                    fAverage = averageItem(x, y, k, fipoil);
                    preAverage = averageItem(x, y, k, pressure);
                    sAverage = averageItem(x, y, k, soil);
                    permAverage = averageItem(x, y, k, permx);
                    dAverage = averageItem(x, y, k, dznet);
                    sw.WriteLine(jh + "   " + isWater + "   " + x + " " + y + " " + k + "   " + permx[count] + "      " + permAverage + "                  " 
                        + dznet[count] + "      " + dAverage + "                 "
                        + pressure[count] + "      " + preAverage + "               " 
                        + soil[count] + "      " + sAverage + "               " 
                        + fipoil[count] + "      " + fAverage);
                    strLine = sr.ReadLine();

                }
                sr.Close();
                sw.Close();
            }
            catch (IOException ex)
            {
                Console.WriteLine("An IOException has been thrown!");
                Console.WriteLine(ex.ToString());
                Console.ReadLine();
                return;
            }
        }


        private static List<decimal> fileReader(string path)
        {
            int index = 0;
            string strLine;
            string trim;
            //string[] array = new string[size];
            //int a=0;
            List<decimal> num = new List<decimal>();
            try
            {
                FileStream aFile = new FileStream(path, FileMode.Open);
                StreamReader sr = new StreamReader(aFile);
                for (int i = 0; i < 4; i++)
                {
                    strLine = sr.ReadLine();
                }
                strLine = sr.ReadLine();
                while (strLine != null)
                {
                    if (strLine.Equals("/"))
                    {
                        break;
                    }
                    trim = strLine.Trim();
                    string[] array = Regex.Split(trim, @"\s+");
                    //trim = Regex.Replace(strLine, @"\s", "");
                    for (int i = 0; i < array.Length; i++)
                    {
                        //num[index] = Convert.ToDecimal(Convert.ToDouble(array[i]));
                        //index++;
                        num.Add(Convert.ToDecimal(Convert.ToDouble(array[i])));
                    }

                    strLine = sr.ReadLine();
                }
                sr.Close();
            }
            catch (IOException ex)
            {
                Console.WriteLine("An IOException has been thrown!");
                Console.WriteLine(ex.ToString());
                Console.ReadLine();
            }
            return num;
        }
        private static decimal averageItem(int x, int y, int k, List<decimal> array)
        {
            int m, n, count, num;
            decimal average=0, total = 0;
            num = 0;
            m = x;
            n = y;
            count = (k - 1) * 152 * 223 + ((n - 1) * 223 + m) - 1;
            num++;
            total = array[count];


            if (y != 1)
            {
                m = x - 1;
                n = y - 1;
                count = (k - 1) * 152 * 223 + ((n - 1) * 223 + m) - 1;
                num++;
                total += array[count];
                m = x;
                n = y - 1;
                count = (k - 1) * 152 * 223 + ((n - 1) * 223 + m) - 1;
                num++;
                total += array[count];
                m = x + 1;
                n = y - 1;
                count = (k - 1) * 152 * 223 + ((n - 1) * 223 + m) - 1;
                num++;
                total += array[count];
            }

            m = x - 1;
            n = y;
            count = (k - 1) * 152 * 223 + ((n - 1) * 223 + m) - 1;
            num++;
            total += array[count];
            m = x + 1;
            n = y;
            count = (k - 1) * 152 * 223 + ((n - 1) * 223 + m) - 1;
            num++;
            total += array[count];
            m = x - 1;
            n = y + 1;
            count = (k - 1) * 152 * 223 + ((n - 1) * 223 + m) - 1;
            num++;
            total += array[count];
            m = x;
            n = y + 1;
            count = (k - 1) * 152 * 223 + ((n - 1) * 223 + m) - 1;
            num++;
            total += array[count];
            m = x + 1;
            n = y + 1;
            count = (k - 1) * 152 * 223 + ((n - 1) * 223 + m) - 1;
            num++;
            total += array[count];
            average = total / num;
            return average;
        }
    }
}
