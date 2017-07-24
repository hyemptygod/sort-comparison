using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SortCompare
{
    class Program
    {
        static int[] data;
        static int dataLength;

        static void Main(string[] args)
        {

            InputDataLength();
            GenerateData();

            ////Console.WriteLine("\n源:");
            ////OutputData(data);
            /////  快排
            //int startTime = System.Environment.TickCount;
            //int[] newData = (int[])data.Clone();
            //QuickSort<int>(newData, 0, dataLength - 1);
            //Console.WriteLine("快排算法运行时间：" + (System.Environment.TickCount - startTime));
            //Console.WriteLine("快排算法运行结果");
            ////OutputData(newData);

            ////Console.WriteLine("\n源:");
            ////OutputData(data);
            ///// 自带List排序
            //startTime = System.Environment.TickCount;
            //int[] dataList = ListSort<int>(new List<int>(data));
            //Console.WriteLine("自带list sort算法运行时间：" + (System.Environment.TickCount - startTime));
            //Console.WriteLine("自带list sort算法运行结果");
            ////OutputData(dataList);

            ////Console.WriteLine("\n源:");
            ////OutputData(data);
            ///// linq 排序
            //startTime = System.Environment.TickCount;
            //int[] dataLinq = LinqSort<int>(new List<int>(data));
            //Console.WriteLine("linq算法运行时间：" + (System.Environment.TickCount - startTime));
            //Console.WriteLine("linq算法运行结果");
            ////OutputData(dataLinq);

            //startTime = System.Environment.TickCount;
            //int[] dataOrder = new List<int>(data).OrderBy(a => a).ToArray();
            //Console.WriteLine("List orderBy算法运行时间：" + (System.Environment.TickCount - startTime));
            ////OutputData();

            Console.WriteLine("源数据:");
            //OutputData(data);
            int startTime = System.Environment.TickCount;
            //QuickSort<int>(data, 0, dataLength - 1);

            //ListSort<int>(new List<int>(data));
            //LinqSort<int>(new List<int>(data));
            //HeapSort<int>(data,dataLength);
            int[] dataOrder = ListOrderBy<int>(new List<int>(data));
            Console.WriteLine("运行时间：" + (System.Environment.TickCount - startTime));
            Console.WriteLine("结果:");
            //OutputData(dataOrder);

            //HeapSort<int>(data, dataLength);
            //Console.WriteLine("源数据:");
            //OutputData(data);
        }

        static void InputDataLength()
        {
            bool stopWrite = false;
            while (!stopWrite)
            {
                string l = Console.ReadLine();
                try
                {
                    dataLength = Int32.Parse(l);
                    stopWrite = true;
                }
                catch
                {
                    stopWrite = false;
                    Console.WriteLine("输入错误");
                }
            }
        }

        static void GenerateData()
        {
            data = new int[dataLength];

            Random ra = new Random();
            for (int i = 0; i < dataLength; i++)
            {
                data[i] = ra.Next(0, dataLength);
            }
        }

        static void OutputData(int[] newData)
        {
            for (int i = 0; i < dataLength; i++)
            {
                if (i % 10 == 0 && i!=0)
                {
                    Console.Write("\n");
                }
                Console.Write(newData[i] + ",");
            }
            Console.Write("\n");
        }


        static void QuickSort<T>(T[] a, int l, int r) where T : IComparable
        {
            if (l < r)
            {
                int i = l, j = r;
                T pivot = a[i];
                while (i < j)
                {
                    while (i < j && a[j].CompareTo(pivot) > 0) j--;
                    if (i < j) a[i++] = a[j];

                    while (i < j && a[i].CompareTo(pivot) < 0) i++;
                    if (i < j) a[j--] = a[i];
                }

                a[i] = pivot;

                QuickSort<T>(a, l, i - 1);
                QuickSort<T>(a, i + 1, r);
            }
        }


        static T[] ListSort<T>(List<T> source) where T: IComparable
        {
            
            source.Sort((a, b) => a.CompareTo(b));
            

            return source.ToArray();
        }

        static T[] LinqSort<T>(List<T> source)
        {
            return (from a in source orderby a select a).ToArray();
        }

        static T[] ListOrderBy<T>(List<T> source)
        {
            return source.OrderBy(a => a).ToArray();
        }

        /// <summary>
        /// 堆排序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="a"></param>
        /// <param name="size">对大小(节点数)</param>
        static void HeapSort<T>(T[] a, int size) where T : IComparable
        {
            /// 初始化堆
            for (int i = size / 2 -1; i >= 0; i--)
            {
                AdjustHeap<T>(a, i, size);
            }

            for (int i = size - 1; i >= 0; i--)
            {
                T tmp = a[0];
                a[0] = a[i];
                a[i] = tmp;

                AdjustHeap<T>(a, 0, i);
            }
        }

        /// <summary>
        /// 调整堆
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="a"></param>
        /// <param name="node">调整的节点</param>
        /// <param name="size">堆的大小</param>
        static void AdjustHeap<T>(T[] a, int node, int size) where T : IComparable
        {
            if (node > size / 2 - 1)
            {
                return;
            }

            int lchild = node * 2 + 1;
            int rchild = node * 2 + 2;
            int max = node;

            if (lchild <= size - 1 && a[lchild].CompareTo(a[max]) > 0) max = lchild;
            if (rchild <= size - 1 && a[rchild].CompareTo(a[max]) > 0) max = rchild;

            if (max != node)
            {
                T tmp = a[max];
                a[max] = a[node];
                a[node] = tmp;

                /// 调整Max节点
                AdjustHeap<T>(a, max,size);
            }
        }
       
    }
}
