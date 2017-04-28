using System;
using System.IO;

namespace MN41_TP3
{
    class Program
    {   
        static void Main(string[] args)
        {
            const string filePath = @"/home/chrx/data.txt";
            StreamReader file = new StreamReader(File.OpenRead(filePath));

            // Leave `Dimension` label line
            file.ReadLine();

            // Read dimension
            int n = Convert.ToInt32(file.ReadLine());

            int i, j;
            double[,] K = new double[n, n + 1];
            string[] vec = new string[n];

            // Leave the `K` label line
            file.ReadLine();

            // Read the n*n-sized K matrix
            for (i = 0; i < n; i++) {
                vec = file.ReadLine().Split(' ');
                for (j = 0; j < n; j++) {
                    K[i, j] = Convert.ToDouble(vec[j]);
                }
            }
            displayMatrix("K", K);

            // Leave the `F` label line
            file.ReadLine();

            // Read the n-sized F vector
            vec = file.ReadLine().Split(' ');
            for (j = 0; j < n; j++) {
                K[j, n] = Convert.ToDouble(vec[j]);
            }


            // Close the file
            file.Dispose();

            int k;
            double coeff;
            for (k = 0; k < n - 1; k++) {
                for (i = k + 1; i < n; i++) {
                    coeff = K[i, k] / K[k, k];
                    for (j = k; j < n + 1; j++) {
                        K[i, j] = K[i, j] - K[k, j] * coeff;
                    }
                }
                Console.WriteLine("\n==============");
                Console.WriteLine("  Étape {0}", k + 1);
                Console.WriteLine("==============");
                displayMatrix("K", K);
            }

            // Solve the new system and display the Xi
            double[] X = new double[n];
            double sum;

            X[n - 1] = K[n - 1, n];
            
            for (i = n - 1; i >= 0; i--) {
                sum = 0;
                for (j = i + 1; j < n; j++) {
                    sum += K[i, j]*X[j];
                }
                X[i] = (K[i, n] - sum) / K[i, i];
            }

            displayVector("X", X);
        }
        
        /*
           Displays any n-sized vector nicely
         */
        private static void displayVector(string name, double[] vec) {
            int i;
            int n = vec.Length;
            Console.Write("{0} = [", name);
            for (i = 0; i < n - 1; i++) {
                Console.Write("{0,8:f4} ", vec[i]);
            }
            Console.Write("{0,8:f4}  ]\n\n", vec[n - 1]);
        }

        /*
            Displays any n*m-sized matrix nicely
         */
        private static void displayMatrix(string name, double[,] mat) {
            int i, j;
            int n = mat.GetLength(0);
            int m = mat.GetLength(1);

            Console.Write("{0} = [\n", name);
            for (i = 0; i < n; i++) {
                for (j = 0; j < m - 1; j++) {
                    Console.Write("{0,8:f4} ", mat[i, j]);
                }
                Console.Write("{0, 8:f4}  \n", mat[i, m - 1]);
            }
            Console.Write("  ]\n\n", mat[i - 1, m - 1]);
        }
    }
}
