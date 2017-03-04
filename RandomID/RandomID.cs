using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RandomIDSystem
{
    class RandomID
    {
        private const string filename = "ID.bin";

        public RandomID()
        {
            // There numbers in three files should be 995, 997, and 999.
            // This can generate the most combinations up to 991,022,985. 
            // (The lowest common multiple of 995, 997, and 999.)
        }

        public string GetID(int index)
        {
            int n1 = 0;
            int n2 = 0;
            int n3 = 0;

            // Read data from file.
            // Should be in binary format.
            using (var file = File.OpenRead(filename))
            {
                byte[] data = new byte[4];
                int t1 = 0;
                int t2 = 0;
                int t3 = 0;

                if (file.Read(data, 0, 4) == 4)
                {
                    t1 = BitConverter.ToInt32(data, 0);
                    int offset = index % t1;
                    file.Seek(offset * 4, SeekOrigin.Current);
                    if (file.Read(data, 0, 4) == 4)
                    {
                        n1 = BitConverter.ToInt32(data, 0);
                    }
                    else
                    {
                        // Something wrong.
                    }
                }
                else
                {
                    // Something wrong.
                }

                file.Seek((t1 + 1) * 4, SeekOrigin.Begin);

                if (file.Read(data, 0, 4) == 4)
                {
                    t2 = BitConverter.ToInt32(data, 0);
                    int offset = index % t2;
                    file.Seek(offset * 4, SeekOrigin.Current);
                    if (file.Read(data, 0, 4) == 4)
                    {

                        n2 = BitConverter.ToInt32(data, 0);
                    }
                    else
                    {
                        // Something wrong.
                    }
                }
                else
                {
                    // Something wrong.
                }

                file.Seek(((t1 + 1) + (t2 + 1)) * 4, SeekOrigin.Begin);
                if (file.Read(data, 0, 4) == 4)
                {
                    t3 = BitConverter.ToInt32(data, 0);
                    int offset = index % t3;
                    file.Seek(offset * 4, SeekOrigin.Current);
                    if (file.Read(data, 0, 4) == 4)
                    {
                        n3 = BitConverter.ToInt32(data, 0);
                    }
                    else
                    {
                        // Something wrong.
                    }
                }
                else
                {
                    // Something wrong.
                }
            }

            return $"{n1:000}-{n2:000}-{n3:000}";
        }

        public void MakeIDs(int seed)
        {
            // Add the excluding numbers.
            var excluded = new List<List<int>>
            {
                new List<int> { 888, 777, 666, 444 },
                new List<int> { 999 , 444 },
                new List<int> {}
            };

            var result = new List<List<int>>();

            List<int> numbers = new List<int>();

            int index = 0;
            while (index < excluded.Count)
            {
                var exc = excluded[index];
                numbers.Clear();
                for (int i = 1; i < 1000; i++)
                {
                    if (exc.Contains(i))
                    {
                        continue;
                    }

                    numbers.Add(i);
                }

                Shuffle(numbers, seed + index);
                result.Add(new List<int>(numbers));
                index++;
            }

            WriteToFile(result, saveAsText: false);
        }

        private void Shuffle(List<int> numbers, int seed)
        {
            Random random = new Random(seed);

            for (int i = 0; i < numbers.Count; i++)
            {
                int swap = random.Next(i, numbers.Count);
                int tmp = numbers[i];
                numbers[i] = numbers[swap];
                numbers[swap] = tmp;
            }
        }

        private void WriteToFile(List<List<int>> result, bool saveAsText = false)
        {
            if (saveAsText)
            {
                // Save in text format.
                using (var file = new StreamWriter(filename, false, Encoding.ASCII))
                {
                    var stringBuilder = new StringBuilder();
                    foreach (var numberList in result)
                    {
                        stringBuilder.AppendLine($"== {numberList.Count} ==");

                        foreach (var number in numberList)
                        {
                            stringBuilder.AppendLine($"{number:000}");
                        }
                    }
                    file.WriteLine(stringBuilder.ToString());
                }
            }
            else
            {
                // Save in binary format.
                using (var file = File.OpenWrite(filename))
                {
                    foreach (var numberList in result)
                    {
                        // Write the size of the list.
                        file.Write(BitConverter.GetBytes(numberList.Count), 0, 4);

                        foreach (var number in numberList)
                        {
                            file.Write(BitConverter.GetBytes(number), 0, 4);
                        }
                    }
                }
            }

        }

        /*
        private void WriteFile(List<int> numbers, bool saveToText = false)
        {

            if (saveToText)
            {
                // Save to a text file.
                using (var file = new StreamWriter(filename))
                {
                    var stringBuilder = new StringBuilder();

                    foreach (var number in numbers)
                    {
                        stringBuilder.Append($"{number:000}").Append("\n");
                    }

                    file.WriteLine(stringBuilder.ToString().Trim());
                }
            }
            else
            {
                // Save to a binary file.
                using (var file = File.Open(filename, FileMode.Create, FileAccess.Write))
                {
                    foreach (var number in numbers)
                    {
                        file.Write(BitConverter.GetBytes(number), 0, 4);
                    }
                }
            }
        }
        */
    }
}
