using System;
using System.IO;

namespace MN41_TP4
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

            int i, j, r, k;
            double[,] K = new double[n, n];
            double[,] L = new double[n, n];
            double[,] U = new double[n, n];
            double[] F = new double[n];
            double[] X = new double[n];
            double[] Y = new double[n];
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
                F[j] = Convert.ToDouble(vec[j]);
            }
            displayVector("F", F);

            // Close the file
            file.Dispose();

            // Computing L
            for (i = 0; i < n; i++) {
                L[i, i] = 1;
            }
            for (r = 0; r < n; r++) {
                for (j = r; j < n; j++) {
                    double sum = 0;
                    for (k = 0; k < r; k++) {
                        sum += L[r, k]*U[k, j];
                    }
                    U[r, j] = K[r, j] - sum;
                }
                for (i = r; i < n; i++) {
                    double sum = 0;
                    for (k = 0; k < r; k++) {
                        sum += L[i, k]*U[k, r];
                    }
                    L[i, r] = (K[i, r] - sum) / U[r, r];
                }
            }
            displayMatrix("L", L);
            displayMatrix("U", U);

            // Compute Y=U.X
            Y[0] = F[0];
            for (k = 1; k < n; k++) {
                double sum = 0;
                for (r = 0; r < k; r++) {
                    sum += L[k, r] * Y[r];
                }
                Y[k] = F[k] - sum;
            }
            displayVector("Y", Y);

            // Compute X solving UX=Y
            X[n - 1] = Y[n - 1] / U[n - 1, n - 1];
            X[n - 2] = (Y[n - 2] - U[n - 2, n-1] * X[n - 1]) / U[n - 2, n - 2];
            for (k = n - 2; k >= 0; k--) {
                double sum = 0;
                for (r = k + 1; r < n; r++) {
                    sum += U[k, r] * X[r];
                }
                X[k] = (Y[k] - sum) / U[k, k];
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
