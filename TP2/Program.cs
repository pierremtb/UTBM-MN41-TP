using System;
using System.IO;

namespace MN41_TP2
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
            double[,] K = new double[n, n];
            double[] F = new double[n];
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


            // Close the file
            file.Dispose();

            // Start the Thomas method
            double[] A = new double[n];
            double[] B = new double[n];
            double[] C = new double[n];
            double[] D = new double[n];
            double[] Alpha = new double[n];
            double[] Beta = new double[n];
            double[] U = new double[n];

            // Retrieve a, under the diagonal of K
            for (i = 1; i < n; i++) {
                A[i] = K[i, i - 1];
            }
            displayVector("A", A);

            // Retrieve b, the diagonal of K
            for (i = 0; i < n; i++) {
                B[i] = K[i, i];
            }
            displayVector("B", B);

            // Retrieve c, on top of the diagonal of K
            for (i = 0; i < n - 1; i++) {
                C[i] = K[i, i + 1];
            }
            displayVector("C", C);

            // Retrieve d, which is the vector F
            for (i = 0; i < n; i++) {
                D[i] = F[i];
            }
            displayVector("D", D);

            // Compute alpha and beta
            Alpha[0] = C[0] / B[0];
            Beta[0] = D[0] / B[0];
            for (i = 1; i < n; i++) {
                if (i < n - 1) {
                    Alpha[i] = C[i] / (B[i] - A[i]*Alpha[i - 1]);
                }
                Beta[i] = (D[i] - A[i]*Beta[i - 1]) / (B[i] - A[i]*Alpha[i - 1]);
            }
            displayVector("Alpha", Alpha);
            displayVector("Beta", Beta);

            // Finally solve our new diagonal system from the bottom to the top
            U[n - 1] = Beta[n - 1];
            for( i = n - 2; i >= 0; i--) {
                U[i] = Beta[i] - Alpha[i] * U[i + 1];
            }
            displayVector("U", U);
        }
        
        /*
           Displays any n-sized vector nicely
         */
        private static void displayVector(string name, double[] vec) {
            int i;
            int n = vec.Length;
            Console.Write("{0} = [", name);
            for (i = 0; i < n - 1; i++) {
                Console.Write("{0,8:f3} ", vec[i]);
            }
            Console.Write("{0,8:f3}  ]\n\n", vec[n - 1]);
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
                    Console.Write("{0,8:f3} ", mat[i, j]);
                }
                Console.Write("{0, 8:f3}  \n", mat[i, m - 1]);
            }
            Console.Write("  ]\n\n", mat[i - 1, m - 1]);
        }
    }
}
