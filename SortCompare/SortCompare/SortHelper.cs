using System;
using System.Collections;
using System.Collections.Generic;

namespace SortCompare
{
    public static class SortHelper
    {
        public static bool USE_INTROSPECTIVE = true;

        public static int BinarySearch<T>(T[] array, int index, int length, T value, IComparer<T> comparer)
        {
            try
            {
                if (comparer == null)
                {
                    comparer = Comparer<T>.Default;
                }
                return InternalBinarySearch(array, index, length, value, comparer);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private static int InternalBinarySearch<T>(T[] array, int index, int length, T value, IComparer<T> comparer)
        {
            int start = index;
            int end = index + length - 1;
            while (start <= end)
            {
                int center = start + (end - start >> 1);
                int flag = comparer.Compare(array[center], value);
                if (flag == 0)
                {
                    return center;
                }
                if (flag < 0)
                {
                    start = center + 1;
                }
                else
                {
                    end = center - 1;
                }
            }
            return ~start;
        }

        public static void Sort<T>(T[] keys, int index, int length, IComparer<T> comparer = null)
        {
            try
            {
                if (comparer == null)
                {
                    comparer = Comparer<T>.Default;
                }

                if (USE_INTROSPECTIVE)
                {
                    IntrospectiveSort(keys, index, length, comparer);
                }
                else
                {
                    DepthLimitedQuickSort(keys, index, length + index - 1, comparer, 32);
                }
            }
            catch
            {

            }
        }


        private static void SwapIfGreater<T>(T[] keys, IComparer<T> comparer, int a, int b)
        {
            if (a != b && comparer.Compare(keys[a], keys[b]) > 0)
            {
                T val = keys[a];
                keys[a] = keys[b];
                keys[b] = val;
            }
        }

        private static void Swap<T>(T[] a, int i, int j)
        {
            if (i != j)
            {
                T val = a[i];
                a[i] = a[j];
                a[j] = val;
            }
        }

        internal static void DepthLimitedQuickSort<T>(T[] keys, int left, int right, IComparer<T> comparer, int depthLimit)
        {
            do
            {
                if (depthLimit == 0)
                {
                    Heapsort(keys, left, right, comparer);
                    break;
                }

                int start = left;
                int end = right;
                int center = start + (end - start >> 1);

                // 第一步先排序3个点(起始点、终点、终点)
                SwapIfGreater(keys, comparer, start, center);
                SwapIfGreater(keys, comparer, start, end);
                SwapIfGreater(keys, comparer, center, end);

                // 取基准点为中点
                T pivot = keys[center];

                while (true)
                {
                    if (comparer.Compare(keys[start], pivot) < 0)
                    {
                        start++;
                        continue;
                    }

                    while (comparer.Compare(pivot, keys[end]) < 0)
                    {
                        end--;
                    }

                    if (start > end)
                    {
                        break;
                    }

                    if (start < end)
                    {
                        T val2 = keys[start];
                        keys[start] = keys[end];
                        keys[end] = val2;
                    }

                    start++;
                    end--;
                    if (start > end)
                    {
                        break;
                    }
                }

                depthLimit--;
                if (end - left <= right - start)
                {
                    if (left < end)
                    {
                        DepthLimitedQuickSort(keys, left, end, comparer, depthLimit);
                    }
                    left = start;
                }
                else
                {
                    if (start < right)
                    {
                        DepthLimitedQuickSort(keys, start, right, comparer, depthLimit);
                    }
                    right = end;
                }
            }
            while (left < right);
        }

        public static void IntrospectiveSort<T>(T[] keys, int left, int length, IComparer<T> comparer)
        {
            if (length >= 2)
            {
                IntroSort(keys, left, length + left - 1, 2 * FloorLog2(keys.Length), comparer);
            }
        }

        public static int FloorLog2(int n)
        {
            int num = 0;
            while (n >= 1)
            {
                num++;
                n /= 2;
            }
            return num;
        }

        private static void IntroSort<T>(T[] keys, int lo, int hi, int depthLimit, IComparer<T> comparer)
        {
            while (hi > lo)
            {
                int num = hi - lo + 1;
                if (num <= 16)
                {
                    switch (num)
                    {
                        case 1:
                            break;
                        case 2:
                            SwapIfGreater(keys, comparer, lo, hi);
                            break;
                        case 3:
                            SwapIfGreater(keys, comparer, lo, hi - 1);
                            SwapIfGreater(keys, comparer, lo, hi);
                            SwapIfGreater(keys, comparer, hi - 1, hi);
                            break;
                        default:
                            InsertionSort(keys, lo, hi, comparer);
                            break;
                    }
                    break;
                }
                if (depthLimit == 0)
                {
                    Heapsort(keys, lo, hi, comparer);
                    break;
                }
                depthLimit--;
                int num2 = PickPivotAndPartition(keys, lo, hi, comparer);
                IntroSort(keys, num2 + 1, hi, depthLimit, comparer);
                hi = num2 - 1;
            }
        }

        private static int PickPivotAndPartition<T>(T[] keys, int lo, int hi, IComparer<T> comparer)
        {
            int num = lo + (hi - lo) / 2;
            SwapIfGreater(keys, comparer, lo, num);
            SwapIfGreater(keys, comparer, lo, hi);
            SwapIfGreater(keys, comparer, num, hi);
            T val = keys[num];
            Swap(keys, num, hi - 1);
            int num2 = lo;
            int num3 = hi - 1;
            while (num2 < num3)
            {
                while (comparer.Compare(keys[++num2], val) < 0)
                {
                }
                while (comparer.Compare(val, keys[--num3]) < 0)
                {
                }
                if (num2 >= num3)
                {
                    break;
                }
                Swap(keys, num2, num3);
            }
            Swap(keys, num2, hi - 1);
            return num2;
        }

        private static void Heapsort<T>(T[] keys, int lo, int hi, IComparer<T> comparer)
        {
            int num = hi - lo + 1;
            for (int num2 = num / 2; num2 >= 1; num2--)
            {
                DownHeap(keys, num2, num, lo, comparer);
            }
            for (int num3 = num; num3 > 1; num3--)
            {
                Swap(keys, lo, lo + num3 - 1);
                DownHeap(keys, 1, num3 - 1, lo, comparer);
            }
        }

        private static void DownHeap<T>(T[] keys, int i, int n, int lo, IComparer<T> comparer)
        {
            T val = keys[lo + i - 1];
            while (i <= n / 2)
            {
                int num = 2 * i;
                if (num < n && comparer.Compare(keys[lo + num - 1], keys[lo + num]) < 0)
                {
                    num++;
                }
                if (comparer.Compare(val, keys[lo + num - 1]) >= 0)
                {
                    break;
                }
                keys[lo + i - 1] = keys[lo + num - 1];
                i = num;
            }
            keys[lo + i - 1] = val;
        }

        private static void InsertionSort<T>(T[] keys, int lo, int hi, IComparer<T> comparer)
        {
            for (int i = lo; i < hi; i++)
            {
                int num = i;
                T val = keys[i + 1];
                while (num >= lo && comparer.Compare(val, keys[num]) < 0)
                {
                    keys[num + 1] = keys[num];
                    num--;
                }
                keys[num + 1] = val;
            }
        }
    }
}
