
class Program
{
    static void Main(string[] args)
    {

        Console.WriteLine("\nAdaptive Finite Difference Gradient Descent Demo" + "\n");

        int maxExperiments = 150;
        double learningRate = 0.75;
        double learningRateDecay = 0.995;
        double tolerance = 0.0000000000001;
        int numHyperparameters = 10;
        Console.WriteLine($"Hyperparameters settings:");
        Console.WriteLine($"Experiments = {maxExperiments}");
        Console.WriteLine($"Learning rate = {learningRate}");
        Console.WriteLine($"Learning decay = {learningRateDecay}");
        Console.WriteLine($"Tolerance = {tolerance}");
        Console.WriteLine($"Hyperparameters = {numHyperparameters}");

        Console.WriteLine("\nTarget solution:");

        double[] targets = Enumerable.Range(0, numHyperparameters)
            .Select(i => i % 2 == 0 ? 0.5 : -0.5).ToArray();

        Console.WriteLine(string.Join(", ", targets.Select(h => h.ToString("F2"))));

        Console.WriteLine("\nInitial hyperparameters:");

        Random rng = new Random(123);
        // Initialize hyperparameters to 1 (for simplicity)
        double[] hyperparameters = new double[numHyperparameters];

        for (int i = 0; i < numHyperparameters; i++)
            hyperparameters[i] = (float)(rng.NextDouble() * 10 - 5); // * 2 - 1; // 1.0;
                                                                     //double[] hyperparameters = RandomGuess(numHyperparameters, 123);

        Console.WriteLine(string.Join(", ", hyperparameters.Select(h => h.ToString("F2"))));

        var initError = CalculateError(hyperparameters, targets);

        Console.WriteLine($"\nStarting error:\n" + $"{initError:F24}");

        var error = Optimize(hyperparameters, targets,
            maxExperiments, learningRate, learningRateDecay, tolerance, numHyperparameters);

        Console.WriteLine($"\nLabel target:");
        Console.WriteLine($"Error after optimization (decimal notation):\n{error:F24}");

        Console.WriteLine($"\nError after optimization (scientific notation):\n{error:E15}");

        static double Optimize(double[] hyperparameters, double[] targets, int maxExperiments,
            double learningRate, double lrDecay, double tolerance, int numHyperparameters)
        {
            Console.WriteLine("\nRunning optimization...");
            double[] gradients = new double[numHyperparameters];
            double previousError = double.MaxValue;
            for (int experiment = 0; experiment < maxExperiments; experiment++)
            {
                double currentError = CalculateError(hyperparameters, targets);
                int i = (experiment + 1) % numHyperparameters;

                double originalValue = hyperparameters[i];

                hyperparameters[i] += tolerance;

                double newError = CalculateError(hyperparameters, targets);

                hyperparameters[i] = originalValue;

                hyperparameters[i] += learningRate * (currentError - newError) / tolerance;

                learningRate *= lrDecay;

                previousError = currentError;

                if ((experiment + 1) % 10 == 0)
                    Console.WriteLine($"{experiment + 1,3}: " +
                        $"{string.Join(", ", hyperparameters.Select(h => h.ToString("F3").PadLeft(6)))} | Error: {newError:F6}");
            }
            return previousError;
        }

        static double CalculateError(double[] hps, double[] t)
        {
            // Hypothetical error calculation (sum of squares)
            double err = 0.0;
            for (int i = 0; i < hps.Length; i++)
                err += (t[i] - hps[i]) * (t[i] - hps[i]);
            return err;
        }
    }
}
